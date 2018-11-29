using CBSP.Core;
using CBSP.Core.Domain.EciqOrderBill;
using CBSP.Core.Domain.OrderBill;
using CBSP.Core.Domain.Users;
using CBSP.Services.EciqOrderBill;
using CBSP.Services.OrderBill;
using CBSP.Services.Users;
using CBSP.Web.Framework;
using CSBP.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace CSBP.WebApi.Controllers
{
    public class EciqOrderBillController : BaseController
    {
        EciqOrderBillService service = new EciqOrderBillService();
        private UserService _userService = new UserService();
        public void PrepareCuInsertInfo(EciqOrderBillHead entity,User user,string guid)
        {
            if (guid != null)
            {
                entity.ORDER_ID = guid.Substring(0, 32);
            }
            else
            {
                entity.ORDER_ID = BillNoGenerator.NewEciqOrderBill();
            }

            foreach (var e in entity.EciqOrderBillLists)
            {
                e.ORDER_ID = entity.ORDER_ID;
            }
            entity.CNEPORT_GUID = null;
            entity.SUBCEN_GUID = null;
            entity.SDEPORT_GUID = null;
            entity.RETURN_STATUS = null;
            entity.RETURN_DATE = null;
            entity.RETURN_INFO = null;

            entity.UPLOAD_FLAG = Convert.ToString((int)CU_UPLOAD_FLAG.NOT_UPLOADED);
            entity.UPLOAD_DATE = null;
            entity.UPLOAD_OP_TYPE = null;
            entity.UPLOAD_OP_BIZ = null;
            entity.UPLOAD_FILE_NAME = null;

            entity.STATUS = Convert.ToString((int)STATUS_VALUE.SAVED);
            entity.STATUS_DESC = null;
            entity.STATUS_DATE = DateTime.Now;

            entity.UPLOAD_ACC_ID = null;
            entity.UPLOAD_ACC_NAME = null;
            entity.UPLOAD_COP_CODE = null;
            entity.UPLOAD_COP_NAME = null;

            entity.INPUT_DATE = DateTime.Now;
            entity.INPUT_ACC_ID = Convert.ToString(user.UserId);
            entity.INPUT_ACC_NAME = user.Name;
            entity.INPUT_COP_CODE = user.CopGbCode;
            entity.INPUT_COP_NAME = user.CopName;
            entity.INPUT_FLAG = Convert.ToString((int)INPUT_FLAG_VALUE.IMPORT);

            entity.UPD_DATE = null;
            entity.UPD_ACC_ID = null;
            entity.UPD_ACC_NAME = null;
        }
        public IHttpActionResult PostCuOrderBill(string token, EciqOrderBillModel orderBill)
        {
            ResultMsg msg = new ResultMsg();
            ResultInfo info = new ResultInfo();
            msg.ResultInfo = info;
            var user = CertValid(token, orderBill, msg);
            orderBill.COP_GB_CODE = user.CopGbCode;
            msg.ResultInfo.ID = orderBill.ORDER_NO;
            if ("1".Equals(msg.ResultInfo.Status))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        PrepareCuInsertInfo(orderBill, user,null);
                        service.Insert(orderBill);
                        msg.ResultInfo.Status = "1";
                        msg.ResultInfo.Remark = string.Format("统一版订单导入成功。");
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

        //public IHttpActionResult GetStatus(string token, string orderNo)
        //{
        //    StatusMsg result = new StatusMsg();
        //    var info = new CSBP.WebApi.Models.StatusInfo();
        //    result.StatusInfo = info;
        //    result.StatusInfo.ID = orderNo;
        //    string msg;
        //    var user = CertValid(token, out msg);
        //    if (user == null)
        //    {
        //        result.StatusInfo.Status = "-1";//因为0是单据状态，这里用-1表示获取失败
        //        result.StatusInfo.Remark = msg;
        //    }
        //    else
        //    {
        //        var orderBill = service.GetOrderBillByOrderNo(orderNo);
        //        if (orderBill == null)
        //        {
        //            result.StatusInfo.Status = "-1";
        //            result.StatusInfo.Remark = "单据不存在";
        //        }
        //        else
        //        {
        //            result.StatusInfo.Status = orderBill.STATUS;
        //            result.StatusInfo.Remark = orderBill.STATUS_DESC;
        //            //WayBillService ws = new WayBillService();
        //            //var wl = ws.GetWaybillByOrder(orderNo, orderBill.EB_PLAT_ID);
        //            //if (wl.Count > 0)
        //            //{
        //            //    DeclformService ds = new DeclformService();
        //            //    List<DeclformItem> declformItem = new List<DeclformItem>();
        //            //    foreach (var w in wl)
        //            //    {
        //            //        if (declformItem.Where(e => e.WaybillID == w.WAYBILL_ID).Count() == 0)
        //            //        {
        //            //            DeclformItem d = new DeclformItem();
        //            //            d.WaybillID = w.WAYBILL_ID;
        //            //            var declform = ds.GetDeclformByKeys(w.WAYBILL_ID, w.LOGI_ENTE_CODE);
        //            //            if (declform != null)
        //            //            {
        //            //                d.CustID = declform.DECLFORM_ID;
        //            //                d.Status = declform.STATUS;
        //            //                d.Remark = declform.STATUS_DESC;
        //            //            }
        //            //            declformItem.Add(d);
        //            //        }
        //            //    }
        //            //    result.StatusInfo.DeclformItem = declformItem.ToArray();
        //            //}
        //        }
        //    }
        //    return Ok<StatusMsg>(result);
        //}
        private string FormatMsg(string msg, string orderNo, string ebpCode)
        {
            return string.Format("ORDER_NO={0}且EBP_CODE={1}的订单，{2}", orderNo, ebpCode, msg);
        }
    }
}
