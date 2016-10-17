using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class UserNeeds
    {
        public long AutoID { get; set; }
        /// <summary>
        /// 0 日志 1 求租 ,2 出租
        /// </summary>
        public int Type { get; set; }
        public string UserLevelID { get; set; }
        /// <summary>
        /// default:0 出租价格类型  详细见 枚举EnumDateType
        /// </summary>
        public int NeedType { get; set; }
        /// <summary>
        /// default:0 1 男性 2 女性
        /// </summary>
        public int NeedSex { get; set; }
        /// <summary>
        /// 0 待审核 1 已审核 2 已聘用 3 成功 4  待评价  6 作废 9 删除
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 发布用户
        /// </summary>
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserAvatar { get; set; }

        /// <summary>
        /// 接单用户
        /// </summary>
        public string InviteID { get; set; }
        public string InviteName { get; set; }
        public string InviteAvatar { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评价
        /// </summary>
        public string Rated { get; set; }
        /// <summary>
        /// 服务内容
        /// </summary>
        public string ServiceConten { get; set; }
        /// <summary>
        /// 租赁 天数
        /// </summary>
        public int LetDays { get; set; }
        //所需城市
        public string NeedCity { get; set; }
        //出发日期
        public DateTime NeedDate { get; set; }

        public decimal Price { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime InviteTime { get; set; }
        public string UpdateID { get; set; }
        public DateTime UpdateTime { get; set; }
        public string Province { get; set; }

        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
