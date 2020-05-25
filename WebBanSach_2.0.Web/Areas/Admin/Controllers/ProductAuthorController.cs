using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.Infrastructure;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Service.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductAuthorController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthorDetailRepository _authorDetailRepository;
        private readonly IProductAuthorRepository _productAuthorRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductAuthorController(IUnitOfWork unitOfWork, IAuthorDetailRepository authorDetailRepository, 
                                       IProductAuthorRepository productAuthorRepository, IProductRepository productRepository, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._authorDetailRepository = authorDetailRepository;
            this._productAuthorRepository = productAuthorRepository;
            this._productRepository = productRepository;
            this._mapper = mapper;
        }

        // GET: Admin/ProductAuthor
        public async Task<ActionResult> Index(int page = 1)
        {
            var list = _productAuthorRepository.GetGrouping();
            IEnumerable<Product> products = await _productRepository.GetAllAsync();
            var pager = new Pager(list.Count(), page);
            var viewModel = new IndexViewModel<IGrouping<string,string>>()
            {
                Items =  list.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(list.Count(), page)
            };
            ViewBag.ProductIds = new SelectList(products, "ID", "Name");
            ViewBag.Authors = _authorDetailRepository.GetAllAsync();
            ViewBag.List = viewModel;
            return View();
        }

        //public JsonResult GetPaggedData(int page = 1)
        //{
        //    var temp = _productAuthorRepository.GetAll();
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
            IEnumerable<Product> list = await _productRepository.GetAllAsync();
            ViewBag.ProductIds = new SelectList(list,"ID","Name");
            ViewBag.Authors = _authorDetailRepository.GetAllAsync();
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
                    ProductAuthor pa = new ProductAuthor() { ProductId = productID, AuthorId = item };
                    await _productAuthorRepository.AddAsync(pa);
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
            var prod = _productRepository.GetProductByNameIDAsync(EntityExtensions.convertToUnSign(id));
            if(prod != null)
            {
                await _productAuthorRepository.ShiftDeleteAsync(prod.ProductId);
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
            var li = _productRepository.GetBySearchAsync(tagc);
            return Json(li, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> getAuthor(string tags = null, int productID = 0)
        {
            IEnumerable<AuthorDetail> li = null;
            if (tags != null)
            {
                var temp = _productAuthorRepository.GetByProduct(productID);
                li =  _authorDetailRepository.GetBySearchAsync(tags);
                
                foreach (var item in temp)
                {                   
                   li.ToList().RemoveAll(x => x.AuthorId == item.AuthorId);
                }                
            }
            else
            {
                li = await _authorDetailRepository.GetAllAsync();
            }

            var result = _mapper.Map<IEnumerable<AuthorDetail>, IEnumerable<AuthorDetailVM>>(li.Take(10));
            
            return Json(result, JsonRequestBehavior.AllowGet);


        }

    }

}