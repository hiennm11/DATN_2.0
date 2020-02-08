using AutoMapper;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Web.Models;

namespace WebBanSach_2_0.Web.Infrastructure
{
    public class AutoMapperConfiguration
    {
        public static MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.CreateMap<Category, CategoryVM>();
            cfg.CreateMap<Product, ProductVM>();
            });

        public static IMapper map = new Mapper(config);
    }
}