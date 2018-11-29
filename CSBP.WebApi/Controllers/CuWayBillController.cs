using CBSP.Core;
using CBSP.Core.Domain.Users;
using CBSP.Core.Domain.WayBill;
using CBSP.Services.Users;
using CBSP.Services.WayBill;
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
    public class CuWayBillController : BaseController
    {
        CuWayBillService service = new CuWayBillService();
        private UserService _userService = new UserService();

        public void PrepareCuInsertInfo(CuWaybillHead entity, User user, string guid)
        {
            if(guid != null)
            {
                entity.GUID = guid;
            }
            else
            {
                entity.GUID = BillNoGenerator.NewWayBillNo();
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
        public IHttpActionResult PostWayBill(string token, CuWayBillModel wayBill)
        {
            ResultMsg msg = new ResultMsg();
            ResultInfo info = new ResultInfo();
            msg.ResultInfo = info;
            var user = CertValid(token, wayBill, msg);
            wayBill.COP_GB_CODE = user.CopGbCode;
            msg.ResultInfo.ID = wayBill.LOGISTICS_NO;
            if ("1".Equals(msg.ResultInfo.Status))
            {
                wayBill.DECLFORM_AUTO_CREATE = "0";
                if (ModelState.IsValid)
                {
                    try
                    {
                        PrepareCuInsertInfo(wayBill, user,null);
                        service.Insert(wayBill);
                        msg.ResultInfo.Status = "1";
                        msg.ResultInfo.Remark = string.Format("统一版运单导入成功。");
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
        public IHttpActionResult GetStatus(string token, string logisticsNo)
        {
            StatusMsg result = new StatusMsg();
            var info = new CSBP.WebApi.Models.StatusInfo();
            result.StatusInfo = info;
            result.StatusInfo.ID = logisticsNo;
            string msg;
            var user = CertValid(token, out msg);
            if (user == null)
            {
                result.StatusInfo.Status = "-1";//因为0是单据状态，这里用-1表示获取失败
                result.StatusInfo.Remark = msg;
            }
            else
            {
                CuWayBillService ws = new CuWayBillService();
                var wayBill = ws.GetCuWaybillByLogiNo(logisticsNo);
                if (wayBill == null)
                {
                    result.StatusInfo.Status = "-1";
                    result.StatusInfo.Remark = "单据不存在";
                }
                else
                {
                    result.StatusInfo.Status = wayBill.STATUS;
                    result.StatusInfo.Remark = wayBill.STATUS_DESC;

                }
            }
            return Ok<StatusMsg>(result);
        }
    }
}
