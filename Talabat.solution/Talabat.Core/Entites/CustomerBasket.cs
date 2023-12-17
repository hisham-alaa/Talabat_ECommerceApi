using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entites
{
    public class CustomerBasket
    {
        public CustomerBasket()
        {

        }
        public CustomerBasket(string basketId)
        {
            Id = basketId;
            Items = new List<BasketItem>();
        }

        public string Id { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public int? DeliveryId { get; set; }
        public decimal ShippingPrice { get; set; }

        public List<BasketItem> Items { get; set; }


    }
}
