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
    public class CuWayBillModel : CuWaybillHead, IValidatableObject
    {
        public string COP_GB_CODE { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 必填项
            //运单编号
            if (string.IsNullOrEmpty(LOGISTICS_NO))
                yield return new ValidationResult("WAYBILL_ID必填。");

            #region 物流企业
            string logisticsName = "";
            ValidationResult logisticsResult = CheckPara.CheckCuCompanyCode(LOGISTICS_CODE, COP_TYPE.COP_TYPE_L, IE_FLAG.IE_FLAG_I, "LOGI_ENTE_CODE", ref logisticsName);
            if (logisticsResult != null)
                yield return logisticsResult;
            else
                LOGISTICS_NAME = logisticsName;

            #endregion

            //件数
            if (PACK_NO != 1)
                yield return new ValidationResult("PACK_NUM应等于【1】。");
            //运费
            //保价费
            //币制
            if (!CURRENCY.Equals(CU_CURRENCY.CU_CURRENCY_142))
                yield return new ValidationResult("CURR_CODE应为【142】。");
            //毛重
            if (WEIGHT <= 0)
                yield return new ValidationResult("GROSS_WEIGHT应大于【0】。");
            //收货人姓名     
            if (string.IsNullOrEmpty(CONSIGNEE))
                yield return new ValidationResult("CONSIGNEE_NAM应必填。");
            //收货人电话
            if (string.IsNullOrEmpty(CONSIGNEE_TELEPHONE))
                yield return new ValidationResult("CONSIGNEE_TEL应必填。");
            //else
            //{
            //    Regex rx = new Regex(@"^1[3|4|5|7|8]\d{9}$|^(\d{3,4}-)?\d{7,8}$");

            //    if (!rx.IsMatch(CONSIGNEE_TELEPHONE))
            //        yield return new ValidationResult("CONSIGNEE_TELEPHONE格式有误。");
            //}
            //收获人地址
            if (string.IsNullOrEmpty(CONSIGNEE_ADDRESS))
                yield return new ValidationResult("CONSIGNEE_ADDR应必填。");


            if (CBSP.Core.StatusInfo.WaybillGoods())
            {
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
                    yield return new ValidationResult("TRAF_CODE应必填。");
                else
                {
                    var paraTrafMode = ParaCacheManager.Instance.ParaCuTrafMode.Find(o => o.CODE == TRAF_MODE);
                    if (paraTrafMode == null)
                        yield return new ValidationResult(String.Format("TRAF_CODE【{0}】不存在,", TRAF_MODE));
                }

                #endregion

                #region 提运单号、航班航次号、运输工具编号
                if (TRADE_MODE.Equals(CU_TRADE_MODE.CU_TRADE_MODE_9610))
                {
                    if (String.IsNullOrWhiteSpace(TRAF_NO))
                        yield return new ValidationResult("TRAF_NAME应必填；");

                    if (String.IsNullOrWhiteSpace(VOYAGE_NO))
                        yield return new ValidationResult("VOYAGE_NO应必填；");

                    if (String.IsNullOrWhiteSpace(BILL_NO))
                        yield return new ValidationResult("BILL_NO应必填；");
                }
                #endregion

                //订单号
                if (string.IsNullOrEmpty(ORDER_NO))
                    yield return new ValidationResult("ORDER_ID必填。");

                #region 电商平台
                if (String.IsNullOrWhiteSpace(EBP_CODE))
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

                //净重
                if (NET_WEIGHT <= 0)
                    yield return new ValidationResult("NET_WEIGHT应大于【0】。");
                else if (NET_WEIGHT > WEIGHT)
                    yield return new ValidationResult("GROSS_WEIGHT应大于等于NET_WEIGHT。");

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
            }
            #endregion

            #region 选填项
            //监管场所
            if (!String.IsNullOrWhiteSpace(LOCT_NO))
            {
                var paraTrafMode = ParaCacheManager.Instance.ParaSuperPlace.Find(o => o.CODE == LOCT_NO);
                if (paraTrafMode == null)
                    yield return new ValidationResult(String.Format("SUPER_PLACE_CODE【{0}】不存在,", LOCT_NO));
            }

            #endregion

            #region 业务验证

            CuWayBillService waybillService = new CuWayBillService();
            CuDeclformService declformService = new CuDeclformService();

            if (waybillService.Exists(LOGISTICS_NO, LOGISTICS_CODE))
                yield return new ValidationResult("统一版运单号为【" + LOGISTICS_NO + "】且物流企业代码为【" + LOGISTICS_CODE + "】的运单已存在。");

            if (CBSP.Core.StatusInfo.WaybillGoods())
            {
                #region 业务验证

                if (!String.IsNullOrWhiteSpace(ORDER_NO) && !String.IsNullOrWhiteSpace(EBP_CODE))
                {
                    //在运单表中，验证订单是否多次绑定
                    IList<CuWaybillHead> waybillTemp = waybillService.GetWaybillByOrder(ORDER_NO, EBP_CODE);
                    CuWaybillHead selectedWaybill = waybillTemp.FirstOrDefault(o => o.GUID != GUID);
                    if (selectedWaybill != null)
                        yield return new ValidationResult(String.Format("其订单信息【{0}】已绑定给其他运单【{1}】。", ORDER_NO, selectedWaybill.LICENSE_NO));

                    //验证同样的订单信息，在运单中和清单中两单包含的运单信息是否一致
                    IList<CuDeclformHead> declformTemp = declformService.GetDeclformByOrder(ORDER_NO, EBP_CODE);
                    CuDeclformHead selectedDeclform = declformTemp.FirstOrDefault(o => o.LOGISTICS_NO != LOGISTICS_NO || o.LOGISTICS_CODE != LOGISTICS_CODE);
                    if (selectedDeclform != null)
                        yield return new ValidationResult(String.Format("运单中的运单号【{0}】【{1}】，与有相同订单的清单中的运单信息【{2}】【{3}】不一致。", LOGISTICS_NO, LOGISTICS_CODE, selectedDeclform.LOGISTICS_NO, selectedDeclform.LOGISTICS_CODE));

                }

                if (!String.IsNullOrWhiteSpace(LOGISTICS_NO) && !String.IsNullOrWhiteSpace(LOGISTICS_CODE))
                {
                    //验证同样的运单信息，在运单中和清单中两单包含的订单信息是否一致
                    IList<CuDeclformHead>  declformTemp = declformService.GetDeclformByWaybill(LOGISTICS_NO, LOGISTICS_CODE);
                    CuDeclformHead selectedDeclform = declformTemp.FirstOrDefault(o => o.ORDER_NO != ORDER_NO || o.EBP_CODE != EBP_CODE);
                    if (selectedDeclform != null)
                        yield return new ValidationResult(String.Format("运单中的订单号【{0}】【{1}】，与有相同运单的清单中的订单信息【{2}】【{3}】不一致。", ORDER_NO, EBP_CODE, selectedDeclform.ORDER_NO, selectedDeclform.EBP_CODE));

                }

                #endregion
            }

            #endregion
        }

    }
}