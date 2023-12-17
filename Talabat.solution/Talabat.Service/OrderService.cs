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
using Talabat.Core.Sepecifications.OrderSpecifications;
using Talabat.Core.Services.Contract;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        ///private readonly IGenericRepository<Product> _productRepository;
        ///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepository;
        ///private readonly IGenericRepository<Order> _orderRepository;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            ///IGenericRepository<Product> productRepository,
            ///IGenericRepository<DeliveryMethod> deliveryMethodRepository,
            ///IGenericRepository<Order> orderRepository)
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            ///_productRepository = productRepository;
            ///_deliveryMethodRepository = deliveryMethodRepository;
            ///_orderRepository = orderRepository;
        }
        public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, Address shippingAddress, int deliveryMethodId)
        {
            //Get the basket through its id
            var basket = await _basketRepository.GetBasketAsync(basketId);


            //Get selected items in this basket
            var orderItems = new List<OrderItem>();

            if (basket?.Items?.Count > 0)
            {
                var productRepo = _unitOfWork.Repository<Product>();
                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);
                    var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);
                    orderItems.Add(orderItem);
                }
            }

            //Calculate the subtotal cost
            var subtotal = orderItems.Sum(item => item.Quantity * item.Price);


            //Get Delivery Method through its id
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);

            var orders = _unitOfWork.Repository<Order>();

            var specs = new OrderWithPaymentIntentSpecs(basket.PaymentIntentId);

            var existedOrder = await orders.GetAsyncWithSpec(specs);

            if (existedOrder is not null)
            {
                orders.Delete(existedOrder);
                await _paymentService.SetPaymentIntent(basketId);
            }

            // Create Order
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subtotal, basket.PaymentIntentId);

            await orders.AddAsync(order);


            //Save changes to the database Which means we need the IUnitOfWork
            var res = await _unitOfWork.CompleteAsync();

            return res <= 0 ? null : order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string BuyerEmail)
        {
            var orderRepo = _unitOfWork.Repository<Order>();

            var orderSpecs = new OrderSpecs(BuyerEmail);

            var orders = await orderRepo.GetAllAsyncWithSpec(orderSpecs);

            return orders;
        }

        public async Task<Order?> GetOrderForUserAsync(int orderId, string BuyerEmail)
            => await _unitOfWork.Repository<Order>().GetAsyncWithSpec(new OrderSpecs(orderId, BuyerEmail));

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethodsRepo = _unitOfWork.Repository<DeliveryMethod>();

            var deliveryMethods = await deliveryMethodsRepo.GetAllAsync();

            return deliveryMethods;
        }
    }
}
