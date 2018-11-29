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
    public class CuDeclformModel:CuDeclformHead,IValidatableObject
    {
        /// <summary>
        /// 当前登录账户对应的组织机构代码
        /// </summary>
        public string COP_GB_CODE { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 必填项
            //运单编号
            if (string.IsNullOrEmpty(LOGISTICS_NO))
                yield return new ValidationResult("WAYBILL_ID必填。");

            #region 物流企业
            if (string.IsNullOrEmpty(LOGISTICS_CODE))
                yield return new ValidationResult("LOGI_ENTE_CODE必填。");
            else
            {
                string logisticsName = "";
                ValidationResult logisticsResult = CheckPara.CheckCuCompanyCode(LOGISTICS_CODE, COP_TYPE.COP_TYPE_L, IE_FLAG.IE_FLAG_I, "LOGI_ENTE_CODE", ref logisticsName);
                if (logisticsResult != null)
                    yield return logisticsResult;
                else
                    LOGISTICS_NAME = logisticsName;
            }
            #endregion

            //订单编号
            if (string.IsNullOrEmpty(ORDER_NO))
                yield return new ValidationResult("ORDER_NO必填。");

            #region 电商平台
            if (string.IsNullOrEmpty(EBP_CODE))
                yield return new ValidationResult("EB_PLAT_ID必填。");
            else
            {
                string ebpName = "";
                ValidationResult ebpResult = CheckPara.CheckCuCompanyCode(EBP_CODE, COP_TYPE.COP_TYPE_EP, IE_FLAG.IE_FLAG_I, "EB_PLAT_ID", ref ebpName);
                if (ebpResult != null)
                    yield return ebpResult;
                else
                    EBP_NAME = ebpName;
            }
            #endregion

            #region 电商企业
            if (string.IsNullOrEmpty(EBC_CODE))
                yield return new ValidationResult("EB_CODE必填。");
            else
            {
                string ebcName = "";
                ValidationResult ebcResult = CheckPara.CheckCuCompanyCode(EBC_CODE, COP_TYPE.COP_TYPE_E, IE_FLAG.IE_FLAG_I, "EB_CODE", ref ebcName);
                if (ebcResult != null)
                    yield return ebcResult;
                else
                    EBC_NAME = ebcName;
            }
            #endregion

            #region 担保企业
            if (ASSURE_CODE.Equals(EBC_CODE))
                ASSURE_NAME = EBC_NAME;
            else if (ASSURE_CODE.Equals(EBP_CODE))
                ASSURE_NAME = EBP_NAME;
            else if (ASSURE_CODE.Equals(LOGISTICS_CODE))
                ASSURE_NAME = LOGISTICS_NAME;
            else
                yield return new ValidationResult(String.Format("ASSURE_CODE应【{0}】只限清单的EB_CODE、EB_PLAT_ID、LOGI_ENTE_CODE之一", ASSURE_CODE));
            #endregion

            #region 申报海关
            if (String.IsNullOrWhiteSpace(CUSTOMS_CODE))
                yield return new ValidationResult("DECL_PORT必填。");
            else
            {
                var paraCustomsCode = ParaCacheManager.Instance.ParaCustoms.Find(o => o.CUST_CODE == CUSTOMS_CODE);
                if (paraCustomsCode == null)
                    yield return new ValidationResult(String.Format("DECL_PORT【{0}】不存在。", CUSTOMS_CODE));
            }
            #endregion

            #region 口岸海关
            if (String.IsNullOrWhiteSpace(PORT_CODE))
                yield return new ValidationResult("IE_PORT必填。");
            else
            {
                var paraPortCode = ParaCacheManager.Instance.ParaCustoms.Find(o => o.CUST_CODE == PORT_CODE);
                if (paraPortCode == null)
                    yield return new ValidationResult(String.Format("IE_PORT【{0}】不存在。", PORT_CODE));
            }
            #endregion

            #region 贸易方式
            if (String.IsNullOrWhiteSpace(TRADE_MODE))
                yield return new ValidationResult("TRADE_MODE必填。");
            else
            {
                var paraTradeMode = ParaCacheManager.Instance.ParaCuTradeMode.Find(o => o.CODE == TRADE_MODE);
                if (paraTradeMode == null)
                    yield return new ValidationResult(String.Format("TRADE_MODE【{0}】不存在,", TRADE_MODE));
            }
            #endregion

            #region 运输方式
            if (String.IsNullOrWhiteSpace(TRAF_MODE))
                yield return new ValidationResult("TRAF_MODE应必填。");
            else
            {
                var paraTrafMode = ParaCacheManager.Instance.ParaCuTrafMode.Find(o => o.CODE == TRAF_MODE);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("TRAF_MODE【{0}】不存在,", TRAF_MODE));
            }

            #endregion

            #region 申报企业
            if (String.IsNullOrWhiteSpace(AGENT_CODE))
                yield return new ValidationResult("AGENT_CODE应必填。");
            else
            {
                string agentName = "";
                ValidationResult agentResult = CheckPara.CheckCuCompanyCode(AGENT_CODE, COP_TYPE.COP_TYPE_A, IE_FLAG.IE_FLAG_I, "AGENT_CODE", ref agentName);
                if (agentResult != null)
                    yield return agentResult;
                else
                    AGENT_NAME = agentName;
            }
            #endregion

            //申报日期

            //订购人证件类型
            if (!BUYER_ID_TYPE.Equals(CU_CERT_TYPE.CU_CERT_TYPE_1))
                yield return new ValidationResult("OWNER_CERT_ID应为【1】");
            //订购人证件号
            if (string.IsNullOrEmpty(BUYER_ID_NUMBER))
                yield return new ValidationResult("OWNER_CERT_ID必填。");
            //订购人姓名
            if (string.IsNullOrEmpty(BUYER_NAME))
                yield return new ValidationResult("OWNER_NAME必填。");
            //订购人电话
            if (string.IsNullOrEmpty(BUYER_TELEPHONE))
                yield return new ValidationResult("OWNER_TEL必填。");
            //else
            //{
            //    Regex rx = new Regex(@"^1[3|4|5|7|8]\d{9}$|^(\d{3,4}-)?\d{7,8}$");

            //    if (!rx.IsMatch(BUYER_TELEPHONE))
            //        yield return new ValidationResult("OWNER_TEL格式有误。");
            //}
            //收货地址
            if (string.IsNullOrEmpty(CONSIGNEE_ADDRESS))
                yield return new ValidationResult("OWNER_ADDR必填。");

            #region 起运国
            if (String.IsNullOrWhiteSpace(COUNTRY))
                yield return new ValidationResult("TRADE_COUNT必填。");
            else
            {
                var paraCountry = ParaCacheManager.Instance.ParaCountry.Find(o => o.CODE == COUNTRY);
                if (paraCountry == null)
                    yield return new ValidationResult(String.Format("TRADE_COUNT【{0}】不存在,", COUNTRY));
            }
            #endregion

            #region 提运单号、航班航次号、运输工具编号
            if (TRADE_MODE.Equals(CU_TRADE_MODE.CU_TRADE_MODE_9610))
            {
                if (String.IsNullOrWhiteSpace(TRAF_NO))
                    yield return new ValidationResult("TRAF_NAME应必填,");

                if (String.IsNullOrWhiteSpace(VOYAGE_NO))
                    yield return new ValidationResult("VOYAGE_NO应必填,");

                if (String.IsNullOrWhiteSpace(BILL_NO))
                    yield return new ValidationResult("BILL_NO应必填；");
            }
            else if (TRADE_MODE.Equals(CU_TRADE_MODE.CU_TRADE_MODE_1210))
            {
                if (String.IsNullOrWhiteSpace(EMS_NO))
                    yield return new ValidationResult("MANUAL_NO应必填；");

                #region 区内企业
                if (String.IsNullOrWhiteSpace(AREA_CODE))
                    yield return new ValidationResult("AREA_CODE应必填,");
                else
                {
                    string areaName = "";
                    ValidationResult areaResult = CheckPara.CheckCuCompanyCode(AREA_CODE, COP_TYPE.COP_TYPE_R, IE_FLAG.IE_FLAG_I, "AREA_CODE", ref areaName);
                    if (areaResult != null)
                        yield return areaResult;
                    else
                        AGENT_NAME = areaName;
                }
                #endregion

            }
            #endregion
            //运费
            if (FREIGHT < 0)
                yield return new ValidationResult("FREIGHT应大于【0】。");
            //保费
            if (INSURED_FEE < 0)
                yield return new ValidationResult("INSURED_FEE应大于【0】。");
            //币制
            if (!CURRENCY.Equals(CU_CURRENCY.CU_CURRENCY_142))
                yield return new ValidationResult("CURR_CODE应为【142】");
            //件数
            if (PACK_NO != 1)
                yield return new ValidationResult("PACK_NUM应为【1】");
            //毛重
            if (GROSS_WEIGHT <= 0)
                yield return new ValidationResult("GROSS_WEIGHT应大于【0】。");
            //净重
            if (NET_WEIGHT <= 0)
                yield return new ValidationResult("NET_WEIGHT应大于【0】。");
            else if (NET_WEIGHT > GROSS_WEIGHT)
                yield return new ValidationResult("GROSS_WEIGHT应大于等于NET_WEIGHT。");


            #region 包装种类

            if (String.IsNullOrWhiteSpace(WRAP_TYPE))
                yield return new ValidationResult("WRAP_TYPE必填。");
            else
            {
                var paraWrapType = ParaCacheManager.Instance.ParaWrapType.Find(o => o.CODE == WRAP_TYPE);
                if (paraWrapType == null)
                    yield return new ValidationResult(String.Format("WRAP_TYPE【{0}】不存在,", WRAP_TYPE));
            }

            #endregion

            #endregion

            #region 菲必填项
            //监管场所
            if (!String.IsNullOrWhiteSpace(LOCT_NO))
            {
                var paraTrafMode = ParaCacheManager.Instance.ParaSuperPlace.Find(o => o.CODE == LOCT_NO);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("SUPER_PLACE_CODE【{0}】不存在,", LOCT_NO));
            }



            #endregion

            #region 商品信息
            #region 验证商品信息

            if (CuDeclformLists == null || CuDeclformLists.Count < 0)
                yield return new ValidationResult("商品信息必填");
            else
            {
                int i = 1;
                foreach (CuDeclformList l in CuDeclformLists)
                {
                    l.G_NUM = i++;
                    StringBuilder validResult = new StringBuilder();
                    //验证商品信息，以及录入的计量单位、法定计量单位是否和备案参数表里的一致
                    var paraComplex = ParaCacheManager.Instance.ParaCuComplex.Find(o => o.HS_G_CODE == l.G_CODE);
                    if (paraComplex == null)
                        validResult.Append(string.Format("CODE_TS【{0}】不存在,", l.G_CODE));
                    else
                    {
                        //if (!String.IsNullOrWhiteSpace(paraComplex.HS_UNIT) && !paraComplex.HS_UNIT.Equals(l.UNIT))
                        //    validResult.Append(string.Format("G_UNIT【{0}】与商品备案参数表中的【{1}】不符,", l.UNIT, paraComplex.HS_UNIT));
                        if (!String.IsNullOrWhiteSpace(paraComplex.HS_UNIT_1) && !paraComplex.HS_UNIT_1.Equals(l.UNIT_1))
                            validResult.Append(string.Format("UNIT_1【{0}】与商品备案参数表中的【{1}】不符,", l.UNIT_1, paraComplex.HS_UNIT_1));
                        if (!String.IsNullOrWhiteSpace(paraComplex.HS_UNIT_2))
                            l.UNIT_2 = paraComplex.HS_UNIT_2;    
                    }

                    //备案号
                    #region 原产国
                    if (String.IsNullOrWhiteSpace(l.COUNTRY))
                        validResult.Append("ORIGIN_COUNTRY应必填,");
                    else
                    {
                        var paraGoodsCountry = ParaCacheManager.Instance.ParaCountry.Find(o => o.CODE == l.COUNTRY);
                        if (paraGoodsCountry == null)
                            validResult.Append(string.Format("ORIGIN_COUNTRY【{0}】不存在,", l.UNIT));
                    }
                    #endregion
                    //商品名称
                    if (String.IsNullOrWhiteSpace(l.G_NAME))
                        validResult.Append("G_NAME应必填,");
                    //商品规格型号
                    if (String.IsNullOrWhiteSpace(l.G_MODEL))
                        validResult.Append("G_MODEL应必填,");


                    //数量
                    if (l.QTY < 0)
                        yield return new ValidationResult("G_NUM应大于【0】。");
                    //法定数量
                    if (l.QTY_1 < 0)
                        yield return new ValidationResult("QTY_1应大于【0】。");


                    #region 计量单位
                    if (String.IsNullOrWhiteSpace(l.UNIT))
                        validResult.Append("G_UNIT应必填,");
                    else
                    {
                        var paraUnit = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.UNIT);
                        if (paraUnit == null)
                            validResult.Append(string.Format("G_UNIT【{0}】不存在,", l.UNIT));
                    }
                    #endregion

                    #region 法定单位
                    if (String.IsNullOrWhiteSpace(l.UNIT_1))
                        validResult.Append("UNIT_1应必填,");
                    else
                    {
                        var paraUnit1 = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.UNIT_1);
                        if (paraUnit1 == null)
                            validResult.Append(string.Format("UNIT_1【{0}】不存在,", l.UNIT_1));
                    }
                    #endregion

                    #region 第二单位
                    if (!String.IsNullOrWhiteSpace(l.UNIT_2))
                    {
                        var paraUnit2 = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.UNIT_2);
                        if (paraUnit2 == null)
                            validResult.Append(string.Format("UNIT_2【{0}】不存在,", l.UNIT_2));
                    }
                    #endregion
                    //单价
                    if (l.PRICE < 0)
                        yield return new ValidationResult("PRICE应大于【0】。");
                    //币制
                    if (!l.CURRENCY.Equals(CU_CURRENCY.CU_CURRENCY_142))
                        validResult.Append("CURR_CODE应为【142】。");

                    if (validResult.Length > 0)
                        yield return new ValidationResult(string.Format("第{0}项商品{1}", l.G_NUM.ToString(), validResult.ToString()));
                }
            }

            #endregion
            #endregion

            #region 业务验证

            CuDeclformService declformService = new CuDeclformService();
            CuWayBillService waybillService = new CuWayBillService();

            if (NET_WEIGHT > GROSS_WEIGHT)
                yield return new ValidationResult("毛重应大于等于净重。");

            if (!String.IsNullOrWhiteSpace(LOGISTICS_NO) && !String.IsNullOrWhiteSpace(LOGISTICS_CODE))
            {
                //从清单表中，验证运单号是否多次绑定
                IList<CuDeclformHead> declformTemp = declformService.GetDeclformByWaybill(LOGISTICS_NO, LOGISTICS_CODE);
                CuDeclformHead selectedDeclform = declformTemp.FirstOrDefault(o => o.GUID != GUID);
                if (selectedDeclform != null)
                    yield return new ValidationResult(String.Format("包含的运单信息【{0}】已绑定给其他清单【{1}】。", LOGISTICS_NO, selectedDeclform.COP_NO));

                //验证同样的运单信息，在运单中和清单中两单包含的订单信息是否一致
                IList<CuWaybillHead> waybillTemp = waybillService.GetWaybillByBusKey(LOGISTICS_NO, LOGISTICS_CODE);
                CuWaybillHead selectedWaybill = waybillTemp.FirstOrDefault(o => o.ORDER_NO != ORDER_NO || o.EBP_CODE != EBP_CODE);
                if (selectedWaybill != null)
                    yield return new ValidationResult(String.Format("清单中的订单号【{0}】【{1}】，与有相同运单的运单中的订单信息【{2}】【{3}】不一致。", ORDER_NO, EBP_CODE, selectedWaybill.ORDER_NO, selectedWaybill.EBP_CODE));

            }


            if (!String.IsNullOrWhiteSpace(ORDER_NO) && !String.IsNullOrWhiteSpace(EBP_CODE))
            {
                //从清单表中，验证订单号是否多次绑定
                IList<CuDeclformHead> declformTemp = declformService.GetDeclformByOrder(ORDER_NO, EBP_CODE);
                CuDeclformHead selectedDeclform = declformTemp.FirstOrDefault(o => o.GUID != GUID);
                if (selectedDeclform != null)
                    yield return new ValidationResult(String.Format("包含的订单信息【{0}】已绑定给其他清单【{1}】。", ORDER_NO, selectedDeclform.COP_NO));

                //验证同样的运单信息，在运单中和清单中两单包含的订单信息是否一致
                IList<CuWaybillHead> waybillTemp = waybillService.GetWaybillByOrder(ORDER_NO, EBP_CODE);
                CuWaybillHead selectedWaybill = waybillTemp.FirstOrDefault(o => o.LOGISTICS_NO != LOGISTICS_NO || o.LOGISTICS_CODE != LOGISTICS_CODE);
                if (selectedWaybill != null)
                    yield return new ValidationResult(String.Format("清单中的运单号【{0}】【{1}】，与有相同订单的运单中的运单信息【{2}】【{3}】不一致。", ORDER_NO, EBP_CODE, selectedWaybill.ORDER_NO, selectedWaybill.EBP_CODE));

            }


            #endregion 


        }
    }
}