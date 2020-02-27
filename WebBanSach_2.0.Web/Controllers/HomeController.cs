using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Web.Infrastructure;
using WebBanSach_2_0.Web.Models;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2._0.Web.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0.Data.WebBanSach_2_0DbContext());

        [Route("")]
        [Route("~/")]
        public ActionResult Index(string cateID = null, string search = null,int page = 1)
        {
            var dataTemp = _unitOfWork.Product.GetAll();
            var newTemp = _unitOfWork.Product.GetNewProduct().Take(18);
            var hotTemp = _unitOfWork.Product.GetHotProduct().Take(18);

            if (search != null && search != "")
            {
                string searchc = EntityExtensions.convertToUnSign(search);
                cateID = null;
                dataTemp = _unitOfWork.Product.GetAll().Where(m => m.NameID.ToLower().Contains(searchc));
            }
            if (cateID != null && cateID != "")
            {
                search = null;
                dataTemp = _unitOfWork.Product.GetByCategory(cateID);
            }

            var newProduct = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(newTemp);
            var hotProduct = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(hotTemp);

            var data = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(dataTemp);
            var pager = new Pager(data.Count(), page, 18);
            var viewModel = new IndexViewModel<ProductVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };
                     
            ViewBag.NewProduct = newProduct.ToList();
            ViewBag.HotProduct = hotProduct.ToList();
            ViewBag.ProductList = viewModel;
            ViewBag.CategoryID = cateID;
            ViewBag.SearchString = search;

            return View();
        }     

        public JsonResult GetPaggedData(int page, string cateID , string search)
        {
            var dataTemp = _unitOfWork.Product.GetAll();
            if (search != null && search != "")
            {
                string searchc = EntityExtensions.convertToUnSign(search);
                cateID = null;
                dataTemp = _unitOfWork.Product.GetAll().Where(m => m.NameID.ToLower().Contains(searchc));
            }
            if(cateID != null && cateID != "")
            {
                search = null;
                dataTemp = _unitOfWork.Product.GetByCategory(cateID);
            }
            var data = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(dataTemp);
            var pager = new Pager(data.Count(), page, 18);
            var viewModel = new IndexViewModel<ProductVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = pager
            };

            return Json(new {data = viewModel, status = true}, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Detail(string nameID)
        {
            var item = _unitOfWork.Product.GetProductByNameID(nameID);
            var temp = _unitOfWork.Product.GetByCategory(item.Categories.Description).Take(6);
            var list = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(temp);
            var product = AutoMapperConfiguration.map.Map<Product, ProductVM>(item);
            var author = item.ProductAuthors.FirstOrDefault(m => m.ProductID == item.ID);

            ViewBag.Author = author.Author.Name;
            ViewBag.CategoryName = item.Categories.CategoryName;
            ViewBag.CateNameID = item.Categories.Description;
            ViewBag.RelatedList = list.ToList();

            return View(product);
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