using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class UsersFocus
    {
        public long AutoID { get; set; } 
        public string UserID { get; set; } 
        public string FocusID { get; set; }
        public string FocusName { get; set; }
        public string FocusAvatar { get; set; } 
        public DateTime CreateTime { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
