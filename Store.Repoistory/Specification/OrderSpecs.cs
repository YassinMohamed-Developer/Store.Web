using Store.Data.Entity.OrderEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repoistory.Specification
{
    public class OrderSpecs : BaseSpecification<Order>
    {
        public OrderSpecs(string BuyerEmail) : 
            base(order => order.BuyerEmail == BuyerEmail)
        {
            AddIncludes(order => order.DeliveryMethod);
            AddIncludes(order => order.OrderItems);
            AddOrderByDesc(order => order.OrderDate);
        }

       public OrderSpecs(Guid Id) :
     base(order => order.Id == Id)
        {
            AddIncludes(order => order.DeliveryMethod);
            AddIncludes(order => order.OrderItems);
        }
    }
}
