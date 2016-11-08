using ProBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ZPY.Controllers
{
    public class RFriendController : BaseController
    {
        //
        // GET: /RFriend/
        public ActionResult RentFriend(int id = 1, string address = "", string agerange = "")
        {
            ViewBag.Type = id;
            ViewBag.AgeRange = agerange;
            var userAddress = "";
            if (CurrentUser != null)
            {
               userAddress= string.IsNullOrEmpty(CurrentUser.Province)
                        ? ""
                        : (CurrentUser.Province + (string.IsNullOrEmpty(CurrentUser.City) ? "" : "," + CurrentUser.City)); 
            }
            ViewBag.UserAddress = userAddress;
            ViewBag.Address = string.IsNullOrEmpty(address) ? userAddress : address;
            return View();
        }

        public ActionResult Recommend(int id = -1)
        {
            ViewBag.Type = id; 
            var userAddress = "";
            if (CurrentUser != null)
            {
                userAddress = string.IsNullOrEmpty(CurrentUser.Province)
                         ? ""
                         : (CurrentUser.Province + (string.IsNullOrEmpty(CurrentUser.City) ? "" : "," + CurrentUser.City));
            }
            ViewBag.UserAddress = userAddress; 
            return View();
        }

        public ActionResult UserPic(string id="")
        {
            //if (CurrentUser == null)
            //{
            //    Response.Write("<script>alert('暂未登陆请，登陆后再查看');location.href='/Home/Index'; </script>");
            //    Response.End();
            //}
            ViewBag.UserID = id;
            ViewBag.CurID = CurrentUser != null ? CurrentUser.UserID : "";
            return View();
        }

        public ActionResult HireMsg(int sex = -1,string address="",string agerange="")
        {
            ViewBag.Sex = sex;
            ViewBag.Address = address;
            ViewBag.AgeRange = agerange;
            return View();
        }
        public ActionResult HireDetail(int id)
        {
            ViewBag.model  = UserNeedsBusiness.FindNeedsDetail(id);
            return View();
        }
        public JsonResult GetUserInfoByType(int sex,int pageIndex,int pageSize,string address="",string age="")
        {
            int total = 0;
            int pageCount = 0;
            var list = M_UsersBusiness.GetUsers(sex, pageSize, pageIndex, ref total, ref pageCount, address,age);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserRecommenCount(int sex, int pageIndex, int pageSize, string address = "", string age = "",string cdesc="")
        {
            int total = 0;
            int pageCount = 0;
            var list = M_UsersBusiness.GetUsersReCommen(sex, pageSize, pageIndex, ref total, ref pageCount, address, age);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /// <summary>
        /// 获取聘请信息
        /// </summary>
        /// <param name="sex"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <param name="age"></param>
        /// <param name="cdesc"></param>
        /// <returns></returns>
        public JsonResult GetUserNeedsList(int sex, int pageIndex, int pageSize,string type="1",string address = "", string age = "", string cdesc = "")
        {
            int total = 0;
            int pageCount = 0;
            var list = UserNeedsBusiness.FindNeedsList(type, "", pageSize, pageIndex, ref total, ref pageCount, "","", sex, age, address);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetImgList(string userid,int pageIndex,int pageSize,int sex=-1)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserImgsBusiness.GetImgList(userid,sex, pageIndex,pageSize, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetAllImgByuserID(string userid, int status, int pageSize, int pageIndex)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserImgsBusiness.GetUserImgList(userid, status, pageIndex, pageSize, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            
        }

        public JsonResult GetNewImg(int tops=6, int status = 1)
        { 
            var list = UserImgsBusiness.GetNewImg(tops, status);
            JsonDictionary.Add("items", list); 
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
