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

            //// Configure Kestrel for HTTP/2 without TLS (for development)
            //builder.WebHost.ConfigureKestrel(options =>
            //{
            //    options.ListenLocalhost(5052, listenOptions =>
            //    {
            //        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
            //    });
            //});

            // Configure Kestrel for Docker gRPC
            builder.WebHost.ConfigureKestrel(serverOptions =>
            {
                serverOptions.ListenAnyIP(8081, listenOptions =>
                {
                    listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http2;
                });
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