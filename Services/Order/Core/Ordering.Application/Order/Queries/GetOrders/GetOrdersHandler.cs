using BuildingBlocks.Pagination;

namespace Ordering.Application.Order.Queries.GetOrders
{
    internal class GetOrdersHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersQueryResult>
    {
        public async Task<GetOrdersQueryResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
        {
            int pageIndex = query.PaginationRequest.PageIndex;
            int pageSize = query.PaginationRequest.PageSize;
            long count = await dbContext.Orders.LongCountAsync();
            var orders = await dbContext.Orders
                .Include(o => o.OrderItems)
                .OrderBy(o => o.OrderName.Value)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new GetOrdersQueryResult(new PaginatedResult<OrderDto> 
            ( 
                pageIndex,
                pageSize,
                count,
                orders.ToOrderDtoList()
            ));


        }
    }
}
