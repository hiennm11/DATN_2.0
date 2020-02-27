using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBanSach_2_0.Data;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Web.Infrastructure;
using WebBanSach_2_0.Web.Models;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private UnitOfWork _unitOfWork = new UnitOfWork(new WebBanSach_2_0DbContext());
        // GET: Admin/Product
        [Route("Product")]
        public ActionResult Index()
        {
            
            //ViewBag.CateList = new SelectList(_unitOfWork.Category.GetAll(), "ID", "CategoryName");
            ViewBag.CateList = AutoMapperConfiguration.map.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>(_unitOfWork.Category.GetAll().Where(m => m.Status == true));

            return View();
        }
                   
        public JsonResult GetPaggedData(int page = 1, string search = null, int cate = 0)
        {
            var dataTemp = _unitOfWork.Product.GetAll();

            if (search != null && search != "")
            {
                dataTemp = _unitOfWork.Product.GetAll().Where(m => m.NameID.Contains(EntityExtensions.convertToUnSign(search)));
            }
            if (cate > 0)
            {
                dataTemp = _unitOfWork.Product.GetAll().Where(m => m.CateID == cate);
            }

            var data = AutoMapperConfiguration.map.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(dataTemp);
            var pager = new Pager(data.Count(), page);
            var viewModel = new IndexViewModel<ProductVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(data.Count(), page)
            };
            return Json(new { data = viewModel, status = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetail(int id)
        {
            var datatemp = _unitOfWork.Product.GetSingleByID(id);
            var data = AutoMapperConfiguration.map.Map<Product, ProductVM>(datatemp);
            return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveDetail(ProductVM product)
        {
            bool status;
            string message = string.Empty;
            var productDB = _unitOfWork.Product.GetSingleByID(product.ID);
            if (productDB != null)
            {
                if (product.file != null)
                {
                    string imgTemp = productDB.Image;
                    string imgDir = SaveImg(productDB.NameID, productDB.CateID, product.file);

                    productDB.Image = (imgDir ?? imgTemp);

                    productDB.UpdateProduct(product);
                    
                    //Change image location
                    string oldPath = Server.MapPath(imgTemp);
                    string newPath = Path.Combine(Server.MapPath("/img/" + product.CateID),
                        productDB.NameID + Path.GetExtension(imgTemp));

                    System.IO.File.Move(oldPath, newPath);

                    productDB.Image = String.Concat("/img/", productDB.CateID, "/",
                        productDB.NameID + Path.GetExtension(imgTemp));
                    
                }

                else
                {
                    productDB.UpdateProduct(product);
                }

                _unitOfWork.Product.Update(productDB);

            }

            else
            {
                Product newProduct = EntityExtensions.CreateNewProduct(product);
                if (product.file != null)
                {
                    newProduct.Image = SaveImg(newProduct.NameID, newProduct.CateID, product.file);
                }

                _unitOfWork.Product.Add(newProduct);

            }
            ViewBag.CateList = new SelectList(_unitOfWork.Category.GetAll(), "ID", "CategoryName");

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

            return Json(new { status = status, message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            bool status;
            string message = string.Empty;
            _unitOfWork.Product.ShiftDelete(id);
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

            return Json(new { status = status, message = message });
        }
      
        private string SaveImg(string nameID, int cateID, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    string pic = nameID + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/img/" + cateID), pic);
                    string result = String.Concat("/img/", cateID, "/", pic);
                    file.SaveAs(path);
                    return result;
                }
                catch
                {
                    
                }
            }
            return null;
        }
        
    }
}