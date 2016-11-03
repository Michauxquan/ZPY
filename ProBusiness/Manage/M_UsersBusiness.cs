using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using System.Web.ModelBinding;
using ProBusiness.Common;
using ProBusiness.Manage;
using ProEntity.Manage;
using ProDAL.Manage;
using ProEntity;
using ProEnum;


namespace ProBusiness
{
    public class M_UsersBusiness
    {
        #region 查询
        /// <summary>
        /// 根据账号密码获取信息
        /// </summary>
        /// <param name="loginname">账号</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static M_Users GetM_UserByUserName(string loginname, string pwd, string operateip)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);
            DataTable dt = new M_UsersDAL().GetM_UserByUserName(loginname, pwd);
            M_Users model = null;
            if (dt.Rows.Count > 0)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
                if (!string.IsNullOrEmpty(model.RoleID))
                {
                    model.Role = ManageSystemBusiness.GetRoleByID(model.RoleID); 
                }
                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {
                    model.Menus = CommonBusiness.ManageMenus;
                }
                else if (model.IsAdmin == 1)
                {
                    model.Menus = CommonBusiness.ManageMenus;
                }
                else
                {
                    model.Menus = model.Role.Menus;
                }
            }
            return model;
        }
        /// <summary>
        /// 根据账号密码获取信息（登录）
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="pwd"></param>
        /// <param name="operateip"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static M_Users GetM_UserByProUserName(string loginname, string pwd, string operateip, out int result,EnumUserOperateType type=EnumUserOperateType.Login,int sourceType=0)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);
            DataSet ds = new M_UsersDAL().GetM_UserByProUserName(loginname, pwd,sourceType, out result);
            M_Users model = null;
            if (ds.Tables.Contains("M_User") && ds.Tables["M_User"].Rows.Count > 0)
            {
                model = new M_Users();
                model.FillData(ds.Tables["M_User"].Rows[0]);
                if (!string.IsNullOrEmpty(model.RoleID))
                    model.Role = ManageSystemBusiness.GetRoleByIDCache(model.RoleID);
                //权限
                if (model.Role != null && model.Role.IsDefault == 1)
                {
                    model.Menus = CommonBusiness.ManageMenus;
                }
                else if (model.IsAdmin == 1)
                {
                    model.Menus = CommonBusiness.ManageMenus;
                }
                else
                {
                    model.Menus = new List<Menu>();
                    foreach (DataRow dr in ds.Tables["Permission"].Rows)
                    {
                        Menu menu = new Menu();
                        menu.FillData(dr);
                        model.Menus.Add(menu);
                    }
                }
            }
            if (model != null && model.Status==1)
            {
                LogBusiness.AddLoginLog(loginname, operateip, model != null ? model.UserID : "",type, model != null ? model.Levelid : "");
                M_UsersDAL.BaseProvider.CreateUserReport(model.UserID, " IsLogin=1 ");
            }
            return model;
        }
        public static int GetM_UserCountByLoginName(string loginname)
        {
            DataTable dt = new M_UsersDAL().GetM_UserByLoginName(loginname);
            return dt.Rows.Count;
        }
        public static List<M_Users> GetUsers(int sex, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string address = "", string age = "", int sourcetype = 0, int status = -1, string keyWords="")
        {
            string whereSql = " a.Status<>9";

            if (sex > -1)
            {
                whereSql += " and a.Sex=" + sex;
            }
            if (status > -1)
            {
                whereSql += " and a.Status=" + status;
            }
            if (!string.IsNullOrEmpty(keyWords))
            {
                whereSql += " and (a.Name like '%" + keyWords + "%' or a.LoginName like'%" + keyWords + "%') ";
            }
            if (sourcetype > -1)
            {
                whereSql += " and a.sourcetype=" + sourcetype;
            }
            if (!string.IsNullOrEmpty(address))
            {
                string[] strArr = address.Split(',');
                for (int i = 0; i < strArr.Length; i++)
                {
                    whereSql += (i == 0
                        ? " and a. province='"
                        : i == 1 ? " and a.City='" : i == 2 ? " and a.District='" : "") + strArr[i] + "'";
                }
            }

            if (!string.IsNullOrEmpty(age))
            {
                string[] strArr = age.Split('~');
                for (int i = 0; i < strArr.Length; i++)
                {
                    whereSql += (i == 0? " and a.Age>=": " and a.Age<=") + strArr[i];
                } 
            }
            string cstr = @"a.AuthorBTime,a.AuthorETime,a.AuthorType,a.userID,a.Avatar,a.Name,a.LoginName,a.Age,a.MyService,a.Province,a.City,a.District,a.CreateTime,a.Status,a.Sex,a.IsMarry,a.Education,a.ROleID,
a.BHeight,a.Levelid,a.BWeight,a.MyContent,a.MyCharacter,a.BPay,a.Account,a.TalkTo,a.Description";
            DataTable dt = CommonBusiness.GetPagerData("M_Users a", cstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
            List<M_Users> list = new List<M_Users>();
            M_Users model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_Users();
                model.FillData(item);
                if (!string.IsNullOrEmpty(model.RoleID))
                    model.Role = ManageSystemBusiness.GetRoleByIDCache(model.RoleID);
                list.Add(model);
            }

            return list;
        }
         
        public static M_Users GetUserDetail(string userID)
        {
            
            DataTable dt = M_UsersDAL.BaseProvider.GetUserDetail(userID);

            M_Users model=null;
            if (dt.Rows.Count == 1)
            {
                model = new M_Users();
                model.FillData(dt.Rows[0]);
            }
            
            return model;
        }

        public static string GetUserPartInfo(string seeID,string seename, string cname,string userid,string username,string leveid,string operateip )
        {
            object  obj=CommonBusiness.Select("M_Users", cname, " Userid='" + seeID + "'");
            if (obj != null)
            {
                LogBusiness.AddOperateLog(userid, username, leveid, seeID, seename, EnumUserOperateType.SeeLink, "联系方式", operateip);
                return obj.ToString();
            }
            return "";
        }


        public static List<M_Users> GetUsersReCommen(int sex, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string address = "", string age = "",string cdesc="")
        {
            string whereSql = " a.Status<>9";

            if (sex > -1)
            {
                whereSql += " and a.Sex=" + sex;
            }

            if (!string.IsNullOrEmpty(address))
            {
                string[] strArr = address.Split(',');
                for (int i = 0; i < strArr.Length; i++)
                {
                    whereSql += (i == 0
                        ? " and a.province='"
                        : i == 1 ? " and a.City='" : i == 2 ? " and a.District='" : "") + strArr[i] + "'";
                }
            }

            if (!string.IsNullOrEmpty(age))
            {
                string[] strArr = age.Split('~');
                for (int i = 0; i < strArr.Length; i++)
                {
                    whereSql += (i == 0 ? " and a.Age>=" : " and a.Age<=") + strArr[i];
                }
            }
            string clumstr = "a.AuthorBTime,a.AuthorETime,a.AuthorType,a.userID,a.Avatar,a.Name,a.Age,a.LoginName,a.MyService,a.Province,a.City,a.District,a.CreateTime,a.Status,a.Sex,a.IsMarry,a.Education," +
                "a.BHeight,a.Levelid,a.BWeight,a.MyContent,a.MyCharacter,a.TalkTo,a.BPay,a.Account,b.ImgCount,b.IsLogin,b.RecommendCount,b.SeeCount";
            DataTable dt = CommonBusiness.GetPagerData("M_Users a left join userReport b on a.Userid=b.Userid ", clumstr, whereSql, "a.AutoID" + (string.IsNullOrEmpty(cdesc) ? "" : "," + cdesc), pageSize, pageIndex, out totalCount, out pageCount);
            List<M_Users> list = new List<M_Users>();
            M_Users model;
            foreach (DataRow item in dt.Rows)
            {
                model = new M_Users();
                model.FillData(item);
                list.Add(model);
            }

            return list;
        }


        #endregion

        #region 改
        /// <summary>
        /// 修改管理员账户
        /// </summary>
        public static bool SetAdminAccount(string userid, string loginname, string pwd)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);

            return M_UsersDAL.BaseProvider.SetAdminAccount(userid, loginname, pwd);
        }
        public static bool UpdateM_User(string userid, string name, string roleid, string email, string mobilephone, string officephone, string jobs, string description)
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateManage_User(userid, name, roleid, email, mobilephone, officephone, jobs, description);
            return bl;
        }
        /// <summary>
        /// 新增或修改用户信息
        /// </summary>
        public static string CreateM_User(M_Users musers)
        {
            string userid = Guid.NewGuid().ToString();
            musers.LoginPWD = ProBusiness.Encrypt.GetEncryptPwd(musers.LoginPWD, musers.LoginName);
            bool bl = M_UsersDAL.BaseProvider.CreateM_User(userid, musers.LoginName, musers.LoginPWD, 
                string.IsNullOrEmpty(musers.Name) ? "" : musers.Name, musers.IsAdmin, musers.RoleID, musers.Email, musers.MobilePhone,
                musers.OfficePhone, musers.Jobs, musers.Avatar, musers.Description, musers.CreateUserID,
                musers.Sex.Value,musers.BHeight,musers.Education,musers.IsMarry.Value,musers.Province,musers.City,
                musers.District,musers.QQ,musers.SourceType);
            return bl ? userid : "";
        }

        public static string CreateM_UserBase(string loginname, string loginpwd)
        {
            string userid = Guid.NewGuid().ToString();
            string userPwd = ProBusiness.Encrypt.GetEncryptPwd(loginpwd, loginname);
            bool bl = M_UsersDAL.BaseProvider.CreateM_UserBase(userid, loginname, userPwd);
            return bl ? userid : "";
        }

        public static bool CreateUserReport(string seeid, string seename, string keyName, string operateip, string userid = "", string username = "", string levelid="")
        {

            var bl = M_UsersDAL.BaseProvider.CreateUserReport(seeid, keyName);
            if (!string.IsNullOrEmpty(userid))
            {
                LogBusiness.AddOperateLog(userid, username, levelid, seeid, seename, EnumUserOperateType.SeeUser, "",
                    operateip);
            }
            return bl;
        }
        public static bool CreateUserFocus(string userid,  string seeid = "")
        {

            return M_UsersDAL.BaseProvider.CreateUserFocus(userid, seeid)>0; 
        }
        /// <summary>
        /// 修改用户户信息
        /// </summary>
        public static bool UpdateM_User(string userid,string avatar)
        {
            bool bl = M_UsersDAL.BaseProvider.UpdateM_User(userid, avatar); 
            return bl;
        }
        public static bool UpdatePwd(string loginname, string pwd)
        {
            pwd = ProBusiness.Encrypt.GetEncryptPwd(pwd, loginname);
            bool bl = M_UsersDAL.BaseProvider.UpdatePwd(loginname, pwd);
            return bl;
        }
        public static  bool DeleteM_User(string userid, int status) {
            return M_UsersDAL.BaseProvider.DeleteM_User(userid, status);
        }
        public static bool UpdateM_UserStatus(string userid, int status)
        {
            return M_UsersDAL.BaseProvider.UpdateM_UserStatus(userid, status);
        }
        public static bool UpdateM_UserBase(string userid, string bHeight, string bWeight, string jobs, string bPay, int isMarry, 
            string myContent,string name ,string talkTo,int age,string myservice)
        {
            return M_UsersDAL.BaseProvider.UpdateM_UserBase(userid, bHeight, bWeight, jobs, bPay, isMarry, myContent, name, talkTo, age, myservice); 
        }
        public static bool UpdateM_UserBase(M_Users user)
        {
            return M_UsersDAL.BaseProvider.UpdateM_UserInfo(user.UserID, user.BHeight, user.BWeight, user.Jobs, user.BPay, user.IsMarry.Value,
                user.MyContent, user.Name, user.TalkTo, user.Age.Value, user.MyService,user.BirthDay,user.Sex.Value,user.Education,user.QQ,user.MobilePhone,user.Email,user.Province,user.City,user.District);
        }

        public static bool CheckEmail(string loginname, string email)
        {
           var result= CommonBusiness.Select("M_Users", "count(1)", " LoginName='" + loginname + "' and email='" + email + "' ");
            return Convert.ToInt32(result) > 0;
        }

        #endregion

    }

    

    
}
