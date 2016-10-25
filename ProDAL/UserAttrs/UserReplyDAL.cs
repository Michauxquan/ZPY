using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL.UserAttrs
{
    public class UserReplyDAL:BaseDAL
    {
        public static UserReplyDAL BaseProvider = new UserReplyDAL();

        public bool CreateUserReply(string guid, string content, string userID, string fromReplyID, string fromReplyUserID,int type)
        {
            string replyID = Guid.NewGuid().ToString();
            string @sql = "insert into UserReply (ReplyID,GUID,[Content],FromReplyID,FromReplyUserID,CreateUserID,Type) " +
                          "values(@ReplyID,@GUID,@Content,@FromReplyID,@FromReplyUserID,@CreateUserID,@Type)";
            SqlParameter[] paras = { 
                                     new SqlParameter("@ReplyID",replyID),
                                     new SqlParameter("@GUID",guid),
                                     new SqlParameter("@Content",content),
                                     new SqlParameter("@FromReplyID",fromReplyID),
                                     new SqlParameter("@CreateUserID" , userID), 
                                      new SqlParameter("@Type" , type), 
                                     new SqlParameter("@FromReplyUserID" , fromReplyUserID),
                                   };

            return ExecuteNonQuery(@sql, paras, CommandType.Text) > 0;
        }
        public DataTable GetReplyDetail(string replyid)
        {
            SqlParameter[] paras = { 
                                    new SqlParameter("@ReplyID",replyid)
                                   };

            return GetDataTable("select * from UserReply where ReplyID=@ReplyID ", paras, CommandType.Text);
        }
    }
}
