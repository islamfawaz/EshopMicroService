using DicountGrpc.Data;
using DicountGrpc.Services;
using Microsoft.EntityFrameworkCore;

namespace DicountGrpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddGrpcReflection();

            builder.Services.AddDbContext<DiscountDbContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("DataBase"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMigrations();
           app.MapGrpcService<DiscountService>();
            app.MapGrpcReflectionService();

            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}
