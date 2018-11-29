using CBSP.Core;
using CBSP.Core.Domain.EciqProductBill;
using CBSP.Core.Domain.OrderBill;
using CBSP.Core.Domain.PaymentBill;
using CBSP.Services.EciqProductBill;
using CBSP.Services.OrderBill;
using CBSP.Services.PaymentBill;
using CBSP.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace CSBP.WebApi.Models
{
    public class EciqProductBillModel : EciqProductBillHead, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            try
            {
                #region 必填项

                #region 电商平台
                if (string.IsNullOrEmpty(ENT_CBEC_CODE))
                    yield return new ValidationResult("ENT_CBEC_CODE必填。");
                {
                    string ebpName = "";
                    ValidationResult ebpResult = CheckPara.CheckEciqCompanyCode(ENT_CBEC_CODE, COP_TYPE.COP_TYPE_EP, IE_FLAG.IE_FLAG_I, "EB_PLAT_ID", ref ebpName);
                    if (ebpResult != null)
                        yield return ebpResult;
                    else
                        ENT_CNAME = ebpName;
                }
                #endregion

                #region HS_CODE
                //验证商品信息
                var paraComplex = ParaCacheManager.Instance.ParaCuComplex.Find(o => o.HS_G_CODE == HS_CODE);
                if (paraComplex == null)
                    yield return new ValidationResult(string.Format("HS_CODE【{0}】不存在,", HS_CODE));
                #endregion

                #region 生产国代码
                if (String.IsNullOrWhiteSpace(PROD_COUNTRY_CODE))
                    yield return new ValidationResult("PROD_COUNTRY_CODE必填。");
                else
                {
                    var paraEciqCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == PROD_COUNTRY_CODE);
                    if (paraEciqCode == null)
                        yield return new ValidationResult(String.Format("PROD_COUNTRY_CODE【{0}】不存在。", PROD_COUNTRY_CODE));
                    if(!String.IsNullOrWhiteSpace(PROD_COUNTRY_NAME))
                    {
                        var paraEciqName = ParaEciqManager.Instance.EciqCountryName(PROD_COUNTRY_CODE);
                        if (!paraEciqName.Equals(PROD_COUNTRY_NAME))
                            yield return new ValidationResult(String.Format("PROD_COUNTRY_NAME【{0}】与PROD_COUNTRY_CODE【{1}】不匹配。", PROD_COUNTRY_NAME, PROD_COUNTRY_CODE));
                    }                  
                }
                #endregion

                #region 施检机构
                if (String.IsNullOrWhiteSpace(MONITOR_ORG_CODE))
                    yield return new ValidationResult("MONITOR_ORG_CODE必填。");
                else
                {
                    var paraEciqInspOrgCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == MONITOR_ORG_CODE);
                    if (paraEciqInspOrgCode == null)
                        yield return new ValidationResult(String.Format("MONITOR_ORG_CODE【{0}】不存在。", MONITOR_ORG_CODE));
                    var paraEciqName = ParaEciqManager.Instance.EciqInspOrgName(MONITOR_ORG_CODE);
                    if (!paraEciqName.Equals(MONITOR_ORG_NAME))
                        yield return new ValidationResult(String.Format("MONITOR_ORG_NAME【{0}】与MONITOR_ORG_CODE【{1}】不匹配。", MONITOR_ORG_NAME, MONITOR_ORG_CODE));
                }
                #endregion

                #region 进出口标志
                if (String.IsNullOrWhiteSpace(ENTER_OUT_FLAG))
                    yield return new ValidationResult("ENTER_OUT_FLAG必填。");
                else
                {
                    var paraEciqInOutFlagCode = ParaEciqManager.Instance.ParaEciqInOutFlag.Find(o => o.CODE == ENTER_OUT_FLAG);
                    if (paraEciqInOutFlagCode == null)
                        yield return new ValidationResult(String.Format("ENTER_OUT_FLAG【{0}】不存在。", ENTER_OUT_FLAG));
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

                #region 商品名称
                if (string.IsNullOrEmpty(PRODUCT_NAME))
                    yield return new ValidationResult("PRODUCT_NAME必填。");
                #endregion

                #region 条形码
                if (string.IsNullOrEmpty(BAR_CODE))
                    yield return new ValidationResult("BAR_CODE必填。");
                #endregion

                #region 品牌
                if (string.IsNullOrEmpty(BRAND))
                    yield return new ValidationResult("BRAND必填。");
                #endregion

                #region 规格型号
                if (string.IsNullOrEmpty(MODEL))
                    yield return new ValidationResult("MODEL必填。");
                #endregion

                #region 主要成分
                if (string.IsNullOrEmpty(BASES))
                    yield return new ValidationResult("BASES必填。");
                #endregion

                #region 货号
                if (string.IsNullOrEmpty(SKU))
                    yield return new ValidationResult("SKU必填。");
                #endregion

                #region 生成企业
                if (string.IsNullOrEmpty(PROD_ENT))
                    yield return new ValidationResult("PROD_ENT必填。");
                #endregion

                #region 申请时间
                if (string.IsNullOrEmpty(APPLY_DATE))
                    yield return new ValidationResult("APPLY_DATE必填。");
                else
                {
                    bool flag = true;
                    try
                    {
                        Convert.ToDateTime(APPLY_DATE).ToString("yyyy-MM-dd");
                    }
                    catch
                    {
                        flag = false;
                    }
                    if (!flag)
                        yield return new ValidationResult("APPLY_DATE格式错误。");
                }
                #endregion

                #region 备案状态
                if (String.IsNullOrWhiteSpace(PRODUCT_STATUS))
                    yield return new ValidationResult("PRODUCT_STATUS必填。");
                else
                {
                    var paraEciqCode = ParaEciqManager.Instance.ECIQ_ParaProducttStatus.Find(o => o.Code == PRODUCT_STATUS);
                    if (paraEciqCode == null)
                        yield return new ValidationResult(String.Format("PRODUCT_STATUS【{0}】不存在。", PRODUCT_STATUS));
                }
                #endregion

                #region 监管机构
                if (String.IsNullOrWhiteSpace(MONITOR_ORG_CODE))
                    yield return new ValidationResult("MONITOR_ORG_CODE必填。");
                else
                {
                    var paraEciqCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == MONITOR_ORG_CODE);
                    if (paraEciqCode == null)
                        yield return new ValidationResult(String.Format("MONITOR_ORG_CODE【{0}】不存在。", MONITOR_ORG_CODE));
                }
                #endregion

                #region 销售方式
                if (String.IsNullOrWhiteSpace(TRADE_MODE))
                    yield return new ValidationResult("TRADE_MODE必填。");
                else
                {
                    var paraEciqCode = ParaEciqManager.Instance.ParaEciqTrade.Find(o => o.CODE == TRADE_MODE);
                    if (paraEciqCode == null)
                        yield return new ValidationResult(String.Format("TRADE_MODE【{0}】不存在。", TRADE_MODE));
                }
                #endregion

                #endregion

                #region 非必填项

                #region 产品有效期
                if (!string.IsNullOrEmpty(VALIDITY))
                {
                    bool flag = true;
                    try
                    {
                        Convert.ToDateTime(VALIDITY).ToString("yyyy-MM-dd");
                    }
                    catch
                    {
                        flag = false;
                    }
                    if (!flag)
                        yield return new ValidationResult("VALIDITY格式错误。");
                }
                #endregion
              
                #region 备案状态
                if (!String.IsNullOrWhiteSpace(PRODUCT_STATUS))
                {
                    var paraEciqInspOrgCode = ParaEciqManager.Instance.ECIQ_ParaProducttStatus.Find(o => o.Code == PRODUCT_STATUS);
                    if (paraEciqInspOrgCode == null)
                        yield return new ValidationResult(String.Format("PRODUCT_STATUS【{0}】不存在。", PRODUCT_STATUS));
                }
                #endregion

                #region 是否法检
                if (!String.IsNullOrWhiteSpace(IS_LAW_REVIEW))
                {
                    var paraEciqInspOrgCode = ParaEciqManager.Instance.ECIQ_ParaIsLawReview.Find(o => o.Code == IS_LAW_REVIEW);
                    if (paraEciqInspOrgCode == null)
                        yield return new ValidationResult(String.Format("IS_LAW_REVIEW【{0}】不存在。", IS_LAW_REVIEW));
                }
                #endregion

                #region 有效期开始时间
                if (!string.IsNullOrEmpty(VALIDITY_BEGIN))
                {
                    bool flag = true;
                    try
                    {
                        Convert.ToDateTime(VALIDITY_BEGIN).ToString("yyyy-MM-dd");
                    }
                    catch
                    {
                        flag = false;
                    }
                    if (!flag)
                        yield return new ValidationResult("VALIDITY_BEGIN格式错误。");
                }
                #endregion

                #region 有效期结束时间
                if (!string.IsNullOrEmpty(VALIDITY_END))
                {
                    bool flag = true;
                    try
                    {
                        Convert.ToDateTime(VALIDITY_END).ToString("yyyy-MM-dd");
                    }
                    catch
                    {
                        flag = false;
                    }
                    if (!flag)
                        yield return new ValidationResult("VALIDITY_END格式错误。");
                }
                #endregion

                #endregion

                #region 附件信息
                #region 验证附件信息

                if (EciqProductBillLists == null || EciqProductBillLists.Count < 0)
                    yield return new ValidationResult("货物信息必填");
                else
                {
                    int i = 1;
                    foreach (EciqProductBillList l in EciqProductBillLists)
                    {
                        l.ATTACHED_SEQ_NO = (i++).ToString();
                        StringBuilder validResult = new StringBuilder();

                        #region 必填信息

                        #region 附件业务类型
                        if (String.IsNullOrWhiteSpace(l.BIZ_TYPE_CODE))
                            yield return new ValidationResult("BIZ_TYPE_CODE必填。");
                        else
                        {
                            var paraEciqCode = ParaEciqManager.Instance.ECIQ_ParaBizTypeCode.Find(o => o.Code == l.BIZ_TYPE_CODE);
                            if (paraEciqCode == null)
                                yield return new ValidationResult(String.Format("BIZ_TYPE_CODE【{0}】不存在。", l.BIZ_TYPE_CODE));
                            var paraEciqName = ParaEciqManager.Instance.BIZ_TYPE_NAME(l.BIZ_TYPE_CODE);
                            if (!paraEciqName.Equals(l.BIZ_TYPE_NAME))
                                yield return new ValidationResult(String.Format("BIZ_TYPE_NAME【{0}】与BIZ_TYPE_CODE【{1}】不匹配。", l.BIZ_TYPE_NAME, l.BIZ_TYPE_CODE));
                        }
                        #endregion

                        #region 具体单据类型
                        if (String.IsNullOrWhiteSpace(l.CERT_TYPE_CODE))
                            yield return new ValidationResult("CERT_TYPE_CODE必填。");
                        else
                        {
                            var paraEciqCountryCode = ParaEciqManager.Instance.ECIQ_ParaProductSheetType.Find(o => o.Code == l.CERT_TYPE_CODE);
                            if (paraEciqCountryCode == null)
                                yield return new ValidationResult(String.Format("CERT_TYPE_CODE【{0}】不存在。", l.CERT_TYPE_CODE));
                            var paraEciqName = ParaEciqManager.Instance.EciqSheetTypeName(l.CERT_TYPE_CODE);
                            if (!paraEciqName.Equals(l.CERT_TYPE_NAME))
                                yield return new ValidationResult(String.Format("CERT_TYPE_NAME【{0}】与CERT_TYPE_CODE【{1}】不匹配。", l.CERT_TYPE_NAME, l.CERT_TYPE_CODE));
                        }
                        #endregion

                        #region 文件名称
                        if (String.IsNullOrWhiteSpace(l.FILE_NAME))
                            validResult.Append("FILE_NAME必填,");
                        #endregion

                        #region 文件类型
                        if (String.IsNullOrWhiteSpace(l.FILE_TYPE))
                            validResult.Append("FILE_TYPE必填,");
                        #endregion

                        #region 证件流
                        if (String.IsNullOrWhiteSpace(l.FILE_CONTENT))
                            validResult.Append("FILE_CONTENT必填,");
                        #endregion

                        #endregion


                    }
                }

                #endregion
                #endregion

                #region 业务验证

                EciqProductBillService productService = new EciqProductBillService();

                if (!String.IsNullOrWhiteSpace(HS_CODE))
                {
                    //从清单表中，验证运单号是否多次绑定
                    EciqProductBillHead declformTemp = productService.GetDeclformByHscode(HS_CODE);
                    if (declformTemp != null)
                        yield return new ValidationResult(String.Format("包含的HS_CODE信息【{0}】已绑定给其他商品备案单【{1}】。", HS_CODE, declformTemp.HS_CODE));

                }

                #endregion

            }
            finally
            {

            }
        }
    }
}