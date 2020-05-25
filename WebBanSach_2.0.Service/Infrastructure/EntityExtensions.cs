using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.ViewModels;

namespace WebBanSach_2_0.Service.Infrastructure
{
    public static class EntityExtensions
    {
        public static string convertToUnSign(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace(" ","-").ToLower();
        }
        public static Product CreateNewProduct(ProductVM viewModel)
        {
            Product obj = new Product();
            obj.Name = viewModel.Name;
            obj.CategoryId = viewModel.CategoryId;
            obj.Description = viewModel.Description;
            obj.Image = viewModel.Image;
            obj.Price = viewModel.Price;
            obj.NameID = convertToUnSign(obj.Name);
            obj.CreateBy = "admin"; obj.UpdateBy = "admin";
            obj.Purchase = 0; obj.Star = 0;
            obj.CreateDate = DateTime.Now;
            obj.UpdatedDate = DateTime.Now;
            obj.Status = true;
            return obj;
        }

        public static AuthorDetail CreateAuthorDetail(AuthorDetailVM viewModel)
        {
            AuthorDetail obj = new AuthorDetail();
            obj.Name = viewModel.Name;
            obj.Description = viewModel.Description;
            obj.CreateBy = "admin"; obj.UpdateBy = "admin";
            obj.CreateDate = DateTime.Now;
            obj.UpdatedDate = DateTime.Now;
            obj.Status = true;
            return obj;
        }

        public static Category CreateCategory(CategoryVM viewModel)
        {
            Category obj = new Category();
            obj.CategoryName = viewModel.CategoryName;
            obj.Description = viewModel.Description;
            obj.CreateBy = "admin"; obj.UpdateBy = "admin";
            obj.CreateDate = DateTime.Now;
            obj.UpdatedDate = DateTime.Now;
            obj.Status = true;
            return obj;
        }

        public static AuthorExtensions CreateProductAuthor(ProductAuthor model)
        {
            AuthorExtensions author = new AuthorExtensions();
            author.AuthorID = model.AuthorId;
            author.ProductID = model.ProductId;
            author.AuthorName = model.Author.Name;
            author.ProductName = model.Product.Name;
            return author;
        }

        //public static Order CreateOrder(ClientViewModel model)
        //{
        //    Order obj = new Order()
        //    {
        //        CustomerName = model.FullName,
        //        CustomerAddress = model.Address,
        //        CustomerMobile = model.PhoneNumber,
        //        CustomerEmail = model.Email,                
        //        PaymentStatus = false,
        //        CreatedDate = DateTime.Now,
        //        PaymentMethod = model.PaymentMethod,
        //        Status = 1
        //    };
        //    if (model.PaymentMethod == "cod")
        //    {               
        //        obj.PaymentStatus = false;
        //    }
        //    return obj;
        //}

        public static void UpdateCategory(this Category categories, CategoryVM categoryVm)
        {
            categories.CategoryId = categoryVm.CategoryId;
            categories.CategoryName = categoryVm.CategoryName;
            categories.Description = categoryVm.Description;            
            categories.UpdatedDate = DateTime.Now;
            categories.Status = categoryVm.Status;
        }

        public static void UpdateProduct(this Product product, ProductVM productVm)
        {
            product.ProductId = productVm.ProductId;
            product.CategoryId = productVm.CategoryId;
            product.Description = productVm.Description;
            product.Name = productVm.Name;
            product.NameID = convertToUnSign(productVm.Name);
            product.Price = productVm.Price;           
            product.UpdatedDate = DateTime.Now;
            product.Status = productVm.Status;
        }

        public static void UpdateAuthorDetail(this AuthorDetail author, AuthorDetailVM authorVM)
        {
            author.AuthorId = authorVM.AuthorDetailId;
            author.Name = authorVM.Name;
            author.Description = authorVM.Description;         
            author.UpdatedDate = DateTime.Now;
            author.Status = authorVM.Status;
        }
       
    }
}