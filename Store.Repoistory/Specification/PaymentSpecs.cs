using Store.Data.Entity.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Specification
{
    public class PaymentSpecs : BaseSpecification<Order>
    {
        public PaymentSpecs(string? paymentintentid) : 
            base(order => order.PaymentIntentId == paymentintentid)
        {
        }
    }
}
