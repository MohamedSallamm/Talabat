﻿using Talabat.Core.Entities;

namespace Talabat.API.DTOs
{
    public class ProductToReturnDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }
        public int ProductBrandId { get; set; } 
        public string ProductBrand { get; set; }
        public int ProductTypeId { get; set; } 
        public string ProductType { get; set; }

    }
}
