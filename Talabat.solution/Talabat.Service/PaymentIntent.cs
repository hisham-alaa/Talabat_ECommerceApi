using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;
using Talabat.Core.Reporitories.Contract;
using Talabat.Core.Sepecifications;
using Talabat.Core.Services.Contract;
using Product = Talabat.Core.Entites.Product;

namespace Talabat.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IBasketRepository basketRepo,
            IConfiguration configuration,
            IUnitOfWork unitOfWork)
        {
            _basketRepo = basketRepo;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket> SetPaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);

            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryId.Value);
                shippingPrice = basket.ShippingPrice = deliveryMethod.Cost;
            }

            if (basket.Items.Count > 0)
            {
                var products = _unitOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                    var product = await products.GetAsync(item.Id);
                    if (item.Price != product.Price)
                        item.Price = product.Price;
                }
            }

            PaymentIntent paymentIntent;

            var paymentIntentService = new PaymentIntentService();


            if (string.IsNullOrEmpty(basket.PaymentIntentId)) /// Creating PaymentIntent
            {
                var createIntentOptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(createIntentOptions);

                basket.PaymentIntentId = paymentIntent.Id;

                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else /// The paymentintent Existed but we will update it
            {
                var updateIntentOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * item.Quantity * 100) + (long)shippingPrice * 100
                };

                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateIntentOptions);
            }

            await _basketRepo.SetBasketAsync(basket);

            return basket;
        }

        public async Task<Order> UpdatePaymentStatus(string PaymentIntentId, bool isPaid)
        {
            var specs = new OrderWithPaymentIntentSpecs(PaymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetAsyncWithSpec(specs);

            order.Status = isPaid ? OrderStatus.PaymentSucceded : OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}
