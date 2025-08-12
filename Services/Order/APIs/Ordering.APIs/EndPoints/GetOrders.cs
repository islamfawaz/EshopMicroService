using BuildingBlocks.Pagination;
using Ordering.Application.Order.Queries.GetOrders;

namespace Ordering.APIs.EndPoints
{
    public record GetOrderResponse(PaginatedResult<OrderDto> Orders);

    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters]PaginationRequest request, ISender sender) =>
            {
                var result=await sender.Send(new GetOrdersQuery(request));
                GetOrderResponse? response = result.Adapt<GetOrderResponse>();
                return Results.Ok(response);
            }).Produces<DeleteOrderResponse>()
              .ProducesProblem(StatusCodes.Status400BadRequest)
              .ProducesProblem(StatusCodes.Status404NotFound)
              .WithDescription("Get Orders  Endpoint")
              .WithSummary("Get Orders  Endpoint");
        }
    }
}
