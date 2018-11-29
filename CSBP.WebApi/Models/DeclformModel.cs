using CBSP.Core.Domain.DeclformBill;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

using CBSP.Web.Framework;

namespace CSBP.WebApi.Models
{
    public class DeclformModel:DeclformHead,IValidatableObject
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
            ValidationResult logiEnteResult = CheckPara.CheckCompanyCode(LOGI_ENTE_CODE, ParaCacheManager.Instance.LOGI_COP_TYPE,IE_FLAG,"LOGI_ENTE_CODE", ref logiEnteName);
            if (logiEnteResult != null)
                yield return logiEnteResult;
            else
                LOGI_ENTE_NAME = logiEnteName;


            //贸易方式
            ValidationResult tradeModeResult = CheckPara.CheckTradeMode(TRADE_MODE);
            if (tradeModeResult != null)
                yield return tradeModeResult;

            //业务类型
            ValidationResult businessTypeResult = CheckPara.CheckTradeMode(BUSINESS_TYPE);
            if (businessTypeResult != null)
                yield return businessTypeResult;

            //账册编号
            if (TRADE_MODE.Equals("1") && string.IsNullOrEmpty(MANUAL_NO))
                yield return new ValidationResult("MANUAL_NO必填");

            //退换货标志
            if (string.IsNullOrEmpty(RETURN_FLAG))
            {
                yield return new ValidationResult("RETURN_FLAG必填。");
            }
            else
            {
                if (RETURN_FLAG.Equals("1"))
                {
                    if (string.IsNullOrEmpty(REL_DECLFORM_ID))
                        yield return new ValidationResult("REL_DECLFORM_ID必填");

                    if (string.IsNullOrEmpty(REL_WAYBILL_ID))
                        yield return new ValidationResult("REL_WAYBILL_ID必填");

                    if (string.IsNullOrEmpty(REL_LOGI_ENTECODE))
                        yield return new ValidationResult("REL_LOGI_ENTECODE必填");

                }
            }
            //申报口岸
            ValidationResult declPortResult = CheckPara.CheckCustCode(DECL_PORT, "DECL_PORT");
            if (declPortResult != null)
                yield return declPortResult;

            //进出口岸
            ValidationResult iePortResult = CheckPara.CheckCustCode(IE_PORT, "IE_PORT");
            if (iePortResult != null)
                yield return iePortResult;

            //进出口日期
            if (IE_DATE == null)
                yield return new ValidationResult("IE_DATE必填。");

            //收货人姓名
            if (string.IsNullOrEmpty(OWNER_NAME))
                yield return new ValidationResult("OWNER_NAME必填。");
            //收货人地址
            if (string.IsNullOrEmpty(OWNER_ADDR))
                yield return new ValidationResult("OWNER_ADDR必填。");
            //收货人电话
            if (string.IsNullOrEmpty(OWNER_TEL))
                yield return new ValidationResult("OWNER_TEL必填。");

            //申报单位
            string agentName = "";
            ValidationResult agentCodeResult = CheckPara.CheckCompanyCode(AGENT_CODE, ParaCacheManager.Instance.APL_COP_TYPE, IE_FLAG, "AGENT_CODE", ref agentName);
            if (agentCodeResult != null)
                yield return agentCodeResult;
            else
                AGENT_NAME = agentName;
            
            //运输方式
            if (string.IsNullOrEmpty(TRAF_MODE))
                yield return new ValidationResult("TRAF_MODE必填。");

            //运输工具名称
            if (string.IsNullOrEmpty(TRAF_NAME))
                yield return new ValidationResult("TRAF_NAME必填。");

            //航次
            if (string.IsNullOrEmpty(VOYAGE_NO))
                yield return new ValidationResult("VOYAGE_NO必填。");

            //提运单号
            if (string.IsNullOrEmpty(BILL_NO))
                yield return new ValidationResult("BILL_NO必填。");

            //运抵国/启运国
            ValidationResult tradeCountResult = CheckPara.CheckCountry(TRADE_COUNT, "TRADE_COUNT");
            if (tradeCountResult != null)
                yield return tradeCountResult;

            //毛重
            if (GROSS_WEIGHT <= 0)
                yield return new ValidationResult("GROSS_WEIGHT必须大于0。");

            //总件数
            if (PACK_NUM <= 0)
                yield return new ValidationResult("PACK_NUM必须大于0。");

            //币制
            ValidationResult currCodeResult = CheckPara.CheckCurrCode(CURR_CODE, "CURR_CODE");
            if (currCodeResult != null)
                yield return currCodeResult;

            //电商平台
            string ebPlatName = "";
            ValidationResult ebPlatIdResult = CheckPara.CheckEbPlatId(EB_PLAT_ID,ref ebPlatName);
            if (ebPlatIdResult != null)
                yield return ebPlatIdResult;
            else
                EB_PLAT_NAME = ebPlatName;

            //电商企业
            string ebName = "";
            ValidationResult ebCodeResult = CheckPara.CheckCompanyCode(EB_CODE, ParaCacheManager.Instance.EB_COP_TYPE, IE_FLAG, "EB_CODE", ref ebName);
            if (ebCodeResult != null)
                yield return ebCodeResult;
            else
                EB_NAME = ebName;


            if (!string.IsNullOrEmpty(OWNER_TEL) && !Regex.IsMatch(OWNER_TEL, @"\d{11}"))
                yield return new ValidationResult("OWNER_TEL必须11位数字。");

            #endregion 

            if (IE_FLAG.Equals("I"))
            {
                #region 进口必填项

                OWNER_CERT_TYPE = "1";

                //收件人证件号
                if (string.IsNullOrEmpty(OWNER_CERT_ID))
                    yield return new ValidationResult("OWNER_CERT_ID必填");

                CURR_CODE = "142";

                //缴费企业
                string payCopName = "";
                ValidationResult payCopResult = CheckPara.CheckCompanyCode(PAYCOP_CODE, ParaCacheManager.Instance.APL_COP_TYPE,IE_FLAG, "PAYCOP_CODE", ref payCopName);
                if (payCopResult != null)
                    yield return payCopResult;
                else
                    PAYCOP_NAME = payCopName;

                //监管场所
                ValidationResult superPlaceCodeResult = CheckPara.CheckSuperPlace(SUPER_PLACE_CODE);
                if (superPlaceCodeResult != null)
                    yield return superPlaceCodeResult;

                //快递企业
                string expressCopName = "";
                ValidationResult expressCopResult = CheckPara.CheckCompanyCode(EXPRESS_COP_CODE, ParaCacheManager.Instance.EXP_COP_TYPE, IE_FLAG, "EXPRESS_COP_CODE", ref expressCopName);
                if (expressCopResult != null)
                    yield return expressCopResult;
                else
                    EXPRESS_COP_NAME = expressCopName;

                #endregion

                #region 进口不填项
                //经营单位
                TRADE_NAME = "";
                TRADE_CODE = "";

                #endregion

            }
            else if (IE_FLAG.Equals("E"))
            {
                #region 出口必填项
                //经营单位
                string tradeName = "";
                ValidationResult tradeCodeResult = CheckPara.CheckCompanyCode(TRADE_CODE, ParaCacheManager.Instance.EB_COP_TYPE, IE_FLAG, "TRADE_CODE", ref tradeName);
                if (tradeCodeResult != null)
                    yield return tradeCodeResult;
                else
                    TRADE_NAME = tradeName;

                #endregion

                #region 出口选填(如果填了，就判断)
                //收货人证件类型 出口选填
                if (!string.IsNullOrEmpty(OWNER_CERT_TYPE))
                {
                    ValidationResult ownerCertTypeResult = CheckPara.CheckCertType(OWNER_CERT_TYPE, "OWNER_CERT_TYPE");
                    if (ownerCertTypeResult != null)
                        yield return ownerCertTypeResult;
                }

                //征免方式

                //用途
                #endregion 

                #region 出口不填项
                //代缴企业
                PAYCOP_CODE = "";
                PAYCOP_NAME = "";
                //监管场所代码
                SUPER_PLACE_CODE = "";
                //国内快递企业
                EXPRESS_COP_CODE = "";
                EXPRESS_COP_NAME = "";
                #endregion
            }
            else
                yield return new ValidationResult("IE_FLAG值无效。");

            #region 检验商品信息

            if (DeclformLists != null && DeclformLists.Count > 0)
            {
                foreach (var d in DeclformLists)
                {
                    #region 根据商品企业内部编号，反填信息
                    var copComplex = ParaCacheManager.Instance.ParaCopComplex(COP_GB_CODE).Find(e => e.COP_INTERNAL_NO == d.COP_INTERNAL_NO);
                    if (copComplex != null)
                    {
                        d.CODE_TS = copComplex.G_CODE;
                        d.CURR_CODE = copComplex.COP_G_CURR;
                        d.G_MODEL = copComplex.COP_G_MODEL;
                        d.G_NAME = copComplex.COP_G_NAME;
                        d.G_UNIT = copComplex.COP_G_UNIT;
                        d.ORIGIN_COUNTRY = copComplex.COP_G_COUNTRY;
                    }

                    #endregion

                    //商品编码，反填商品描述
                    string gDesc = "";
                    ValidationResult codeTSResult = CheckPara.CheckComplexCode(d.CODE_TS, "CODE_TS", ref gDesc);
                    if (codeTSResult != null)
                        yield return codeTSResult;
                    else
                        d.G_DESC = gDesc;
                    //商品名称
                    if (string.IsNullOrEmpty(d.G_NAME))
                        yield return new ValidationResult("G_NAME必填。");
                    //数量
                    if (d.G_NUM <= 0)
                        yield return new ValidationResult("G_NUM必须大于0");
                    //计量单位
                    ValidationResult gUnitResult = CheckPara.CheckUnitCode(d.G_UNIT, "G_UNIT");
                    if (gUnitResult != null)
                        yield return gUnitResult;
                    //单价
                    if (d.PRICE <= 0)
                        yield return new ValidationResult("PRICE必须大于0.");
                    //法定数量
                    if (d.QTY_1 <= 0)
                        yield return new ValidationResult("QTY_1必须大于0");
                    //法定单位
                    ValidationResult unit1Result = CheckPara.CheckUnitCode(d.UNIT_1, "UNIT_1");
                    if (unit1Result != null)
                        yield return unit1Result;
                    //目的国
                    ValidationResult originCountryResult = CheckPara.CheckCountry(d.ORIGIN_COUNTRY, "ORIGIN_COUNTRY");
                    if (originCountryResult != null)
                        yield return originCountryResult;

                    //总价
                    d.TOTAL_PRICE = d.PRICE * d.G_NUM;
                    //总货款
                    TRANS_TOTAL_PRICE += d.TOTAL_PRICE==null?0:Convert.ToDecimal(d.TOTAL_PRICE);

                    //进口币制必须为142，出口商品币制=表头币制
                    if (IE_FLAG.Equals("I"))
                    {
                        d.CURR_CODE = "142";

                        if (TRADE_MODE.Equals("1"))
                        {
                            //料件成品
                            if (string.IsNullOrEmpty(d.G_TYPE))
                                yield return new ValidationResult("G_TYPE必填。");
                            //项号
                            if (d.EMS_G_NO<=0)
                                yield return new ValidationResult("EMS_G_NO必填。");
                        }
                    }
                    else
                        d.CURR_CODE = CURR_CODE;

                }//foreach
            }//if
            else
                yield return new ValidationResult("无商品列表。");

            #endregion


            #region 业务验证
            //运单对应的唯一的清单判断
            if (!string.IsNullOrEmpty(WAYBILL_ID) && !string.IsNullOrEmpty(LOGI_ENTE_CODE))
            {
                var bill = new CBSP.Services.DeclformBill.DeclformService().GetDeclformByKeys(WAYBILL_ID, LOGI_ENTE_CODE);
                if (bill != null)
                {
                    yield return new ValidationResult(string.Format("清单【{0}】、【{1}】已存在。", WAYBILL_ID, LOGI_ENTE_CODE));
                }
            }


            if (DeclformLists != null && DeclformLists.Count > 0)
            {
                foreach (var item in DeclformLists)
                {
                    if (item.G_NUM > 1 && item.PRICE.CompareTo(1000) > 0)
                    {
                        yield return new ValidationResult("单种商品数量多于1件时，PRICE不能超过1000");
                    }
                }
            }

            if (TRAF_MODE.Equals("2"))//水运
            {
                if (DeclformContains != null)
                {
                    for (int i = 0; i < DeclformContains.Count; i++)
                    {
                        if (string.IsNullOrWhiteSpace(DeclformContains.ToList()[i].CONTAINER_ID))
                        {
                            yield return new ValidationResult("运输方式为水运时，CONTAINER_ID必填");
                        }
                    }
                }
            }
            #endregion
            
        }
    }
}