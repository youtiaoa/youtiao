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
using CBSP.Core.Domain.EciqExitDeclare;
using CBSP.Services.EciqExitDeclareBill;

namespace CSBP.WebApi.Models
{
    public class EciqExitDeclareBillModel : EciqExitDeclareBillHead, IValidatableObject
    {
        /// <summary>
        /// 当前登录账户对应的组织机构代码
        /// </summary>
        public string COP_GB_CODE { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 必填项

            #region 申报日期
            if (string.IsNullOrEmpty(DECLARE_DATE))
                yield return new ValidationResult("DECLARE_DATE必填。");
            else
            {
                bool flag = true ;
               try
                {
                    Convert.ToDateTime(DECLARE_DATE).ToString("yyyy-MM-dd");
                }
                catch
                {
                    flag = false;
                }
                if(!flag)
                yield return new ValidationResult("DECLARE_DATE格式错误。");
            }
            #endregion

            #region 电商企业
            if (string.IsNullOrEmpty(E_BUSINESS_COMPANY_CODE))
                yield return new ValidationResult("E_BUSINESS_COMPANY_CODE必填。");
            else
            {
                string ebcName = "";
                ValidationResult ebcResult = CheckPara.CheckEciqCompanyCode(E_BUSINESS_COMPANY_CODE, COP_TYPE.COP_TYPE_E, CBSP.Core.IE_FLAG.IE_FLAG_I, "EB_CODE", ref ebcName);
                if (ebcResult != null)
                    yield return ebcResult;
                else
                    E_BUSINESS_COMPANY_NAME = ebcName;
            }
            #endregion

            #region 电商平台
            if (string.IsNullOrEmpty(E_BUSINESS_PLATFORM_CODE))
                yield return new ValidationResult("E_BUSINESS_PLATFORM_CODE必填。");
            else
            {
                string ebcName = "";
                ValidationResult ebcResult = CheckPara.CheckEciqCompanyCode(E_BUSINESS_PLATFORM_CODE, COP_TYPE.COP_TYPE_EP, CBSP.Core.IE_FLAG.IE_FLAG_I, "EB_CODE", ref ebcName);
                if (ebcResult != null)
                    yield return ebcResult;
                else
                    E_BUSINESS_PLATFORM_NAME = ebcName;
            }
            #endregion

            #region 物流企业
            if (String.IsNullOrWhiteSpace(LOGISTICS_CODE))
                yield return new ValidationResult("LOGISTICS_CODE必填。");
            else
            {
                string logisticsName = "";
                ValidationResult payResult = CheckPara.CheckEciqCompanyCode(LOGISTICS_CODE, COP_TYPE.COP_TYPE_L, IE_FLAG.IE_FLAG_I, "LOGISTICS_CODE", ref logisticsName);
                if (payResult != null)
                    yield return payResult;
                else
                    LOGISTICS_NAME = logisticsName;
            }
            #endregion

            #region 贸易国别
            if (String.IsNullOrWhiteSpace(FROM_COUNTRY_CODE))
                yield return new ValidationResult("FROM_COUNTRY_CODE必填。");
            else
            {
                var paraEciqCountryCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == FROM_COUNTRY_CODE);
                if (paraEciqCountryCode == null)
                    yield return new ValidationResult(String.Format("FROM_COUNTRY_CODE【{0}】不存在。", FROM_COUNTRY_CODE));

                var paraEciqName = ParaEciqManager.Instance.EciqCountryName(FROM_COUNTRY_CODE);
                if (!paraEciqName.Equals(FROM_COUNTRY_NAME))
                    yield return new ValidationResult(String.Format("FROM_COUNTRY_NAME【{0}】与FROM_COUNTRY_CODE【{1}】不匹配。", FROM_COUNTRY_NAME, FROM_COUNTRY_CODE));
            }
            #endregion

            #region 进出口标志
            if (String.IsNullOrWhiteSpace(IN_OUT_FLAG))
                yield return new ValidationResult("IN_OUT_FLAG必填。");
            else
            {
                var paraEciqInOutFlagCode = ParaEciqManager.Instance.ParaEciqInOutFlag.Find(o => o.CODE == IN_OUT_FLAG);
                if (paraEciqInOutFlagCode == null)
                    yield return new ValidationResult(String.Format("IN_OUT_FLAG【{0}】不存在。", IN_OUT_FLAG));
            }
            #endregion

            #region 进出口类型
            if (String.IsNullOrWhiteSpace(IMPORT_TYPE))
                yield return new ValidationResult("IMPORT_TYPE必填。");
            else
            {
                var paraEciqInOutFlagCode = ParaEciqManager.Instance.ParaEciqImportType.Find(o => o.Code == IMPORT_TYPE);
                if (paraEciqInOutFlagCode == null)
                    yield return new ValidationResult(String.Format("IMPORT_TYPE【{0}】不存在。", IMPORT_TYPE));
            }
            #endregion

            #region 清单类型
            if (String.IsNullOrWhiteSpace(CHECKLIST_TYPE))
                yield return new ValidationResult("CHECKLIST_TYPE必填。");
            else
            {
                var paraEciqInOutFlagCode = ParaEciqManager.Instance.ECIQ_ParaCheckListType.Find(o => o.Code == CHECKLIST_TYPE);
                if (paraEciqInOutFlagCode == null)
                    yield return new ValidationResult(String.Format("CHECKLIST_TYPE【{0}】不存在。", CHECKLIST_TYPE));
            }
            #endregion

            #region 贸易方式
            if (String.IsNullOrWhiteSpace(TRADE_MODE))
                yield return new ValidationResult("TRADE_MODE必填。");
            else
            {
                var paraEciqTradeCode = ParaEciqManager.Instance.ParaEciqTrade.Find(o => o.CODE == TRADE_MODE);
                if (paraEciqTradeCode == null)
                    yield return new ValidationResult(String.Format("TRADE_MODE【{0}】不存在。", TRADE_MODE));
            }
            #endregion

            #region 账册编号
            if (string.IsNullOrEmpty(ACCOUNT_BOOK_NO))
                yield return new ValidationResult("ACCOUNT_BOOK_NO必填。");
            #endregion

            #region 主要货物名称
            if (string.IsNullOrEmpty(MAJOR_GOODS_NAME))
                yield return new ValidationResult("MAJOR_GOODS_NAME必填。");
            #endregion

            #region 订单编号
            if (string.IsNullOrEmpty(ORDER_NUMBER))
                yield return new ValidationResult("ORDER_NUMBER必填。");
            #endregion

            #region 价值
            if (true)
            {
                bool flag = true;
                try
                {
                    Convert.ToDecimal(WORTH);
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("WORTH格式错误。");
            }
            #endregion

            #region 物流运单号
            if (string.IsNullOrEmpty(LOGISTICS_NO))
                yield return new ValidationResult("LOGISTICS_NO必填。");
            #endregion

            #region 总运单号
            if (string.IsNullOrEmpty(MAIN_WB_NO))
                yield return new ValidationResult("MAIN_WB_NO必填。");
            #endregion

            #region 发件人
            if (string.IsNullOrEmpty(SENDER))
                yield return new ValidationResult("SENDER必填。");
            #endregion

            #region 币制
            if (CURRENCY_CODE != "156")
                yield return new ValidationResult(String.Format("CURRENCY_CODE【{0}】限定为156。", CURRENCY_CODE));
            if (CURRENCY_NAME != "人民币")
                yield return new ValidationResult(String.Format("CURRENCY_NAME【{0}】限定为人民币。", CURRENCY_NAME));
            #endregion

            #region 监管机构
            if (String.IsNullOrWhiteSpace(INSP_ORG_CODE))
                yield return new ValidationResult("INSP_ORG_CODE必填。");
            else
            {
                var paraEciqInspCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == INSP_ORG_CODE);
                if (paraEciqInspCode == null)
                    yield return new ValidationResult(String.Format("INSP_ORG_CODE【{0}】不存在。", INSP_ORG_CODE));
            }
            #endregion

            #region 收货人地址
            if (string.IsNullOrEmpty(CONSIGNEE_ADDR))
                yield return new ValidationResult("CONSIGNEE_ADDR必填。");
            #endregion

            #region 收货人电话
            if (string.IsNullOrEmpty(CONSIGNEE_TEL))
                yield return new ValidationResult("CONSIGNEE_TEL必填。");
            #endregion

            #region 件数
            if (string.IsNullOrEmpty(PACK_NO.ToString()))
                yield return new ValidationResult("PACK_NO必填。");
            else
            {
                bool flag = true;
                try
                {
                    Convert.ToDecimal(PACK_NO);
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("PACK_NO格式错误。");
            }
            #endregion

            #region 毛重
            if (string.IsNullOrEmpty(ROUGH_WEIGHT.ToString()))
                yield return new ValidationResult("ROUGH_WEIGHT必填。");
            else
            {
                bool flag = true;
                try
                {
                    Convert.ToDecimal(ROUGH_WEIGHT);
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("ROUGH_WEIGHT格式错误。");
            }
            #endregion

            #region 销售方式
            if (String.IsNullOrWhiteSpace(TRADE_MODE))
                yield return new ValidationResult("TRADE_MODE必填。");
            else
            {
                var paraEciqCode = ParaEciqManager.Instance.ECIQ_ParaTradeMode.Find(o => o.Code == TRADE_MODE);
                if (paraEciqCode == null)
                    yield return new ValidationResult(String.Format("TRADE_MODE【{0}】不存在。", TRADE_MODE));
            }
            #endregion

            #region 清单标记
            if (String.IsNullOrWhiteSpace(MONITOR_FLAG))
                yield return new ValidationResult("MONITOR_FLAG必填。");
            else
            {
                var paraEciqInOutFlagCode = ParaEciqManager.Instance.ECIQ_ParaMonitorFlag.Find(o => o.Code == MONITOR_FLAG);
                if (paraEciqInOutFlagCode == null)
                    yield return new ValidationResult(String.Format("MONITOR_FLAG【{0}】不存在。", MONITOR_FLAG));
            }
            #endregion

            #endregion

            #region 非必填项

            #region 进出口时间
            if (!string.IsNullOrEmpty(IN_OUT_DATE))
            {
                bool flag = true;
                try
                {
                    Convert.ToDateTime(IN_OUT_DATE).ToString("yyyy-MM-dd");
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("IN_OUT_DATE格式错误。");
            }
            #endregion

            #region 抵运港
            if (!String.IsNullOrWhiteSpace(ARRIVED_PORT_CODE))
            {
                var paraEciqArrivedCode = ParaEciqManager.Instance.ParaEciqPort.Find(o => o.CODE == ARRIVED_PORT_CODE);
                if (paraEciqArrivedCode == null)
                    yield return new ValidationResult(String.Format("ARRIVED_PORT_CODE【{0}】不存在。", ARRIVED_PORT_CODE));
            }
            #endregion

            #region 目的国代码
            if (!String.IsNullOrWhiteSpace(PURPOS_CTRY_CODE))
            {
                var paraEciqPurposCode = ParaEciqManager.Instance.ParaEciqPurpose.Find(o => o.CODE == PURPOS_CTRY_CODE);
                if (paraEciqPurposCode == null)
                    yield return new ValidationResult(String.Format("PURPOS_CTRY_CODE【{0}】不存在。", PURPOS_CTRY_CODE));
            }
            #endregion

            #region 启运国代码
            if (!String.IsNullOrWhiteSpace(DESP_CTRY_CODE))
            {
                var paraEciqDespCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == DESP_CTRY_CODE);
                if (paraEciqDespCode == null)
                    yield return new ValidationResult(String.Format("DESP_CTRY_CODE【{0}】不存在。", DESP_CTRY_CODE));
            }
            #endregion

            #region 运输方式
            if (!String.IsNullOrWhiteSpace(TRAF_MODE))
            {
                var paraEciqTrafCode = ParaEciqManager.Instance.ParaEciqTrafMode.Find(o => o.CODE == TRAF_MODE);
                if (paraEciqTrafCode == null)
                    yield return new ValidationResult(String.Format("TRAF_MODE【{0}】不存在。", TRAF_MODE));
            }
            #endregion

            #region 运输工具代码
            if (!String.IsNullOrWhiteSpace(TRANS_TYPE_CODE))
            {
                var paraEciqTransCode = ParaEciqManager.Instance.ParaEciqTransType.Find(o => o.CODE == TRANS_TYPE_CODE);
                if (paraEciqTransCode == null)
                    yield return new ValidationResult(String.Format("TRANS_TYPE_CODE【{0}】不存在。", TRANS_TYPE_CODE));
            }
            #endregion

            #region 申报口岸代码
            if (!String.IsNullOrWhiteSpace(DECLARE_PORT_CODE))
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqPort.Find(o => o.CODE == DECLARE_PORT_CODE);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("DECLARE_PORT_CODE【{0}】不存在。", DECLARE_PORT_CODE));
                DECLARE_PORT_NAME = ParaEciqManager.Instance.EciqPortName(DECLARE_PORT_CODE);
            }
            #endregion

            #region 进出口岸代码
            if (!String.IsNullOrWhiteSpace(IN_OUT_PORT_NUMBER))
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqPort.Find(o => o.CODE == IN_OUT_PORT_NUMBER);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("IN_OUT_PORT_NUMBER【{0}】不存在。", IN_OUT_PORT_NUMBER));
            }
            #endregion

            #region 包装种类代码
            if (!String.IsNullOrWhiteSpace(PACK_TYPE_CODE))
            {
                var paraEciqPackCode = ParaEciqManager.Instance.ParaEciqPackType.Find(o => o.CODE == PACK_TYPE_CODE);
                if (paraEciqPackCode == null)
                    yield return new ValidationResult(String.Format("PACK_TYPE_CODE【{0}】不存在。", PACK_TYPE_CODE));
                PACK_TYPE_NAME = ParaEciqManager.Instance.EciqPackTypeName(PACK_TYPE_CODE);
            }
            #endregion

            #region 电商平台备案时间
            if (!string.IsNullOrEmpty(E_BUSINESS_COMPANY_DATE))
            {
                bool flag = true;
                try
                {
                    Convert.ToDateTime(E_BUSINESS_COMPANY_DATE).ToString("yyyy-MM-dd");
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("E_BUSINESS_COMPANY_DATE格式错误。");
            }
            #endregion

            #region 发件人国别
            if (!String.IsNullOrWhiteSpace(SENDER_COUNTRY_CODE))
            {
                var paraEciqPackCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == SENDER_COUNTRY_CODE);
                if (paraEciqPackCode == null)
                    yield return new ValidationResult(String.Format("SENDER_COUNTRY_CODE【{0}】不存在。", SENDER_COUNTRY_CODE));
                SENDER_COUNTRY_NAME = ParaEciqManager.Instance.EciqCountryName(SENDER_COUNTRY_CODE);
            }
            #endregion

            #region 证件类型
            if (!String.IsNullOrWhiteSpace(ID_TYPE))
            {
                var paraEciqCode = ParaEciqManager.Instance.ParaEciqCertType.Find(o => o.CODE == ID_TYPE);
                if (paraEciqCode == null)
                    yield return new ValidationResult(String.Format("ID_TYPE【{0}】不存在。", ID_TYPE));
            }
            #endregion

            #region 净重
            if (!string.IsNullOrEmpty(NET_WEIGHT.ToString()))
            {
                bool flag = true;
                try
                {
                    Convert.ToDecimal(NET_WEIGHT);
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("NET_WEIGHT格式错误。");
            }
            #endregion

            #region 状态
            if (!String.IsNullOrWhiteSpace(STATUS))
            {
                var paraEciqCode = ParaEciqManager.Instance.ECIQ_ParaStatus.Find(o => o.Code == STATUS);
                if (paraEciqCode == null)
                    yield return new ValidationResult(String.Format("STATUS【{0}】不存在。", STATUS));
            }
            #endregion

            #region 数据来源
            if (!String.IsNullOrWhiteSpace(FROM_WHERE))
            {
                var paraEciqCode = ParaEciqManager.Instance.ECIQ_ParaFromWhere.Find(o => o.Code == FROM_WHERE);
                if (paraEciqCode == null)
                    yield return new ValidationResult(String.Format("FROM_WHERE【{0}】不存在。", FROM_WHERE));
            }
            #endregion

            #region 发送源节点
            if (!String.IsNullOrWhiteSpace(SEND_SOURCE_NODE))
            {
                var paraEciqCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == SEND_SOURCE_NODE);
                if (paraEciqCode == null)
                    yield return new ValidationResult(String.Format("SEND_SOURCE_NODE【{0}】不存在。", SEND_SOURCE_NODE));
            }
            #endregion

            #endregion

            #region 商品信息
            #region 验证商品信息

            if (EciqExitDeclareBillLists == null || EciqExitDeclareBillLists.Count < 0)
                yield return new ValidationResult("货物信息必填");
            else
            {
                foreach (EciqExitDeclareBillList l in EciqExitDeclareBillLists)
                {
                    // l.GOODS_NO = (i++).ToString();
                    StringBuilder validResult = new StringBuilder();

                    #region 必填信息

                    #region 商品名称
                    if (String.IsNullOrWhiteSpace(l.GOODS_NAME))
                        validResult.Append("GOODS_NAME必填,");
                    #endregion

                    #region HS_CODE
                    //验证商品信息，以及录入的计量单位、法定计量单位是否和备案参数表里的一致
                    var paraComplex = ParaCacheManager.Instance.ParaCuComplex.Find(o => o.HS_G_CODE == l.HS_CODE);
                    if (paraComplex == null)
                        validResult.Append(string.Format("HS_CODE【{0}】不存在,", l.HS_CODE));
                    //else
                    //{
                    //    if (!String.IsNullOrWhiteSpace(paraComplex.HS_UNIT_1) && !paraComplex.HS_UNIT_1.Equals(l.QTY_UNIT_CODE))
                    //        validResult.Append(string.Format("QTY_UNIT_CODE【{0}】与商品备案参数表中的【{1}】不符,", l.QTY_UNIT_CODE, paraComplex.HS_UNIT_1)); 
                    //}
                    #endregion

                    #region 规格型号
                    if (String.IsNullOrWhiteSpace(l.GOODS_SPECIFICATION))
                        validResult.Append("GOODS_SPECIFICATION必填,");
                    #endregion

                    #region 产销国代码
                    if (String.IsNullOrWhiteSpace(l.PRO_MARKETING_COUNTRY_CODE))
                        validResult.Append("PRO_MARKETING_COUNTRY_CODE必填,");
                    else
                    {
                        var paraEciqCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == l.PRO_MARKETING_COUNTRY_CODE);
                        if (paraEciqCode == null)
                            yield return new ValidationResult(String.Format("PRO_MARKETING_COUNTRY_CODE【{0}】不存在。", l.PRO_MARKETING_COUNTRY_CODE));

                        var paraEciqName = ParaEciqManager.Instance.EciqCountryName(l.PRO_MARKETING_COUNTRY_CODE);
                        if (!paraEciqName.Equals(l.PRO_MARKETING_COUNTRY_NAME))
                            yield return new ValidationResult(String.Format("PRO_MARKETING_COUNTRY_NAME【{0}】与PRO_MARKETING_COUNTRY_CODE【{1}】不匹配。", l.DECLARE_MEASURE_UNIT_NAME, l.PRO_MARKETING_COUNTRY_CODE));
                    }
                    #endregion

                    #region 数量
                    if (true)
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.DECLARE_COUNT);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("DECLARE_COUNT格式错误。");
                    }
                    #endregion

                    #region 单价
                    if (true)
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.DECLARE_PRICE);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("DECLARE_PRICE格式错误。");
                    }
                    #endregion

                    #region 计量单位
                    if (String.IsNullOrWhiteSpace(l.DECLARE_MEASURE_UNIT_CODE))
                        validResult.Append("DECLARE_MEASURE_UNIT_CODE必填,");
                    else
                    {
                        var paraUnit = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.DECLARE_MEASURE_UNIT_CODE);
                        if (paraUnit == null)
                            validResult.Append(string.Format("DECLARE_MEASURE_UNIT_CODE【{0}】不存在,", l.DECLARE_MEASURE_UNIT_CODE));

                        var paraEciqName = ParaEciqManager.Instance.EciqUnitName(l.DECLARE_MEASURE_UNIT_CODE);
                        if (!paraEciqName.Equals(l.DECLARE_MEASURE_UNIT_NAME))
                            yield return new ValidationResult(String.Format("DECLARE_MEASURE_UNIT_NAME【{0}】DECLARE_MEASURE_UNIT_CODE【{1}】不匹配。", l.DECLARE_MEASURE_UNIT_NAME, l.DECLARE_MEASURE_UNIT_CODE));
                    }
                    #endregion

                    #region 国检备案号
                    if (String.IsNullOrWhiteSpace(l.PRODUCT_RECORD_NO))
                        validResult.Append("PRODUCT_RECORD_NO必填,");
                    #endregion

                    #region 序号
                    if (String.IsNullOrWhiteSpace(l.SEQ_NO.ToString()))
                        validResult.Append("SEQ_NO必填,");
                    #endregion

                    #region 货号
                    if (String.IsNullOrWhiteSpace(l.SKU_NO))
                        validResult.Append("SKU_NO必填,");
                    #endregion

                    #region 品牌
                    if (String.IsNullOrWhiteSpace(l.PROD_BRD_CN))
                        validResult.Append("PROD_BRD_CN必填,");
                    #endregion

                    #region 总价
                    if (true)
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.PRICE_TOTAL_VAL);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("PRICE_TOTAL_VAL格式错误。");
                    }
                    #endregion

                    #region 币制代码
                    if (String.IsNullOrWhiteSpace(l.CURR_UNIT))
                        validResult.Append("CURR_UNIT必填,");
                    else
                    {
                        var paraUnit = ParaEciqManager.Instance.ParaEciqCurrency.Find(o => o.CODE == l.CURR_UNIT);
                        if (paraUnit == null)
                            validResult.Append(string.Format("CURR_UNIT【{0}】不存在,", l.CURR_UNIT));
                    }
                    #endregion

                    #region 条形码
                    if (String.IsNullOrWhiteSpace(l.COMM_BARCODE))
                        validResult.Append("COMM_BARCODE必填,");
                    #endregion

                    #region 提运单
                    if (String.IsNullOrWhiteSpace(l.MAIN_WB_NO))
                        validResult.Append("MAIN_WB_NO必填,");
                    #endregion

                    #endregion

                    #region 非必填信息

                    #region 商品毛重
                    if (!String.IsNullOrWhiteSpace(l.GOODS_ROUGH_WEIGHT.ToString()))
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.GOODS_ROUGH_WEIGHT);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("GOODS_ROUGH_WEIGHT格式错误。");
                    }
                    #endregion

                    #region 创建时间
                    if (!string.IsNullOrEmpty(CREATE_TIME))
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDateTime(CREATE_TIME).ToString("yyyy-MM-dd");
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("CREATE_TIME格式错误。");
                    }
                    #endregion

                    #endregion

                }
            }

            #endregion
            #endregion

            #region 业务验证

            EciqExitDeclareBillService DeclareBillService = new EciqExitDeclareBillService();

            if (!String.IsNullOrWhiteSpace(LOGISTICS_NO))
            {
                //从清单表中，验证运单号是否多次绑定
                IList<EciqExitDeclareBillHead> declformTemp = DeclareBillService.GetDeclformByWaybill(LOGISTICS_NO);
                EciqExitDeclareBillHead selectedDeclform = declformTemp.FirstOrDefault(o => o.LOGISTICS_NO != LOGISTICS_NO);
                if (selectedDeclform != null)
                    yield return new ValidationResult(String.Format("包含的运单信息【{0}】已绑定给其他出区清单【{1}】。", LOGISTICS_NO, selectedDeclform.LOGISTICS_NO));
 
            }

            #endregion 


        }
    }
}