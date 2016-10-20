using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProDAL
{
    public class WebSetDAL : BaseDAL
    {
        public static WebSetDAL BaseProvider = new WebSetDAL();
        public DataTable GetMemberLevel()
        { 

            DataTable dt = GetDataTable("select * from MemberLevel where  Status=1 order by Origin");

            return dt;
        }
        public DataTable GetAdvertSetList(string imgType ,string view)
        {
            string sqlwhere = "";
            if (!string.IsNullOrEmpty(imgType))
            {
                sqlwhere += " and imgType='" + imgType + "' ";
            }
            if (!string.IsNullOrEmpty(view))
            {
                sqlwhere += " and view='" + view + "' ";
            }
            DataTable dt = GetDataTable("select * from AdvertSet where 1=1 "+sqlwhere);

            return dt;
        }
        public DataTable GetMemberLevelByLevelID(string levelID)
        {
            string sqlText = "select * from MemberLevel where LevelID=@LevelID and Status=1";
            SqlParameter[] paras = { 
                                     new SqlParameter("@LevelID",levelID)
                                   };

            return GetDataTable(sqlText, paras, CommandType.Text);
        }
        public DataTable GetMemberLevelByOrigin(string origin)
        {
            string sqlText = "select * from MemberLevel where origin=@Origin  and Status=1 ";
            SqlParameter[] paras = { 
                                     new SqlParameter("@Origin",origin)
                                   };

            return GetDataTable(sqlText, paras, CommandType.Text);
        }

        #region 新增
        public string InsertMemberLevel(string levelid, string name, decimal golds,  string userid, decimal discountfee, decimal integfeemore, int origin, int status = 1, string imgurl = "")
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@result" , SqlDbType.VarChar,300),
                                     new SqlParameter("@LevelID" , levelid),
                                     new SqlParameter("@Name" , name), 
                                     new SqlParameter("@Golds" , golds),
                                     new SqlParameter("@DiscountFee" , discountfee),
                                     new SqlParameter("@IntegFeeMore" , integfeemore),
                                     new SqlParameter("@Status" , status),
                                     new SqlParameter("@CreateUserID" , userid),
                                      new SqlParameter("@ImgUrl",imgurl), 
                                    
                                   };
            paras[0].Direction = ParameterDirection.Output;
            origin = ExecuteNonQuery("P_InsertMemberLevel", paras, CommandType.StoredProcedure);
            return Convert.ToString(paras[0].Value);
        }

        public bool InsertAdvert(string userid, string view, string content, string imgtype, string imgurl)
        {
            SqlParameter[] paras =
            { 
                new SqlParameter("@View", view),
                new SqlParameter("@Content", content),
                new SqlParameter("@ImgType", imgtype),
                new SqlParameter("@ImgUrl", imgurl),
                new SqlParameter("@UserID", userid)
            };
            return ExecuteNonQuery("Insert into  AdvertSet([View],[Content],ImgType,ImgUrl,CreateTime,CreateUserID) values (@View,@Content,@ImgType,@ImgUrl,getDate(),@UserID)", paras, CommandType.Text) > 0;
        }

        #endregion

        #region 修改
         public string UpdateMemberLevel(decimal golds, string levelid, string name, decimal discountfee, decimal integfeemore, string imgurl="")
        {  
            SqlParameter[] paras = { 
                                     new SqlParameter("@result" , SqlDbType.VarChar,300),
                                     new SqlParameter("@LevelID",levelid),
                                     new SqlParameter("@Name",name), 
                                     new SqlParameter("@DiscountFee" , discountfee),
                                     new SqlParameter("@IntegFeeMore" , integfeemore),
                                     new SqlParameter("@Golds" , golds),
                                     new SqlParameter("@ImgUrl",imgurl), 
                                   };
            paras[0].Direction = ParameterDirection.Output;
            ExecuteNonQuery("P_UpdateMemberLevel", paras, CommandType.StoredProcedure);
            return paras[0].Value.ToString(); ;
        }
        public string DeleteMemberLevel(string levelid)
        {
            SqlParameter[] paras = { 
                                     new SqlParameter("@result" , SqlDbType.VarChar,300),
                                     new SqlParameter("@LevelID",levelid) 
                                   };
            paras[0].Direction = ParameterDirection.Output;
            ExecuteNonQuery("P_DeleteMemberLevel", paras, CommandType.StoredProcedure);
            return paras[0].Value.ToString();
        }

        public bool UpdateAdvert(int autoid, string view, string content, string imgtype, string imgurl)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@AutoID", autoid),
                new SqlParameter("@View", view),
                new SqlParameter("@Content", content),
                new SqlParameter("@ImgType", imgtype),
                new SqlParameter("@ImgUrl", imgurl)
            };
            return ExecuteNonQuery("Update AdvertSet set [View]=@View,[Content]=@Content,ImgType=@ImgType,ImgUrl=@ImgUrl  where AutoID=@AutoID ", paras, CommandType.Text) > 0;
        }

        public bool DeleteAdvertSet(int autoid)
        {
            SqlParameter[] paras =
            {
                new SqlParameter("@AutoID", autoid)
            };
            return ExecuteNonQuery("Delete from AdvertSet where AutoID=@AutoID ", paras,CommandType.Text) > 0;
        } 
        #endregion
    }
}
