using BuildingBlocks.Exceptions.Handler;
using DicountGrpc;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using BuildingBlocks.Messaging.Mass_Transit;
using Grpc.Net.Client;

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

            builder.Services.Decorate<IBasketRepository, CashedBasketRepository>();
            builder.Services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = builder.Configuration.GetConnectionString("Redis")!;
            });

            // Updated gRPC client configuration to match Docker Compose environment variable
            builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
            {
                // Use the environment variable name that matches Docker Compose: GrpcSettings__DiscountUrl
                var grpcUrl = builder.Configuration["GrpcSettings:DiscountUrl"] ??
                             builder.Configuration["GrpcSettings:Discount:Url"] ??
                             "https://localhost:7001"; // fallback for local development

                options.Address = new Uri(grpcUrl);

                // Configure channel options for better Docker support
                if (!grpcUrl.StartsWith("https"))
                {
                    options.ChannelOptionsActions.Add(channelOptions =>
                    {
                        channelOptions.Credentials = Grpc.Core.ChannelCredentials.Insecure;
                    });
                }
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                if (builder.Environment.IsDevelopment())
                {
                    // Accept any certificate in development (important for Docker containers)
                    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                    {
                        return true; // Accept all certificates in development
                    };
                }
                return handler;
            });

            builder.Services.AddMessageBroker(builder.Configuration);
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            builder.Services.AddHealthChecks()
                .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
                .AddRedis(builder.Configuration.GetConnectionString("Redis")!);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.Run();
        }
    }
}