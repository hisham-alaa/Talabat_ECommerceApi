using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entites;

namespace Talabat.APIs.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
        public string Id { get; set; }
        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryId { get; set; }
        public decimal ShippingPrice { get; set; }
        public List<BasketItemDto> Items { get; set; }
    }
}
