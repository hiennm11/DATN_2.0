using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Infrastructure;
using WebBanSach_2_0.Service.Models;

namespace WebBanSach_2_0.Service.Controllers
{
    public class CartController : Controller
    {
        private const string cartSession = "CartSession";
        private readonly IUnitOfWork _unitOfWork;
        private readonly IApplicationUserRepository _applicationUserRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public CartController(IUnitOfWork unitOfWork, IApplicationUserRepository applicationUserRepository, IProductRepository productRepository
            , IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._applicationUserRepository = applicationUserRepository;
            this._productRepository = productRepository;
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._mapper = mapper;
        }
   
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[cartSession] as List<CartItem>;
            if (cart != null)
            {
                ViewBag.Cart = cart;
                ViewBag.Total = cart.Sum(m => m.Product.Price * m.Quantity);
            }
           
            return View();
        }

        public ActionResult Checkout(string user = null)
        {
            var client = new ClientViewModel();
            if (user != null)
            {
                var temp = _applicationUserRepository.GetUserByUserName(user);
                client = new ClientViewModel() { FullName = temp.FullName, Address = temp.Address, Email = temp.Email, PhoneNumber = temp.PhoneNumber };              
            }

            
            var cart = Session[cartSession] as List<CartItem>;
            if (cart != null)
            {
                ViewBag.Cart = cart;
                ViewBag.Total = cart.Sum(m => m.Product.Price * m.Quantity);
            }
            ViewBag.Client = client;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckoutConfirmed(ClientViewModel client)
        {
            var user = _applicationUserRepository.GetUserByUserName(client.Email);
            var cart = Session[cartSession] as List<CartItem>;
            var order = EntityExtensions.CreateOrder(client);
            await _orderRepository.AddAsync(order);
            
            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail() { OrderId = order.OrderId, ProductId = item.Product.ID, Quantity = item.Quantity };
                await _orderDetailRepository.AddAsync(orderDetail);
            }
            await _unitOfWork.SaveAsync();
            Session.Remove(cartSession);
            return RedirectToAction("Index","Home");
        }

        public ActionResult AddToCart(string nameID)
        {
            bool status = false;
            var currentCart = Session[cartSession] as List<CartItem>;
            List<CartItem> cart = new List<CartItem>();
            var product = _mapper.Map<Product, ProductVM>(_productRepository.GetProductByNameIDAsync(nameID));
            if(product != null)
            {
                if (currentCart == null || currentCart.Count == 0)
                {
                    cart.Add(new CartItem { Product = product, Quantity = 1 });
                }
                else
                {
                    cart = Session[cartSession] as List<CartItem>;
                    int index = isExist(nameID);
                    if (index != -1)
                    {
                        cart[index].Quantity++;
                    }
                    else
                    {
                        cart.Add(new CartItem { Product = product, Quantity = 1 });
                    }
                    status = true;
                }

            }

            Session[cartSession] = cart;
            return Json(new {cart = Session[cartSession], status = status });
        }

        public ActionResult UpdateCart(string nameID, int quantity)
        {
            var cart = Session[cartSession] as List<CartItem>;
            int index = isExist(nameID);
            cart[index].Quantity = quantity;

            var total = cart.Sum(m => m.Product.Price * m.Quantity);

            return Json(new { cart = cart,total = total , status = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string deleteId)
        {
            var cart = Session[cartSession] as List<CartItem>;
            int index = isExist(deleteId);
            cart.RemoveAt(index);
            ViewBag.Quantity = cart.Count - 1;
            if (cart.Count == 0)
            {
                Session.Remove(cartSession);
                ViewBag.Quantity = 0;
            }
            
            return RedirectToAction("Index");
        }

        private int isExist(string id)
        {
            var cart = Session[cartSession] as List<CartItem>;
            for (int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.NameID.Equals(id))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}