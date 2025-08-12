namespace Ordering.Domain.Models
{
    public class Order : Aggregate<OrderId>
    {
        private readonly List<OrderItem> _OrderItems = new();

        public IReadOnlyCollection<OrderItem> OrderItems => _OrderItems.AsReadOnly();
        public CustomerId CustomerId { get; private set; }
        public OrderName OrderName { get; private set; } = default!;
        public Address ShippingAddress { get; private set; } = default!;
        public Address BillingAddress { get; private set; } = default!;
        public Payment Payment { get; private set; } = default!;
        public OrderStatus Status { get; private set; } = OrderStatus.Pending;

        public decimal TotalPrice
        {
            get => OrderItems.Sum(x => x.Price * x.Quantity);
            private set { }
        }

        public static Order Create(OrderId id, CustomerId customerId, OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment)
        {
            ArgumentNullException.ThrowIfNull(orderName);
            ArgumentNullException.ThrowIfNull(shippingAddress);
            ArgumentNullException.ThrowIfNull(billingAddress);
            ArgumentNullException.ThrowIfNull(payment);
            var order = new Order
            {
                Id = id,
                CustomerId = customerId,
                OrderName = orderName,
                ShippingAddress = shippingAddress,
                BillingAddress = billingAddress,
                Payment = payment,
                Status = OrderStatus.Pending
            };
            order.AddDomainEvent(new OrderCreatedEvent(order));
            return order;
        }

        public void Update(OrderName orderName, Address shippingAddress, Address billingAddress, Payment payment,OrderStatus status)
        {
            OrderName= orderName;
            ShippingAddress= shippingAddress;
            BillingAddress= billingAddress;
            Status = status;
            Payment = payment;
            AddDomainEvent(new OrderUpdatedEvent(this));
        }

        public void Add(ProductId productId,int quantity,decimal price)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

            var orderItem= new OrderItem(Id, productId, quantity, price);
            _OrderItems.Add(orderItem);  
        }
        public void Remove(ProductId productId)
        {
            var orderItem = _OrderItems.FirstOrDefault(x => x.ProductId == productId);
            if (orderItem != null)
            {
                _OrderItems.Remove(orderItem);
            }
        }
    }
}
