
namespace Catalog.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Add Services to DI Container
            var assembly = typeof(Program).Assembly;

            builder.Services.AddCarter();

            builder.Services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(assembly);
                //Before Handle() MeditR trigger ValidationBehavior
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            });
            builder.Services.AddValidatorsFromAssembly(assembly);
            builder.Services.AddMarten(options =>
            {
                options.Connection(builder.Configuration.GetConnectionString("Database")!);
            }).UseLightweightSessions();
            if (builder.Environment.IsDevelopment())
            {
                builder.Services.InitializeMartenWith<CatalogInitialData>();
            }
            builder.Services.AddExceptionHandler<CustomExceptionHandler>(); 
            #endregion

            var app = builder.Build();

            #region Configure HTTP requests Pipeline
            app.MapCarter();
            app.UseExceptionHandler(options => { });
            app.Run(); 
            #endregion
        }
    }
}