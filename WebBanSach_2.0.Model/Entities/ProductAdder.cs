using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Entities
{
    public class ProductAdder
    {
        [Key]
        public int AdderId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }        
        public int CategoryId { get; set; }
        public int AvailableQuantity { get; set; }
        public double Price { get; set; }
        public DateTime PurchaseDate { get; set; }

    }
}
