using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProBusiness.Common;
using ProDAL;
using ProEntity;
using ProEnum;

namespace ProBusiness
{
    public class UserNeedsBusiness
    {
        #region 查询

        public static List<UserNeeds> FindNeedsList(string type, string userId, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string inviteID="",string status="",int needSex=-1, string age="",string address="")
        {
            string sqlwhere = "  a.Status not in (6,9)";
            if (!string.IsNullOrEmpty(status))
            {
                sqlwhere += " and a.Status in (" + status + ") ";
            }
            else
            {
                sqlwhere += " and a.Status in (1,2,3,4,5) ";
            }
            if (!string.IsNullOrEmpty(userId))
            {
                sqlwhere += " and a.UserID='" + userId + "'";
            }
            if (type!="-1" && !string.IsNullOrEmpty(type))
            {
                sqlwhere += " and a.Type in (" + type+")";
            }
            if (needSex > -1)
            {
                sqlwhere += " and a.needSex=" + needSex;
            }
            if (!string.IsNullOrEmpty(inviteID))
            {
                sqlwhere += " and a.InviteID='" + inviteID + "' ";
            }
            if (!string.IsNullOrEmpty(address))
            {
                string[] strArr = address.Split(',');
                for (int i = 0; i < strArr.Length; i++)
                {
                    sqlwhere += (i == 0
                        ? " and b.province='"
                        : i == 1 ? " and b.City='" : i == 2 ? " and b.District='" : "") + strArr[i] + "'";
                }

            }
            DataTable dt = CommonBusiness.GetPagerData(" UserNeeds a left join m_users b  on a.UserID=b.UserID ",
                 "a.AutoID,a.NeedDate,a.NeedCity,a.UserID,a.Type,a.Status,a.UserName,a.Title,a.LetDays,a.InviteName,a.NeedSex,a.NeedType,a.CreateTime,b.province,b.Avatar as UserAvatar", sqlwhere, "a.AutoID ", pageSize, pageIndex,
                out totalCount, out pageCount);
            List<UserNeeds> list = new List<UserNeeds>();
            foreach (DataRow dr in dt.Rows)
            {
                UserNeeds model = new UserNeeds();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        public static List<UserNeeds> FindNeedsList(string keyWords,string type, string userId, string status ,int pageSize, int pageIndex, ref int totalCount, ref int pageCount,string begintime="",string endtime="", int needSex = -1, string age = "", string address = "")
        {
            string sqlwhere = "  a.Status <>9";
            if (!string.IsNullOrEmpty(userId))
            {
                sqlwhere += " and a.UserID='" + userId + "'";
            }
            if (type != "-1" && !string.IsNullOrEmpty(type))
            {
                sqlwhere += " and a.Type in (" + type + ")";
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and a.Content like '%" + keyWords + "%' ";
            }
            if (needSex > -1)
            {
                sqlwhere += " and a.needSex=" + needSex;
            }
            if (!string.IsNullOrEmpty(status))
            {
                sqlwhere += " and a.status in(" + status + ") ";
            }
            if (!string.IsNullOrEmpty(begintime))
            {
                sqlwhere += " and a.CreateTime>='" + begintime + " 00:00:00'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
            if (!string.IsNullOrEmpty(address))
            {
                string[] strArr = address.Split(',');
                for (int i = 0; i < strArr.Length; i++)
                {
                    sqlwhere += (i == 0
                        ? " and b.province='"
                        : i == 1 ? " and b.City='" : i == 2 ? " and b.District='" : "") + strArr[i] + "'";
                }

            }
            DataTable dt = CommonBusiness.GetPagerData(" UserNeeds a left join m_users b  on a.UserID=b.UserID ",
                 "a.AutoID,a.NeedDate,a.NeedCity,a.UserID,a.Content,a.Status,a.Type,a.UserName,a.Title,a.LetDays,a.InviteName,a.NeedSex,a.NeedType,a.CreateTime,b.province,b.Avatar as UserAvatar", sqlwhere, "a.AutoID ", pageSize, pageIndex,
                out totalCount, out pageCount);
            List<UserNeeds> list = new List<UserNeeds>();
            foreach (DataRow dr in dt.Rows)
            {
                UserNeeds model = new UserNeeds();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        public static List<UserNeeds> FindNeedsRated(int type, string userId, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            string sqlwhere = "  a.Status not in (6,9)";
            if (!string.IsNullOrEmpty(userId))
            {
                sqlwhere += " and a.UserID='" + userId + "'";
            }
            if (type > -1)
            {
                sqlwhere += " and a.Type in(" + type+")";
            }
            DataTable dt = CommonBusiness.GetPagerData(" UserNeeds a left join m_users b  on a.UserID=b.UserID left join m_users c on a.InviteID=c.UserID ",
                 "a.AutoID,a.NeedDate,a.NeedCity,a.Type,a.UserID,a.UserName,a.Status,a.Title,a.LetDays,a.InviteName,a.NeedSex,a.NeedType,a.CreateTime,b.province,b.Avatar as UserAvatar ,c.Avatar as InviteAvatar", sqlwhere, "a.AutoID ", pageSize, pageIndex,
                out totalCount, out pageCount);
            List<UserNeeds> list = new List<UserNeeds>();
            foreach (DataRow dr in dt.Rows)
            {
                UserNeeds model = new UserNeeds();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }
     
        /// <summary>
        /// 
        /// </summary>
        /// <param name="autoId"></param>
        /// <returns></returns>
        public static UserNeeds FindNeedsDetail(int autoId)
        { 
            UserNeeds model=new UserNeeds();
            DataTable dt = UserNeedsDAL.BaseProvider.GetNeedsDetail(autoId);
            foreach (DataRow dr in dt.Rows)
            { 
                model.FillData(dr); ;
            }
            return model;
        }
        #endregion

        #region 操作

         public static bool CreateNeeds(UserNeeds needs,string operateip)
        {
            var result= UserNeedsDAL.BaseProvider.CreateNeeds(needs.UserID, needs.UserName, needs.Type, needs.Title,
                needs.Content, needs.LetDays, needs.ServiceConten, needs.NeedSex, needs.Price, needs.NeedType,needs.NeedCity,needs.NeedDate);
            if (result && needs.Type==0)
             {
                 LogBusiness.AddOperateLog(needs.UserID,needs.UserName,needs.UserLevelID,"","",EnumUserOperateType.SendLog, 
                     needs.Title,operateip);
             }
             return result;
        }
         public static bool UpdateStatus(string ids, int status, string operater)
         {
             return UserNeedsDAL.BaseProvider.UpdateStatus(ids, status, operater);  
         }
        #endregion
       
    }
}
