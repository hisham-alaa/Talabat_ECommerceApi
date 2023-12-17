using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entites;
using Talabat.Core.Entites.Order_Aggregate;

using UserAddress = Talabat.Core.Entites.Identity.Address;
using OrderAddress = Talabat.Core.Entites.Order_Aggregate.Address;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles(/*IConfiguration config*/)
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, o => o.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                //.ForMember(d =d.PictureUrl, o => o.MapFrom(s => $"{config["ApiBaseUrl"]}/{s.PictureUrl}"));
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<AddressDto, OrderAddress>();

            CreateMap<UserAddress, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>())
                .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId));


        }
    }
}
