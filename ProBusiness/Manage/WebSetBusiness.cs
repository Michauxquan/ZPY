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
       /// <summary>
       /// 
       /// </summary>
       /// <param name="type">0:会员等级 1 优惠活动</param>
       /// <returns></returns>
        public static List<MemberLevel> GetMemberLevel(int type=0)
        {
           if (MemberLevelList.Count == 0)
           {
               List<MemberLevel> list = new List<MemberLevel>();
               DataTable dt = WebSetDAL.BaseProvider.GetMemberLevel();
               foreach (DataRow dr in dt.Rows)
               {
                   MemberLevel model = new MemberLevel();
                   model.FillData(dr);
                   list.Add(model);
               }
               MemberLevelList.AddRange(list);
           }
           if (type > -1)
           {
               return MemberLevelList.Where(x => x.Type == type).ToList();
           }
           else
           {
               return MemberLevelList.ToList();
           } 
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

        public static List<AdvertSet> GetAdvertSetList(string imgType = "", string view = "")
        {
            List<AdvertSet> list = new List<AdvertSet>();
            DataTable dt = WebSetDAL.BaseProvider.GetAdvertSetList(imgType, view);
            foreach (DataRow dr in dt.Rows)
            {
                AdvertSet model = new AdvertSet();
                model.FillData(dr);
                list.Add(model);
            } 
            return list;
        }

        public static List<ChargeSet> GetChargeSet(string keyWords, string userid,int status, int pageSize, int pageIndex, ref int totalCount, ref int pageCount, string begintime = "", string endtime = "")
        {
            string tablename = " ChargeSet  a left join M_Users b  on a.UserID =b.UserID ";
            string sqlwhere = " a.status<>9 ";
            if (!string.IsNullOrEmpty(keyWords))
            {
                sqlwhere += " and (b.Name like '%" + keyWords + "%' or a.Remark like '%" + keyWords + "%' or a.View like '%" + keyWords + "%')";
            } 
            if (status > -1)
            {
                sqlwhere += " and a.status=" + status;
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
            List<ChargeSet> list = new List<ChargeSet>();
            foreach (DataRow dr in dt.Rows)
            {
                ChargeSet model = new ChargeSet();
                model.FillData(dr);
                list.Add(model);
            }
            return list;
        }

        public static ChargeSet GetChargeSetDetail(string view)
        {
            DataTable dt = WebSetDAL.BaseProvider.GetChargeSetDetail(view.Trim());
            ChargeSet model = null;
            if (dt.Rows.Count == 1)
            {
                model = new ChargeSet();
                model.FillData(dt.Rows[0]);
            }
            return model;
        }

        #endregion
        #region 新增
        public static string CreateMemberLevel(string levelid, string name,  decimal golds, string userid, decimal discountfee, decimal integfeemore, int status = 1, string imgurl = "", int origin = 1,int type=0)
        {
            imgurl = GetUploadImgurl(imgurl);
            string result = WebSetDAL.BaseProvider.InsertMemberLevel(levelid, name.Trim(), golds, userid, discountfee, integfeemore, origin, status, imgurl,type);
            if (string.IsNullOrEmpty(result))
            {
                MemberLevelList.Add(new MemberLevel()
                {
                    Golds = golds,
                    LevelID = levelid,
                    DiscountFee = discountfee,
                    Name = name.Trim(),
                    ImgUrl = imgurl,
                    Origin = origin, 
                    IntegFeeMore = integfeemore,
                    CreateUserID = userid,
                    CreateTime = DateTime.Now,
                    Type = type,
                    Status = 0
                });
            }
            return result;
        }

        public static bool InsertAdvert(AdvertSet model)
        {
            return WebSetDAL.BaseProvider.InsertAdvert(model.CreateUserID,model.View.ToLower(), model.Content, model.ImgType, model.ImgUrl,model.LinkUrl);
        }

        public static bool InsertChargeSet(ChargeSet model)
        {
            return WebSetDAL.BaseProvider.InsertChargeSet(model.UserID, model.View.ToLower(), model.Remark, model.Golds);
        }

        #endregion

        #region 改
        public static  string UpdateMemberLevel(decimal golds, string levelid, string name, decimal discountfee, decimal integfeemore, string imgurl)
        {
            var model = GetMemberLevelByID(levelid);
            if (model == null)
            {
                return "信息已被删除,操作失败";
            }
            imgurl = GetUploadImgurl(imgurl);
            string result = WebSetDAL.BaseProvider.UpdateMemberLevel(golds, levelid, name.Trim(), discountfee, integfeemore, imgurl);
            if (string.IsNullOrEmpty(result))
            {
                var model2 = MemberLevelList.Where(x => x.LevelID == levelid).FirstOrDefault();
                model2.Name = name;
                model2.DiscountFee = discountfee;
                model2.Golds = golds;
                model2.IntegFeeMore = integfeemore;
                model2.ImgUrl = imgurl;
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

        public static bool UpdateAdvert(AdvertSet model)
        {
            return WebSetDAL.BaseProvider.UpdateAdvert(model.AutoID, model.View, model.Content, model.ImgType,
                model.ImgUrl,model.LinkUrl);
        }
        public static bool UpdateChargeSet(ChargeSet model)
        {
            return WebSetDAL.BaseProvider.UpdateChargeSet(model.AutoID, model.View, model.Remark, model.Golds);
        }
        public static bool UpdateChargeSetStatus(int autoid,int status)
        {
            return CommonBusiness.Update("ChargeSet", "Status", status, " where Status<>9 and AutoID=" + autoid);
        }
        public static bool DeleteAdvertSet(int autoid)
        {
            return WebSetDAL.BaseProvider.DeleteAdvertSet(autoid);  
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
