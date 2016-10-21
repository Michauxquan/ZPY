using ProEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProEnum;

namespace Proc.Controllers
{
    public class HomeController :Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (Session["ZPYManager"] == null)
            {
                return Redirect("/Home/Login");
            }
            return View();
        }

        public ActionResult Login()
        {
            if (Session["ZPYManager"] != null)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["ZPYManager"] = null;
            return Redirect("/Home/Login");
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult UserLogin(string userName, string pwd)
        {
            bool bl = false;
            Dictionary<string, object> JsonDictionary = new Dictionary<string, object>();
            string operateip = string.IsNullOrEmpty(Request.Headers.Get("X-Real-IP")) ? Request.UserHostAddress : Request.Headers["X-Real-IP"];
            int result = 0;
            ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(userName, pwd, operateip, out result,EnumUserOperateType.Manage,1);
            if (model != null)
            {
                Session["ZPYManager"] = model;
                Session["ZPYManager"] = model;                
                bl = true;
            }
            JsonDictionary.Add("result", bl);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

    }
}
