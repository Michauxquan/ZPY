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
        /// <summary>
        /// 会员动态
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 会员评价
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 我的关注
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 会员信息
        /// </summary>
        /// <returns></returns>
        public JsonResult UserMyInfo()
        {
            JsonDictionary.Add("item", CurrentUser); 
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /// <summary>
        /// 关注会员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 保存会员信息
        /// </summary>
        /// <param name="bHeight"></param>
        /// <param name="bWeight"></param>
        /// <param name="jobs"></param>
        /// <param name="bPay"></param>
        /// <param name="isMarry"></param>
        /// <param name="myContent"></param>
        /// <param name="name"></param>
        /// <param name="talkTo"></param>
        /// <param name="age"></param>
        /// <param name="myservice"></param>
        /// <returns></returns>
        public JsonResult SaveUserInfo(string bHeight,string bWeight,string jobs,string bPay,int isMarry,
            string myContent, string name, string talkTo, int age, string myservice)
        {
            M_Users users = CurrentUser;
            users.BHeight = bHeight;
            users.BWeight = bWeight;
            users.Jobs = jobs;
            users.TalkTo = talkTo;
            users.BPay = bPay;
            users.IsMarry = isMarry;
            users.MyService = myservice;
            users.Age = age;
            users.Name = name;
            users.IsMarry = isMarry;
            users.MyContent = myContent;
            var result = M_UsersBusiness.UpdateM_UserBase(CurrentUser.UserID, bHeight, bWeight, jobs, bPay, isMarry,
                myContent, name, talkTo,age,myservice);
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
        /// <summary>
        /// 保存需求
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 我的日记
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 租售信息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ismyself"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult NeedsList(string type,bool ismyself, int pageIndex, int pageSize)
        {
            int total = 0;
            int pageCount = 0;
            var list = UserNeedsBusiness.FindNeedsList(type, ismyself ? CurrentUser.UserID : "", pageSize, pageIndex, ref total, ref pageCount, !ismyself?CurrentUser.UserID:"",ismyself?"1,2,3,4,5,6":"");
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
        /// 最新需求
        /// </summary>
        /// <param name="type"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 需求详情
        /// </summary>
        /// <param name="autoid"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 会员评价
        /// </summary>
        /// <param name="type"></param>
        /// <param name="userid"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 查看联系信息
        /// </summary>
        /// <param name="cname"></param>
        /// <param name="seeid"></param>
        /// <param name="seename"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取用户浏览量
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public JsonResult GetUserReport(string userid)
        {
            JsonDictionary.Add("item", LogBusiness.GetUserCount(userid));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpPost]
        public JsonResult UserAvatarUpload(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            string pic = "", error = "";
            if (file != null)
            {
                if (ValidateImg(name))
                {                   
                    try
                    {
                        pic = SaveImg(file, name, "UserAvatar/"); 
                        if (M_UsersBusiness.UpdateM_User(CurrentUser.UserID, pic))
                        {  
                            CurrentUser.Avatar=pic;
                            Session["Manage"] = CurrentUser;
                        }
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }
                }
                else { error = "图片格式不正确"; }
            }
            else { error = "暂未获取到图片信息"; }
            JsonDictionary.Add("msgError", error);
            JsonDictionary.Add("imgUrl", pic);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UserImgUpload(string id, string name, string type, string lastModifiedDate, int size, HttpPostedFileBase file)
        {
            string pic = "", error = "";
            if (CurrentUser == null) { error = "请登录后在操作"; }
            else
            {
                if (file != null)
                {
                    if (ValidateImg(name))
                    {
                        try
                        {
                            pic = SaveImg(file, name, "UserAvatar/");
                            UserImgs imgs = new UserImgs() { GoodCount = 0, Size = size, UserID = CurrentUser.UserID, ImgUrl = pic };
                            UserImgsBusiness.Create(imgs, OperateIP);
                        }
                        catch (Exception ex)
                        {
                            error = ex.Message;
                        }
                    }
                    else { error = "图片格式不正确"; }
                }
                else { error = "暂未获取到图片信息"; }
            }
            JsonDictionary.Add("msgError", error);
            JsonDictionary.Add("imgUrl", pic);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public bool ValidateImg(string fname)
        {
            if (string.IsNullOrEmpty(fname)) { return false; }
            string fileName = fname.Substring(fname.LastIndexOf('.')+1).ToLower();
            string[] imgType = new string[] { "gif", "jpg", "png", "bmp" };
            int i = 0;
            bool blean = false;
            string message = string.Empty;
            //判断是否为Image类型文件
            while (i < imgType.Length)
            {
                if (fileName.Equals(imgType[i].ToString()))
                {
                    blean = true;
                    break;
                }
                else if (i == (imgType.Length - 1))
                {
                    break;
                }
                else
                {
                    i++;
                }
            }
            return blean;
        }

        public string SaveImg(HttpPostedFileBase file,string fileName,string path) {
            string filePhysicalPath = Server.MapPath("~/Upload/" + path + fileName);
            file.SaveAs(filePhysicalPath);
            return "/Upload/" + path + fileName;             
        }
    }
}
