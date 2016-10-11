using ProEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProEntity.Manage;

namespace ZPY.Controllers
{
    public class HomeController : BaseController
    {
        //
        // GET: /Home/

        public ActionResult Index()
        { 
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (CurrentUser != null)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public ActionResult Logout()
        {
            CurrentUser = null;
            return Redirect("/Home/Index");
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

            string operateip = string.IsNullOrEmpty(Request.Headers.Get("X-Real-IP")) ? Request.UserHostAddress : Request.Headers["X-Real-IP"];
            int result = 0;
            ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(userName, pwd, operateip, out result);
            if (model != null)
            {
                CurrentUser = model;
                Session["Manager"] = model;                
                bl = true;
            }
            JsonDictionary.Add("result", bl);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UserNameCheck(string userName)
        {
            JsonDictionary.Add("result", ProBusiness.M_UsersBusiness.GetM_UserCountByLoginName(userName)==0);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UserRegister(string entity)
        {
            M_Users users = JsonConvert.DeserializeObject<M_Users>(entity);
            JsonDictionary.Add("result", !string.IsNullOrEmpty(ProBusiness.M_UsersBusiness.CreateM_User(users)));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
