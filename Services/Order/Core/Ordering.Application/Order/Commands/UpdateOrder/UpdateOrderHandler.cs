
namespace Ordering.Application.Order.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId=OrderId.Of(command.Order.Id);
            var order = await dbContext.Orders
                .FindAsync(orderId, cancellationToken);
            if (order == null) throw new OrderNotFoundException(command.Order.Id);

            UpdateOrderWithItsValue(order,command.Order);

            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new UpdateOrderResult(true);
        }

        private void UpdateOrderWithItsValue(Domain.Models.Order order,OrderDto orderDto)
        {
            var UpdatedShippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
            var UpdatedBillingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
            var UpdatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.ExpirationDate, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);
            order.Update(OrderName.Of(orderDto.OrderName), UpdatedShippingAddress,UpdatedBillingAddress, UpdatedPayment, orderDto.Status);
                
            


        }

    }
}
