using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entites.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {

        Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, Address ShippingAddress, int DeliveryMethodId);

        Task<IReadOnlyList<Order?>> GetOrdersForUserAsync(string BuyerEmail);

        Task<Order> GetOrderForUserAsync(int orderId, string BuyerEmail);
        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
