using ProDAL;
using ProEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBusiness
{
    public class UserImgsBusiness
    {
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
