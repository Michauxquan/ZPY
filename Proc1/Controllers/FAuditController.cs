using ProBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proc.Controllers
{
    public class FAuditController : BaseController
    {
        //
        // GET: /FAudit/

        public ActionResult ImgAudit()
        {
            ViewBag.Url = ProTools.Common.GetKeyValue("Url");
            return View();
        }
        public ActionResult NeedsAudit()
        {
            return View();
        }
        public ActionResult OrdersAudit()
        {
            return View();
        }
        #region Ajax

        public JsonResult ImgList( int Status,string Keywords,string BeginTime,string EndTime,int PageIndex,int PageSize)
        {
            int totalCount = 0;
            int pageCount = 0;
            var result = UserImgsBusiness.GetUserImgList(Keywords, Status, BeginTime, EndTime, PageIndex, PageSize,
                ref totalCount, ref pageCount);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            JsonDictionary.Add("items", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult ImgAuditing(string ids, int status)
        { 
            var result = UserImgsBusiness.UpdateStatus(ids, status); 
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult NeedsList(string keywords, string type, string status, string beginTime, string endTime, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            int pageCount = 0;
            var result = UserNeedsBusiness.FindNeedsList(keywords,type, "", status, pageSize, pageIndex, ref totalCount, ref pageCount, beginTime, endTime);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            JsonDictionary.Add("items", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateNeedStatus(string ids, int status)
        {
            ids=ids.Trim(',');
            var result = UserNeedsBusiness.UpdateStatus(ids, status,CurrentUser.UserID);
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult OrdersList(int paytype, int status, string keywords, string userID, string beginTime,
            string endTime, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            int pageCount = 0;
            var result = UserOrdersBusiness.GetUserOrders(keywords, userID, -1, status, paytype,pageSize,pageIndex,ref totalCount, ref pageCount, beginTime, endTime);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            JsonDictionary.Add("items", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }
        public JsonResult BoutOrder(string ordercode)
        {
            var result = UserOrdersBusiness.BoutOrder(ordercode);
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult OrderAuditing(string ordercode)
        {
            string msg = "";
            var result = false;
            var model = UserOrdersBusiness.GetUserOrderDetail(ordercode);
            if (model != null && model.Status ==0)
            {
                result = UserOrdersBusiness.OrderAuditting(ordercode, "", model.TotalFee);
            }
            else
            {
                msg = "订单状态不正确";
            }
            JsonDictionary.Add("errorMsg", msg);
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion
    }
}
