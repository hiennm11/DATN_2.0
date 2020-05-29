using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Model.ViewModels
{
    public class AuthorVM : AbstractProps
    {
        public int AuthorId { get; set; }
        [Required(ErrorMessage = "An author has a name?")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string NameAlias { get; set; }

        public ICollection<ProductVM> Products { get; set; }

    }
}