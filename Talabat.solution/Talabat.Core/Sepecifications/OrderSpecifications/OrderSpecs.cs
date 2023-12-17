using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Sepecifications.OrderSpecifications
{
    public class OrderSpecs : BaseSpecification<Order>
    {
        public OrderSpecs(string email) :
            base(o => o.BuyerEmail == email)
        {
            ObjsToInclude.Add(o => o.DeliveryMethod);
            ObjsToInclude.Add(o => o.Items);

            AddOrderByDesc(o => o.OrderDate);
        }

        public OrderSpecs(int orderId, string email)
            : base(o => o.BuyerEmail == email && o.Id == orderId)
        {
            ObjsToInclude.Add(o => o.DeliveryMethod);
            ObjsToInclude.Add(o => o.Items);
        }
    }
}
