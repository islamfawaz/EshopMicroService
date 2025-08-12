namespace Basket.API.Basket.StatusBasket
{
    public record StatusBasketResponse(bool IsHealthy, string ServiceName, DateTime Timestamp);

    public class StatusBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/basket/status", (ILogger<StatusBasketEndpoint> logger) =>
            {
                logger.LogInformation("=== BASKET STATUS ENDPOINT ===");
                
                var response = new StatusBasketResponse(
                    IsHealthy: true,
                    ServiceName: "Basket API",
                    Timestamp: DateTime.UtcNow
                );

                return Results.Ok(response);
            })
            .WithDescription("Get basket service status")
            .WithName("StatusBasket")
            .Produces<StatusBasketResponse>(StatusCodes.Status200OK)
            .WithTags("Basket")
            .WithSummary("Basket Service Status");
        }
    }
} 
