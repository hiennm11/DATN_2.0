using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBanSach_2_0.Data.Infrastructure;
using WebBanSach_2_0.Data.Repositories;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ResponseModels;
using WebBanSach_2_0.Model.ViewModels;
using WebBanSach_2_0.Service.AdminServices;
using WebBanSach_2_0.Service.Infrastructure;
using static WebBanSach_2_0.Model.ViewModels.Pagination;

namespace WebBanSach_2_0.Web.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;            
        }

        // GET: Admin/Product
        [Route("Product")]
        public async Task<ActionResult> Index(int page = 1, string search = null, int cate = 0)
        {
            var response = new AdminListProductResponse()
            {
                Products = await _productService.GetDataAsync(page, search, cate),
                Categories = await _productService.GetCategoriesListAsync()
            };
            //ViewBag.CateList = new SelectList(_categoryRepository.GetAll(), "ID", "CategoryName");
           
            return View(response);
        }

        public async Task<ActionResult> Details(int productId = 0)
        {
            var response = new AdminProductDetailResponse() 
            {
                Product = productId != 0 ? await _productService.GetProductAsync(productId) : new ProductVM(),
                Categories = await _productService.GetCategoriesListAsync()
            };
            
            return View(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Details(ProductVM product, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                product.NameID = EntityExtensions.convertToUnSign(product.Name);
                if (file != null && file.ContentLength > 0)
                {
                    string pic = product.NameID + Path.GetExtension(file.FileName);
                    string path = Path.Combine(Server.MapPath("/img/" + product.CategoryId), pic);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);                        
                    }
                    file.SaveAs(path);
                    product.Image = String.Concat("/img/", product.CategoryId, "/", pic);
                }
                if(await _productService.SaveProduct(product) > 0)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError("Error", "Cannot save your product");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            
            if (await _productService.DeleteProduct(id) > 0)
            {
                ViewData["Message"] = "Product has been deleted";
            }
            ViewData["Message"] = "Delete Error!";
            return RedirectToAction("Index");
        }


        //public async Task<JsonResult> GetPaggedData(int page = 1, string search = null, int cate = 0)
        //{
        //    var viewModel = await _productService.GetDataAsync(page, search, cate);
        //    return Json(new { data = viewModel, status = true }, JsonRequestBehavior.AllowGet);
        //}

        //public async Task<JsonResult> GetDetail(int id)
        //{
        //    var datatemp = await _productRepository.GetSingleByIDAsync(id);
        //    var data = _mapper.Map<Product, ProductVM>(datatemp);
        //    return Json(new { data = data, status = true }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public async Task<JsonResult> SaveDetail(ProductVM product)
        //{
        //    bool status;
        //    string message = string.Empty;
        //    var productDB = await _productRepository.GetSingleByIDAsync(product.ID);
        //    if (productDB != null)
        //    {
        //        if (product.file != null)
        //        {
        //            string imgTemp = productDB.Image;
        //            string imgDir = SaveImg(productDB.NameID, productDB.CategoryId, product.file);

        //            productDB.Image = (imgDir ?? imgTemp);

        //            productDB.UpdateProduct(product);

        //            //Change image location
        //            string oldPath = Server.MapPath(imgTemp);
        //            string newPath = Path.Combine(Server.MapPath("/img/" + product.CateID),
        //                productDB.NameID + Path.GetExtension(imgTemp));

        //            System.IO.File.Move(oldPath, newPath);

        //            productDB.Image = String.Concat("/img/", productDB.CategoryId, "/",
        //                productDB.NameID + Path.GetExtension(imgTemp));

        //        }

        //        else
        //        {
        //            productDB.UpdateProduct(product);
        //        }

        //        await _productRepository.UpdateAsync(productDB);

        //    }

        //    else
        //    {
        //        Product newProduct = EntityExtensions.CreateNewProduct(product);
        //        if (product.file != null)
        //        {
        //            newProduct.Image = SaveImg(newProduct.NameID, newProduct.CategoryId, product.file);
        //        }

        //        await _productRepository.AddAsync(newProduct);

        //    }
        //    ViewBag.CateList = new SelectList(await _categoryRepository.GetAllAsync(), "ID", "CategoryName");

        //    try
        //    {
        //        await _unitOfWork.SaveAsync();
        //        status = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        status = false;
        //        message = ex.Message;
        //    }

        //    return Json(new { status = status, message = message });
        //}


        //private string SaveImg(string nameID, int cateID, HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        try
        //        {
        //            string pic = nameID + Path.GetExtension(file.FileName);
        //            string path = Path.Combine(Server.MapPath("/img/" + cateID), pic);
        //            string result = String.Concat("/img/", cateID, "/", pic);
        //            file.SaveAs(path);
        //            return result;
        //        }
        //        catch
        //        {

        //        }
        //    }
        //    return null;
        //}

    }
}