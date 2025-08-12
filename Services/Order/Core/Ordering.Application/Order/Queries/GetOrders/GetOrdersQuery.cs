using BuildingBlocks.Pagination;

namespace Ordering.Application.Order.Queries.GetOrders
{
    public record GetOrdersQuery(PaginationRequest PaginationRequest) :IQuery<GetOrdersQueryResult>;

    public record GetOrdersQueryResult(PaginatedResult<OrderDto> Orders);

}
