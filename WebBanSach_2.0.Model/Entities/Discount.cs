using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.Enums;

namespace WebBanSach_2_0.Model.Entities
{
    public class Discount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountId { get; set; }
        public string DiscountName { get; set; }
        public string DiscountCode { get; set; }
        public double DiscountValue { get; set; }
        public DiscountType DiscountType { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
