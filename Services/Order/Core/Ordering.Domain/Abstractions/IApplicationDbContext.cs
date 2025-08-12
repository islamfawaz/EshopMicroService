using BuildingBlocks.Messaging.Outbox;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Domain.Abstractions
{
    public interface IApplicationDbContext
    {
        public DbSet<Customer> Customers { get; }
        public DbSet<Product> Products { get; }
        public DbSet<Order> Orders { get; }
        public DbSet<OrderItem> OrderItems { get; }
        DbSet<OutboxMessage> OutboxMessages { get; } 


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
