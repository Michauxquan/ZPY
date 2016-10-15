using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ProBusiness;
using ProBusiness.Common;
using ProEntity;
using ProEntity.Manage;

namespace ZPY.Controllers
{
    public class UserController : BaseController
    {
        //
        // GET: /User/

        public ActionResult UserInfo()
        {
            if (CurrentUser == null)
            {
                Response.Write("<script>alert('暂未登陆请，登陆后再查看');location.href='/Home/Index'; </script>");
                Response.End(); 
            }
            return View();
        }
        public ActionResult UserMsg(string id)
        {
            M_Users users = M_UsersBusiness.GetUserDetail(id);
            ViewBag.model = users;
            if (CurrentUser != null)
            {
                if (id != CurrentUser.UserID)
                {
                    M_UsersBusiness.CreateUserReport(id, users != null ? users.LoginName : "", "seecount=seecount+1",
                        OperateIP, CurrentUser.UserID, CurrentUser.LoginName, CurrentUser.Levelid);
                }
            }
            return View();
        }

        public JsonResult UserActions(string type,int pageIndex,int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = LogBusiness.GetLogs(type, pageSize, pageIndex, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetUserRated(string type,string userid, int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = LogBusiness.GetLogs(type, pageSize, pageIndex, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UserMyFocus(int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = UsersFocusBusiness.GetPagList(CurrentUser.UserID, pageSize, pageIndex, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UserMyInfo()
        {
            JsonDictionary.Add("item", CurrentUser); 
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult Focususer(string id)
        {
            var result = 1;
            if (CurrentUser == null)
            {
                result = -1;
            }
            else
            {
                if (id != CurrentUser.UserID)
                {
                    M_UsersBusiness.CreateUserFocus(CurrentUser.UserID, id);
                }
                else
                {
                    result = -2;
                }
            }
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveUserInfo(string bHeight,string bWeight,string jobs,string bPay,int isMarry,
            string myContent,string name ,string talkTo,int age)
        {
            M_Users users = CurrentUser;
            users.BHeight = bHeight;
            users.BWeight = bWeight;
            users.Jobs = jobs;
            users.TalkTo = talkTo;
            users.BPay = bPay;
            users.IsMarry = isMarry;
            users.Age = age;
            users.Name = bPay;
            users.IsMarry = isMarry;
            users.MyContent = myContent;
            var result = M_UsersBusiness.UpdateM_UserBase(CurrentUser.UserID, bHeight, bWeight, jobs, bPay, isMarry,
                myContent, name, talkTo,age);
            if (result)
            {
                Session["Manage"] = users;
            }
            JsonDictionary.Add("result",result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveNeeds(string entity)
        {
            UserNeeds needs = JsonConvert.DeserializeObject<UserNeeds>(entity);
            needs.UserID = CurrentUser.UserID;
            needs.UserName = CurrentUser.Name;
            needs.UserLevelID = CurrentUser.Levelid; 
            needs.Content = needs.Content;   
            JsonDictionary.Add("result", UserNeedsBusiness.CreateNeeds(needs,OperateIP));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UserDiaryList(string type, int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserNeedsBusiness.FindNeedsList(type, CurrentUser.UserID, pageSize, pageIndex, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult NeedsList(string type,bool ismyself, int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserNeedsBusiness.FindNeedsList(type, ismyself ? CurrentUser.UserID : "", pageSize, pageIndex, ref total, ref pageCount, !ismyself?CurrentUser.UserID:"");
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetNewNeeds(string type,int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserNeedsBusiness.FindNeedsList(type,  "", pageSize, pageIndex, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult NeedsDetail (int autoid)
        {
            var model = UserNeedsBusiness.FindNeedsDetail(autoid);
            JsonDictionary.Add("item", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetUserRateds(int type,string userid, int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserNeedsBusiness.FindNeedsRated(type, userid, pageSize, pageIndex, ref total, ref pageCount);
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserLinkInfo(string cname, string seeid, string seename)
        {
            //判断是否有用户登录查看他人联系信息并扣除金额
            string errMsg = "";
            if (CurrentUser != null )
            {
                var ishasGlod = true;
                if (CurrentUser.UserID != seeid)
                {
                    //判断金额是否足够
                    if (!true)
                    {
                        ishasGlod = false;
                        errMsg = "账户金币不足，不能查看";
                    } 
                }
                if (ishasGlod)
                {
                    string result = M_UsersBusiness.GetUserPartInfo(seeid,seename, cname,CurrentUser.UserID,CurrentUser.LoginName,CurrentUser.Levelid
                        ,OperateIP);  
                    JsonDictionary.Add("result", result);
                }
            }
            else
            {
                errMsg="请登录后在操作！";
            }
            JsonDictionary.Add("msgError", errMsg);
           return new JsonResult
           {
               Data = JsonDictionary,
               JsonRequestBehavior = JsonRequestBehavior.AllowGet
           };
        }
    }
}
