using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebBanSach_2_0.Model.Entities;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class CommentVM
    {
        public int CommentId { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; }

        [Required(ErrorMessage = "Bạn không được sử dụng kí tự đặc biệt")]
        public string Review { get; set; }
        public double Rating { get; set; }
        public DateTime CommentDate { get; set; }

        public ApplicationUser User { get; set; }

    }
}
