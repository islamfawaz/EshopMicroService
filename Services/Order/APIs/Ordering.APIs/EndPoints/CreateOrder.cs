using Ordering.Application.Order.Commands.CreateOrder;

namespace Ordering.APIs.EndPoints
{
    public record CreateOrderRequest(OrderDto Order);

    public record CreateOrderResponse(Guid Id);
    public class CreateOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/orders", async (CreateOrderRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateOrderCommand>();
                var result = await sender.Send(command);
                var response = result.Adapt<CreateOrderResponse>();
                return Results.Created($"/orders/{response.Id}", response);
            })
                .WithName("CreateOrder")
                .WithSummary("Create a new order")
                .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
                .WithDescription("Creates a new order in the system. The order details are provided in the request body. On success, it returns the created order's ID.")
                .ProducesProblem(StatusCodes.Status400BadRequest);

        }
    }
}
