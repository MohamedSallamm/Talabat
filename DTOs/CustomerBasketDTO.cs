﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.API.DTOs
{
    public class CustomerBasketDTO
    {
        [Required]
        public string Id { get; set; }
        public List<BasketItemDTO> Items { get; set; }
        public string? PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }



    }
}
