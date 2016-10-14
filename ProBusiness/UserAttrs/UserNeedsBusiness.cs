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

        public static List<UserNeeds> FindNeedsList(int type, string userId, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string inviteID="",int needSex=-1, string age="",string address="")
        {
            string sqlwhere = "  a.Status not in (6,9)";
            if (!string.IsNullOrEmpty(userId))
            {
                sqlwhere += " and a.UserID='" + userId+"'";
            }
            if (type > -1)
            {
                sqlwhere += " and a.Type=" + type;
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
                 "a.AutoID,a.UserID,a.UserName,a.Title,a.LetDays,a.InviteName,a.NeedSex,a.NeedType,a.CreateTime,b.province", sqlwhere, "a.AutoID ", pageSize, pageIndex,
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
            string sqlwhere = " a.AutoID='" + autoId + "'";
            int totalCount = 0;
            DataTable dt = CommonBusiness.GetPagerData(" UserNeeds a",
                 "a.AutoID,a.UserID,a.UserName,a.Title,a.Content,a.NeedSex,a.NeedType,a.Type,a.InviteID,a.InviteName,a.LetDays,a.ServiceConten,a.Price,a.Status", sqlwhere, "a.AutoID ", 1, 1,
                out totalCount, out totalCount);
            UserNeeds model = new UserNeeds();
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
                needs.Content, needs.LetDays, needs.ServiceConten, needs.NeedSex, needs.Price, needs.NeedType);
            if (result && needs.Type==0)
             {
                 LogBusiness.AddOperateLog(needs.UserID,needs.UserName,needs.UserLevelID,"","",EnumUserOperateType.SendLog, 
                     needs.Title,operateip);
             }
             return result;
        }
        #endregion
       
    }
}
