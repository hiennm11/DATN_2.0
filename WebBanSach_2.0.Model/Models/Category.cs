using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Model.Models
{
    public class Category : AbstractProps
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string Description { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
