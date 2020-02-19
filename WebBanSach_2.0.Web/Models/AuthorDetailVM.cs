using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebBanSach_2_0.Model.Abstract;

namespace WebBanSach_2_0.Web.Models
{
    public class AuthorDetailVM : AbstractProps
    {        
        public int ID { get; set; }
        [Required(ErrorMessage ="An author has a name?")]
        public string Name { get; set; }
        public string Description { get; set; }


    }
}