using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBanSach_2_0.Model.Abstract
{
    public interface IAbstractProps
    {
        DateTime? CreateDate { get; set; }
        string CreateBy { get; set; }
        DateTime? UpdatedDate { get; set; }
        string UpdateBy { get; set; }  
        bool Status { get; set; }
    }
    public class AbstractProps :IAbstractProps
    {
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdateBy { get; set; }
        public bool Status { get; set; }
    }
}
