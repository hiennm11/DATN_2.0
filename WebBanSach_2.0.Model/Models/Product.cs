using WebBanSach_2_0.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace WebBanSach_2_0.Model.Models
{
    public class Product : AbstractProps
    {      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("Categories")]
        public int CateID { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [Required]
        public double Price { get; set; }
        public int Purchase { get; set; }
        public double Star { get; set; }
        public string NameID { get; set; }
        public string Link { get; set; }

        public virtual Category Categories { get; set; }

        public virtual ICollection<ProductAuthor> ProductAuthors { get; set; }

    }
}
