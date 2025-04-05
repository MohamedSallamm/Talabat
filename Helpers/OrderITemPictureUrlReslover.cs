using AutoMapper;
using Talabat.API.DTOs;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.API.Helpers
{
    public class OrderITemPictureUrlReslover : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _configuration;

        public OrderITemPictureUrlReslover(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {

            if (!string.IsNullOrEmpty(source.Product.PictureUrl))
                return $"{_configuration["ApiBaseUrl"]}{source.Product.PictureUrl}";
            return string.Empty;

        }
    }
}
