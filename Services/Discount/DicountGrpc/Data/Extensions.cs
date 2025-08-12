using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DicountGrpc.Data
{
    public static class Extensions
    {
        public static  IApplicationBuilder UseMigrations(this IApplicationBuilder app)
        {
            using var scope =  app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();

            context.Database.MigrateAsync();
            return app;


        }
    }
}
