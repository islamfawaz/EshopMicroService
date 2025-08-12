using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.ValueObjects
{
    
    public record OrderId
    {
        private OrderId(Guid value)
        {
            Value = value;
        }
        public Guid Value { get;  }

        public static OrderId Of(Guid value)
        {
            if (value == Guid.Empty)
            {
                throw new ArgumentException("OrderId cannot be empty", nameof(value));
            }
            return new OrderId(value);
        }

    }
   

}
