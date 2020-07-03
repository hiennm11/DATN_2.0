using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.Abstract;
using WebBanSach_2_0.Model.Enums;

namespace WebBanSach_2_0.Model.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        [Required]
        public string CustomerName { get; set; }
        [Required]
        public string CustomerAddress { get; set; }
        [Required]
        public string CustomerEmail { get; set; }
        [Required]
        public string CustomerMobile { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public bool PaymentStatus { get; set; }
        public OrderStatus Status { get; set; }
        

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        public Shipper Shipper { get; set; }
        public Discount Discount { get; set; }
        public ApplicationUser User { get; set; }
    }
}
