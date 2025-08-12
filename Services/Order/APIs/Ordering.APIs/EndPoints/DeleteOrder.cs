
using Ordering.Application.Order.Commands.DeleteOrder;

namespace Ordering.APIs.EndPoints
{
    public record DeleteOrderRequest(OrderDto Order);
    public record DeleteOrderResponse(bool IsSuccess);

    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/orders/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(Id));
                var response = result.Adapt<DeleteOrderResponse>();
                return Results.Ok(response);
            }).WithName("Order - Delete")
              .Produces<DeleteOrderResponse>()
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .ProducesProblem(StatusCodes.Status404NotFound)
              .WithDescription("Order Delete Endpoint")
              .WithSummary("Order Delete Endpoint");
        }
    }
}
