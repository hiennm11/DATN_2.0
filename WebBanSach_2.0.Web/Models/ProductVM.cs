using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Web.Models
{
    public class ProductVM : AbstractProps
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CateID { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Purchase { get; set; }
        public double Star { get; set; }
        public string NameID { get; set; }        
    }
}