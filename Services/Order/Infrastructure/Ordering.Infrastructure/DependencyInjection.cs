using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ordering.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Get connection string and validate
            var connectionString = configuration.GetConnectionString("Database");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            // Register interceptors
            services.AddScoped<AuditableEntityInterceptor>();
            //services.AddScoped<DispatchDomainEventsInterceptor>();

            // Register DbContext
            services.AddDbContext<ApplicationDbContext>((provider, options) =>
            {
                var auditInterceptor = provider.GetRequiredService<AuditableEntityInterceptor>();
           //     var eventsInterceptor = provider.GetRequiredService<DispatchDomainEventsInterceptor>();

                options.AddInterceptors(auditInterceptor);
                options.UseSqlServer(
                    connectionString,
                    b => b.MigrationsAssembly("Ordering.Infrastructure") // Ensure EF Core knows where migrations are
                );
            });


            // Register the interface
            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();

            return services;
        }
    }
}
