using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEntity.Manage;

namespace ProEntity
{
    public class LogEntity
    {
        [Property("Lower")]
        public int AutoID { get; set; }

        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        public string UserID { get; set; } 

        public string UserName { get; set; }

        public string LeveID { get; set; }

        public string SeeID { get; set; }

        public string SeeAvatar { get; set; }

        public string SeeName { get; set; }

        public int Type { get; set; }

        public string OpearteID { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
