using ProDAL;
using ProEntity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProBusiness.Manage
{
    public class WebSetBusiness
    {
        private static List<MemberLevel> _memberLevelList;
        public static List<MemberLevel> MemberLevelList
        {
            get
            {
                if (_memberLevelList == null)
                {
                    _memberLevelList = new  List<MemberLevel>();
                }
                return _memberLevelList;
            }
        }
          
        #region 查询
        #endregion
        public static List<MemberLevel> GetMemberLevel()
        {
            if (MemberLevelList.Count>0)
            {
                return MemberLevelList;
            }

            List<MemberLevel> list = new List<MemberLevel>();
            DataTable dt = WebSetDAL.BaseProvider.GetMemberLevel();
            foreach (DataRow dr in dt.Rows)
            {
                MemberLevel model = new MemberLevel();
                model.FillData(dr); 
                list.Add(model);
            }
            MemberLevelList.AddRange(list);
            return list;

        }
        public static MemberLevel GetMemberLevelByID(string levelid)
        {
            if (string.IsNullOrEmpty(levelid))
            {
                return null;
            }
            var list = GetMemberLevel();
            if (list.Where(m => m.LevelID == levelid).Count() > 0)
            {
                return list.Where(m => m.LevelID == levelid).FirstOrDefault();
            }
            MemberLevel model = new MemberLevel();
            DataTable dt = WebSetDAL.BaseProvider.GetMemberLevelByLevelID(levelid);
            if (dt.Rows.Count > 0)
            {
                model.FillData(dt.Rows[0]); 
                list.Add(model);
            }
            return model;
        }
        #region 新增
        public static string CreateMemberLevel(string levelid, string name,  decimal golds, string userid, decimal discountfee, decimal integfeemore, int status = 1, string imgurl = "", int origin = 1)
        {
            imgurl = GetUploadImgurl(imgurl);
            string result = WebSetDAL.BaseProvider.InsertMemberLevel(levelid, name, golds,  userid, discountfee, integfeemore, origin, status, imgurl);
            if (string.IsNullOrEmpty(result))
            {
                MemberLevelList.Add(new MemberLevel()
                {
                    Golds = golds,
                    LevelID = levelid,
                    DiscountFee = discountfee,
                    Name = name,
                    ImgUrl = imgurl,
                    Origin = origin, 
                    IntegFeeMore = integfeemore,
                    CreateUserID = userid,
                    CreateTime = DateTime.Now,
                    Status = 0
                });
            }
            return result;
        }
        #endregion

        #region 改
        public static  string UpdateMemberLevel(decimal golds, string levelid, string name, decimal discountfee, decimal integfeemore, string imgurl)
        {
            var model = GetMemberLevelByID(levelid);
            if (model == null)
            {
                return "会员等级已被删除,操作失败";
            }
            imgurl = GetUploadImgurl(imgurl);
            string result = WebSetDAL.BaseProvider.UpdateMemberLevel(golds, levelid, name, discountfee, integfeemore, imgurl);
            if (string.IsNullOrEmpty(result))
            {
                model.Name = name;
                model.DiscountFee = discountfee;
                model.IntegFeeMore = integfeemore;
                model.ImgUrl = imgurl;
            }
            return result;
        }

        public static string DeleteMemberLevel(string levelid)
        {
            var model = GetMemberLevelByID(levelid);
            string bl = WebSetDAL.BaseProvider.DeleteMemberLevel( levelid);
            if (string.IsNullOrEmpty(bl))
            {
                MemberLevelList.Remove(model);
            }
            return bl;
        }
        #endregion
        public static string GetUploadImgurl(string imgurl)
        {
            //if (!string.IsNullOrEmpty(imgurl) && imgurl.IndexOf(TempPath) >= 0)
            //{
            //    DirectoryInfo directory = new DirectoryInfo(HttpContext.Current.Server.MapPath(FILEPATH));
            //    if (!directory.Exists)
            //    {
            //        directory.Create();
            //    }
            //    if (imgurl.IndexOf("?") > 0)
            //    {
            //        imgurl = imgurl.Substring(0, imgurl.IndexOf("?"));
            //    }
            //    FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath(imgurl));
            //    imgurl = FILEPATH + file.Name;
            //    if (file.Exists)
            //    {
            //        file.MoveTo(HttpContext.Current.Server.MapPath(imgurl));
            //    }
            //}
            return imgurl;
        }
    }
}
