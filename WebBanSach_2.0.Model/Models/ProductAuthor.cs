using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Models
{
    public class ProductAuthor
    {
        [Key]
        [Column(Order = 1)]
        public int ProductID { get; set; }
        [Key]
        [Column(Order = 2)]
        public int AuthorID { get; set; }

        public virtual Product Product { get; set; }
        public virtual AuthorDetail Author { get; set; }

    }
}
