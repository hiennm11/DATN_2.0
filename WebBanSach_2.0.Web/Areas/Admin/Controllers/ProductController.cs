using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.AdminServices;
using WebBanSach_2_0.Service.Infrastructure;
using WebBanSach_2_0.Service.Interfaces;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    //[Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IAdminProductService _productService;
        private readonly IAdminAuthorService _adminAuthorService;

        public ProductController(IAdminProductService productService, IAdminAuthorService adminAuthorService)
        {
            this._productService = productService;
            this._adminAuthorService = adminAuthorService;
        }

        // GET: Admin/Product
        [Route("Product")]
        public async Task<ActionResult> Index(StatusMessageId? status, int page = 1, string search = null, int categoryId = 0)
        {
            //ViewBag.StatusMessage = status == StatusMessageId.AddSuccess ? "Đã tạo thành công bản ghi." :
            //    status == StatusMessageId.UpdateSuccess ? "Đã cập nhật thành công bản ghi." :
            //    status == StatusMessageId.DeleteSuccess ? "Đã xóa thành công bản ghi." :
            //    status == StatusMessageId.Error ? "Đã có lỗi xảy ra." : "";
            
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var response = new AdminListProductResponse()
            {
                Products = await _productService.GetDataAsync(page, search, categoryId),
                Categories = await _productService.GetCategoriesListAsync()
            };

            ViewBag.SearchString = search;
            ViewBag.CategoryID = categoryId;
           
            return View(response);
        }

        public async Task<ActionResult> Detail(string productId = null)
        {
            var response = new AdminProductDetailResponse() 
            {
                Product = !string.IsNullOrEmpty(productId) ? await _productService.GetDataByIDAsync(productId) : new ProductVM(),
                Categories = await _productService.GetCategoriesListAsync()
            };
            
            return View(response);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Detail(ProductVM product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                product.NameAlias = EntityExtensions.ConvertToUnSign(product.Name);
                if (file != null && file.ContentLength > 0)
                {
                    string pic = product.NameAlias + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/img/" + product.CategoryId), pic);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);                        
                    }
                    file.SaveAs(path);
                    product.Image = String.Concat("/img/", product.CategoryId, "/", pic);
                }
                if(await _productService.SaveDataAsync(product) > 0)
                {
                    if(product.ProductId == 0)
                    {
                        return RedirectToAction("Index", new { status = StatusMessageId.AddSuccess });
                    }
                    return RedirectToAction("Index", new { status = StatusMessageId.UpdateSuccess });
                }
            }
            ModelState.AddModelError("Error", "Cannot save your product");
            return View();
        }

        public async Task<ActionResult> EditAuthor(StatusMessageId? status, string productId)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var model = await _productService.GetDataByIDAsync(productId);
            var list = await _adminAuthorService.GetAllAuthorAsync();
            ViewBag.AuthorId = new SelectList(list.Where(item => model.Authors.FirstOrDefault(r => r.AuthorId == item.AuthorId) == null).ToList(), "AuthorId", "Name");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddAuthorToProduct(string productId, int[] authorId)
        {
            await _productService.AddAuthorToProduct(productId, authorId);

            var model = await _productService.GetDataByIDAsync(productId);
            var list = await _adminAuthorService.GetAllAuthorAsync();

            ViewBag.AuthorId = new SelectList(list.Where(item => model.Authors.FirstOrDefault(r => r.AuthorId == item.AuthorId) == null).ToList(), "AuthorId", "Name");
            return RedirectToAction("EditAuthor", new { status = StatusMessageId.UpdateSuccess, productId = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAuthorFromProduct(string productId, int authorId)
        {
            await _productService.DeleteAuthorFromProduct(productId, authorId);

            var model = await _productService.GetDataByIDAsync(productId);           
            var list = await _adminAuthorService.GetAllAuthorAsync();

            ViewBag.AuthorId = new SelectList(list.Where(item => model.Authors.FirstOrDefault(r => r.AuthorId == item.AuthorId) == null).ToList(), "AuthorId", "Name");
            return RedirectToAction("EditAuthor", new { status = StatusMessageId.DeleteSuccess, productId = productId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {           
            if (await _productService.DeleteDataAsync(id) > 0)
            {
                return RedirectToAction("Index", new { status = StatusMessageId.DeleteSuccess });
            }            
            return RedirectToAction("Index", new { status = StatusMessageId.Error });
        }
    }
}