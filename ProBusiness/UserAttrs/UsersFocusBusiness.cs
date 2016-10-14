using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ProBusiness.Common;
using ProBusiness.Manage;
using ProDAL;
using ProEntity.Manage;
using ProDAL.Manage;
using ProEntity;
using ProEnum;


namespace ProBusiness
{
    public class UsersFocusBusiness
    {
        #region 查询   
        #endregion
        public static  List<UsersFocus>  GetPagList(string userid, int pageSize, int pageIndex, ref int totalCount, ref int pageCount)
        {
            string   sqlwhere=" b.UserID='" + userid + "'"; 
            DataTable dt = CommonBusiness.GetPagerData(" UsersFocus b left join M_Users a on a.userid=b.focusid",
                "b.*,a.LoginName as FocusName ,a.Avatar as FocusAvatar", sqlwhere, "b.AutoID ", pageSize, pageIndex,
                out totalCount, out pageCount);
            List<UsersFocus> list = new List<UsersFocus>();
            foreach (DataRow dr in dt.Rows)
            {
                UsersFocus model = new UsersFocus();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        #region 改 
        /// <summary>
        /// 新增用户关注
        /// </summary>
        public static bool CreateM_User(string userid, string focusid)
        {
            return UsersFocusDAL.BaseProvider.CreateRelation(userid, focusid);
            
        } 
        public static  bool DeleteM_User(string userid,string focusid) {
            return UsersFocusDAL.BaseProvider.DeleteUserRelation(userid, focusid);
        }
        #endregion

    }

    

    
}
