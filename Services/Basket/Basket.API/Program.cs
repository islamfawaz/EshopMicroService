
using BuildingBlocks.Exceptions.Handler;
using DicountGrpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            System.Reflection.Assembly? assembly = typeof(Program).Assembly;
            builder.Services.AddCarter();
            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));


            });
            builder.Services.AddMarten(opt =>
            {
                opt.Connection(builder.Configuration.GetConnectionString("Database")!);
                opt.Schema.For<ShoppingCart>()
                    .Identity(x => x.UserName);
            }).UseLightweightSessions();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            ///builder.Services.AddScoped<IBasketRepository>(provider =>
            ///{
            ///   var basketRepo= provider.GetRequiredService<BasketRepository>();
            ///     return new CashedBasketRepository(basketRepo, provider.GetRequiredService<IDistributedCache>());
            ///});

            builder.Services.Decorate<IBasketRepository, CashedBasketRepository>();
            builder.Services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration=builder.Configuration.GetConnectionString("Redis")!;
            });

            // In your Basket API Program.cs or wherever you configure gRPC client
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
            {
                options.Address = new Uri(builder.Configuration["GrpcSettings:Discount:Url"]!);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                if (builder.Environment.IsDevelopment())
                {
                    // Skip certificate validation in development
                    handler.ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                }
                return handler;
            });
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            builder.Services.AddHealthChecks().AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                .AddRedis(builder.Configuration.GetConnectionString("Redis")!);
            
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            app.MapCarter();
            app.UseExceptionHandler(options => {});
            app.UseHealthChecks("/health",new HealthCheckOptions
            {
                ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
            });
            app.Run();
        }
    }
}
