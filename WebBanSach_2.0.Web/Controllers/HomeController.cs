using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Web.Infrastructure;
using WebBanSach_2_0.Web.Models;

namespace WebBanSach_2._0.Web.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0.Data.WebBanSach_2_0DbContext());

       
        public ActionResult Index(string cateID)
        {
            var temp = _unitOfWork.Product.GetAll().Take(20);
            var newTemp = _unitOfWork.Product.GetNewProduct().Take(20);
            var hotTemp = _unitOfWork.Product.GetHotProduct().Take(20);

            if (cateID != null && cateID != "")
            {
                temp = _unitOfWork.Product.GetByCategory(cateID).Take(20);
            }
            var list = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(temp);
            var newProduct = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(newTemp);
            var hotProduct = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(hotTemp);

            ViewBag.ProductList = list.ToList();
            ViewBag.NewProduct = newProduct.ToList();
            ViewBag.HotProduct = hotProduct.ToList();

            return View();
        }     
        
        public ActionResult Detail(string nameID)
        {
            return View();
        }
       
        #region childView

        [ChildActionOnly]
        public ActionResult CategoryMenu()
        {
            var cate = _unitOfWork.Category.GetAll().Where(x => x.Status).ToList();

            var category = AutoMapperConfiguration.map.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(cate);

            return PartialView("_CateMenu", category);
        }

        #endregion

        #region About

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        #endregion
    }
}