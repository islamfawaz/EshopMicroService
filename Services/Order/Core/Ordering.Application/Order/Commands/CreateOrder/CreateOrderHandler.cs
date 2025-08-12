using BuildingBlocks.Messaging.Outbox;
using Ordering.Domain.Models;
using System.Text.Json;

namespace Ordering.Application.Order.Commands.CreateOrder
{
    internal class CreateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateOrderCommand, CreateOrderResult>
    {
        public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
        {


            var order = CreateNewOrder(command.Order);
            var integrationEvent = order.ToOrderDto();

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };


            var outBoxMessage = new OutboxMessage
            {
                Type = integrationEvent.GetType().AssemblyQualifiedName!,
                Content = JsonSerializer.Serialize(integrationEvent,serializerOptions),

                OccurredOnUtc = DateTime.UtcNow,
                 Id= Guid.NewGuid(),

    
            };

            dbContext.Orders.Add(order);
            dbContext.OutboxMessages.Add(outBoxMessage);


            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateOrderResult(order.Id.Value);
        }


        private Domain.Models.Order CreateNewOrder(OrderDto orderDto)
        {
            var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
            var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

            var newOrder = Domain.Models.Order.Create(
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(orderDto.CustomerId),
                OrderName.Of(orderDto.OrderName),
                shippingAddress,
                billingAddress,
                Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.ExpirationDate, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod)
            );

            foreach (var orderItemDto in orderDto.OrderItems)
            {
                newOrder.Add(ProductId.Of(orderItemDto.ProductId), orderItemDto.Quantity, orderItemDto.Price);
            }

            return newOrder;
        }
    }
}
