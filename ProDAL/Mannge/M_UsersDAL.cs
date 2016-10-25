using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace ProDAL.Manage
{
    public class M_UsersDAL : BaseDAL
    {
        public static M_UsersDAL BaseProvider = new M_UsersDAL();

        public DataTable GetM_UserByUserName(string loginname, string pwd)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@UserName",loginname),
                                    new SqlParameter("@LoginPwd",pwd)
                                   };
            return GetDataTable("select * from M_Users where LoginName=@UserName and LoginPwd=@LoginPwd and Status=1", paras, CommandType.Text);
        }
        public DataSet GetM_UserByProUserName(string loginname, string pwd, int sourceType,out int result)
        {
            result = 0;
            SqlParameter[] paras = {
                                    new SqlParameter("@Result",result),
                                    new SqlParameter("@LoginName",loginname),
                                    new SqlParameter("@LoginPwd",pwd),
                                    new SqlParameter("@SourceType",sourceType)
                                   };
            paras[0].Direction = ParameterDirection.Output;
            DataSet ds = GetDataSet("M_GetM_UserToLogin", paras, CommandType.StoredProcedure, "M_User|Permission");
            result = Convert.ToInt32(paras[0].Value);

            return ds;
        }
        public DataTable GetM_UserByLoginName(string loginname)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@LoginName",loginname)
                                   };
            return GetDataTable("select * from M_Users where LoginName=@LoginName and Status<>9", paras, CommandType.Text);
        }
        public DataTable GetUserDetail(string userID)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@UserID",userID)
                                   };

            return GetDataTable("select * from M_Users where UserID=@UserID", paras, CommandType.Text);
        } 
        public bool SetAdminAccount(string userid,string loginname, string pwd)
        {

            SqlParameter[] paras = { 
                                    new SqlParameter("@userID",userid),
                                    new SqlParameter("@UserName",loginname),
                                    new SqlParameter("@LoginPwd",pwd)
                                   };

            return ExecuteNonQuery("update M_Users set LoginName=@UserName , LoginPwd=@LoginPwd where userID=@userID", paras, CommandType.Text) > 0;
        }

        public bool CreateUserReport(string userid, string keyName)
        { 
            SqlParameter[] paras = { 
                                    new SqlParameter("@Result",SqlDbType.Int),
                                    new SqlParameter("@UserID",userid),
                                    new SqlParameter("@KeyName",keyName)
                                   };
            paras[0].Direction = ParameterDirection.Output;
            ExecuteNonQuery("M_InsertUserReport", paras, CommandType.StoredProcedure);
            var result = Convert.ToInt32(paras[0].Value);
            return result > 0;
        }
        public bool UpdateUserAccount(string userid, decimal golds,int  type,string remark)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@Result",SqlDbType.Int),
                                    new SqlParameter("@UserID",userid),
                                    new SqlParameter("@Type",type),
                                    new SqlParameter("@Golds",golds),
                                    new SqlParameter("@Remark",remark)
                                   };
            paras[0].Direction = ParameterDirection.Output;
            ExecuteNonQuery("M_UserAccountChange", paras, CommandType.StoredProcedure);
            var result = Convert.ToInt32(paras[0].Value);
            return result > 0;
        }
        public int CreateUserFocus(string userid, string seeid)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@Result",SqlDbType.Int),
                                    new SqlParameter("@UserID",userid),
                                    new SqlParameter("@SeeID",seeid)
                                   };
            paras[0].Direction = ParameterDirection.Output;
            ExecuteNonQuery("M_InsertUserFocus", paras, CommandType.StoredProcedure);
            var result = Convert.ToInt32(paras[0].Value);
            return result;
        }

        public bool CreateM_User(string userid, string loginname, string loginpwd,string name,int? isadmin,string roleid,string email,string mobilephone,string officephone,string jobs,string avatar, string description, string operateid
            ,int Sex,string BHeight,string Education,int IsMarry,string Province,string City,string District,string QQ,int SourceType)
        {
            string sql = "INSERT INTO M_Users(UserID,LoginName ,LoginPWD,Name,Email,MobilePhone,OfficePhone,Jobs ,Avatar ,IsAdmin ,Status  ,Description ,CreateUserID ,RoleID,Sex,BHeight,Education,IsMarry,Province,City,District,QQ,SourceType) " +
                        " values(@UserID,@LoginName,@LoginPWD,@Name,@Email,@MobilePhone,@OfficePhone,@Jobs,@Avatar,@IsAdmin,1,@Description,@CreateUserID,@RoleID,@Sex,@BHeight,@Education,@IsMarry,@Province,@City,@District,@QQ,@SourceType)";

            SqlParameter[] paras = { 
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@LoginName",loginname),
                                       new SqlParameter("@LoginPWD",loginpwd),
                                       new SqlParameter("@Name",name),
                                       new SqlParameter("@Email",email),
                                       new SqlParameter("@MobilePhone",mobilephone),
                                       new SqlParameter("@OfficePhone",officephone),
                                       new SqlParameter("@Jobs",jobs),
                                       new SqlParameter("@Avatar",avatar),
                                       new SqlParameter("@IsAdmin",isadmin),
                                       new SqlParameter("@Description",description),
                                       new SqlParameter("@CreateUserID",operateid),
                                       new SqlParameter("@Sex",Sex),
                                       new SqlParameter("@BHeight",BHeight),
                                       new SqlParameter("@Education",Education),
                                       new SqlParameter("@IsMarry",IsMarry),
                                       new SqlParameter("@Province",Province),
                                       new SqlParameter("@City",City),
                                       new SqlParameter("@District",District),
                                       new SqlParameter("@QQ",QQ),
                                       new SqlParameter("@SourceType",SourceType), 
                                       new SqlParameter("@RoleID",roleid)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }

        public bool CreateM_UserBase(string userid, string loginname, string loginpwd)
        {
            string sql = "INSERT INTO M_Users(UserID,LoginName ,LoginPWD,Status) " +
                        " values(@UserID,@LoginName,@LoginPWD,0)";

            SqlParameter[] paras = { 
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@LoginName",loginname),
                                       new SqlParameter("@LoginPWD",loginpwd)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
        public bool UpdateManage_User(string userid, string name, string roleid, string email, string mobilephone, string officephone, string jobs,  string description)
        {
            string sql = "update M_Users set Name=@Name,Email=@Email,MobilePhone=@MobilePhone,OfficePhone=@OfficePhone,Jobs=@Jobs  ,Description=@Description ,RoleID=@RoleID where UserID=@UserID ";

            SqlParameter[] paras = {  
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@Name",name),
                                       new SqlParameter("@Email",email),
                                       new SqlParameter("@MobilePhone",mobilephone),
                                       new SqlParameter("@OfficePhone",officephone),
                                       new SqlParameter("@Jobs",jobs),  
                                       new SqlParameter("@Description",description), 
                                       new SqlParameter("@RoleID",roleid)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
        public bool UpdateM_User(string userid, string avatar)
        {
            string sql = "update M_Users set Avatar=@Avatar where UserID=@UserID ";

            SqlParameter[] paras = {  
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@Avatar",avatar)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
        public bool UpdateM_UserBase(string userid, string bHeight, string bWeight, string jobs, string bPay, int isMarry,
            string myContent, string name, string talkTo, int age, string myservice)
        {
            string sql = "update M_Users set bHeight=@bHeight,bWeight=@bWeight,Name=@Name,TalkTo=@TalkTo,MyService=@MyService," +
                         "bPay=@bPay,Jobs=@Jobs ,IsMarry=@isMarry,myContent=@myContent,Age=@Age  " +
                         "where UserID=@UserID ";

            SqlParameter[] paras = {  
                                       new SqlParameter("@MyService",myservice), 
                                       new SqlParameter("@Name",name), 
                                       new SqlParameter("@Age",age), 
                                       new SqlParameter("@TalkTo",talkTo), 
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@bHeight",bHeight),
                                       new SqlParameter("@bWeight",bWeight),
                                       new SqlParameter("@isMarry",isMarry),
                                       new SqlParameter("@bPay",bPay),
                                       new SqlParameter("@Jobs",jobs),
                                       new SqlParameter("@myContent",myContent) 
                                   };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
        public bool UpdateM_UserInfo(string userid, string bHeight, string bWeight, string jobs, string bPay, int isMarry,string myContent, string name, string talkTo,
            int age, string myservice,DateTime birthday,int sex,string education,string qq,string mobile,string email,string province,string city ,string district)
        {
            string sql = "update M_Users set Status=1,bHeight=@bHeight,bWeight=@bWeight,Name=@Name,TalkTo=@TalkTo,MyService=@MyService,Sex=@Sex,Education=@Education,Province=@Province," +
                         "bPay=@bPay,Jobs=@Jobs ,IsMarry=@isMarry,myContent=@myContent,Age=@Age,BirthDay=@BirthDay,Email=@Email,QQ=@QQ,MobilePhone=@Mobile,City=@City,District=@District " +
                         "where UserID=@UserID ";
            SqlParameter[] paras = {  
                                       new SqlParameter("@MyService",myservice), 
                                       new SqlParameter("@Name",name), 
                                       new SqlParameter("@BirthDay",birthday), 
                                       new SqlParameter("@Sex",sex), 
                                       new SqlParameter("@Education",education), 
                                       new SqlParameter("@QQ",qq), 
                                       new SqlParameter("@Mobile",mobile), 
                                       new SqlParameter("@Email",email), 
                                       new SqlParameter("@Province",province), 
                                       new SqlParameter("@City",city), 
                                       new SqlParameter("@District",district), 
                                       new SqlParameter("@Age",age), 
                                       new SqlParameter("@TalkTo",talkTo), 
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@bHeight",bHeight),
                                       new SqlParameter("@bWeight",bWeight),
                                       new SqlParameter("@isMarry",isMarry),
                                       new SqlParameter("@bPay",bPay),
                                       new SqlParameter("@Jobs",jobs),
                                       new SqlParameter("@myContent",myContent) 
                                   };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
        public bool DeleteM_User(string userid, int status)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@userID",userid),
                                    new SqlParameter("@Status",status),
                                   };

            return ExecuteNonQuery("update M_Users set Status=@Status where userID=@userID", paras, CommandType.Text) > 0;
        }
        public bool UpdateM_UserStatus(string userid, int status)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@userID",userid),
                                    new SqlParameter("@Status",status),
                                   };

            return ExecuteNonQuery("update M_Users set Status=@Status where userID=@userID and Status not in (0,9)", paras, CommandType.Text) > 0;
        }
        public bool Update_userLevel(string userid, string levelID)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@userID",userid),
                                    new SqlParameter("@LevelID",levelID),
                                   };

            return ExecuteNonQuery("update M_Users set LevelID=@LevelID where userID=@userID", paras, CommandType.Text) > 0;
        }

        public bool UpdatePwd(string loginname, string pwd)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@LoginName",loginname),
                                    new SqlParameter("@LoginPwd",pwd),
                                   };

            return ExecuteNonQuery("update M_Users set LoginPwd=@LoginPwd where LoginName=@LoginName", paras, CommandType.Text) > 0;
        }
    }
}
