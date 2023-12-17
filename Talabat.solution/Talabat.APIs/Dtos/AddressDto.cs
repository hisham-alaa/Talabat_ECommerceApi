using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.Dtos
{
    public class AddressDto
    {
        [Required]
        public string Fname { get; set; }

        [Required]
        public string Lname { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }
    }
}