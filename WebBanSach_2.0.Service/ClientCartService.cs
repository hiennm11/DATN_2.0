using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Repositories.Interfaces;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Service
{
    public interface IClientCartService
    {
        Task<ClientCartResponse> AddCodeToCart(List<CartItem> carts, string code);
        List<CartItem> AddToCart(List<CartItem> carts, string nameId, int quantity);
        List<CartItem> UpdateCart(List<CartItem> carts, string nameId, int quantity);
        List<CartItem> DeleteItem(List<CartItem> carts, string id);
        ClientCartResponse GetCheckoutModel(ShoppingCart carts, string userId);

    }
    public class ClientCartService : IClientCartService
    {
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IMapper _mapper;

        public ClientCartService(IApplicationUserRepository applicationUserRepository, 
                                 IProductRepository productRepository, IDiscountRepository discountRepository, IMapper mapper)
        {
            this._applicationUserRepository = applicationUserRepository;
            this._productRepository = productRepository;
            this._discountRepository = discountRepository;
            this._mapper = mapper;
        }

        public async Task<ClientCartResponse> AddCodeToCart(List<CartItem> carts, string code)
        {
            var temp = await _discountRepository.GetDiscountByPromoCode(code);
            var cartPrice = carts.Sum(m => m.Product.Price * (100 - m.Product.Discount.DiscountValue) / 100 * m.Quantity);

            var response = new ClientCartResponse
            {
                Cart = carts,
                TotalPrice = cartPrice
            };

            if (temp != null)
            {
                response.PromoCode = temp.DiscountCode;
                response.TotalPrice = cartPrice;
                response.CodePriceBonus = cartPrice * (100 - temp.DiscountValue) / 100;
                return response;
            }
            return response;
        }

        public List<CartItem> AddToCart(List<CartItem> carts, string nameId, int quantity)
        {
            var product = _mapper.Map<Product, ProductVM>(_productRepository.GetProductByNameID(nameId));
            if (product != null)
            {
                if (carts == null || carts.Count == 0)
                {
                    carts.Add(new CartItem { Product = product, Quantity = quantity });
                }
                else
                {
                    int index = isExist(carts, nameId);
                    if (index != -1)
                    {
                        carts[index].Quantity++;
                    }
                    else
                    {
                        carts.Add(new CartItem { Product = product, Quantity = quantity });
                    }
                }
            }
            return carts;
        }

        public List<CartItem> DeleteItem(List<CartItem> carts, string id)
        {
            int index = isExist(carts, id);
            carts.RemoveAt(index);
            return carts;
        }

        public ClientCartResponse GetCheckoutModel(ShoppingCart carts, string userId)
        {
            var orderInfo = new OrderVM();
            if (userId != null)
            {
                var temp = _applicationUserRepository.GetUserByUserName(userId);
                if(temp != null)
                {
                    orderInfo = new OrderVM()
                    {
                        CustomerName = temp.FullName,
                        CustomerAddress = temp.Address,
                        CustomerEmail = temp.Email,
                        CustomerMobile = temp.PhoneNumber
                    };
                }                
            }

            return new ClientCartResponse()
            {
                Cart = carts.Cart,
                TotalPrice = carts.Cart.Sum(m => m.Product.Price * (100 - m.Product.Discount.DiscountValue) / 100 * m.Quantity),
                PromoCode = carts.CartPromoCode,
                OrderInfo = orderInfo
            };
        }

        public List<CartItem> UpdateCart(List<CartItem> carts, string nameId, int quantity)
        {
            int index = isExist(carts, nameId);
            carts[index].Quantity = quantity;
            return carts;
        }

        private int isExist(List<CartItem> carts, string id)
        {
            for (int i = 0; i < carts.Count; i++)
            {
                if (carts[i].Product.NameAlias.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
