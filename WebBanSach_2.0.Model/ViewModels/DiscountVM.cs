using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebBanSach_2_0.Model.Entities;
using WebBanSach_2_0.Model.Enums;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class DiscountVM
    {
        public int DiscountId { get; set; }
        [Required]
        public string DiscountName { get; set; }
        public string DiscountCode { get; set; }
        [Required]
        [RegularExpression("([0-9]+)")]
        public double DiscountValue { get; set; }
        public DiscountType DiscountType { get; set; }
        public string DiscountCover { get; set; }
        public string DiscountNameAlias { get; set; }

        public ICollection<ProductVM> Products { get; set; }
        public ICollection<OrderVM> Orders { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
    }
}
