using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Web.Infrastructure;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2._0.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0.Data.WebBanSach_2_0DbContext());

        [Route("")]
        [Route("~/")]        
        public async Task<ActionResult> Index(string cateID = null, string search = null,int page = 1)
        {
            var dataTemp = await _unitOfWork.Product.GetAll();
            var newTemp = _unitOfWork.Product.GetNewProduct();
            var hotTemp = _unitOfWork.Product.GetHotProduct();

            if (search != null && search != "")
            {
                string searchc = EntityExtensions.convertToUnSign(search);
                cateID = null;
                dataTemp = _unitOfWork.Product.GetBySearch(searchc);
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
                     
            ViewBag.NewProduct = newProduct.Take(18).ToList();
            ViewBag.HotProduct = hotProduct.Take(18).ToList();
            ViewBag.ProductList = viewModel;
            ViewBag.CategoryID = cateID;
            ViewBag.SearchString = search;

            return View();
        }

        [HandleError]
        [OutputCache(Duration = 3600 * 24 * 10, Location = System.Web.UI.OutputCacheLocation.Any, VaryByParam = "nameID")]
        public ActionResult Detail(string nameID)
        {
            var item = _unitOfWork.Product.GetProductByNameID(nameID);
            var temp = _unitOfWork.Product.GetByCategory(item.Categories.Description);
            var list = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(temp.Take(6));
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
        [OutputCache(Duration = 3600 * 24 * 10)]
        public ActionResult CategoryMenu()
        {
            return Task.Run(async () => {
                var cate = await _unitOfWork.Category.GetAll();

                var category = AutoMapperConfiguration.map.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(cate);

                return PartialView("_CateMenu", category);
            }).Result;           
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