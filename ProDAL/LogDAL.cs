using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class LogDAL : BaseDAL
    {

        public static Task<bool> AddLoginLog(string loginname, int type, string operateip, string userid, string leveid="")
        {
            string sqlText = "insert into UsersLog(UserName,Type,CreateTime,UserID,OperateIP) " +
                            " values(@UserName,@Type,GETDATE(),@OperateIP,@UserID)";
            SqlParameter[] paras = { 
                                     new SqlParameter("@UserName" , loginname),
                                     new SqlParameter("@Type" , type),   
                                     new SqlParameter("@LeveID" , leveid),
                                     new SqlParameter("@UserID" , userid),
                                     new SqlParameter("@OperateIP" , operateip)
                                   };
            return Task.Run(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
        }

        public static Task<bool> AddOperateLog(string userid, string username, string leveid, string seeid, string seename, int type,string message, string operateip)
        {
            string sqlText = "insert into UsersLog(UserID,UserName,LeveID,Type,SeeID,SeeName,Remark,CreateTime,OperateIP) " +
                            " values(@UserID,@UserName,@LeveID,@Type,@SeeID,@SeeName,@Message,GETDATE(),@OperateIP)";
            SqlParameter[] paras = { 
                                     new SqlParameter("@UserID" , userid),
                                     new SqlParameter("@UserName" , username),
                                     new SqlParameter("@Type" , type),
                                     new SqlParameter("@LeveID" , leveid),
                                     new SqlParameter("@SeeID" , seeid),
                                     new SqlParameter("@SeeName" , seename),
                                     new SqlParameter("@Message" , message),
                                     new SqlParameter("@OperateIP" , operateip)
                                   };
            return Task.Run(() => { return ExecuteNonQuery(sqlText, paras, CommandType.Text) > 0; });
        }

    }
}
