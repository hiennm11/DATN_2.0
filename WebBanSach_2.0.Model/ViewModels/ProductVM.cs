using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class ProductVM : AbstractProps
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Your product need a name.")]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int AvailableQuantity { get; set; }        
        public string NameAlias { get; set; }
        public string Link { get; set; }

        [Required(ErrorMessage = "Your product need a pub date.")]
        public DateTime PublicationDate { get; set; }

        public CategoryVM Category { get; set; }
        public ICollection<AuthorVM> Authors { get; set; }
        public ICollection<CommentVM> Comments { get; set; }
        public DiscountVM Discount { get; set; }
    }
}