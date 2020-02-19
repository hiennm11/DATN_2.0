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
    public class AuthorController : Controller
    {
        UnitOfWork _unitOfWork = new UnitOfWork(new Data.WebBanSach_2_0DbContext());

        // GET: Admin/Author
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetPaggedData(int page = 1)
        {
            string[] includes = { "AuthorDetails", "Product" };
            var data = _unitOfWork.Author.GetAll(includes);
            var pager = new Pager(data.Count(), page);
            var aExtensions = new List<AuthorExtensions>();
            foreach (var item in data)
            {
                AuthorExtensions ex = new AuthorExtensions();
                ex.AuthorID = item.AuthorID;
                ex.AuthorName = item.AuthorDetails.Name;
                ex.ProductID = item.ProductID;
                ex.ProductName = item.Product.Name;

                aExtensions.Add(ex);
            }
            
            var viewModel = new IndexViewModel<AuthorExtensions>()
            {
                Items = aExtensions.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(data.Count(), page)
            };
            return Json(viewModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(int productID, int authorID)
        {

            if (productID != 0)
            {
                var authorTemp = _unitOfWork.Author.GetAuthor(productID,authorID);
                var ex = new AuthorExtensions()
                {
                    AuthorID = authorTemp.AuthorID,
                    ProductID = authorTemp.ProductID,
                    ProductName = authorTemp.Product.Name,
                    AuthorName = authorTemp.AuthorDetails.Name
                };
                
                return View(ex);
            }
            return View(new AuthorExtensions());
        }

        [HttpPost]
        public ActionResult Detail(int productIDnew, int authorIDnew, AuthorExtensions author)
        {
            if (ModelState.IsValid)
            {
                var addedAuthor = new Author();                

                if (productIDnew > 0)
                {
                    addedAuthor = _unitOfWork.Author.GetAuthor(productIDnew, authorIDnew);
                    addedAuthor.ProductID = author.ProductID;
                    addedAuthor.AuthorID = author.AuthorID;
                    _unitOfWork.Author.Update(addedAuthor);
                }
                else
                {
                    addedAuthor.AuthorID = author.AuthorID;
                    addedAuthor.ProductID = author.ProductID;
                    _unitOfWork.Author.Add(addedAuthor);
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(author);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _unitOfWork.Author.ShiftDelete(id);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        public JsonResult getProduct(string tags)
        {
            string tagc = EntityExtensions.convertToUnSign(tags); 
            List<Product> li = _unitOfWork.Product.GetAll().Where(x=>x.NameID.Contains(tagc)).OrderBy(x => x.Name).Take(10).ToList();
            return Json(li, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAuthor(string tags)
        {           
            List<AuthorDetail> li = _unitOfWork.AuthorDetail.GetAll().Where(x => x.Name.ToLower().Contains(tags)).OrderBy(x => x.Name).Take(10).ToList();
            return Json(li, JsonRequestBehavior.AllowGet);
        }
    }
}