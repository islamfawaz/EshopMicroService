
namespace Basket.API.Basket.StoreBasket
{
    public record StoreBasketRequest(ShoppingCart Cart);
    public record StoreBasketResponse(string UserName);
    public class StoreBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/basket/{userName}",async (StoreBasketRequest request,ISender Sender) =>
            {
                var command = request.Adapt<StoreBasketCommand>();
                var result = await Sender.Send(command);
                var response=result.Adapt<StoreBasketResponse>();
                return Results.Ok(response);
            }).WithDescription("Store a user's shopping basket")
              .WithName("StoreBasket")
              .Produces<StoreBasketResponse>(StatusCodes.Status200OK)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithTags("Basket")
              .WithSummary("Store Basket"); 
        }
    }
}
