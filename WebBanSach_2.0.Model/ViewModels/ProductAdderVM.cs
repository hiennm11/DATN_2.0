using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class ProductAdderVM
    {
        public int AdderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public int AvailableQuantity { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
    }
}
