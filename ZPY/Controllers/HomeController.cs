using ProEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProBusiness;
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
                return Redirect("/User/UserInfo");
            }
            HttpCookie cook = Request.Cookies["intfactory_system"];
            if (cook != null)
            {
                if (cook["status"] == "1")
                {
                    string operateip = OperateIP;
                    int result;
                    M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(cook["username"], cook["pwd"], operateip, out result);
                    if (model != null)
                    {
                        Session["Manager"] = model;
                        return Redirect("/User/UserInfo");
                    }
                }
            }
            return View();
        }

        public ActionResult Logout()
        { 
            HttpCookie cook = Request.Cookies["zpy"];
            if (cook != null)
            {
                cook["status"] = "0";
                Response.Cookies.Add(cook);
            }
            Session["Manager"] = null;
            return Redirect("/Home/Index");
        }

        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public JsonResult UserLogin(string userName, string pwd,string remember="")
        {
            bool bl = false; 
            string operateip =OperateIP;
            int result = 0;
            ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(userName, pwd, operateip, out result);
            if (model != null)
            {
                HttpCookie cook = new HttpCookie("zpy");
                cook["username"] = userName;
                cook["pwd"] = pwd;
                if (remember == "1")
                {
                    cook["status"] = remember;
                }
                cook.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(cook);
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
