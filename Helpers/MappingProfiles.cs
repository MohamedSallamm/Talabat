using AutoMapper;
using Talabat.API.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using IdentityAddress = Talabat.Core.Entities.Identity.Address;
using OrderAddress = Talabat.Core.Entities.Order_Aggregate.Address;

namespace Talabat.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductTypeId, O => O.MapFrom(S => S.ProductCategory.Id)) // Ensure Id is used
                .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());

            CreateMap<IdentityAddress, AddressDTO>().ReverseMap();
            CreateMap<AddressDTO, OrderAddress>();
            CreateMap<CustomerBasketDTO, CustomerBasket>();

            CreateMap<CustomerBasket, CustomerBasketDTO>().ReverseMap();
            CreateMap<BasketItemDTO, BasketItem>();


            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName, O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PicturerUrl, O => O.MapFrom<OrderITemPictureUrlReslover>()); // Corrected spelling
        }
    }
}
