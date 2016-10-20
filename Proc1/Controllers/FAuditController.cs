using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProBusiness;

namespace Proc.Controllers
{
    public class FAuditController : BaseController
    {
        //
        // GET: /FAudit/

        public ActionResult ImgAudit()
        {
            return View();
        }
        public ActionResult NeedsAudit()
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

        #endregion
    }
}
