using CBSP.Services.DeclformBill;
using CBSP.Services.OrderBill;
using CBSP.Services.Users;
using CBSP.Services.WayBill;
using CSBP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;

namespace CSBP.WebApi.Controllers
{
    public class IOrderBillController : BaseController
    {
        OrderBillService service = new OrderBillService();
        private UserService _userService = new UserService();
        // POST api/values
        public IHttpActionResult PostOrderBill(string token, OrderBillModel orderBill)
        {
            ResultMsg msg = new ResultMsg();
            ResultInfo info = new ResultInfo();
            msg.ResultInfo = info;
            var user = CertValid(token, orderBill, msg);
            orderBill.COP_GB_CODE = user.CopGbCode;
            msg.ResultInfo.ID = orderBill.ORDER_ID;
            if ("1".Equals(msg.ResultInfo.Status))
            {
                orderBill.IE_FLAG = "I";
                if (ModelState.IsValid)
                {
                    try
                    {
                        PrepareInsertInfo(orderBill, user);
                        service.Insert(orderBill);
                        msg.ResultInfo.Status = "1";
                        msg.ResultInfo.Remark = string.Format("进口订单导入成功。");
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

        public IHttpActionResult GetStatus(string token, string orderId)
        {
            StatusMsg result = new StatusMsg();
            StatusInfo info = new StatusInfo();
            result.StatusInfo = info;

            result.StatusInfo.ID = orderId;
            string msg;
            var user = CertValid(token, out msg);
            if (user == null)
            {
                result.StatusInfo.Status = "-1";//因为0是单据状态，这里用-1表示获取失败
                result.StatusInfo.Remark = msg;
            }
            else
            {
                OrderBillService os = new OrderBillService();
                var orderBill = os.GetOrderBillById(orderId);
                if (orderBill != null && orderBill.IE_FLAG.Equals("E"))
                {
                    orderBill = null;
                }
                if (orderBill == null)
                {
                    result.StatusInfo.Status = "-1";
                    result.StatusInfo.Remark = "单据不存在";
                }
                else
                {
                    result.StatusInfo.Status = orderBill.STATUS;
                    result.StatusInfo.Remark = orderBill.STATUS_DESC;
                    WayBillService ws = new WayBillService();
                    var wl = ws.GetWaybillByOrder(orderId, orderBill.EB_PLAT_ID);
                    if (wl.Count > 0)
                    {
                        DeclformService ds = new DeclformService();
                        List<DeclformItem> declformItem = new List<DeclformItem>();
                        foreach (var w in wl)
                        {
                            if (declformItem.Where(e => e.WaybillID == w.WAYBILL_ID).Count() == 0)
                            {
                                DeclformItem d = new DeclformItem();
                                d.WaybillID = w.WAYBILL_ID;
                                var declform = ds.GetDeclformByKeys(w.WAYBILL_ID, w.LOGI_ENTE_CODE);
                                if (declform != null)
                                {
                                    d.CustID = declform.DECLFORM_ID;
                                    d.Status = declform.STATUS;
                                    d.Remark = declform.STATUS_DESC;
                                }
                                declformItem.Add(d);
                            }
                        }
                        result.StatusInfo.DeclformItem = declformItem.ToArray();
                    }
                }
            }
            return Ok<StatusMsg>(result);
        }
        
        private string FormatMsg(string msg, string orderId, string ebPlatId)
        {
            return string.Format("ORDER_ID={0}且EB_PLAT_ID={1}的订单，{2}", orderId, ebPlatId, msg);
        }
    }
}
