using Ordering.Application.Order.Queries.GetOrderByName;

namespace Ordering.APIs.EndPoints
{
    public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

    public class GetOrdersByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/{orderName}", async (string orderName, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByNameQuery(orderName));
                var response = result.Adapt<GetOrdersByNameResponse>();
                return  Results.Ok(response);
            }).WithName("Get order")
              .Produces<GetOrdersByNameResponse>()
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .ProducesProblem(StatusCodes.Status404NotFound)
              .WithDescription("Get Order by Name Endpoint")
              .WithSummary("Get Order by Name Delete Endpoint");
        }
    }
}
