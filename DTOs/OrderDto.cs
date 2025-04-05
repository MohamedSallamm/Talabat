using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class OrderDto
    {

        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethodId { get; set; }
        [Required]
        public AddressDTO ShippingAddress { get; set; }
    }

}
