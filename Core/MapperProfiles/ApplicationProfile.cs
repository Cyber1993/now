using AutoMapper;
using Core.Entities;
using Core.Models.Account;
using Core.Models.Address;
using Core.Models.Banner;
using Core.Models.Cart;
using Core.Models.Category;
using Core.Models.Comment;
using Core.Models.CreditCard;
using Core.Models.Order;
using Core.Models.Product;
using Core.Models.Promocode;
using Core.Models.PromoCode;
using Core.Models.User;

namespace Core.MapperProfiles
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, GetUserDto>().ReverseMap();
            CreateMap<User, RegisterDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();
            CreateMap<EditUserDto, User>().ReverseMap();

            CreateMap<Comment, GetCommentDto>().ReverseMap();
            CreateMap<Comment, CreateCommentDto>().ReverseMap();
            CreateMap<Comment, EditCommentDto>().ReverseMap();

            CreateMap<Category, GetCategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
            CreateMap<EditCategoryDto, Category>().ReverseMap();


            CreateMap<CreateCommentDto, Comment>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<EditCommentDto, Comment>()
                 .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Comment, GetCommentDto>().ReverseMap();

         


            CreateMap<CreateProductDto, Product>()
                 .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<EditProductDto, Product>()
                 .ForMember(dest => dest.Images, opt => opt.Ignore());
            CreateMap<Product, GetProductDto>().ReverseMap();

            CreateMap<Product, GetProductPreviewDto>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.OrderBy(img => img.Id).FirstOrDefault().Image))
                .ReverseMap();


            CreateMap<ProductImage, GetProductImageDto>().ReverseMap();
            CreateMap<CommentImage, GetCommnetImageDto>().ReverseMap();

            CreateMap<Cart, GetCartDto>().ReverseMap();
            CreateMap<Cart, CreateCartDto>().ReverseMap();

            CreateMap<CartItem, GetCartItemDto>().ReverseMap();

            CreateMap<Order, GetOrderDto>().ReverseMap();
            CreateMap<Order, CreateOrderDto>().ReverseMap();

            CreateMap<OrderItem, GetOrderItemDto>().ReverseMap();
            CreateMap<OrderItem, ListProductToOrder>().ReverseMap();

            CreateMap<CreditCard, GetCreditCardDto>().ReverseMap();
            CreateMap<CreditCard, CreateCreditCardDto>().ReverseMap();

            CreateMap<Address, GetAddressDto>().ReverseMap();
            CreateMap<Address, CreateAddressDto>().ReverseMap();
            CreateMap<Address, EditAddressDto>().ReverseMap();

            CreateMap<Banner, GetBannerDto>().ReverseMap();
            CreateMap<Banner, CreateBannerDto>().ReverseMap();
            CreateMap<Banner, EditBannerDto>().ReverseMap();

            CreateMap<PromoCode, GetPromoCodeDto>().ReverseMap();
            CreateMap<PromoCode, CreatePromoCodeDto>().ReverseMap();
            CreateMap<PromoCode, EditPromoCodeDto>().ReverseMap();

            CreateMap<CartItem, OrderItem>()
                .ForMember(dest => dest.OrderId, opt => opt.Ignore())
                .ForMember(dest => dest.Order, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}