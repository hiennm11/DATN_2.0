using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Web.Models
{
    public class CategoryVM : AbstractProps
    {
        public int ID { get; set; }       
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}