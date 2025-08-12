
namespace Ordering.Domain.ValueObjects
{
    public record CustomerId
    {
        private CustomerId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get;  }

      
        public static CustomerId Of(Guid value)
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value == Guid.Empty)
            {
                throw new DomainException("CustomerId cannot be empty.");
            }
            return new CustomerId(value);
        }
    }
}
