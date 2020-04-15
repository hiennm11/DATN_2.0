using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Web.Infrastructure;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductAuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductAuthorController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        // GET: Admin/ProductAuthor
        public async Task<ActionResult> Index(int page = 1)
        {
            var list = _unitOfWork.ProductAuthor.GetGrouping();
            IEnumerable<Product> products = await _unitOfWork.Product.GetAllAsync();
            var pager = new Pager(list.Count(), page);
            var viewModel = new IndexViewModel<IGrouping<string,string>>()
            {
                Items =  list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(list.Count(), page)
            };
            ViewBag.ProductIds = new SelectList(products, "ID", "Name");
            ViewBag.Authors = _unitOfWork.AuthorDetail.GetAllAsync();
            ViewBag.List = viewModel;
            return View();
        }

        //public JsonResult GetPaggedData(int page = 1)
        //{
        //    var temp = _unitOfWork.ProductAuthor.GetAll();
        //    var pager = new Pager(temp.Count(), page);
        //    var data = new List<AuthorExtensions>();
        //    foreach(var item in temp)
        //    {
        //        var a = EntityExtensions.CreateProductAuthor(item);
        //        data.Add(a);
        //    }
        //    var viewModel = new IndexViewModel<AuthorExtensions>()
        //    {
        //        Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
        //        Pager = new Pager(data.Count(), page)
        //    };
        //    return Json(viewModel, JsonRequestBehavior.AllowGet);
        //}       

        public async Task<ActionResult> Create()
        {
            IEnumerable<Product> list = await _unitOfWork.Product.GetAllAsync();
            ViewBag.ProductIds = new SelectList(list,"ID","Name");
            ViewBag.Authors = _unitOfWork.AuthorDetail.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(int productID, int[] author)
        {
            bool status;
            string message = string.Empty;
            if(author != null && author.Length > 0)
            {
                foreach (int item in author)
                {
                    ProductAuthor pa = new ProductAuthor() { ProductID = productID, AuthorID = item };
                    await _unitOfWork.ProductAuthor.AddAsync(pa);
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
            }
            else
            {
                status = false;
                message = "Please choose at least one author !";
            }

            return Json(new { status = status, message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(string id)
        {
            bool status;
            string message = string.Empty;
            var prod = _unitOfWork.Product.GetProductByNameID(EntityExtensions.convertToUnSign(id));
            if(prod != null)
            {
                await _unitOfWork.ProductAuthor.ShiftDeleteAsync(prod.ID);
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
            }
            else
            {
                status = false;
                message = "Item's not exist !";
            }
            return Json(new { status = status, message = message });
        }

        public JsonResult GetProduct(string tags)
        {
            string tagc = EntityExtensions.convertToUnSign(tags);
            var li = _unitOfWork.Product.GetBySearch(tagc);
            return Json(li, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> getAuthor(string tags = null, int productID = 0)
        {
            IEnumerable<AuthorDetail> li = null;
            if (tags != null)
            {
                var temp = _unitOfWork.ProductAuthor.GetByProduct(productID);
                li =  _unitOfWork.AuthorDetail.GetBySearchAsync(tags);
                
                foreach (var item in temp)
                {                   
                   li.ToList().RemoveAll(x => x.ID == item.AuthorID);
                }                
            }
            else
            {
                li = await _unitOfWork.AuthorDetail.GetAllAsync();
            }

            var result = _mapper.Map<IEnumerable<AuthorDetail>, IEnumerable<AuthorDetailVM>>(li.Take(10));
            
            return Json(result, JsonRequestBehavior.AllowGet);


        }

    }

}