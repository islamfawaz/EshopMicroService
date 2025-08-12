using Ordering.Application.Extensions;

namespace Ordering.Application.Order.Queries.GetOrderByName
{
    public class GetOrderByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameQueryResult>
    {
        public async Task<GetOrdersByNameQueryResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
        {
            var orders =await dbContext.Orders
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.OrderName.Value.Contains(query.Name))
                .OrderBy(o => o.OrderName.Value)
                .ToListAsync(cancellationToken);

            var orderDto =orders.ToOrderDtoList(); 

            return new GetOrdersByNameQueryResult(orderDto);
        }
    }
}
