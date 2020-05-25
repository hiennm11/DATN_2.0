using WebBanSach_2_0.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebBanSach_2_0.Model.Entities
{
    public class Product : AbstractProps
    {      
        [Key]
        public int ProductId { get; set; }
        [Required]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Required]
        public double Price { get; set; }
        public int Purchase { get; set; }
        public double Star { get; set; }
        public string NameID { get; set; }
        public string Link { get; set; }

        public Category Category { get; set; }

        public ICollection<ProductAuthor> ProductAuthors { get; set; }
        public ICollection<Comment> Comments { get; set; }

    }
}
