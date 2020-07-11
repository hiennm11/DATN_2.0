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
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service;
using WebBanSach_2_0.Service.Infrastructure;
using WebBanSach_2_0.Service.Interfaces;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2._0.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        private static int pageSize = 16;
        private readonly IClientProductService _clientProductService;
        private readonly IClientCategoryService _clientCategoryService;
        private readonly IClientCommentService _clientCommentService;

        public HomeController(IClientProductService clientProductService, IClientCategoryService clientCategoryService, IClientCommentService clientCommentService)
        {
            this._clientProductService = clientProductService;
            this._clientCategoryService = clientCategoryService;
            this._clientCommentService = clientCommentService;
        }

        [Route("")]
        [Route("~/")]        
        public async Task<ActionResult> Index(string cateID = null, string search = null, int page = 1)
        {            
            var response = new ClientProductListResponse()
            {
                NewsProducts = await _clientProductService.GetNewProducts(),
                HotProducts = await _clientProductService.GetHotProducts(),
                ProductList = await _clientProductService.GetAllProducts(cateID, search, pageSize, page)
            };

            ViewBag.CategoryID = cateID;
            ViewBag.SearchString = search;

            return View(response);
        }

        [HandleError]
        //[OutputCache(Duration = 3600 * 24 * 10, Location = System.Web.UI.OutputCacheLocation.Any, VaryByParam = "nameID")]
        public async Task<ActionResult> Detail(string nameID)
        {
            var response = new ClientProductDetailResponse();
            response.Product = await _clientProductService.GetProductByNameAlias(nameID);
            var relateList = await _clientProductService.GetProductsByCategoryId(response.Product.CategoryId);
            response.RelateProducts = relateList.Take(10);
            response.Comments = await _clientCommentService.GetProductListComment(response.Product.ProductId);

            return View(response);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Detail(CommentVM comment, string nameAlias)
        {
            var response = new ClientProductDetailResponse();
            response.Product = await _clientProductService.GetProductByNameAlias(nameAlias);
            var relateList = await _clientProductService.GetProductsByCategoryId(response.Product.CategoryId);
            //Orderby Rating
            response.RelateProducts = relateList.Take(10);

            if (ModelState.IsValid)
            {
                if (await _clientCommentService.AddCommentToDB(comment) > 0)
                {
                    response.Comments = await _clientCommentService.GetProductListComment(response.Product.ProductId);
                    return View(response);
                }               
            }
            response.Comments = await _clientCommentService.GetProductListComment(response.Product.ProductId);

            ModelState.AddModelError("Review", "Bạn chưa bình luận");
            return View(response);
        }

        #region childView

        [ChildActionOnly]
        //[OutputCache(Duration = 3600 * 24 * 10)]
        public ActionResult CategoryMenu()
        {
            return Task.Run(async () =>
            {
                var response = await _clientCategoryService.GetAllCategory();
                return PartialView("_CategoryMenu", response);
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