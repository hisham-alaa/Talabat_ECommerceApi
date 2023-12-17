using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Sepecifications
{
    public class OrderWithPaymentIntentSpecs : BaseSpecification<Order>
    {
        public OrderWithPaymentIntentSpecs(string paymentInetntId)
            : base(o => o.PaymentIntentId == paymentInetntId)
        {

        }
    }
}
