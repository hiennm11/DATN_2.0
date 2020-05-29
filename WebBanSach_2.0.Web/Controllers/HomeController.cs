using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Infrastructure;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2._0.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        //private UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0.Data.WebBanSach_2_0DbContext());
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork unitOfWork, IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._productRepository = productRepository;
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }

        [Route("")]
        [Route("~/")]        
        public async Task<ActionResult> Index(string cateID = null, string search = null,int page = 1)
        {
            //var dataTemp = await _productRepository.GetAllAsync();
            //var newTemp = _productRepository.GetNewProductAsync();
            //var hotTemp = _productRepository.GetHotProductAsync();

            //if (search != null && search != "")
            //{
            //    string searchc = EntityExtensions.convertToUnSign(search);
            //    cateID = null;
            //    dataTemp = _productRepository.GetBySearchAsync(searchc);
            //}
            //if (cateID != null && cateID != "")
            //{
            //    search = null;
            //    dataTemp = _productRepository.GetByCategoryAsync(cateID);
            //}

            //var newProduct = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(newTemp);
            //var hotProduct = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(hotTemp);

            //var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(dataTemp);
            //var pager = new Pager(data.Count(), page, 18);
            //var viewModel = new IndexViewModel<ProductVM>()
            //{
            //    Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
            //    Pager = pager
            //};
                     
            //ViewBag.NewProduct = newProduct.Take(18).ToList();
            //ViewBag.HotProduct = hotProduct.Take(18).ToList();
            //ViewBag.ProductList = viewModel;
            //ViewBag.CategoryID = cateID;
            //ViewBag.SearchString = search;

            return View();
        }

        //[HandleError]
        //[OutputCache(Duration = 3600 * 24 * 10, Location = System.Web.UI.OutputCacheLocation.Any, VaryByParam = "nameID")]
        //public ActionResult Detail(string nameID)
        //{
        //    var item = _productRepository.GetProductByNameIDAsync(nameID);
        //    var temp = _productRepository.GetByCategory(item.Category.Description);
        //    var list = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(temp.Take(6));
        //    var product = _mapper.Map<Product, ProductVM>(item);
        //    var author = item.ProductAuthors.FirstOrDefault(m => m.ProductId == item.ProductId);

        //    ViewBag.Author = author.Author.Name;
        //    ViewBag.CategoryName = item.Category.CategoryName;
        //    ViewBag.CateNameID = item.Category.Description;
        //    ViewBag.RelatedList = list.ToList();

        //    return View(product);
        //}

        #region childView

        //[ChildActionOnly]
        //[OutputCache(Duration = 3600 * 24 * 10)]
        //public ActionResult CategoryMenu()
        //{
        //    return Task.Run(async () => {
        //        var cate = await _categoryRepository.GetAllAsync();

        //        var category = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(cate);

        //        return PartialView("_CateMenu", category);
        //    }).Result;           
        //}

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