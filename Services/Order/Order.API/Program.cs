namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.

            var app = builder.Build();

            // Configure the HTTP request pipeline.



            app.Run();
        }
    }
}
