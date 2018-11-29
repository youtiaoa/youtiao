﻿using CBSP.Core;
using CBSP.Core.Domain.DeclformBill;
using CBSP.Core.Domain.EciqExitDeclare;
using CBSP.Core.Domain.Users;
using CBSP.Services.DeclformBill;
using CBSP.Services.EciqExitDeclareBill;
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
    public class EciqExitDeclareBillController : BaseController
    {
        private EciqExitDeclareBillService service = new EciqExitDeclareBillService();
        private UserService _userService = new UserService();

        public void PrepareCuInsertInfo(EciqExitDeclareBillHead entity,User user)
        {
            entity.GOODS_DECLAR_CHECK_ID = BillNoGenerator.NewEciqExitDeclareBill();
            entity.CNEPORT_GUID = null; 
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
            foreach(var e in entity.EciqExitDeclareBillLists)
            {
                e.GOODS_DECLAR_CHECK_ID = entity.GOODS_DECLAR_CHECK_ID;
            }
            
        }

        // POST api/<controller>
        public IHttpActionResult PostDeclform(string token, EciqExitDeclareBillModel declform)
        {
            ResultMsg msg = new ResultMsg();
            ResultInfo info = new ResultInfo();
            msg.ResultInfo = info;
            var user = CertValid(token, declform, msg);
            declform.COP_GB_CODE = user.CopGbCode;
           // msg.ResultInfo.ID = declform.LOGISTICS_NO;
            if ("1".Equals(msg.ResultInfo.Status))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        PrepareCuInsertInfo(declform, user);
                        service.Insert(declform);
                        msg.ResultInfo.Status = "1";
                        msg.ResultInfo.Remark = string.Format("国检版出区清单导入成功。");
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
    }
}