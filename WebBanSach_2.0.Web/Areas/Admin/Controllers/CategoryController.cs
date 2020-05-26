using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.AdminServices;

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
        public async Task<ActionResult> Index(int page = 1, string search = null)
        {
            var response = await _categoryService.GetDataAsync(page, search);
            return View(response);
        }

        public async Task<ActionResult> Detail(int categoryId = 0)
        {
            var response = categoryId == 0 ? new CategoryVM() : await _categoryService.GetCategoryAsync(categoryId);
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(CategoryVM category)
        {
            if (ModelState.IsValid)
            {
                if (await _categoryService.SaveCategory(category) > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("Error", "Cannot save your product");

            return View();
        }
    }
}