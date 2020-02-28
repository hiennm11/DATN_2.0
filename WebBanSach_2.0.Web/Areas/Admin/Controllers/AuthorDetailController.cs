using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Web.Infrastructure;
using WebBanSach_2_0.Web.Models;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorDetailController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork(new Data.WebBanSach_2_0DbContext());
        // GET: Admin/AuthorDetail
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetPaggedData(int page = 1, string search = null)
        {
            var dataTemp = await _unitOfWork.AuthorDetail.GetAll();

            if (search != null && search != "")
            {
                dataTemp = _unitOfWork.AuthorDetail.GetBySearchAsync(search);
            }
            
            var data = AutoMapperConfiguration.map.Map<IEnumerable<AuthorDetail>, IEnumerable<AuthorDetailVM>>(dataTemp);
            var pager = new Pager(data.Count(), page);
            var viewModel = new IndexViewModel<AuthorDetailVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(data.Count(), page)
            };
            return Json(new {data = viewModel, searchstring = search, status = true }, JsonRequestBehavior.AllowGet);
        }

        [OutputCache(Duration = 3600 * 24 * 10, Location = System.Web.UI.OutputCacheLocation.Any , VaryByParam = "id")]
        public async Task<JsonResult> GetDetail(int id)
        {
            var dataTemp = await _unitOfWork.AuthorDetail.GetSingleByID(id);
            var data = AutoMapperConfiguration.map.Map<AuthorDetail, AuthorDetailVM>(dataTemp);
            return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveDetail(string postData)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<AuthorDetailVM>(postData);
            bool status;
            string message = string.Empty;
            if (obj.ID > 0)
            {
                var data = await _unitOfWork.AuthorDetail.GetSingleByID(obj.ID);
                data.UpdateAuthorDetail(obj);
                _unitOfWork.AuthorDetail.Update(data);
            }
            else
            {
                var newcate = EntityExtensions.CreateAuthorDetail(obj);
                _unitOfWork.AuthorDetail.Add(newcate);
            }

            try
            {
                await _unitOfWork.Save();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> DeleteConfirmed(int id)
        {
            bool status;
            string message = string.Empty;
            _unitOfWork.AuthorDetail.ShiftDelete(id);
            try
            {
                await _unitOfWork.Save();
                status = true;
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }

    }
}