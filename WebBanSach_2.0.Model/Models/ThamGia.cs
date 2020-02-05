using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebBanSach_2_0.Models
{
    public class ThamGia
    {
        [Key]
        [Column(Order = 1)]
        public int MaSach { get; set; }
        [Key]
        [Column(Order = 2)]
        public int MaTacGia { get; set; }
        public string VaiTro { get; set; }

        public virtual Sach Sach { get; set; }
        public virtual TacGia TacGia { get; set; }
    }
}