using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Abstract
{   
    public class AbstractProps
    {
        [Index(IsUnique = true)]       
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UniqueStringKey { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateBy { get; set; }
        public bool Status { get; set; }
    }
}
