using CBSP.Services.DeclformBill;
using CBSP.Services.Users;
using CBSP.Services.WayBill;
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
    public class WayBillController : BaseController
    {
        WayBillService service = new WayBillService();
        private UserService _userService = new UserService();
        public IHttpActionResult PostWayBill(string token, WayBillModel wayBill)
        {
            ResultMsg msg = new ResultMsg();
            ResultInfo info = new ResultInfo();
            msg.ResultInfo = info;
            var user = CertValid(token, wayBill, msg);
            wayBill.COP_GB_CODE = user.CopGbCode;
            msg.ResultInfo.ID = wayBill.WAYBILL_ID;
            if ("1".Equals(msg.ResultInfo.Status))
            {
                wayBill.IE_FLAG = "E";
                wayBill.DECLFORM_AUTO_CREATE = "0";
                if (ModelState.IsValid)
                {
                    try
                    {
                        PrepareInsertInfo(wayBill, user);
                        service.Insert(wayBill);
                        msg.ResultInfo.Status = "1";
                        msg.ResultInfo.Remark = string.Format("出口运单导入成功。");
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
        public IHttpActionResult GetStatus(string token, string waybillId)
        {
            StatusMsg result = new StatusMsg();
            StatusInfo info = new StatusInfo();
            result.StatusInfo = info;
            result.StatusInfo.ID = waybillId;
            string msg;
            var user = CertValid(token, out msg);
            if (user == null)
            {
                result.StatusInfo.Status = "-1";//因为0是单据状态，这里用-1表示获取失败
                result.StatusInfo.Remark = msg;
            }
            else
            {
                WayBillService ws = new WayBillService();
                var wayBill = ws.GetWaybillById(waybillId);
                if (wayBill != null && wayBill.IE_FLAG.Equals("I"))
                {
                    wayBill = null;
                }
                if (wayBill == null)
                {
                    result.StatusInfo.Status = "-1";
                    result.StatusInfo.Remark = "单据不存在";
                }
                else
                {
                    result.StatusInfo.Status = wayBill.STATUS;
                    result.StatusInfo.Remark = wayBill.STATUS_DESC;

                    DeclformService ds = new DeclformService();
                    List<DeclformItem> declformItem = new List<DeclformItem>();
                    var declform = ds.GetDeclformByKeys(waybillId, wayBill.LOGI_ENTE_CODE);
                    if (declform != null)
                    {
                        DeclformItem d = new DeclformItem();
                        d.CustID = declform.DECLFORM_ID;
                        d.Status = declform.STATUS;
                        d.Remark = declform.STATUS_DESC;
                        declformItem.Add(d);
                    }
                    result.StatusInfo.DeclformItem = declformItem.ToArray();
                }
            }
            return Ok<StatusMsg>(result);
        }
        private string FormatMsg(string msg, string waybillId, string logiEnteCode)
        {
            return string.Format("WAYBILL_ID={0}且LOGI_ENTE_CODE={1}的运单，{2}", waybillId, logiEnteCode, msg);
        }
    }
}
