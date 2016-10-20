using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class AdvertSet
    {
        public int AutoID { get; set; }
        public string ImgType { get; set; }
        public string View { get; set; }
        public string CreateUseID { get; set; } 
        public DateTime CreateTime { get; set; } 
        public string ImgUrl { get; set; }
        public string Content { get; set; }

        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
