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
    public class ProductAdderController : Controller
    {
        private static int pageSize = 10;
        private readonly IAdminProductAdderService _adminProductService;

        public ProductAdderController(IAdminProductAdderService adminProductService)
        {
            _adminProductService = adminProductService;
        }

        // GET: Admin/ProductAdder
        public async Task<ActionResult> Index(StatusMessageId? status, int page = 1, string search = null, int categoryId = 0)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var response = await _adminProductService.GetDataAsync(page, pageSize, categoryId, search);
            return View(response);
        }

        public ActionResult Create()
        {
            return View(new ProductAdderVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductAdderVM product)
        {
            if (ModelState.IsValid)
            {
                
                if (await _adminProductService.SaveDataAsync(product) > 0)
                {
                    return RedirectToAction("Index", new { status = StatusMessageId.AddSuccess });
                }

                return RedirectToAction("Index", new { status = StatusMessageId.Error });
            }
            ModelState.AddModelError("Error", "Cannot save your product");
            return View();
        }

    }
}