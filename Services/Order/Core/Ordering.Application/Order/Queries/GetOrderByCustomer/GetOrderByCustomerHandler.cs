namespace Ordering.Application.Order.Queries.GetOrderByCustomer
{
    public class GetOrderByCustomerHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrderByCustomerQuery, GetOrderByCustomerQueryResult>
    {
        public async Task<GetOrderByCustomerQueryResult> Handle(GetOrderByCustomerQuery query, CancellationToken cancellationToken)
        {
            var orders =await dbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.CustomerId == CustomerId.Of(query.CustomerId))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync();
            return new GetOrderByCustomerQueryResult(orders.ToOrderDtoList());
        }
    }
}
