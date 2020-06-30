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
using WebBanSach_2_0.Service.Interfaces;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IAdminCategoryService _categoryService;

        public CategoryController(IAdminCategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        // GET: Admin/Category
        public async Task<ActionResult> Index(StatusMessageId? status, int page = 1, string search = null)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var response = await _categoryService.GetDataAsync(page, search);
            ViewBag.SearchString = search;

            return View(response);
        }

        public async Task<ActionResult> Detail(int categoryId = 0)
        {
            var response = categoryId == 0 ? new CategoryVM() : await _categoryService.GetDataByIDAsync(categoryId);
            return View(response);
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Detail(CategoryVM category)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryService.SaveDataAsync(category) > 0)
                {
                    return RedirectToAction("Index", new { @status = StatusMessageId.UpdateSuccess });
                }
            }
            ModelState.AddModelError("Error", "Cannot save your product");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (await _categoryService.DeleteDataAsync(id) > 0)
            {
                return RedirectToAction("Index", new { status = StatusMessageId.DeleteSuccess });
            }
            return RedirectToAction("Index", new { status = StatusMessageId.Error });
        }
    }
}