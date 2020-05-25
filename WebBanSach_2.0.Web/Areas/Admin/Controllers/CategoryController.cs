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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(IUnitOfWork unitOfWork, ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }


        // GET: Admin/Category
        public ActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> GetPaggedData(int page = 1, string search = null)
        {
            var dataTemp = await _categoryRepository.GetAllAsync();

            if (search != null && search != "")
            {
                dataTemp = _categoryRepository.GetBySearch(search);
            }
            
            var data = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(dataTemp);
            var pager = new Pager(data.Count(), page);
            var viewModel = new IndexViewModel<CategoryVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(data.Count(), page)
            };
            return Json(new { data = viewModel, status = true }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetDetail(int id)
        {
            var dataTemp = await _categoryRepository.GetSingleByIDAsync(id);
            var data = _mapper.Map<Category, CategoryVM>(dataTemp);
            return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> SaveDetail(string postData)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var obj = serializer.Deserialize<CategoryVM>(postData);
            bool status;
            string message = string.Empty;
            if (obj.ID > 0)
            {
                var data = await _categoryRepository.GetSingleByIDAsync(obj.ID);
                data.UpdateCategory(obj);
                await _categoryRepository.UpdateAsync(data);
            }
            else
            {
                var newcate = EntityExtensions.CreateCategory(obj);
                await _categoryRepository.AddAsync(newcate);
            }

            try
            {
                await _unitOfWork.SaveAsync();
                status = true;
            }
            catch(Exception ex)
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
            await _categoryRepository.ShiftDeleteAsync(id);
            try
            {
                await _unitOfWork.SaveAsync();
                status = true;
            }
            catch(Exception ex)
            {
                status = false;
                message = ex.Message;
            }

            return Json(new { status = status, message = message });
        }
    }
}