namespace Ordering.Domain.Models
{
    public class Customer :Entity<CustomerId>
    {
        public string Name { get; private set; } = default!;
        public string Email { get; private set; } = default!;

        public static Customer Create(CustomerId id, string name, string email)
        {
            ArgumentException.ThrowIfNullOrEmpty(name);
            ArgumentException.ThrowIfNullOrEmpty(email);
            var customer = new Customer
            {
                Id = id,
                Name = name,
                Email = email,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "System"
            };
            return customer;
           
        }
    }
}
