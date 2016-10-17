using ProDAL;
using ProEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProEntity.Manage;

namespace ProBusiness
{
    public class UserImgsBusiness
    {
        #region  查询

        public static List<M_Users> GetImgList(string userid, int sex, int pageIndex, int pageSize,ref int totalCount,ref int pageCount)
        {

            string whereSql = " a.Status<>9 and b.ImgCount>0";

            if (sex > -1)
            {
                whereSql += " and a.Sex=" + sex;
            } 
             
            string clumstr = "a.userID,a.Avatar,a.Name,a.Age,a.LoginName,a.MyService,a.Province,a.City,a.District,a.CreateTime,a.Status,a.Sex,a.IsMarry,a.Education," +
                "a.BHeight,a.Levelid,a.BWeight,a.MyContent,a.MyCharacter,a.TalkTo,a.BPay,a.Account,b.ImgCount,b.IsLogin,b.RecommendCount,b.SeeCount";
            DataTable dt = CommonBusiness.GetPagerData("M_Users a join userReport b on a.Userid=b.Userid ", clumstr, whereSql, "a.AutoID", pageSize, pageIndex, out totalCount, out pageCount);
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

        #region 操作

        public static bool Create(UserImgs userimg, string operateip)
        {
            var result = UserImgsDAL.BaseProvider.Create(userimg.UserID, userimg.ImgUrl, userimg.Size);
            if (result)
            {
                ProDAL.Manage.M_UsersDAL.BaseProvider.CreateUserReport(userimg.UserID, " ImgCount=ImgCount+1 ");
            }
            return result;
        }
        #endregion
    }
}
