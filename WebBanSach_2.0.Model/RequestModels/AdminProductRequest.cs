using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebBanSach_2_0.Model.RequestModels
{
    public class AdminProductRequest
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Your product need a name.")]
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Link { get; set; }
        public HttpPostedFileBase file { get; set; }

    }
}
