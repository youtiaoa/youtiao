using CBSP.Services.DeclformBill;
using CBSP.Services.Users;
using CBSP.Web.Framework;
using CSBP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CSBP.WebApi.Controllers
{
    public class IDeclformController : BaseController
    {
        private DeclformService service = new DeclformService();
        private UserService _userService = new UserService();

        // POST api/<controller>
        public IHttpActionResult PostDeclform(string token, DeclformModel declform)
        {
            ResultMsg msg = new ResultMsg();
            ResultInfo info = new ResultInfo();

            msg.ResultInfo = info;
            var user = CertValid(token, declform, msg);
            declform.COP_GB_CODE = user.CopGbCode;

            msg.ResultInfo.ID = declform.WAYBILL_ID;
            if ("1".Equals(msg.ResultInfo.Status))
            {
                declform.IE_FLAG = "I";
                if (ModelState.IsValid)
                {
                    try
                    {
                        PrepareInsertInfo(declform, user);
                        service.Insert(declform);
                        msg.ResultInfo.Status = "1";
                        msg.ResultInfo.Remark = string.Format("进口清单导入成功。");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, ex.Message);
                    }
                }
                if (!ModelState.IsValid)//这里包含插入时有异常的情况
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var v in ModelState.Values)
                    {
                        foreach (var e in v.Errors)
                        {
                            sb.AppendLine(e.ErrorMessage);
                        }
                    }
                    msg.ResultInfo.Status = "0";
                    msg.ResultInfo.Remark = sb.ToString();
                }
            }
            return Ok<ResultMsg>(msg);
        }

        private string FormatMsg(string msg,string waybillId,string logiEnteCode)
        {
            return string.Format("WAYBILL_ID={0}且LOGI_ENTE_CODE={1}的清单，{2}",waybillId,logiEnteCode,msg);
        }
    }
}
