using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebBanSach_2_0.Models
{
    public class TacGia
    {
        [Key]
        public int MaTacGia { get; set; }
        [Required(ErrorMessage = "Bạn chưa nhập tên tác giả")]
        [DisplayName("Tên tác giả")]
        public string TenTacGia { get; set; }
        [DisplayName("Tiểu sử")]
        public string ThongTin { get; set; }

    }
}