using AutoMapper;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebBanSach_2_0.Data;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Model.Models;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Web.Infrastructure;
using static WebBanSach_2_0.Web.Infrastructure.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        // GET: Admin/Product
        [Route("Product")]
        public ActionResult Index()
        {
            
            //ViewBag.CateList = new SelectList(_unitOfWork.Category.GetAll(), "ID", "CategoryName");
            ViewBag.CateList = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryVM>>( _unitOfWork.Category.GetTrueCategories());

            return View();
        }

        public async Task<JsonResult> GetPaggedData(int page = 1, string search = null, int cate = 0)
        {
            var dataTemp = await _unitOfWork.Product.GetAllAsync();

            if (search != null && search != "")
            {
                dataTemp = _unitOfWork.Product.GetBySearch(search);
            }
            if (cate > 0)
            {
                dataTemp = _unitOfWork.Product.GetByCategoryInt(cate);
            }

            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductVM>>(dataTemp);
            var pager = new Pager(data.Count(), page);
            var viewModel = new IndexViewModel<ProductVM>()
            {
                Items = data.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize),
                Pager = new Pager(data.Count(), page)
            };
            return Json(new { data = viewModel, status = true }, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetDetail(int id)
        {
            var datatemp = await _unitOfWork.Product.GetSingleByIDAsync(id);
            var data = _mapper.Map<Product, ProductVM>(datatemp);
            return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> SaveDetail(ProductVM product)
        {
            bool status;
            string message = string.Empty;
            var productDB = await _unitOfWork.Product.GetSingleByIDAsync(product.ID);
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

                await _unitOfWork.Product.UpdateAsync(productDB);

            }

            else
            {
                Product newProduct = EntityExtensions.CreateNewProduct(product);
                if (product.file != null)
                {
                    newProduct.Image = SaveImg(newProduct.NameID, newProduct.CateID, product.file);
                }

                await _unitOfWork.Product.AddAsync(newProduct);

            }
            ViewBag.CateList = new SelectList(await _unitOfWork.Category.GetAllAsync(), "ID", "CategoryName");

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
            await _unitOfWork.Product.ShiftDeleteAsync(id);
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