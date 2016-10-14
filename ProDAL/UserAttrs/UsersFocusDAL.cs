using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class UsersFocusDAL:BaseDAL
    {
        public static UsersFocusDAL BaseProvider = new UsersFocusDAL();

        public bool CreateRelation(string userid, string focusid)
        {
            string sql = "INSERT INTO UsersFocus(UserID,FocusID,CreateTime ) " +
                        " values(@UserID,@FocusID,getdate())";

            SqlParameter[] paras = { 
                                       new SqlParameter("@UserID",userid),
                                       new SqlParameter("@FocusID",focusid)
                                   };

            return ExecuteNonQuery(sql, paras, CommandType.Text) > 0;
        }

        public bool DeleteUserRelation(string userid, string focusid)
        {
            SqlParameter[] paras = {new SqlParameter("@focusid",focusid),
                                       new SqlParameter("@userid",userid)
                                   };
            return ExecuteNonQuery("delete from UsersFocus  where focusid=@focusid and userid=@userid", paras, CommandType.Text) > 0;
        }

    }
}
