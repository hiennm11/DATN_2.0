using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Service
{
    public interface IClientOrderService
    {
        Task<int> PlaceOrder(ShoppingCart carts, OrderVM order);
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
        private readonly IMapper _mapper;

        public ClientOrderService(IUnitOfWork unitOfWork, IProductRepository productRepository, IDiscountRepository discountRepository
                , IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._productRepository = productRepository;
            this._discountRepository = discountRepository;
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<OrderVM>> GetOrder(string userEmail)
        {
            return _mapper.Map<IEnumerable<Order>, IEnumerable<OrderVM>>(await _orderRepository.GetOrdersByUserEmailAsync(userEmail));
        }

        public async Task<ClientOrderDetailResponse> GetOrderDetailCartView(int id)
        {
            var list = _mapper.Map<IEnumerable<OrderDetailVM>>(await _orderDetailRepository.GetDetailByOrderId(id));
            var order = _mapper.Map<OrderVM>(await _orderRepository.GetByOrderIdAsync(id));

            return new ClientOrderDetailResponse(list.ToList(), order);          
        }

        public async Task<int> PlaceOrder(ShoppingCart carts, OrderVM order)
        {
            var orderToAdd = _mapper.Map<OrderVM, Order>(order);
            orderToAdd.PaymentStatus = false;
            orderToAdd.CreatedDate = DateTime.Now;
            orderToAdd.CreatedBy = "admin";
            orderToAdd.Status = OrderStatus.Waiting;

            if (!string.IsNullOrEmpty(carts.CartPromoCode))
            {
                var discount = await _discountRepository.GetDiscountByPromoCode(carts.CartPromoCode);
                orderToAdd.Discount = discount;
            }

            await _orderRepository.AddAsync(orderToAdd);

            foreach(var item in carts.Cart)
            {
                var orderDetail = new OrderDetail() { OrderId = orderToAdd.OrderId, ProductId = item.Product.ProductId, Quantity = item.Quantity };                
                await _orderDetailRepository.AddAsync(orderDetail);

                var product = await _productRepository.GetProductByNameIDAsync(item.Product.NameAlias);
                product.Purchase += 1;
                await _productRepository.UpdateAsync(product);
            }
            
            return await _unitOfWork.SaveAsync();
        }
    }
}
