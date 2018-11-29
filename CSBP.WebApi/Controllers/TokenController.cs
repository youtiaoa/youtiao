using CBSP.Services.Users;
using CBSP.Web.Framework;
using CSBP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace CSBP.WebApi.Controllers
{
    public class TokenController : ApiController
    {
        // GET api/token/5
        public string Get(string appId,string appSecret)
        {
            bool isSuccess;
            string msg = string.Empty;
            var service = new UserService();
            isSuccess = service.GetToken(appId, appSecret, out msg);

            return new JavaScriptSerializer().Serialize(new ResultMsg() { ResultInfo = new ResultInfo { Status = (isSuccess ? "1" : "0"), Remark = msg } });
        }
    }
}
