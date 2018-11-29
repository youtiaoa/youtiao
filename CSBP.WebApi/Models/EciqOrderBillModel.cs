using CBSP.Core;
using CBSP.Core.Domain.EciqOrderBill;
using CBSP.Core.Domain.OrderBill;
using CBSP.Core.Domain.PaymentBill;
using CBSP.Services.EciqOrderBill;
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
    public class EciqOrderBillModel : EciqOrderBillHead,IValidatableObject
    {
        /// <summary>
        /// 当前登录账户对应的组织机构代码
        /// </summary>
        public string COP_GB_CODE { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 必填项判断

            #region 进出口标志
            if (String.IsNullOrWhiteSpace(I_E_FLAG))
                yield return new ValidationResult("I_E_FLAG必填。");
            else
            {
                var paraDistrict = ParaEciqManager.Instance.ECIQ_ParaInOutFlag.Find(o => o.Code == I_E_FLAG);
                if (paraDistrict == null)
                    yield return new ValidationResult(String.Format("I_E_FLAG【{0}】不存在,", I_E_FLAG));
            }
            #endregion

            //订单编号
            if (string.IsNullOrEmpty(ORDER_NO))
                yield return new ValidationResult("ORDER_NO必填。");

            #region 电商平台
            string ebpName = "";
            ValidationResult ebpResult = CheckPara.CheckEciqCompanyCode(COMPANY_CODE, COP_TYPE.COP_TYPE_EP, IE_FLAG.IE_FLAG_I, "COMPANY_CODE", ref ebpName);
            if (ebpResult != null)
                yield return ebpResult;
            else
                COMPANY_NAME = ebpName;
            #endregion

            //币制
            if (!CURR_CODE.Equals(CU_CURRENCY.CU_CURRENCY_156))
                yield return new ValidationResult("CURR_CODE应为【156】。");

            //收货人电话
            if (string.IsNullOrEmpty(CONSIGNEE_TEL))
                yield return new ValidationResult("CONSIGNEE_TELEPHONE必填。");

            //收货人姓名
            if (string.IsNullOrEmpty(CONSIGNEE))
                yield return new ValidationResult("CONSIGNEE必填。");

            //收货地址
            if (string.IsNullOrEmpty(CONSIGNEE_ADDRESS))
                yield return new ValidationResult("CONSIGNEE_ADDRESS必填。");

            # region 物流企业
            if (!String.IsNullOrWhiteSpace(LOGIS_COMPANY_CODE))
            {
                string payName = "";
                ValidationResult payResult = CheckPara.CheckEciqCompanyCode(LOGIS_COMPANY_CODE, COP_TYPE.COP_TYPE_L, IE_FLAG.IE_FLAG_I, "LOGIS_COMPANY_CODE", ref payName);
                if (payResult != null)
                    yield return payResult;
                else
                    LOGIS_COMPANY_NAME = payName;
            }
            #endregion

            //优免金额
            if (DISCOUNT < 0)
                yield return new ValidationResult("DISCOUNT应大于等于0。");

            //税款金额
            if (TAX_TOTAL < 0)
                yield return new ValidationResult("TAX_TOTAL应大于等于0。");

            //实际支付金额
            if (ACTURAL_PAID <= 0)
                yield return new ValidationResult("ACTURAL_PAID应大于0。");

            //订购人注册号
            if (string.IsNullOrEmpty(BUYER_REG_NO))
                yield return new ValidationResult("BUYER_REG_NO必填。");

            //订购人姓名
            if (string.IsNullOrEmpty(BUYER_NAME))
                yield return new ValidationResult("BUYER_NAME必填。");

            //收货人姓名
            if (string.IsNullOrEmpty(CONSIGNEE))
                yield return new ValidationResult("CONSIGNEE必填。");

            //订购人证件类型
            if (!BUYER_ID_TYPE.Equals(CU_CERT_TYPE.CU_CERT_TYPE_1))
                yield return new ValidationResult("BUYER_ID_TYPE应为【1】。");

            //订购人证件号码
            if (string.IsNullOrEmpty(BUYER_ID_NUMBER))
                yield return new ValidationResult("BUYER_ID_NUMBER必填。");

            //商品货款
            if (PRICE_TOTAL_VAL <= 0)
                yield return new ValidationResult("PRICE_TOTAL_VAL应大于0。");

            //运费
            if (FREIGHT < 0)
                yield return new ValidationResult("FREIGHT应大于等于0。");

            #endregion

            #region 非必填判断

            # region 发货人国家
            if (!String.IsNullOrWhiteSpace(SALER_COUNTRY))
            {
                var paraDistrict = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == SALER_COUNTRY);
                if (paraDistrict == null)
                    yield return new ValidationResult(String.Format("SALER_COUNTRY【{0}】不存在,", SALER_COUNTRY));
            }
            #endregion

            # region 创建时间
            if (!String.IsNullOrWhiteSpace(CREATE_TIME))
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

            #region 验证商品信息
            int i = 1;
            foreach (EciqOrderBillList l in EciqOrderBillLists)
            {
                #region 商品必填项

                l.SEQ_NO = (i++).ToString();
                StringBuilder validResult = new StringBuilder();

                //商品名称
                if (string.IsNullOrEmpty(l.GOODS_NAME))
                    yield return new ValidationResult("GOODS_NAME必填。");

                //规格型号
                if (string.IsNullOrEmpty(l.GOODS_SPECIFICATION))
                    yield return new ValidationResult("GOODS_SPECIFICATION必填。");

                //产销国
                var paraDistrict = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == l.PRODUCTION_MARKETING_COUNTRY);
                if (paraDistrict == null)
                    yield return new ValidationResult(String.Format("PRODUCTION_MARKETING_COUNTRY【{0}】不存在,", l.PRODUCTION_MARKETING_COUNTRY));

                //单价
                if (l.DECLARE_PRICE <= 0)
                    yield return new ValidationResult("DECLARE_PRICE应大于0。");

                //数量
                if (l.DECLARE_COUNT <= 0)
                    yield return new ValidationResult("DECLARE_COUNT应大于0。");

                //计量单位
                var paraUnit = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.DECLARE_MEASURE_UNIT);
                if (paraUnit == null)
                    validResult.Append(string.Format("DECLARE_MEASURE_UNIT【{0}】不存在,", l.DECLARE_MEASURE_UNIT));

                //总局备案号
                if (string.IsNullOrEmpty(l.PRODUCT_RECORD_NO))
                    yield return new ValidationResult("PRODUCT_RECORD_NO必填。");

                //HS_CODE
                var paraComplex = ParaCacheManager.Instance.ParaCuComplex.Find(o => o.HS_G_CODE == l.HS_CODE);
                if (paraComplex == null)
                    validResult.Append(string.Format("HS_CODE【{0}】不存在,", l.HS_CODE));

                //商品货号
                if (string.IsNullOrEmpty(l.SKU))
                    yield return new ValidationResult("SKU必填。");

                //商品条码
                if (string.IsNullOrEmpty(l.COMM_BARCODE))
                    yield return new ValidationResult("COMM_BARCODE必填。");
             
                if (validResult.Length > 0)
                    yield return new ValidationResult(string.Format("第{0}项商品{1}", l.SEQ_NO.ToString(), validResult.ToString()));

                #endregion

            }

            #endregion

            #region 业务验证
            EciqOrderBillService eciqOrderBillService = new EciqOrderBillService();

            if (eciqOrderBillService.Exists(ORDER_NO, COMPANY_CODE))
            {
                yield return new ValidationResult(String.Format("国检版订单号为【{0}】且电商平台代码为【{1}】的订单已存在。", ORDER_NO, COMPANY_CODE));
            }

            #endregion
        }
    }
}