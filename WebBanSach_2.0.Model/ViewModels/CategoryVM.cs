using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class CategoryVM : AbstractProps
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public string NameAlias { get; set; }

    }
}