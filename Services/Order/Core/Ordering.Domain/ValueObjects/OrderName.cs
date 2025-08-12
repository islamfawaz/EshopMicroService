namespace Ordering.Domain.ValueObjects
{
    public record OrderName
    {
        private OrderName(string value)
        {
            Value = value;
        }
        private const int DefaultLength = 5;


        public string Value { get;  }

        public static OrderName Of(string value)
        {
            ArgumentNullException.ThrowIfNull(value);
         //   ArgumentOutOfRangeException.ThrowIfNotEqual(value.Length, DefaultLength);
            return new OrderName(value);
        }
    }
}
