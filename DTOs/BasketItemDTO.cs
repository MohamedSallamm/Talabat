using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class BasketItemDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage ="Price Cannot be Zero")]
        public decimal Price { get; set; }
        [Required]
        public string Brand { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        [Range (1, int.MaxValue, ErrorMessage ="Qauntity Must Be one Item atleast")]
        public int Quantity { get; set; }
    }
}
