namespace Ordering.Application.Order.Queries.GetOrderByName
{
  public  record  GetOrdersByNameQuery(string Name)
        : IQuery<GetOrdersByNameQueryResult>;

    public record GetOrdersByNameQueryResult(IEnumerable<OrderDto> Orders);

 }
