using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Data.Repositories.Interfaces;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Service
{
    public interface IClientOrderService
    {
        Task<int> PlaceOrder(ShoppingCart carts, OrderVM order, string userEmail);
        Task<IEnumerable<OrderVM>> GetOrder(string userEmail);
        Task<ClientOrderDetailResponse> GetOrderDetailCartView(int id);
    }

    public class ClientOrderService : IClientOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IDiscountRepository _discountRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IProductRankRepository _productRankRepository;
        private readonly IMapper _mapper;

        public ClientOrderService(IUnitOfWork unitOfWork, IProductRepository productRepository, IDiscountRepository discountRepository, 
                                  IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, 
                                  IApplicationUserRepository applicationUserRepository, IProductRankRepository productRankRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._productRepository = productRepository;
            this._discountRepository = discountRepository;
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._applicationUserRepository = applicationUserRepository;
            this._productRankRepository = productRankRepository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<OrderVM>> GetOrder(string userEmail)
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(await _orderRepository.GetOrdersByUserAsync(userEmail));
        }

        public async Task<ClientOrderDetailResponse> GetOrderDetailCartView(int id)
        {
            var list = _mapper.Map<IEnumerable<OrderDetailVM>>(await _orderDetailRepository.GetDetailByOrderId(id));
            var order = _mapper.Map<OrderVM>(await _orderRepository.GetByOrderIdAsync(id));

            return new ClientOrderDetailResponse(list.ToList(), order);          
        }

        public async Task<int> PlaceOrder(ShoppingCart carts, OrderVM order, string userEmail)
        {
            
            var orderToAdd = _mapper.Map<OrderVM, Order>(order);
            orderToAdd.PaymentStatus = false;
            orderToAdd.CreatedDate = DateTime.Now;
            orderToAdd.CreatedBy = "admin";
            orderToAdd.Status = OrderStatus.Waiting;

            if (!string.IsNullOrEmpty(userEmail))
            {
                var user = _applicationUserRepository.GetUserByUserName(userEmail);
                orderToAdd.User = user;
            }

            if (!string.IsNullOrEmpty(carts.CartPromoCode))
            {
                var discount = await _discountRepository.GetDiscountByPromoCode(carts.CartPromoCode);
                orderToAdd.Discount = discount;
            }
            else
            {
                var discount = await _discountRepository.GetDiscountById(1);
                orderToAdd.Discount = discount;

            }

            await _orderRepository.AddAsync(orderToAdd);

            foreach(var item in carts.Cart)
            {
                var product = await _productRepository.GetSingleByIDAsync(item.Product.ProductId);
                if(product.AvailableQuantity >= item.Quantity)
                {
                    var orderDetail = new OrderDetail() { OrderId = orderToAdd.OrderId, ProductId = item.Product.ProductId, Quantity = item.Quantity };
                    await _orderDetailRepository.AddAsync(orderDetail);

                    //Update product sold
                    var rank = await _productRankRepository.GetSingleByIDAsync(item.Product.ProductId);
                    rank.Sold += item.Quantity;
                    await _productRankRepository.UpdateAsync(rank);

                    product.AvailableQuantity -= item.Quantity;
                    await _productRepository.UpdateAsync(product);
                }
                else
                {
                    return 0;
                }
                
            }
            
            return await _unitOfWork.SaveAsync();
        }
    }
}
