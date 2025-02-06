using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Specifiations.Order_Spec
{
    public class OrderWithPaymentIntentIdSpec:BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentIdSpec(string PaymentIntentId)
            : base(o => o.PaymentIntentId == PaymentIntentId)
        {
            
        }
    }
}
