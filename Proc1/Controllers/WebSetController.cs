﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proc.Controllers;
using ProBusiness.Manage;
using ProEntity;
using System.Web.Script.Serialization;
namespace Proc1.Controllers
{
    public class WebSetController : BaseController
    {
        //
        // GET: /WebSet/

        public ActionResult Member()
        {
            return View();
        }
        public ActionResult Advert()
        {
            return View();
        }

        #region Ajax

        public JsonResult GetMemberLevel() 
        {
            JsonDictionary.Add("items", WebSetBusiness.GetMemberLevel());
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SaveMemberLevel(string clientmemberlevel)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<MemberLevel> modelList = serializer.Deserialize<List<MemberLevel>>(clientmemberlevel);
            var tempList = WebSetBusiness.GetMemberLevel();
            modelList.ForEach(x =>
            {
                x.CreateUserID = CurrentUser.UserID; 
                x.Status = 1;
                var temp = tempList.Where(y => y.Origin == x.Origin).FirstOrDefault();
                if (temp != null)
                {
                    x.LevelID = temp.LevelID;
                }
            });
            var delList = tempList.Where(x => !modelList.Exists(y => y.Origin == x.Origin)).OrderByDescending(x => x.Origin).ToList();
            var addList = modelList.Where(x => string.IsNullOrEmpty(x.LevelID)).ToList();
            var updList = modelList.Where(x => !string.IsNullOrEmpty(x.LevelID)).ToList();
            string result = "";
            if (delList.Any())
            {
                delList.ForEach(x =>
                {
                    string tempresult = WebSetBusiness.DeleteMemberLevel(x.LevelID);
                    if (result.IndexOf(tempresult) == -1)
                    {
                        result += tempresult + ",";
                    }
                });
            }
            updList.ForEach(x =>
            {
                result += WebSetBusiness.UpdateMemberLevel(x.Golds, x.LevelID,
                x.Name, x.DiscountFee, x.IntegFeeMore, x.ImgUrl);
            });
            if (addList.Any())
            {
                addList.ForEach(x =>
                {
                    string mes = WebSetBusiness.CreateMemberLevel(Guid.NewGuid().ToString(),
                        x.Name.Trim(), x.Golds, x.CreateUserID, x.DiscountFee,
                        x.IntegFeeMore, x.Status, x.ImgUrl, x.Origin);
                    result += string.IsNullOrEmpty(mes) ? result : mes;
                });
            }
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DeleteMemberLevel(string levelid)
        {
            string result = WebSetBusiness.DeleteMemberLevel( levelid);
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