using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Entities
{
    public class ProductRank
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Sold { get; set; }
        public double Rate { get; set; }
        public int CategoryId { get; set; }

    }
}
