using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class UserNeedsDAL:BaseDAL
    {
        public static UserNeedsDAL BaseProvider = new UserNeedsDAL();

        public bool CreateNeeds(string userID, string userName, int type, string title, string content, int letDays, string serviceConten,
            int needSex,decimal price,int needtype,string needCity,DateTime needDate)
        {
            string sql = @"insert into UserNeeds(UserID,UserName,Type,Status,Title,Content,LetDays,ServiceConten,CreateTime,NeedType,Price,NeedSex,NeedDate,NeedCity)" +
                         "values(@UserID,@UserName,@Type,0,@Title,@Content,@LetDays,@ServiceConten,getdate(),@NeedType,@Price,@NeedSex,@NeedDate,@NeedCity)";
            SqlParameter[] paras = { 
                                    new SqlParameter("@UserID",userID),
                                    new SqlParameter("@UserName",userName),
                                    new SqlParameter("@Type",type),
                                    new SqlParameter("@NeedDate",needDate), 
                                    new SqlParameter("@NeedCity",needCity), 
                                    new SqlParameter("@Title",title), 
                                    new SqlParameter("@Content",content),
                                    new SqlParameter("@LetDays",letDays),
                                    new SqlParameter("@ServiceConten",serviceConten),
                                    new SqlParameter("@Price",price),
                                    new SqlParameter("@NeedSex",needSex),
                                    new SqlParameter("@NeedType",needtype) 
                                   };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }

        public DataTable GetNeedsDetail(int auotid)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@AutoID", auotid)
            };

            return GetDataTable("select * from UserNeeds where AutoID=@AutoID", paras, CommandType.Text);
        }

        public bool UpdateStatus(string autoid,int status,string operater )
        {
            //string sql =
            //    @"update   UserNeeds set  Status=@Status ,UpdateTime=getdate(),UpdateID=@UpdateID where AutoID in (@AutoID)";
            SqlParameter[] paras =
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@AutoID", autoid),
                new SqlParameter("@UpdateID", operater)
            };
            return ExecuteNonQuery("P_UserNeedUpdateStatus", paras, CommandType.StoredProcedure) > 0;
        }
        public bool UpdateAcceptStatus(string autoid, int status, string operater, string inviteName)
        {
            string sql =
                @"update   UserNeeds set  Status=@Status ,InviteTime=getdate(),InviteID=@UpdateID,InviteName=@InviteName where AutoID=@AutoID";
            SqlParameter[] paras =
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@AutoID", autoid),
                new SqlParameter("@UpdateID", operater),
                new SqlParameter("@InviteName", inviteName)
            };
            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }
    }
}
