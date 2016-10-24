using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProDAL;
using ProDAL.UserAttrs;
using ProEntity;

namespace ProBusiness
{
   public class UserOrdersBusiness
    {
       public static UserOrdersBusiness BaseBusiness = new UserOrdersBusiness();

        #region 查询

       public static List<UserOrders> GetUserOrders(string keyWords, string userid, int type, int status, int payway, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string begintime = "", string endtime = "")
        {
            string tablename = "UserOrders  a left join M_Users b  on a.UserID =b.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and a.UserName like '%" + keyWords + "%' ";
            }
            if (type > -1)
            {
                sqlwhere += " and a.Type=" + type;
            }
            if (status > -1)
            {
                sqlwhere += " and a.status=" + status;
            }
            if (payway > -1)
            {
                sqlwhere += " and a.PayType=" + payway;
            }
            if (!string.IsNullOrEmpty(userid))
            {
                sqlwhere += " and a.UserID='" + userid + "' ";
            } 
           if (!string.IsNullOrEmpty(begintime))
            {
                sqlwhere += " and a.CreateTime>='" + begintime + " 00:00:00'";
            }
            if (!string.IsNullOrEmpty(endtime))
            {
                sqlwhere += " and a.CreateTime<'" + endtime + " 23:59:59:999'";
            }
            DataTable dt = CommonBusiness.GetPagerData(tablename, "a.*,b.Name as UserName ", sqlwhere, "a.AutoID ", pageSize, pageIndex, out totalCount, out pageCount);
            List<UserOrders> list = new List<UserOrders>();
            foreach (DataRow dr in dt.Rows)
            {
                UserOrders model = new UserOrders();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        #endregion

        #region 添加.删除

       public static bool CreateUserOrder(string levelid, int paytype, string orderCode, string UserID,ref string msg)
        {
            return UserOrdersDAL.BaseProvider.CreateUserOrder(levelid, paytype, orderCode, UserID,out msg);
        }
       public static bool CreateUserOrder(string ordercode, int paytype, string spname, string sku, string content, decimal totalfee, string othercode, int type, decimal num, string userID)
       {
           return UserOrdersDAL.BaseProvider.CreateUserOrder(ordercode, paytype, spname, sku, content, totalfee, othercode, type, num, userID);
       }
        public static bool DeleteReply(string ordercode)
        {
            bool bl = CommonBusiness.Update("UserOrders", "Status", 9, "ordercode='" + ordercode + "'");
            return bl;
        }
        #endregion 
    }
}
