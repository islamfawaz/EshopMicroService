using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

// شيلنا الـ using دي لأننا مش هنستخدم الـ Registry
// using Ordering.Application.Extentions; 

namespace Ordering.Application.BackgroundServices
{
    public class ProcessOutboxMessagesJob : BackgroundService
    {
        private readonly ILogger<ProcessOutboxMessagesJob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ProcessOutboxMessagesJob(ILogger<ProcessOutboxMessagesJob> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Outbox Processor Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await ProcessMessages(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }

        private async Task ProcessMessages(CancellationToken stoppingToken)
        {
            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null && m.Error == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(20)
                .ToListAsync(stoppingToken);

            if (!messages.Any())
            {
                return;
            }

            _logger.LogInformation("Found {count} messages to process.", messages.Count);

            // تجهيز إعدادات الـ Deserializer عشان تتوافق مع الـ Handler
            var serializerOptions = new JsonSerializerOptions
            {
                // لازم نستخدم نفس الإعدادات اللي استخدمناها في الـ Handler
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                // ودي كأمان إضافي عشان لو حصل أي اختلاف
                PropertyNameCaseInsensitive = true
            };

            foreach (var message in messages)
            {
                try
                {
                    // ------------------ التغيير الأساسي هنا ------------------
                    // رجعنا نستخدم Type.GetType() مباشرةً.
                    // بما إن message.Type يحتوي على AssemblyQualifiedName، فالميثود دي هتشتغل صح.
                    var eventType = Type.GetType(message.Type);
                    // ---------------------------------------------------------

                    if (eventType is null)
                    {
                        _logger.LogError("Could not find type for: {type}. Marking as failed.", message.Type);
                        message.Error = $"Type '{message.Type}' could not be loaded."; // سجل الخطأ
                        continue;
                    }

                    // بنعمل Deserialization مع استخدام الـ Options اللي جهزناها
                    var integrationEvent = JsonSerializer.Deserialize(message.Content, eventType, serializerOptions);

                    if (integrationEvent is null)
                    {
                        _logger.LogError("Failed to deserialize message content for ID: {id}. Marking as failed.", message.Id);
                        message.Error = "Deserialization resulted in a null object."; // سجل الخطأ
                        continue;
                    }

                    await publishEndpoint.Publish(integrationEvent, stoppingToken);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing outbox message {id}. Will retry.", message.Id);
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
