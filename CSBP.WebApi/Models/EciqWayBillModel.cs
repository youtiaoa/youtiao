using CBSP.Core;
using CBSP.Core.Domain.DeclformBill;
using CBSP.Core.Domain.WayBill;
using CBSP.Services.DeclformBill;
using CBSP.Services.WayBill;
using CBSP.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace CSBP.WebApi.Models
{
    public class EciqWayBillModel : EciqWaybillHead, IValidatableObject
    {
        public string COP_GB_CODE { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 必填项
           
            #region 电商企业
            if (String.IsNullOrWhiteSpace(IN_OUT_FLAG))
                yield return new ValidationResult("IN_OUT_FLAG必填。");
            else
            {
                string logisticsName = "";
                ValidationResult logisticsResult = CheckPara.CheckEciqCompanyCode(E_BUSINESS_COMPANY_CODE, COP_TYPE.COP_TYPE_E, IE_FLAG.IE_FLAG_I, "E_BUSINESS_COMPANY_CODE", ref logisticsName);
                if (logisticsResult == null)
                    yield return new ValidationResult(String.Format("E_BUSINESS_COMPANY_CODE【{0}】不存在。", E_BUSINESS_COMPANY_CODE));
                else
                    E_BUSINESS_COMPANY_NAME = logisticsName;
            }
            #endregion

            #region 进出口标志
            if (String.IsNullOrWhiteSpace(IN_OUT_FLAG))
                yield return new ValidationResult("IN_OUT_FLAG必填。");
            else
            {
                var paraCustomsCode = ParaEciqManager.Instance.ECIQ_ParaInOutFlag.Find(o => o.Code == IN_OUT_FLAG);
                if (paraCustomsCode == null)
                    yield return new ValidationResult(String.Format("IN_OUT_FLAG【{0}】不存在。", IN_OUT_FLAG));
            }
            #endregion

            #region 进出口模式
            if (String.IsNullOrWhiteSpace(IMPORT_TYPE))
                yield return new ValidationResult("IMPORT_TYPE必填。");
            else
            {
                var paraCustomsCode = ParaEciqManager.Instance.ParaEciqImportType.Find(o => o.Code == IMPORT_TYPE);
                if (paraCustomsCode == null)
                    yield return new ValidationResult(String.Format("IMPORT_TYPE【{0}】不存在。", IMPORT_TYPE));
            }
            #endregion

            //订单编号
            if (string.IsNullOrEmpty(ORDER_NUMBER))
                yield return new ValidationResult("ORDER_NUMBER必填。");

            //分运单号
            if (string.IsNullOrEmpty(SUB_CARRIAGE_NO))
                yield return new ValidationResult("SUB_CARRIAGE_NO必填。");

            //毛重
            if (ROUGH_WEIGHT <= 0)
                yield return new ValidationResult("ROUGH_WEIGHT应大于【0】。");

            //币制
            if (!CURRENCY.Equals(CU_CURRENCY.CU_CURRENCY_156))
                yield return new ValidationResult("CURR_CODE应为【156】。");
           
            //发件人姓名     
            if (string.IsNullOrEmpty(SENDER))
                yield return new ValidationResult("SENDER应必填。");

            //收件人姓名     
            if (string.IsNullOrEmpty(RECEIVER))
                yield return new ValidationResult("RECEIVER应必填。");

            
            #region 发件人国别
            if (String.IsNullOrWhiteSpace(SENDER_COUNTRY))
                yield return new ValidationResult("SENDER_COUNTRY必填。");
            else
            {
                var paraCustomsCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == SENDER_COUNTRY);
                if (paraCustomsCode == null)
                    yield return new ValidationResult(String.Format("SENDER_COUNTRY【{0}】不存在。", SENDER_COUNTRY));
            }
            #endregion

            //主要货物名称     
            if (string.IsNullOrEmpty(MAJOR_GOODS_NAME))
                yield return new ValidationResult("MAJOR_GOODS_NAME应必填。");

            #region 监管机构代码
            if (String.IsNullOrWhiteSpace(ORG_CODE))
                yield return new ValidationResult("ORG_CODE必填。");
            else
            {
                var paraCustomsCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == ORG_CODE);
                if (paraCustomsCode == null)
                    yield return new ValidationResult(String.Format("ORG_CODE【{0}】不存在。", ORG_CODE));
            }
            #endregion

            #endregion

            #region 选填项

            # region 进出口岸代码
            if (!String.IsNullOrWhiteSpace(IN_OUT_PORT_NUMBER))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqEntryPort.Find(o => o.CODE == IN_OUT_PORT_NUMBER);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("IN_OUT_PORT_NUMBER【{0}】不存在,", IN_OUT_PORT_NUMBER));
            }

            #endregion

            #region 抵运港
            if (!String.IsNullOrWhiteSpace(ARRIVED_PORT))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqPort.Find(o => o.CODE == ARRIVED_PORT);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("ARRIVED_PORT【{0}】不存在,", ARRIVED_PORT));
            }

            #endregion

            #region 贸易国别
            if (!String.IsNullOrWhiteSpace(FROM_COUNTRY))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == FROM_COUNTRY);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("FROM_COUNTRY【{0}】不存在,", FROM_COUNTRY));
            }

            #endregion

            #region 包装种类
            if (!String.IsNullOrWhiteSpace(PACK_TYPE))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqPackType.Find(o => o.CODE == PACK_TYPE);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("PACK_TYPE【{0}】不存在,", PACK_TYPE));
            }

            #endregion

            # region 申报口岸代码
            if (!String.IsNullOrWhiteSpace(DECLARE_PORT_CODE))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqEntryPort.Find(o => o.CODE == DECLARE_PORT_CODE);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("DECLARE_PORT_CODE【{0}】不存在,", DECLARE_PORT_CODE));
            }

            #endregion

            # region 码头货场代码
            if (!String.IsNullOrWhiteSpace(GOODS_YARD_CODE))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqPort.Find(o => o.CODE == GOODS_YARD_CODE);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("GOODS_YARD_CODE【{0}】不存在,", GOODS_YARD_CODE));
            }

            #endregion

            # region 发件人城市
            if (!String.IsNullOrWhiteSpace(SENDER_CITY))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqDitric.Find(o => o.CODE == SENDER_CITY);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("SENDER_CITY【{0}】不存在,", SENDER_CITY));
            }

            #endregion

            #region 数据来源
            if (!String.IsNullOrWhiteSpace(FROM_WHERE))
            {
                var paraTrafMode = ParaEciqManager.Instance.ECIQ_ParaFromWhere.Find(o => o.Code == FROM_WHERE);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("FROM_WHERE【{0}】不存在,", FROM_WHERE));
            }

            #endregion

            #region 所属直属局节点
            if (!String.IsNullOrWhiteSpace(DEST_NODE))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == DEST_NODE);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("DEST_NODE【{0}】不存在,", DEST_NODE));
            }

            #endregion

            #region 三单标记
            if (!String.IsNullOrWhiteSpace(MONITOR_DECL_FLAG))
            {
                var paraTrafMode = ParaEciqManager.Instance.ECIQ_ParaMonitorDeclFlag.Find(o => o.Code == MONITOR_DECL_FLAG);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("MONITOR_DECL_FLAG【{0}】不存在,", MONITOR_DECL_FLAG));
            }

            #endregion

            #region 发送源节点
            if (!String.IsNullOrWhiteSpace(SEND_SOURCE_NODE))
            {
                var paraTrafMode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == SEND_SOURCE_NODE);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("SEND_SOURCE_NODE【{0}】不存在,", SEND_SOURCE_NODE));
            }

            #endregion

            #endregion

            #region 业务验证

            EciqWayBillService waybillService = new EciqWayBillService();

            if (waybillService.Exists(ORDER_NUMBER, SUB_CARRIAGE_NO))
                yield return new ValidationResult("国检版订单号为【" + ORDER_NUMBER + "】且分运单号为【" + SUB_CARRIAGE_NO + "】的国检运单已存在。");


            #endregion
        }

    }
}