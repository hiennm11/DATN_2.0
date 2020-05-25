using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Model.Entities
{
    public class AuthorDetail : AbstractProps
    {       
        [Key]
        public int AuthorId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<ProductAuthor> ProductAuthors { get; set; }
    }
}
