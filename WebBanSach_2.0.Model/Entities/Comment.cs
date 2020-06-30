using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }
        public string Review { get; set; }
        public double Rating { get; set; }
        public DateTime CommentDate { get; set; }

        public Product Product { get; set; }
        public ApplicationUser User { get; set; }
    }
}
