using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.AdminServices;
using WebBanSach_2_0.Service.Infrastructure;
using WebBanSach_2_0.Service.Interfaces;
using WebBanSach_2_0.Web.Infrastructure;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IAdminDiscountService _adminDiscountService;
        private readonly IAdminProductService _adminProductService;

        public DiscountController(IAdminDiscountService adminDiscountService, IAdminProductService adminProductService)
        {
            this._adminDiscountService = adminDiscountService;
            this._adminProductService = adminProductService;
        }
        // GET: Admin/Discount
        public async Task<ActionResult> Index(StatusMessageId? status, string search = null, int pageSize = 10, int page = 1)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";
            var response = await _adminDiscountService.GetDataAsync(search, pageSize, page);
            return View(response);
        }

        public async Task<ActionResult> Detail(int discountId = 0)
        {
            var response = discountId == 0 ? new DiscountVM() : await _adminDiscountService.GetDataByIDAsync(discountId);
            ViewBag.DiscountType = GetSelectListOfType();
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Detail(DiscountVM viewModel, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                viewModel.DiscountNameAlias = EntityExtensions.ConvertToUnSign(viewModel.DiscountName);

                if (file != null && file.ContentLength > 0)
                {
                    string pic = "discount_" + (int)viewModel.DiscountType + "_" + viewModel.DiscountNameAlias + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/img/"), pic);
                    viewModel.DiscountCover = EntityExtensions.SaveImage(file, pic, path);
                }
                if(await _adminDiscountService.SaveDataAsync(viewModel) > 0)
                {
                    return RedirectToAction("Index", new { status = StatusMessageId.UpdateSuccess });
                }
            }
            ModelState.AddModelError("Error", "Cannot save your product");
            return View();
        }

        public async Task<ActionResult> EditDiscount(StatusMessageId? status, int discountId)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var model = await _adminDiscountService.GetDataByIDAsync(discountId);
            var list = await _adminProductService.GetAllProductAsync();
            ViewBag.ProductId = new SelectList(list.Where(item => model.Products.FirstOrDefault(r => r.ProductId == item.ProductId) == null).ToList(), "ProductId", "Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddProductToDiscount(int discountId, int[] productId)
        {
            await _adminDiscountService.AddProductToDiscount(discountId, productId);

            var model = await _adminDiscountService.GetDataByIDAsync(discountId);
            var list = await _adminProductService.GetAllProductAsync();
            ViewBag.ProductId = new SelectList(list.Where(item => model.Products.FirstOrDefault(r => r.ProductId == item.ProductId) == null).ToList(), "ProductId", "Name");
            return RedirectToAction("EditDiscount", new { status = StatusMessageId.UpdateSuccess, discountId = discountId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProductFromDiscount(int discountId, int productId)
        {
            await _adminDiscountService.DeleteProductFromDiscount(discountId, productId);

            var model = await _adminDiscountService.GetDataByIDAsync(discountId);
            var list = await _adminProductService.GetAllProductAsync();
            ViewBag.ProductId = new SelectList(list.Where(item => model.Products.FirstOrDefault(r => r.ProductId == item.ProductId) == null).ToList(), "ProductId", "Name");
            return RedirectToAction("EditDiscount", new { status = StatusMessageId.DeleteSuccess, discountId = discountId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (await _adminDiscountService.DeleteDataAsync(id) > 0)
            {
                return RedirectToAction("Index", new { status = StatusMessageId.DeleteSuccess });
            }
            return RedirectToAction("Index", new { status = StatusMessageId.Error });
        }

        private SelectList GetSelectListOfType()
        {
            var type = from DiscountType d in Enum.GetValues(typeof(DiscountType))
                         select new { ID = (int)d, Name = Extension.GetEnumDisplayName(d) };
            return new SelectList(type, "ID", "Name");
        }
    }
}