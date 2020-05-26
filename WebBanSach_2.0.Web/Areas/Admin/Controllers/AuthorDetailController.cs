using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Infrastructure;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorDetailController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorDetailRepository _authorDetailRepository;
        private readonly IMapper _mapper;

        public AuthorDetailController(IUnitOfWork unitOfWork, IAuthorDetailRepository authorDetailRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._authorDetailRepository = authorDetailRepository;
            this._mapper = mapper;
        }

        // GET: Admin/AuthorDetail
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetPaggedData(int page = 1, string search = null)
        {
            var dataTemp = await _authorDetailRepository.GetAllAsync();

            if (search != null && search != "")
            {
                dataTemp = _authorDetailRepository.GetBySearchAsync(search);
            }
            
            var data = _mapper.Map<IEnumerable<AuthorDetail>, IEnumerable<AuthorDetailVM>>(dataTemp);
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
            var dataTemp = await _authorDetailRepository.GetSingleByIDAsync(id);
            var data = _mapper.Map<AuthorDetail, AuthorDetailVM>(dataTemp);
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
            if (obj.AuthorDetailId > 0)
            {
                var data = await _authorDetailRepository.GetSingleByIDAsync(obj.AuthorDetailId);
                data.UpdateAuthorDetail(obj);
                await _authorDetailRepository.UpdateAsync(data);
            }
            else
            {
                var newcate = EntityExtensions.CreateAuthorDetail(obj);
                await _authorDetailRepository.AddAsync(newcate);
            }

            try
            {
                await _unitOfWork.SaveAsync();
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
            await _authorDetailRepository.ShiftDeleteAsync(id);
            try
            {
                await _unitOfWork.SaveAsync();
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