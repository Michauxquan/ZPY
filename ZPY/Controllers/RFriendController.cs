using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProBusiness;

namespace ZPY.Controllers
{
    public class RFriendController : BaseController
    {
        //
        // GET: /RFriend/
        public ActionResult RentFriend(int id=1)
        {
            ViewBag.Type = id;
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

    }
}
