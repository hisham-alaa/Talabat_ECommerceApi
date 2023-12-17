using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }

        [Required]
        public int DeliveryId { get; set; }

        public AddressDto ShippingAddress { get; set; }
    }
}
