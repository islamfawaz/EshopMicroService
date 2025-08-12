
namespace Ordering.Application.Order.Commands.DeleteOrder
{
    internal class DeleteOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
    {
        public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
        {
            OrderId? orderId = OrderId.Of(command.OrderId);
            var order =await dbContext.Orders.FindAsync(orderId,cancellationToken);
            if (order is null) throw new OrderNotFoundException(command.OrderId);
            dbContext.Orders.Remove(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteOrderResult(true);

        }
    }
}
