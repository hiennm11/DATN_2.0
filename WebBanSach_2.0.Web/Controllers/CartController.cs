using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Service.Controllers
{
    public class CartController : Controller
    {
        private const string cartSession = "CartSession";
        private readonly IClientCartService _clientCartService;
        private readonly IClientOrderService _clientOrderService;

        public CartController(IClientCartService clientCartService, IClientOrderService clientOrderService)
        {
            this._clientCartService = clientCartService;
            this._clientOrderService = clientOrderService;
        }
   
        // GET: Cart
        public ActionResult Index()
        {
            var shoppingCart = (ShoppingCart)Session[cartSession] ?? new ShoppingCart();
            var cart = new ClientCartResponse() 
            { 
                Cart = shoppingCart.Cart              
            };

            if (cart.Cart != null)
            {
                cart.TotalPrice = cart.Cart.Sum(m => m.Product.Price * (100 - m.Product.Discount.DiscountValue) / 100 * m.Quantity);
            }  
            return View(cart);
        }

        
        public async Task<ActionResult> Checkout(string user = null)
        {
            var shoppingCart = (ShoppingCart)Session[cartSession];
            var response = _clientCartService.GetCheckoutModel(shoppingCart, user);
            if (!string.IsNullOrEmpty(shoppingCart.CartPromoCode))
            {
                var order = response.OrderInfo;
                response = await _clientCartService.AddCodeToCart(shoppingCart.Cart, shoppingCart.CartPromoCode);
                response.OrderInfo = order;
            }
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Checkout(string promoCode, string user = null)
        {
            var shoppingCart = (ShoppingCart)Session[cartSession];
            var checkoutModel = _clientCartService.GetCheckoutModel(shoppingCart, user);
            var response = await _clientCartService.AddCodeToCart(shoppingCart.Cart, promoCode);
            response.OrderInfo = checkoutModel.OrderInfo;
            if (string.IsNullOrEmpty(response.PromoCode))
            {
                ModelState.AddModelError("promoCode", "Mã không tồn tại.");
            }

            SaveCartSession(response);
            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckoutConfirmed(ClientCartResponse order, string user = null)
        {
            if (ModelState.IsValid)
            {
                var shoppingCart = (ShoppingCart)Session[cartSession];
                var orderToAdd = order.OrderInfo;
                if (await _clientOrderService.PlaceOrder(shoppingCart, orderToAdd, user) > 0)
                {
                    Session.Remove(cartSession);
                    return RedirectToAction("OrderComplete");
                }
            }
            
            return RedirectToAction("Checkout");
        }

        public ActionResult AddToCart(string nameID)
        {
            var shoppingCart = (ShoppingCart)Session[cartSession] ?? new ShoppingCart() { Cart = new List<CartItem>(), CartPromoCode = string.Empty };

            shoppingCart.Cart = _clientCartService.AddToCart(shoppingCart.Cart, nameID, 1);
            SaveCartSession(shoppingCart);
            return Json(new { cartLength = shoppingCart.Cart.Count, status = true });
        }

        public ActionResult UpdateCart(string nameID, int quantity)
        {
            var shoppingCart = (ShoppingCart)Session[cartSession];
            shoppingCart.Cart = _clientCartService.UpdateCart(shoppingCart.Cart, nameID, quantity);
            var totalPrice = shoppingCart.Cart.Sum(m => (m.Product.Price * (100 - m.Product.Discount.DiscountValue) / 100) * m.Quantity);
            SaveCartSession(shoppingCart);
            return Json(new { total = totalPrice, status = true });
        }        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string deleteId)
        {
            var shoppingCart = (ShoppingCart)Session[cartSession];
            shoppingCart.Cart = _clientCartService.DeleteItem(shoppingCart.Cart, deleteId);
            SaveCartSession(shoppingCart);
            if (shoppingCart.Cart.Count == 0)
            {
                Session.Remove(cartSession);
            }

            return RedirectToAction("Index");
        }

        public ActionResult OrderComplete(string message = null)
        {
            return View();
        }

        private void SaveCartSession(ClientCartResponse response)
        {
            var shoppingCart = new ShoppingCart { Cart = response.Cart, CartPromoCode = response.PromoCode };
            Session[cartSession] = shoppingCart;
        }

        private void SaveCartSession(ShoppingCart response)
        {
            Session[cartSession] = response;
        }
    }
}