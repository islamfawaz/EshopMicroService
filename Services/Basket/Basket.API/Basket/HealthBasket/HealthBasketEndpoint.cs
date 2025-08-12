namespace Basket.API.Basket.HealthBasket
{
    public record HealthBasketResponse(string Status, string Service, DateTime CheckedAt);

    public class HealthBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/health", (ILogger<HealthBasketEndpoint> logger) =>
            {
                logger.LogInformation("=== BASKET HEALTH CHECK ===");
                
                var response = new HealthBasketResponse(
                    Status: "Healthy",
                    Service: "Basket API",
                    CheckedAt: DateTime.UtcNow
                );

                return Results.Ok(response);
            })
            .WithDescription("Health check for basket service")
            .WithName("HealthBasket")
            .Produces<HealthBasketResponse>(StatusCodes.Status200OK)
            .WithTags("Basket")
            .WithSummary("Basket Health Check");
        }
    }
} 
