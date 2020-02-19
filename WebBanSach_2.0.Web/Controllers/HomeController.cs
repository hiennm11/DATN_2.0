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

       
        public ActionResult Index()
        {
            var cate = _unitOfWork.Category.GetAll().ToList();          
            
            var category = AutoMapperConfiguration.map.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(cate);
                 
            ViewBag.Cate = category.ToList();

            return View();
        }

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
    }
}