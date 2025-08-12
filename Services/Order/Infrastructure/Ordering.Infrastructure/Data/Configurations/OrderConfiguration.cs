using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Enums;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o=>o.Id);

            builder.Property(o => o.Id)
                .HasConversion(orderId => orderId.Value,
                               dbId => OrderId.Of(dbId));

            builder.HasOne<Customer>()
                .WithMany()
                .HasForeignKey(o => o.CustomerId);

            builder.HasMany(o=>o.OrderItems)
                .WithOne()
                .HasForeignKey(oi => oi.OrderId);

            builder.ComplexProperty(
                o => o.OrderName, nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Order.OrderName))
                    .HasMaxLength(100)
                    .IsRequired();
                });

            builder.ComplexProperty(
                o => o.ShippingAddress, addressBuilder =>
                {
                    addressBuilder.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                    addressBuilder.Property(a => a.LastName)
                   .IsRequired()
                   .HasMaxLength(50);


                    addressBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);
                    addressBuilder.Property(a => a.AddressLine)
                    .IsRequired()
                    .HasMaxLength(180);

                    addressBuilder.Property(a => a.Country)
                   .HasMaxLength(50);
                    addressBuilder.Property(a => a.State)
                  .HasMaxLength(50);

                    addressBuilder.Property(a => a.ZipCode)
                  .HasMaxLength(5)
                  .IsRequired();
                });
            builder.ComplexProperty(o=>o.BillingAddress, addressBuilder =>
                {
                    addressBuilder.Property(a => a.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);
                    addressBuilder.Property(a => a.LastName)
                   .IsRequired()
                   .HasMaxLength(50);
                    addressBuilder.Property(a => a.EmailAddress)
                    .HasMaxLength(50);
                    addressBuilder.Property(a => a.AddressLine)
                    .IsRequired()
                    .HasMaxLength(180);
                    addressBuilder.Property(a => a.Country)
                   .HasMaxLength(50);
                    addressBuilder.Property(a => a.State)
                  .HasMaxLength(50);
                    addressBuilder.Property(a => a.ZipCode)
                  .HasMaxLength(5)
                  .IsRequired();
                });
            builder.ComplexProperty(o=>o.Payment, paymentBuilder =>
                {
                    paymentBuilder.Property(p => p.CardName)
                    .IsRequired()
                    .HasMaxLength(50);
                    paymentBuilder.Property(p => p.CardNumber)
                    .IsRequired()
                    .HasMaxLength(24);
                    paymentBuilder.Property(p => p.Expiration)
                    .IsRequired()
                    .HasMaxLength(10);
                    paymentBuilder.Property(p => p.Cvv)
                    .IsRequired()
                    .HasMaxLength(3);
                    paymentBuilder.Property(p => p.PaymentMethod);
                });

            builder.Property(o => o.Status)
                .HasDefaultValue(OrderStatus.Draft)
                 .HasSentinel(OrderStatus.Draft)
                .HasConversion(
                    status => status.ToString(),
                    dbStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), dbStatus));

            builder.Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");



        }
    }
}
