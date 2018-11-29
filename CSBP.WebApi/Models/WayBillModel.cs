using CBSP.Core.Domain.WayBill;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using CBSP.Web.Framework;

using CBSP.Services.WayBill;


namespace CSBP.WebApi.Models
{
    public class WayBillModel : WaybillHead, IValidatableObject
    {
        /// <summary>
        /// 当前登录账户对应的组织机构代码
        /// </summary>
        public string COP_GB_CODE { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            #region 公共必填项

            if (string.IsNullOrEmpty(WAYBILL_ID))
                yield return new ValidationResult("WAYBILL_ID必填。");

            //物流企业代码
            string logiEnteName = "";
            ValidationResult logiEnteResult = CheckPara.CheckCompanyCode(LOGI_ENTE_CODE, ParaCacheManager.Instance.LOGI_COP_TYPE, IE_FLAG,"LOGI_ENTE_CODE", ref logiEnteName);
            if (logiEnteResult != null)
                yield return logiEnteResult;
            else
                LOGI_ENTE_NAME = logiEnteName;

            //运输方式
            ValidationResult trafCodeResult = CheckPara.CheckTrafMode(TRAF_CODE, "TRAF_CODE");
            if (trafCodeResult != null)
                yield return trafCodeResult;

            //贸易方式
            ValidationResult tradeModeResult = CheckPara.CheckTradeMode(TRADE_MODE);
            if (tradeModeResult != null)
                yield return tradeModeResult;

            //申报口岸
            ValidationResult declPortResult = CheckPara.CheckCustCode(DECL_PORT, "DECL_PORT");
            if (declPortResult != null)
                yield return declPortResult;

            //进出口岸
            ValidationResult iePortResult = CheckPara.CheckCustCode(IE_PORT, "IE_PORT");
            if (iePortResult != null)
                yield return iePortResult;
            //船名
            if (string.IsNullOrEmpty(TRAF_NAME))
                yield return new ValidationResult("TRAF_NAME必填。");
            //航次
            if (string.IsNullOrEmpty(VOYAGE_NO))
                yield return new ValidationResult("VOYAGE_NO必填。");
            //提运单号
            if (string.IsNullOrEmpty(BILL_NO))
                yield return new ValidationResult("BILL_NO必填。");
            //件数
            if (PACK_NUM <= 0)
                yield return new ValidationResult("PACK_NUM必须大于0。");
            //毛重
            if (GROSS_WEIGHT <= 0)
                yield return new ValidationResult("GROSS_WEIGHT必须大于0。");
            //币制
            ValidationResult currCodeResult = CheckPara.CheckCurrCode(CURR_CODE, "CURR_CODE");
            if (currCodeResult != null)
                yield return currCodeResult;

            //运抵国/启运国
            ValidationResult tradeCountResult = CheckPara.CheckCountry(TRADE_COUNT, "TRADE_COUNT");
            if (tradeCountResult != null)
                yield return tradeCountResult;

            #endregion


            if (IE_FLAG.Equals("I"))
            {
                #region 进口必填项

                CURR_CODE = "142";
                //收货人名称
                if (string.IsNullOrEmpty(CONSIGNEE_NAME))
                    yield return new ValidationResult("CONSIGNEE_NAME必填");
                //收货人地址
                if (string.IsNullOrEmpty(CONSIGNEE_ADDR))
                    yield return new ValidationResult("CONSIGNEE_ADDR必填");
                //收货人电话
                if (string.IsNullOrEmpty(CONSIGNEE_TEL))
                    yield return new ValidationResult("CONSIGNEE_TEL必填");
                //发货人国家
                ValidationResult consignerCountResult = CheckPara.CheckCountry(CONSIGNER_COUN, "CONSIGNER_COUN");
                if (consignerCountResult != null)
                    yield return consignerCountResult;

                //监管场所
                ValidationResult superPlaceResult = CheckPara.CheckSuperPlace(SUPER_PLACE_CODE);
                if (superPlaceResult != null)
                    yield return superPlaceResult;


                //国内快递公司
                string expressCopName = "";
                ValidationResult expressCopResult = CheckPara.CheckCompanyCode(EXPRESS_COP_CODE, ParaCacheManager.Instance.EXP_COP_TYPE,IE_FLAG,"EXPRESS_COP_CODE", ref expressCopName);
                if (expressCopResult != null)
                    yield return expressCopResult;
                else
                    EXPRESS_COP_NAME = expressCopName;

                #endregion

                #region 进口选填
                if (!string.IsNullOrEmpty(CONSIGNEE_COUN))
                {
                    //收货人国家
                    ValidationResult countryResult = CheckPara.CheckCountry(CONSIGNEE_COUN, "CONSIGNEE_COUN");
                    if (countryResult != null)
                        yield return countryResult;
                }
                #endregion
            }
            else if (IE_FLAG.Equals("E"))
            {
                #region 出口必填项
                //收货人国家
                ValidationResult consigneeCountResult = CheckPara.CheckCountry(CONSIGNEE_COUN, "CONSIGNEE_COUN");
                if (consigneeCountResult != null)
                    yield return consigneeCountResult;
                //发货人名称
                if (string.IsNullOrEmpty(CONSIGNER_NAME))
                    yield return new ValidationResult("CONSIGNER_NAME必填");
                //发货人地址
                if (string.IsNullOrEmpty(CONSIGNER_ADDR))
                    yield return new ValidationResult("CONSIGNER_ADDR必填");
                //发货人电话
                if (string.IsNullOrEmpty(CONSIGNER_TEL))
                    yield return new ValidationResult("CONSIGNER_TEL必填");
                #endregion
                
                #region 出口选填
                if (!string.IsNullOrEmpty(CONSIGNER_COUN))
                {
                    //发货人国家
                    ValidationResult countryResult = CheckPara.CheckCountry(CONSIGNER_COUN, "CONSIGNER_COUN");
                    if (countryResult != null)
                        yield return countryResult;
                }
                #endregion

                #region 出口不填
                //国内快递
                EXPRESS_COP_CODE = "";
                EXPRESS_COP_NAME = "";
                //监管场所
                SUPER_PLACE_CODE = "";


                #endregion
            }
            else
                yield return new ValidationResult("IE_FLAG值无效。");

            #region 选填项的参数值的有效性

            if (!string.IsNullOrEmpty(CONSIGNEE_TEL) && !Regex.IsMatch(CONSIGNEE_TEL, @"\d{11}"))
                yield return new ValidationResult("CONSIGNEE_TEL必须11位数字。");

            if (!string.IsNullOrEmpty(CONSIGNER_TEL) && !Regex.IsMatch(CONSIGNER_TEL, @"\d{11}"))
                yield return new ValidationResult("CONSIGNER_TEL必须11位数字。");


            #endregion

            #region 检验商品信息

            if (WaybillLists != null && WaybillLists.Count > 0)
            {
                foreach (var w in WaybillLists)
                {
                    #region 根据商品企业内部编号，反填信息
                    var copComplex = ParaCacheManager.Instance.ParaCopComplex(COP_GB_CODE).Find(e => e.COP_INTERNAL_NO == w.COP_INTERNAL_NO);
                    if (copComplex != null)
                    {
                        w.CODE_TS = copComplex.G_CODE;
                        w.CURR_CODE = copComplex.COP_G_CURR;
                        w.G_MODEL = copComplex.COP_G_MODEL;
                        w.G_NAME = copComplex.COP_G_NAME;
                        w.G_UNIT = copComplex.COP_G_UNIT;
                    }

                    #endregion

                    //商品编码，反填商品描述
                    string gDesc = "";
                    ValidationResult codeTSResult = CheckPara.CheckComplexCode(w.CODE_TS, "CODE_TS", ref gDesc);
                    if (codeTSResult != null)
                        yield return codeTSResult;
                    else
                        w.G_DESC = gDesc;
                    //商品名称
                    if (string.IsNullOrEmpty(w.G_NAME))
                        yield return new ValidationResult("G_NAME必填。");
                    //数量
                    if (w.G_NUM <= 0)
                        yield return new ValidationResult("G_NUM必须大于0");
                    //计量单位
                    ValidationResult gUnitResult = CheckPara.CheckUnitCode(w.G_UNIT, "G_UNIT");
                    if (gUnitResult != null)
                        yield return gUnitResult;
                    //单价
                    if (w.PRICE <= 0)
                        yield return new ValidationResult("PRICE必须大于0.");

                    //运保费
                    if (w.FREIGHT < 0)
                        yield return new ValidationResult("FREIGHT必须大于等于0.");

                    //总价
                    w.TOTAL_PRICE = w.PRICE * w.G_NUM;
                    //表头总货款
                    TOTAL_PREMIUM += w.TOTAL_PRICE;
                    //表头总运保费
                    TOTAL_FREIGHT += w.FREIGHT;

                    if (string.IsNullOrEmpty(w.ORDER_ID))
                        yield return new ValidationResult("ORDER_ID必填。");

                    //电商平台
                    string ebPlatName = "";
                    ValidationResult ebPlatIdResult = CheckPara.CheckEbPlatId(w.EB_PLAT_ID,ref ebPlatName);
                    if (ebPlatIdResult != null)
                        yield return ebPlatIdResult;

                    //进口币制必须为142，出口商品币制=表头币制
                    if (IE_FLAG.Equals("I"))
                    {
                        w.CURR_CODE = "142";
                        w.F_CURR_CODE = "142";
                    }
                    else
                    {
                        //单价币制
                        w.CURR_CODE = CURR_CODE;

                        //运保费币制
                        ValidationResult fCurrCodeResult = CheckPara.CheckCurrCode(w.F_CURR_CODE, "F_CURR_CODE");
                        if (fCurrCodeResult != null)
                            yield return fCurrCodeResult;
                    }

                }//foreach
            }//if
            else
                yield return new ValidationResult("无商品列表。");

            #endregion

            #region 业务单据唯一性验证

            if (!string.IsNullOrEmpty(WAYBILL_ID) && !string.IsNullOrEmpty(LOGI_ENTE_CODE))
            {
                var bill = new CBSP.Services.WayBill.WayBillService().GetWaybillByKeys(WAYBILL_ID, LOGI_ENTE_CODE);
                if (bill != null)
                {
                    yield return new ValidationResult(string.Format("运单【{0}】、【{1}】已存在。", WAYBILL_ID, LOGI_ENTE_CODE));
                }
            }

            if (IE_FLAG.Equals("I"))
            {

                if (WaybillLists != null && WaybillLists.Count > 0)
                {
                    foreach (var item in WaybillLists)
                    {
                        if (item.G_NUM > 1 && item.PRICE.CompareTo(1000) > 0)
                        {
                            yield return new ValidationResult("单种商品数量多于1件时，PRICE不能超过1000");
                        }
                    }
                }

                if (WaybillLists != null && WaybillLists.Count > 0 && !string.IsNullOrEmpty(WaybillLists.First().ORDER_ID) && !string.IsNullOrEmpty(WaybillLists.First().EB_PLAT_ID))
                {
                    WayBillService waybillService = new CBSP.Services.WayBill.WayBillService();

                    var waybillTemp = waybillService.GetWaybillByOrder(WaybillLists.First().ORDER_ID, WaybillLists.First().EB_PLAT_ID);

                    if (waybillTemp != null)
                        {
                         if (waybillTemp.FirstOrDefault(o => o.LOGI_ENTE_CODE != LOGI_ENTE_CODE || o.WAYBILL_ID != WAYBILL_ID) != null)
                                yield return new ValidationResult(String.Format("运单【{0}】、【{1}】中的订单信息【{2}】、【{3}】的已绑定给了其他运单", LOGI_ENTE_CODE, WAYBILL_ID, WaybillLists.First().ORDER_ID, WaybillLists.First().EB_PLAT_ID));
                        }
                }
            }


            #endregion
        }
    }
}