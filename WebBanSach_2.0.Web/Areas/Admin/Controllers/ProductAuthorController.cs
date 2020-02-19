using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Web.Infrastructure;
using WebBanSach_2_0.Web.Models;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    public class ProductAuthorController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new Data.WebBanSach_2_0DbContext());
        // GET: Admin/ProductAuthor
        public ActionResult Index()
        {
            var list = _unitOfWork.ProductAuthor.GetAll().GroupBy(m => m.Product.Name, m => m.Author.Name);
            IEnumerable<Product> products = _unitOfWork.Product.GetAll();
            ViewBag.ProductIds = new SelectList(products, "ID", "Name");
            ViewBag.Authors = _unitOfWork.AuthorDetail.GetAll();
            return View(list);
        }

        public JsonResult GetPaggedData(int page = 1)
        {
            var temp = _unitOfWork.ProductAuthor.GetAll();
            var pager = new Pager(temp.Count(), page);
            var data = new List<AuthorExtensions>();
            foreach(var item in temp)
            {
                var a = EntityExtensions.CreateProductAuthor(item);
                data.Add(a);
            }
            var viewModel = new IndexViewModel<AuthorExtensions>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(data.Count(), page)
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }       

        public ActionResult Create()
        {
            IEnumerable<Product> list = _unitOfWork.Product.GetAll();
            ViewBag.ProductIds = new SelectList(list,"ID","Name");
            ViewBag.Authors = _unitOfWork.AuthorDetail.GetAll();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int productID, int[] author)
        {
            bool status;
            string message = string.Empty;
            if(author != null && author.Length > 0)
            {
                foreach (int item in author)
                {
                    ProductAuthor pa = new ProductAuthor() { ProductID = productID, AuthorID = item };
                    _unitOfWork.ProductAuthor.Add(pa);
                }
                try
                {
                    _unitOfWork.Save();
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
        public ActionResult Delete(string id)
        {
            bool status;
            string message = string.Empty;
            var prod = _unitOfWork.Product.GetProductByNameID(EntityExtensions.convertToUnSign(id));
            if(prod != null)
            {
                _unitOfWork.ProductAuthor.Delete(prod.ID);
                try
                {
                    _unitOfWork.Save();
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

        public JsonResult getProduct(string tags)
        {
            string tagc = EntityExtensions.convertToUnSign(tags);
            List<Product> li = _unitOfWork.Product.GetAll().Where(x => x.NameID.Contains(tagc)).OrderBy(x => x.Name).Take(10).ToList();
            return Json(li, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAuthor(string tags = null, int productID = 0)
        {
            List<AuthorDetail> li = new List<AuthorDetail>();
            if (tags != null)
            {
                var temp = _unitOfWork.ProductAuthor.GetAll().Where(m => m.ProductID == productID);
                li = _unitOfWork.AuthorDetail.GetAll().Where(x => x.Name.ToLower().Contains(tags)).OrderBy(x => x.Name).Take(10).ToList();
                
                foreach (var item in temp)
                {                   
                    li.RemoveAll(x => x.ID == item.AuthorID);
                }                
            }
            else
            {
                li = _unitOfWork.AuthorDetail.GetAll().OrderBy(x => x.Name).Take(10).ToList();
            }

            var result = AutoMapperConfiguration.map.Map<IEnumerable<AuthorDetail>, IEnumerable<AuthorDetailVM>>(li);
            
            return Json(result, JsonRequestBehavior.AllowGet);


        }

    }

}