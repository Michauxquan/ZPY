using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Proc.Controllers;
using ProBusiness.Manage;
using ProEntity;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Schema;
using ProBusiness;
using ProEnum;
using ProTools;

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
            ViewBag.Url = Common.GetKeyValue("WebUrl");
            return View();
        }

        public ActionResult ChargeSet()
        {
            return View();
        }

        public ActionResult Activity()
        {
            ViewBag.integerFee = CommonBusiness.getSysSetting(EnumSettingKey.GoldScale, "DValue");
            return View();
        }

        #region Ajax

        public JsonResult GetMemberLevel(int  type=0) 
        {
            JsonDictionary.Add("items", WebSetBusiness.GetMemberLevel(type));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SaveMemberLevel(string memberlevel)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            List<MemberLevel> modelList = serializer.Deserialize<List<MemberLevel>>(memberlevel);
            var tempList = WebSetBusiness.GetMemberLevel(modelList[0].Type);
            modelList.ForEach(x =>
            {
                x.CreateUserID = CurrentUser.UserID; 
                x.Status = 1;
                var temp = tempList.Where(y => y.Origin == x.Origin && y.Type==x.Type).FirstOrDefault();
                if (temp != null)
                {
                    x.LevelID = temp.LevelID;
                }
            });
            var delList = tempList.Where(x => !modelList.Exists(y => y.Origin == x.Origin && y.Type == x.Type)).OrderByDescending(x => x.Origin).ToList();
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
                        x.IntegFeeMore, x.Status, x.ImgUrl, x.Origin,x.Type);
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



        public JsonResult GetAdvertList()
        {
            JsonDictionary.Add("items", WebSetBusiness.GetAdvertSetList());
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DeleteAdvertSet(int autoid)
        {
            JsonDictionary.Add("result", WebSetBusiness.DeleteAdvertSet(autoid));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult SaveAdvert(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            AdvertSet model = serializer.Deserialize<AdvertSet>(entity);
            var result = false;
            if (model.AutoID == -1)
            {
                model.CreateUserID = CurrentUser.UserID;
                 result = WebSetBusiness.InsertAdvert(model);
            }
            else
            {
                result = WebSetBusiness.UpdateAdvert(model);
            }
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult SaveGoldRule(int integerFee)
        {
            var result = false;
            JsonDictionary.Add("result", CommonBusiness.SetSysSetting(EnumSettingKey.GoldScale,integerFee,CurrentUser.UserID));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult GetChargeList(int status, string keywords, string userID, string beginTime,
            string endTime, int pageIndex, int pageSize)
        {
            int totalCount = 0;
            int pageCount = 0;
            var result = WebSetBusiness.GetChargeSet(keywords, userID, status, pageSize, pageIndex, ref totalCount, ref pageCount, beginTime, endTime);
            JsonDictionary.Add("totalCount", totalCount);
            JsonDictionary.Add("pageCount", pageCount);
            JsonDictionary.Add("items", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            }; 
        }

        public JsonResult SaveCharge(string entity)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            ChargeSet model = serializer.Deserialize<ChargeSet>(entity);
            var result = false;
            if (model.AutoID == -1)
            {
                model.UserID = CurrentUser.UserID;
                result = WebSetBusiness.InsertChargeSet(model);
            }
            else
            {
                result = WebSetBusiness.UpdateChargeSet(model);
            }
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult UpdateChargeStatus(int autoid,int status)
        { 
            var result = false;
            result = WebSetBusiness.UpdateChargeSetStatus(autoid,status); 
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
