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
        public ActionResult Register2()
        {
            return View();
        }
        public ActionResult Login()
        {
            if (CurrentUser != null)
            {
                return Redirect("/User/UserInfo");
            }
            HttpCookie cook = Request.Cookies["zpy"];
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
            if (CurrentUser!=null)
            {
                M_UsersBusiness.CreateUserReport(CurrentUser.UserID, CurrentUser.LoginName, " IsLogin=0 ", OperateIP);
            }
            Session["Manager"] = null;
            Session["PartManage"] = null;
            return Redirect("/Home/Index");
        }

        /// <summary>
        /// 用户登录
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
                if (model.Status == 0)
                {
                    Session["PartManage"] = model;
                    Response.Write("<script>alert('还没有注册完成,请继续注册');location.href='/Home/Register2'; </script>");
                    Response.End(); 
                }
                else
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
        public JsonResult UserRegister(string loginname,string pwd)
        {

            var result = !string.IsNullOrEmpty(ProBusiness.M_UsersBusiness.CreateM_UserBase(loginname, pwd));
            if (result)
            {
                var outresult = 0;
                ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(loginname, pwd,
                    OperateIP, out outresult);
                if (model != null)
                {
                    Session["PartManage"] = model;
                }
                else
                { 
                    Response.Write("<script>location.href='/Home/Register2'; </script>");
                    Response.End(); 
                }
            } 
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }
        public JsonResult UserRegister2(string entity)
        {
            M_Users users = JsonConvert.DeserializeObject<M_Users>(entity);
            string lodpwd = users.LoginPWD;
            users.Jobs = "";
            users.OfficePhone = "";
            users.Avatar = "";
            users.CreateUserID = "";
            users.IsAdmin = 0;
            users.RoleID = "";
            users.Description = "";

            var result = !string.IsNullOrEmpty(ProBusiness.M_UsersBusiness.CreateM_User(users));
            if (result)
            {
                var outresult = 0;
                ProEntity.Manage.M_Users model = ProBusiness.M_UsersBusiness.GetM_UserByProUserName(users.LoginName, lodpwd,
                    OperateIP, out outresult);
                if (model != null)
                {  
                    CurrentUser = model;
                    Session["Manager"] = model; 
                }
            }
            JsonDictionary.Add("result", result);

            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
