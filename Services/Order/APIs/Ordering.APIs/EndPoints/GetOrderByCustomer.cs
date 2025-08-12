
using Ordering.Application.Order.Queries.GetOrderByCustomer;

namespace Ordering.APIs.EndPoints
{
    public record GetOrderByCustomerResponse(IEnumerable<OrderDto> Orders);
    public class GetOrderByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrderByCustomerQuery(customerId));
                var response=result.Adapt<GetOrderByCustomerResponse>();
                return Results.Ok(response);
            }).WithName("GetOrderByCustomer")
              .Produces<GetOrdersByNameResponse>()
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .ProducesProblem(StatusCodes.Status404NotFound)
              .WithDescription("Get Order by Customer Endpoint")
              .WithSummary("Get Order by Customer  Endpoint");
        }
    }
}
