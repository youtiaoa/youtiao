using CBSP.Services.PaymentBill;
using CBSP.Services.Users;
using CBSP.Web.Framework;
using CSBP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;

namespace CSBP.WebApi.Controllers
{
    public class PaymentController : BaseController
    {
        PaymentBillService service = new PaymentBillService();
        private UserService _userService = new UserService();
        public IHttpActionResult PostPayment(string token, PaymentBillModel paymentBill)
        {
            ResultMsg msg = new ResultMsg();
            ResultInfo info = new ResultInfo();
            msg.ResultInfo = info;
            var user = CertValid(token, paymentBill, msg);
            msg.ResultInfo.ID = paymentBill.DEAL_ID;
            if ("1".Equals(msg.ResultInfo.Status))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        PrepareInsertInfo(paymentBill, user);
                        //支付单据单独加入创建企业信息
                        paymentBill.INPUT_COP_CODE = user.CopGbCode;
                        paymentBill.INPUT_COP_NAME = user.CopName;
                        service.Insert(paymentBill);
                        msg.ResultInfo.Status = "1";
                        msg.ResultInfo.Remark = string.Format("支付单导入成功。");
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
        private string FormatMsg(string msg, string dealId, string dealPlatId)
        {
            return string.Format("DEAL_ID={0}且DEAL_PLAT_ID={1}的支付单，{2}", dealId, dealPlatId, msg);
        }
    }
}
