
namespace Basket.API.Basket.DeleteBasket
{
    public record DeleteBasketRequest(string UserName);

    public record  DeleteBasketResponse(bool IsSuccess);
    public class DeleteBasketEndPoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{userName}",async(string userName,ISender sender) =>
            {
                var result = await sender.Send(new DeleteBasketCommand(userName));
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            }).WithDescription("Delete a user's shopping basket")
              .WithName("DeleteBasket")
              .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .WithTags("Basket")
              .WithSummary("Delete Basket");    
        }
    }
}
