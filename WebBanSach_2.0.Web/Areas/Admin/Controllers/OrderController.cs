using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebBanSach_2_0.Model.Enums;
using WebBanSach_2_0.Service.AdminServices;
using WebBanSach_2_0.Service.Enums;
using WebBanSach_2_0.Service.Infrastructure;
using WebBanSach_2_0.Web.Infrastructure;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IAdminOrderService _adminOrderService;

        public OrderController(IAdminOrderService adminOrderService)
        {
            this._adminOrderService = adminOrderService;
        }

        public async Task<ActionResult> Index(StatusMessageId? status, DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";

            var response = await _adminOrderService.GetDataAsync(fromDate, toDate, orderStatus, page, OrderTypeRequest.UndoneOrder);

            ViewBag.OrderStatusList = selectsStatus(1);
            ViewBag.OrderStatus = orderStatus;
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(response);
        }

        public async Task<ActionResult> WaitingOrder(DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            var response = await _adminOrderService.GetDataAsync(fromDate, toDate, null, page, OrderTypeRequest.WaitingOrder);

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(response);
        }

        public async Task<ActionResult> CompleteOrder(DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            var response = await _adminOrderService.GetDataAsync(fromDate, toDate, null, page, OrderTypeRequest.CompletedORder);

            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(response);
        }

        public async Task<ActionResult> DeletedOrder(StatusMessageId? status, DateTime? fromDate, DateTime? toDate, int? orderStatus, int page = 1)
        {
            ViewBag.StatusMessage = status != null ? EntityExtensions.HtmlStatusMessage(status) : "";
            var response = await _adminOrderService.GetDataAsync(fromDate, toDate, orderStatus, page, OrderTypeRequest.DeletedOrder);

            ViewBag.OrderStatusList = selectsStatus(2);
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;

            return View(response);
        }

        public ActionResult GetOrderDetailPartial(int id)
        {
            return Task.Run(async () =>
            {
                var cart = await _adminOrderService.GetOrderDetailCartView(id);
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_OrderDetail", cart);
                }
                return PartialView("_OrderDetail", cart);
            }).Result;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeOrderStatus(int id)
        {
            if(await _adminOrderService.ChangeOrderStatus(id) > 0)
            {
                return RedirectToAction("Index", new { @status = StatusMessageId.UpdateSuccess });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            if (await _adminOrderService.ChangeOrderStatusToFalse(id) > 0)
            {
                return RedirectToAction("DeletedOrder", new { @status = StatusMessageId.DeleteSuccess });
            }
            return RedirectToAction("Index");
        }

        public ActionResult ExportToPDF(int orderId)
        {
            string fileName = "Order_" + orderId + "_" + DateTime.Now.Ticks.ToString() + ".pdf";
            string reportPath = Server.MapPath(@"~\Reports\Report1.rdlc");
            string filePath = HttpContext.Server.MapPath(@"~\TempFiles\");
            string agent = HttpContext.Request.Headers["User-Agent"].ToString();

            var FilePathReturn = _adminOrderService.SaveReportFile(orderId, fileName, reportPath, filePath, agent);
            return Content(FilePathReturn);
        }

        private SelectList selectsStatus(int cond)
        {
            if(cond == 1)
            {
                var status = from OrderStatus d in Enum.GetValues(typeof(OrderStatus))
                             where d == OrderStatus.Accepted || d == OrderStatus.InProgress || d == OrderStatus.Shipping || d == OrderStatus.Deliveried
                             select new { ID = (int)d, Name = Extension.GetEnumDisplayName(d) };
                return new SelectList(status, "ID", "Name");

            }           
            else
            {
                var status = from OrderStatus d in Enum.GetValues(typeof(OrderStatus))
                             where d == OrderStatus.Declined || d == OrderStatus.Cancelled || d == OrderStatus.Deleted
                             select new { ID = (int)d, Name = Extension.GetEnumDisplayName(d) };
                return new SelectList(status, "ID", "Name");
            }
        }
    }
}