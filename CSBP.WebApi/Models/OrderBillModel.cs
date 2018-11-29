using CBSP.Core.Domain.OrderBill;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CBSP.Web.Framework;

namespace CSBP.WebApi.Models
{
    public class OrderBillModel : OrderHead, IValidatableObject
    {
        /// <summary>
        /// 当前登录账户对应的组织机构代码
        /// </summary>
        public string COP_GB_CODE { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 公共必填项
            
            if (string.IsNullOrEmpty(ORDER_ID))
                yield return new ValidationResult("ORDER_ID必填。");

            //电商平台
            string ebPlatName = "";
            ValidationResult ebPlatIdResult = CheckPara.CheckEbPlatId(EB_PLAT_ID,ref ebPlatName);
            if (ebPlatIdResult != null)
                yield return ebPlatIdResult;

            //电商企业
            string ebName = "";
            ValidationResult ebCodeResult = CheckPara.CheckCompanyCode(EB_CODE, ParaCacheManager.Instance.EB_COP_TYPE, IE_FLAG, "EB_CODE", ref ebName);
            if (ebCodeResult != null)
                yield return ebCodeResult;
            else
                EB_NAME = ebName;

            //贸易方式
            ValidationResult tradeModeResult = CheckPara.CheckTradeMode(TRADE_MODE);
            if (tradeModeResult != null)
                yield return tradeModeResult;

            //业务类型
            ValidationResult businessTypeResult = CheckPara.CheckBusinessType(BUSINESS_TYPE);
            if (businessTypeResult != null)
                yield return businessTypeResult;

            //总件数
            if (PACK_NUM <= 0)
                yield return new ValidationResult("PACK_NUM必须大于0。");

            //币制
            ValidationResult currCodeResult = CheckPara.CheckCurrCode(CURR_CODE,"CURR_CODE");
            if (currCodeResult != null)
                yield return currCodeResult;

            //贸易国别
            ValidationResult countryResult = CheckPara.CheckCountry(BUYER_COUNTRY, "BUYER_COUNTRY");
            if (countryResult != null)
                yield return countryResult;


            #endregion

            if (IE_FLAG.Equals("I"))
            {
                #region 进口必填项

                if (string.IsNullOrEmpty(BUYER_NAME))
                    yield return new ValidationResult("BUYER_NAME必填");

                //证件类型为身份证
                BUYER_CERT_TYPE = "1";
                //币制为142
                CURR_CODE = "142";

                if (string.IsNullOrEmpty(BUYER_CERT_ID))
                    yield return new ValidationResult("BUYER_CERT_ID必填");

                if (string.IsNullOrEmpty(BUYER_TEL))
                    yield return new ValidationResult("BUYER_TEL必填");

                if (string.IsNullOrEmpty(DELIVERY_ADDR))
                    yield return new ValidationResult("DELIVERY_ADDR必填");

                #endregion

            }
            else if (IE_FLAG.Equals("E"))
            {


            }
            else
                yield return new ValidationResult("IE_FLAG值无效。");

            #region 验证选填项的代码有效性

            if (!string.IsNullOrEmpty(BUYER_TEL) && !Regex.IsMatch(BUYER_TEL, @"\d{11}"))
                yield return new ValidationResult("BUYER_TEL必须11位数字。");

            //判断参数是否符合要求
            if (!string.IsNullOrEmpty(BUYER_CERT_TYPE))
            {
                ValidationResult certTypeResult = CheckPara.CheckCertType(BUYER_CERT_TYPE, "BUYER_CERT_TYPE");
                if (certTypeResult != null)
                    yield return certTypeResult;
            }

            #endregion

            #region 商品项验证
            if (OrderLists != null && OrderLists.Count > 0)
            {
                foreach (OrderList o in OrderLists)
                {
                    #region 根据商品企业内部编号，反填信息
                    var copComplex = ParaCacheManager.Instance.ParaCopComplex(COP_GB_CODE).Find(e => e.COP_INTERNAL_NO == o.COP_INTERNAL_NO);
                    if (copComplex != null)
                    {
                        o.CODE_TS = copComplex.G_CODE;
                        o.CURR_CODE = copComplex.COP_G_CURR;
                        o.G_MODEL = copComplex.COP_G_MODEL;
                        o.G_NAME = copComplex.COP_G_NAME;
                        o.G_UNIT = copComplex.COP_G_UNIT;
                    }

                    #endregion
                    //商品代码
                    string  gDesc = "";
                    ValidationResult codeTSResult = CheckPara.CheckComplexCode(o.CODE_TS, "CODE_TS", ref gDesc);
                    if (codeTSResult != null)
                        yield return codeTSResult;
                    else
                        o.G_DESC = gDesc;

                    //商品名称
                    if (string.IsNullOrEmpty(o.G_NAME))
                        yield return new ValidationResult("G_NAME必填。");
                    //数量
                    if (o.G_NUM <= 0)
                        yield return new ValidationResult("G_NUM必须大于0");

                    //计量单位
                    ValidationResult unitResult = CheckPara.CheckUnitCode(o.G_UNIT, "G_UNIT");
                    if (unitResult != null)
                        yield return unitResult;
                    //价格
                    if (o.PRICE <= 0)
                        yield return new ValidationResult("PRICE必须大于0.");

                    o.TOTAL_PRICE = o.G_NUM * o.PRICE;
                    //总货款
                    TOTAL_PAYMENT += o.TOTAL_PRICE;

                    //币制
                    if (IE_FLAG.Equals("I"))
                        o.CURR_CODE = "142";
                    else
                    {
                        ValidationResult currCodeResult2 = CheckPara.CheckCurrCode(o.CURR_CODE, "CURR_CODE");
                        if (currCodeResult2 != null)
                            yield return currCodeResult2;
                    }

                }//foreach
            }
            else
                yield return new ValidationResult("无商品列表。");

            #endregion

            #region 业务验证

            //单据是否存在
            if (!string.IsNullOrEmpty(ORDER_ID) && !string.IsNullOrEmpty(EB_PLAT_ID))
            {
                var bill = new CBSP.Services.OrderBill.OrderBillService().GetOrderBillByKeys(ORDER_ID, EB_PLAT_ID);
                if (bill != null)
                {
                    yield return new ValidationResult(string.Format("订单【{0}】、【{1}】已存在。", ORDER_ID, EB_PLAT_ID));
                }
            }

            if (IE_FLAG.Equals("I"))
            {
                if (OrderLists != null && OrderLists.Count > 0)
                {
                    foreach (var item in OrderLists)
                    {
                        if (item.G_NUM > 1 && item.PRICE.CompareTo(1000) > 0)
                        {
                            yield return new ValidationResult("单种商品数量多于1件时，PRICE不能超过1000");
                        }
                    }
                }
            }

            #endregion

        }
    }
}