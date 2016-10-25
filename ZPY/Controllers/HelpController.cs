using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using ProBusiness;
using ProBusiness.Manage;
using ProBusiness.UserAttrs;
using ProEntity;
using ProEntity.Manage;
using ProEnum;
using ProTools;
namespace ZPY.Controllers
{
    public class HelpController : BaseController
    {
        //
        // GET: /Help/

        public ActionResult Helps()
        {
            return View();
        }
        public ActionResult Forget()
        {
            return View();
        }

        public JsonResult RestPwd(string loginname,string useremail)
        {
            string newpwd = CreateRandomCode(6);
            string errorMsg = "";
            var result = false;
            if (M_UsersBusiness.CheckEmail(loginname, useremail))
            {
                result = M_UsersBusiness.UpdatePwd(loginname, newpwd);
                if (result)
                {
                    StringBuilder bodyInfo = new StringBuilder();
                    bodyInfo.Append("亲爱会员：<br/>");
                    bodyInfo.Append("    您好！<br/>你于");
                    bodyInfo.Append(DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss"));
                    bodyInfo.Append("通过<a href='#'>http://localhost:6323/Help/Forget</a>忘记密码功能，审请重置密码。<br/>");
                    bodyInfo.Append("　　　重置之后的个人密码为：" + newpwd + "<br/>请妥善保管");
                    SendMail email = new SendMail();
                    email.mailFrom = ConfigurationManager.AppSettings["EmailAccount"];
                    email.mailPwd = ConfigurationManager.AppSettings["EmailPwd"];
                    email.isEnableSsl = true;
                    email.mailSubject = "会员中心--充值密码";
                    email.mailBody = bodyInfo.ToString();
                    email.isbodyHtml = true; //是否是HTML
                    email.host = ConfigurationManager.AppSettings["EmailHost"]; //如果是QQ邮箱则：smtp:qq.com,依次类推
                    email.mailToArray = new string[] {useremail}; //接收者邮件集合 
                    result = email.Send();
                }
                else
                {
                    errorMsg = "发送邮件失败，请稍后再试！";
                }
            }
            else
            {
                errorMsg = "注册邮箱与用户不符！";
            }
            JsonDictionary.Add("errorMsg", errorMsg);
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        } 
        public JsonResult SaveFeedBack(string entity)
        {
            var result = false;
            string msg = "提交失败，请稍后再试！";
            if (CurrentUser == null)
            {
                msg = "您尚未登录，请登录后在操作！";
            }
            else
            {
                FeedBack model = JsonConvert.DeserializeObject<FeedBack>(entity);
                model.CreateUserID = CurrentUser.CreateUserID;
                result = FeedBackBusiness.InsertFeedBack(model);
            }
            JsonDictionary.Add("result",result );
            JsonDictionary.Add("errorMsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult SaveReply(string entity)
        {
            var result = false;
            string msg = "提交失败，请稍后再试！";
            if (CurrentUser == null)
            {
                msg = "您尚未登录，请登录后在操作！";
            }
            else
            {
                UserReply model = JsonConvert.DeserializeObject<UserReply>(entity);
                model.FromReplyID = string.IsNullOrEmpty(model.FromReplyID) ? "" : model.FromReplyID;
                model.FromReplyUserID = string.IsNullOrEmpty(model.FromReplyUserID) ? "" : model.FromReplyUserID; 
                result = UserReplyBusiness.CreateUserReply(model.GUID,model.Content,CurrentUser.UserID,model.FromReplyID,model.FromReplyUserID,model.Type);
            }
            JsonDictionary.Add("result", result);
            JsonDictionary.Add("errorMsg", msg);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult ReplyList(int type,int pageIndex,int pageSize)
        {
            
            int total = 0;
            int pageCount = 0;
            var list = UserReplyBusiness.GetUserReplys(type == 1 ? "": CurrentUser.UserID ,
                type == 1 ?  CurrentUser.UserID:"", type == 2 ? 1 : type, pageSize, pageIndex, ref total, ref pageCount);
            if (type == 2)
            {
                list.ForEach(x=>
                {
                    if (x.Status == 0)
                    {
                        x.Content = "该信息共有" + x.Content.Length+"个字...";
                    }
                }
            );
            }
            JsonDictionary.Add("items", list);
            JsonDictionary.Add("totalCount", total);
            JsonDictionary.Add("pageCount", pageCount);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult DeleteReply(string replyid)
        {
            JsonDictionary.Add("result", UserReplyBusiness.DeleteReply(replyid));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult GetReplyInfo(string replyid)
        {
            string errMsg = "";
            var result = false;
            string url = RouteData.Values["controller"] + "/" + RouteData.Values["action"];
            if (!checkGolds(url))
            {
                errMsg = "账户金币不足，不能查看";
            }
            
            if (string.IsNullOrEmpty(errMsg))
            {
                errMsg = UserReplyBusiness.GetReplyDetail(replyid).Content;
                result = UserReplyBusiness.UpdateReplyStatus(replyid, 1);
            }
            JsonDictionary.Add("errorMsg", errMsg);
            JsonDictionary.Add("result", result);
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult GetActivity(int type=1)
        {
            JsonDictionary.Add("items", WebSetBusiness.GetMemberLevel(type));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        public JsonResult PayMoney(int type,string levelid,string payway)
        {
            var model=WebSetBusiness.GetMemberLevelByID(levelid);
            string msg = "";
            string ordercode = CurrentUser.AutoID + DateTime.Now.ToString("yyyMMddHHmmssfff");
            JsonDictionary.Add("result", UserOrdersBusiness.CreateUserOrder(levelid, payway == "zfbpay" ? 0 : 1, ordercode, CurrentUser.UserID, ref msg));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public JsonResult PayOtherMoney(decimal gold, string payway)
        {
            var grule = Convert.ToDecimal(CommonBusiness.getSysSetting(EnumSettingKey.GoldScale, "DValue"));
            var totalFee = grule*gold;
            string ordercode = CurrentUser.AutoID + DateTime.Now.ToString("yyyMMddHHmmssfff");
            var model = WebSetBusiness.GetMemberLevel(1).Where(x => x.IntegFeeMore <= totalFee).OrderByDescending(x=>x.Origin).FirstOrDefault();
            string content = "购买金币";
            if (model != null)
            {
                content = model.Golds > 0 ? ("满足优惠活动，赠送金币：" + model.Golds) : "";
                gold += model.Golds;
            }
            JsonDictionary.Add("result", UserOrdersBusiness.CreateUserOrder(ordercode, payway == "zfbpay" ? 0 : 1, "金币", "", content, totalFee, "", 1, gold, CurrentUser.UserID));
            return new JsonResult
            {
                Data = JsonDictionary,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}
