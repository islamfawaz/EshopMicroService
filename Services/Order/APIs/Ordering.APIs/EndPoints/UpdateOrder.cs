using Ordering.Application.Order.Commands.UpdateOrder;

namespace Ordering.APIs.EndPoints
{
   public record UpdateOrderRequest(OrderDto Order);
   public record UpdateOrderResponse(bool IsSuccess);

    public class UpdateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/orders", async (UpdateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateOrderCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<UpdateOrderResponse>();

                return Results.Ok(response);
            })
                .WithName("UpdateOrder")
                .WithSummary("Update an existing order")
                .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithSummary("Update an existing order")
                .WithDescription("This endpoint allows you to update an existing order by providing the order details. " +
                    "The order must already exist in the system. " +
                    "If the update is successful, it returns a success response with a status code of 200 OK.");

        }
    }
}
