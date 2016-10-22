using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ProBusiness;
using ProBusiness.Manage;
using ProEntity.Manage;

namespace Proc.Controllers
{
    public class SysSetController : BaseController
    {
        //
        // GET: /SysSet/

        public ActionResult Users()
        {
            ViewBag.Roles = ManageSystemBusiness.GetRoles();
            return View();
        }

        public ActionResult WebUser()
        {
            return View();
        }

        public ActionResult Role()
        {
            return View();
        }
        public ActionResult RolePermission(string id)
        {
            ViewBag.Model = ManageSystemBusiness.GetRoleByID(id);
            ViewBag.Menus = CommonBusiness.ManageMenus.Where(m => m.PCode == ExpandClass.CLIENT_TOP_CODE).ToList();
            return View();
        }
        public ActionResult Feedback()
        {
            return View();
        }

        public ActionResult FeedDetail(string id)
        {
            ViewBag.id = id;
            return View();
        }

        #region Ajax

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        public JsonResult GetRoles()
        {
            var list = ManageSystemBusiness.GetRoles();
            JsonDictionary.Add("items", list);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetRoleByID(string id)
        {
            var model = ManageSystemBusiness.GetRoleByID(id);
            JsonDictionary.Add("model", model);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JsonResult SaveRole(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            M_Role model = serializer.Deserialize<M_Role>(entity);

            if (string.IsNullOrEmpty(model.RoleID))
            {
                model.RoleID = new ManageSystemBusiness().CreateRole(model.Name, model.Description, string.Empty);
            }
            else
            {
                bool bl = new ManageSystemBusiness().UpdateRole(model.RoleID, model.Name, model.Description, string.Empty);
                if (!bl)
                {
                    model.RoleID = "";
                }
            }
            JsonDictionary.Add("model", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleid"></param>
        /// <returns></returns>
        public JsonResult DeleteRole(string roleid)
        {
            int result = 0;
            bool bl = new ManageSystemBusiness().DeleteRole(roleid, CurrentUser.UserID, OperateIP, out result);
            JsonDictionary.Add("status", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleid"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public JsonResult SaveRolePermission(string roleid, string permissions)
        {
            if (permissions.Length > 0)
            {
                permissions = permissions.Substring(0, permissions.Length - 1);

            }
            bool bl = new ManageSystemBusiness().UpdateRolePermission(roleid, permissions, CurrentUser.UserID, OperateIP);
            JsonDictionary.Add("status", bl);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUsers(string keyWords, int pageIndex,int status=-1,int sourcetype=1)
        {
            int totalCount = 0, pageCount = 0;
            var list = M_UsersBusiness.GetUsers(-1, PageSize, pageIndex, ref totalCount, ref pageCount, "", "", sourcetype, status, keyWords);

            JsonDictionary.Add("Items", list);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetUserDetail(string id)
        {
            var item = M_UsersBusiness.GetUserDetail(id);

            JsonDictionary.Add("Item", item);
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        /// <summary>
        /// 新增或修改用户
        /// </summary>
        public JsonResult ValidateLoginName(string loginName)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonDictionary.Add("Info", M_UsersBusiness.GetM_UserCountByLoginName(loginName) > 0 ? "登录名已存在" : "");
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SaveUser(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            M_Users model = serializer.Deserialize<M_Users>(entity);
            JsonDictionary.Add("errmeg", "执行成功");
            if (string.IsNullOrEmpty(model.UserID))
            {
                if (M_UsersBusiness.GetM_UserCountByLoginName(model.LoginName) == 0)
                {
                    model.CreateUserID = CurrentUser.UserID;
                    model.Avatar = "";
                    model.IsAdmin = 0;
                    model.SourceType = 1;
                    model.UserID =  M_UsersBusiness.CreateM_User(model);
                }
                else { JsonDictionary["errmeg"] = "登录名已存在,操作失败"; }
            }
            else
            {
                bool bl = M_UsersBusiness.UpdateM_User(model.UserID, model.Name, model.RoleID, model.Email, model.MobilePhone, model.OfficePhone, model.Jobs,model.Description);
                if (!bl)
                {
                    model.UserID = "";
                }
            }
            if (string.IsNullOrEmpty(model.UserID)) { JsonDictionary["errmeg"] = "操作失败"; }
            JsonDictionary.Add("model", model);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult DeleteMUser(string id)
        {

            bool bl =  M_UsersBusiness.DeleteM_User(id, 9);
            JsonDictionary.Add("status", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult UpdateUserStatus(string id,int status)
        {

            bool bl = M_UsersBusiness.UpdateM_UserStatus(id, status);
            JsonDictionary.Add("status", (bl ? 1 : 0));
            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }


        /// <summary>
        /// 获取举报反馈信息
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="keyWords"></param>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public JsonResult GetFeedBacks(int pageIndex, int type, int status, string keyWords, string beginDate, string endDate)
        {

            int totalCount = 0, pageCount = 0;
            var list = FeedBackBusiness.GetFeedBacks(keyWords, beginDate, endDate, type, status, "", PageSize, pageIndex, out totalCount, out pageCount);
            JsonDictionary.Add("Items", list);
            JsonDictionary.Add("TotalCount", totalCount);
            JsonDictionary.Add("PageCount", pageCount);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetFeedBackDetail(string id)
        {
            var item = FeedBackBusiness.GetFeedBackDetail(id);
            JsonDictionary.Add("Item", item);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateFeedBackStatus(string id, int status, string content)
        {
            bool flag = FeedBackBusiness.UpdateFeedBackStatus(id, status, content);
            JsonDictionary.Add("Result", flag ? 1 : 0);

            return new JsonResult()
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
