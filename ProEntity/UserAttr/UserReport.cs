using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    [Serializable]
   public  class UserReport
    {
       /// <summary>
       /// 图品数量
       /// </summary>
       public int ImgCount { get; set; }
       [Property("Lower")] 
       public string UserID { get; set; }
       //是否登陆
       public int IsLogin { get; set; }
       //浏览数
       public int SeeCount { get; set; }
       //推荐数
       public int RecommendCount { get; set; }

       public void FillData(System.Data.DataRow dr)
       {
           dr.FillData(this);
       }
    }
}
