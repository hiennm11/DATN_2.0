using AutoMapper;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Web.Infrastructure
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Product, ProductVM>().ReverseMap();
            CreateMap<AuthorDetail, AuthorDetailVM>().ReverseMap();
            CreateMap<Order, OrderVM>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailVM>().ReverseMap();
        }

        //public static MapperConfiguration config = new MapperConfiguration(cfg => {
        //    cfg.CreateMap<Category, CategoryVM>();
        //    cfg.CreateMap<Product, ProductVM>();
        //    cfg.CreateMap<AuthorDetail, AuthorDetailVM>();
        //    cfg.CreateMap<Order, OrderVM>();
        //    cfg.CreateMap<OrderDetail, OrderDetailVM>();
        //});

        //public static IMapper map = new Mapper(config);
    }
}