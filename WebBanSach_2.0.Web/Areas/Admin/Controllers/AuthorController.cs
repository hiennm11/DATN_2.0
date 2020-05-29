using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.AdminServices;
using WebBanSach_2_0.Service.Infrastructure;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAdminAuthorService _adminAuthorService;
        private readonly IAdminProductService _adminProductService;

        public AuthorController(IAdminAuthorService adminAuthorService, IAdminProductService adminProductService)
        {
            this._adminAuthorService = adminAuthorService;
            this._adminProductService = adminProductService;
        }

        // GET: Admin/AuthorDetail
        public async Task<ActionResult> Index(StatusMessageId? status, int page = 1, string search = null)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var response = await _adminAuthorService.GetDataAsync(page, search);
            ViewBag.SearchString = search;

            return View(response);
        }

        public async Task<ActionResult> Detail(int authorId = 0)
        {
            var response = authorId == 0 ? new AuthorVM() : await _adminAuthorService.GetDataByIDAsync(authorId);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(AuthorVM author)
        {
            if (ModelState.IsValid)
            {
                if (await _adminAuthorService.SaveDataAsync(author) > 0)
                {
                    return RedirectToAction("Index", new { @status = StatusMessageId.UpdateSuccess });
                }
            }
            ModelState.AddModelError("Error", "Cannot save your product");

            return View();
        }

        public async Task<ActionResult> EditProduct(StatusMessageId? status, int authorId)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var model = await _adminAuthorService.GetDataByIDAsync(authorId);
            var list = await _adminProductService.GetAllProductAsync();
            ViewBag.ProductId = new SelectList(list.Where(item => model.Products.FirstOrDefault(r => r.ProductId == item.ProductId) == null).ToList(), "ProductId", "Name");
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProductToAuthor(int authorId, int[] productId)
        {
            await _adminAuthorService.AddProductToAuthor(authorId, productId);

            var model = await _adminAuthorService.GetDataByIDAsync(authorId);
            var list = await _adminProductService.GetAllProductAsync();
            ViewBag.ProductId = new SelectList(list.Where(item => model.Products.FirstOrDefault(r => r.ProductId == item.ProductId) == null).ToList(), "AuthorId", "Name");
            return RedirectToAction("EditProduct", new { status = StatusMessageId.UpdateSuccess, authorId = authorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProductFromAuthor(int authorId, int productId)
        {
            await _adminAuthorService.DeleteProductFromAuthor(authorId, productId);

            var model = await _adminAuthorService.GetDataByIDAsync(authorId);
            var list = await _adminProductService.GetAllProductAsync();
            ViewBag.ProductId = new SelectList(list.Where(item => model.Products.FirstOrDefault(r => r.ProductId == item.ProductId) == null).ToList(), "AuthorId", "Name");
            return RedirectToAction("EditProduct", new { status = StatusMessageId.DeleteSuccess, authorId = authorId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (await _adminAuthorService.DeleteDataAsync(id) > 0)
            {
                return RedirectToAction("Index", new { status = StatusMessageId.DeleteSuccess });
            }
            return RedirectToAction("Index", new { status = StatusMessageId.Error });
        }
    }
}