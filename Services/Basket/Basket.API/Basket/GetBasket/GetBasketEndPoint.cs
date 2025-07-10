
namespace Basket.API.Basket.GetBasket
{
   // public record GetProductRequest(string UserName);
   public record GetBasketResponse(ShoppingCart Cart);
    public class GetBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {

            app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
            {
                Console.WriteLine($"*** GET /basket/{userName} endpoint HIT! ***"); // Add this
                var result = await sender.Send(new GetBasketQuery(userName));
                var response = result.Adapt<GetBasketResponse>();
                return Results.Ok(response);
            })
            .WithDescription("Get a user's shopping basket")
            .WithName("GetBasket")
            .Produces<GetBasketResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Basket");
        }
    }
} 
