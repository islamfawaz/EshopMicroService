using BuildingBlocks.Exceptions.Handler;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Ordering.APIs.Extensions;
using Ordering.APIs.Services;
using Ordering.Application;
using Ordering.Domain.Abstractions;
using Ordering.Infrastructure;


namespace Ordering.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ILoggedInUserService, LoggedInUserService>();
            builder.Services.AddCarter();
            builder.Services.AddExceptionHandler<CustomExceptionHandler>();
            builder.Services.AddHealthChecks()
                .AddSqlServer(builder.Configuration.GetConnectionString("Database")!);
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.UseHealthChecks("/health",new HealthCheckOptions
            {
                ResponseWriter=UIResponseWriter.WriteHealthCheckUIResponse
            });
           
             await app.InitialiseDatabaseAsync();
           

            app.Run();
        }
    }
}
