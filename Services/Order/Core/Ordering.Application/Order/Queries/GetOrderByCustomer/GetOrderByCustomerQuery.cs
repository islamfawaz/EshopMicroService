namespace Ordering.Application.Order.Queries.GetOrderByCustomer
{
    public record GetOrderByCustomerQuery(Guid CustomerId) :IQuery<GetOrderByCustomerQueryResult>;

    public record GetOrderByCustomerQueryResult(IEnumerable<OrderDto> Orders);



}
