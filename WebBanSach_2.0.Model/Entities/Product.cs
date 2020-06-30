using WebBanSach_2_0.Model.Abstract;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;

namespace WebBanSach_2_0.Model.Entities
{
    public class Product : AbstractProps
    {      
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public string NameAlias { get; set; }
        public string Link { get; set; }
        public DateTime PublicationDate { get; set; }
        public bool IsAvailable { get; set; }

        public Category Category { get; set; }

        public ICollection<Author> Authors { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public Discount Discount { get; set; }

    }
}
