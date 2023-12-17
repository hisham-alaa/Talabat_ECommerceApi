using System.Text.Json.Serialization;

namespace Talabat.Core.Entites.Identity
{
    public class Address
    {
        public int id { get; set; }
        public string Fname { get; set; }
        public string Lname { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string AppUserId { get; set; }

        //[JsonIgnore]
        public AppUser User { get; set; }
    }
}