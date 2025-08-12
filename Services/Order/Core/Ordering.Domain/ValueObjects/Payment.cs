namespace Ordering.Domain.ValueObjects
{
    public record Payment
    {
        protected Payment()
        {
            
        }

        private Payment(string? cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            CardName = cardName;
            CardNumber = cardNumber;
            Expiration = expiration;
            Cvv = cvv;
            PaymentMethod = paymentMethod;
        }

        public string ? CardName { get;  }
        public string CardNumber { get; } = default!;
        public string Expiration { get; } = default!;

        public string Cvv { get; } = default!;
        public int PaymentMethod { get;}

        public static Payment Of(string? cardName, string cardNumber, string expiration, string cvv, int paymentMethod)
        {
            ArgumentNullException.ThrowIfNull(cardNumber);
            ArgumentNullException.ThrowIfNull(expiration);
            ArgumentNullException.ThrowIfNull(cvv);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(cvv.Length,3);

            return new Payment(cardName, cardNumber, expiration, cvv, paymentMethod);
        }

    }
}
