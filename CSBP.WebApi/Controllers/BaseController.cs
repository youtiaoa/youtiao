using CBSP.Core;
using CBSP.Core.Domain.Users;
using CBSP.Services.Users;
using CSBP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace CSBP.WebApi.Controllers
{
    public class BaseController : ApiController
    {
        protected HttpResponseMessage Json(object obj)
        {
            var resultJson = new JavaScriptSerializer().Serialize(obj);
            return new HttpResponseMessage { Content = new StringContent(resultJson, Encoding.GetEncoding("UTF-8"), "application/json") };

        }

        protected HttpResponseMessage Json(ResultMsg result)
        {
            var resultJson = new JavaScriptSerializer().Serialize(result);
            return new HttpResponseMessage { Content = new StringContent(resultJson, Encoding.GetEncoding("UTF-8"), "application/json") };

        }
        [NonAction]
        public User CertValid(string token,out string msg)
        {
            User user = null;
            msg = string.Empty;
            int hours = int.Parse(ConfigurationManager.AppSettings["period"] ?? "12");
            if (string.IsNullOrEmpty(token))
            {
                msg = "请提供完整签名信息";
            }
            else
            {
                UserService _userService = new UserService();
                user = _userService.GetUserByToken(token, hours);
                if (user == null)
                {
                    msg = "签名无效";
                }
            }
            return user;
        }
        [NonAction]
        public User CertValid(string token, ResultMsg msg)
        {
            User user = null;
            ResultInfo info = new ResultInfo();
            info.Status = "1";
            int hours = int.Parse(ConfigurationManager.AppSettings["period"] ?? "12");
            if (string.IsNullOrEmpty(token))
            {
                info.Status = "0";
                info.Remark = "请提供完整签名信息";
            }
            else
            {
                UserService _userService = new UserService();
                user = _userService.GetUserByToken(token, hours);
                if (user == null)
                {
                    info.Status = "0";
                    info.Remark = "签名无效";
                }
            }
            msg.ResultInfo = info;
            return user;
        }
        [NonAction]
        public User CertValid(string token,BillBaseEntity bill, ResultMsg msg)
        {
            ResultInfo info = new ResultInfo();
            if (bill == null)
            {
                info.Status = "0";
                info.Remark = "无效的数据。";
                msg.ResultInfo = info;
                return null;
            }
            else
            {
                return CertValid(token, msg);
            }
        }
        [NonAction]
        public void PrepareInsertInfo(BillBaseEntity entity, User user)
        {
            DateTime currentTime = DateTime.Now;
            entity.INPUT_ACC_ID = Convert.ToString(user.UserId);
            entity.INPUT_ACC_NAME = user.Name;
            entity.INPUT_DATE = currentTime;
            entity.INPUT_FLAG = Convert.ToString((int)INPUT_FLAG_VALUE.IMPORT);
            if (!string.IsNullOrEmpty(entity.UPLOAD_FLAG) && entity.UPLOAD_FLAG.Equals(UPLOAD_FLAG_VALUE.MESSAGED))
            {
                entity.STATUS = Convert.ToString((int)STATUS_VALUE.UPLOADING);
            }
            else
            {
                entity.STATUS = Convert.ToString((int)STATUS_VALUE.SAVED);
            }
            entity.STATUS_DATE = currentTime;

            entity.UPLOAD_OP_RESULT = (int)UPLOAD_OP_RESULT.UNSEND + string.Empty;
            entity.UPLOAD_FLAG = (int)UPLOAD_FLAG_VALUE.NOT_UPLOADED + string.Empty;
            entity.SCN = currentTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        protected int MaxCount
        {
            get
            {
                var maxCount = ConfigurationManager.AppSettings["MaxCount"];
                int count;
                if (!int.TryParse(maxCount, out count))
                    count = 10;

                return count;
            }
        }
    }
}
