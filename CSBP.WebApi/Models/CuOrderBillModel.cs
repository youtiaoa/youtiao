using CBSP.Core;
using CBSP.Core.Domain.OrderBill;
using CBSP.Core.Domain.PaymentBill;
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
    public class CuOrderBillModel:CuOrderHead,IValidatableObject
    {/// <summary>
        /// 当前登录账户对应的组织机构代码
        /// </summary>
        public string COP_GB_CODE { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 必填项判断

            if (string.IsNullOrEmpty(ORDER_NO))
                yield return new ValidationResult("ORDER_ID必填。");

            //币制
            if ((ORDER_BILL_TYPE != CU_TRADE_MODE.CU_TRADE_MODE_1210) && (ORDER_BILL_TYPE != CU_TRADE_MODE.CU_TRADE_MODE_9610))
                yield return new ValidationResult("ORDER_BILL_TYPE应为【1210】或【9610】。");

            #region 电商平台
            string ebpName = "";
            ValidationResult ebpResult = CheckPara.CheckCuCompanyCode(EBP_CODE, COP_TYPE.COP_TYPE_EP, IE_FLAG.IE_FLAG_I, "EB_PLAT_ID", ref ebpName);
            if (ebpResult != null)
                yield return ebpResult;
            else
                EBP_NAME = ebpName;
            #endregion

            #region 电商企业
            string ebcName = "";
            ValidationResult ebcResult = CheckPara.CheckCuCompanyCode(EBC_CODE, COP_TYPE.COP_TYPE_E, IE_FLAG.IE_FLAG_I, "EB_CODE", ref ebcName);
            if (ebcResult != null)
                yield return ebcResult;
            else
                EBC_NAME = ebcName;
            #endregion

            //商品价格
            if (GOODS_VALUE <= 0)
                yield return new ValidationResult("GOODS_VALUE应大于0。");
            //运杂费
            //非现金抵扣
            //代扣税款
            //实际支付总金额
            if (ACTURAL_PAID < 0)
                yield return new ValidationResult("计算得出的ACTURAL_PAID应大于0。");
            //币制
            if (!CURRENCY.Equals(CU_CURRENCY.CU_CURRENCY_142))
                yield return new ValidationResult("CURR_CODE应为【142】。");
            //订购人注册号
            if (string.IsNullOrEmpty(BUYER_REGNO))
                yield return new ValidationResult("BUYER_REGNO必填。");
            //订购人姓名
            if (string.IsNullOrEmpty(BUYER_NAME))
                yield return new ValidationResult("BUYER_NAME必填。");
            //订购人电话
            if (string.IsNullOrEmpty(BUYER_TELEPHONE))
                yield return new ValidationResult("BUYER_TELEPHONE必填。");
            //订购人证件类型
            if (!BUYER_ID_TYPE.Equals(CU_CERT_TYPE.CU_CERT_TYPE_1))
                yield return new ValidationResult("BUYER_ID_TYPE应为【1】。");
            //订购人证件号码
            if (string.IsNullOrEmpty(BUYER_ID_NUMBER))
                yield return new ValidationResult("BUYER_ID_NUMBER必填。");
            //收货人姓名
            if (string.IsNullOrEmpty(CONSIGNEE))
                yield return new ValidationResult("CONSIGNEE必填。");
            //收货人电话
            if (string.IsNullOrEmpty(CONSIGNEE_TELEPHONE))
                yield return new ValidationResult("CONSIGNEE_TELEPHONE必填。");
            //收货地址
            if (string.IsNullOrEmpty(CONSIGNEE_ADDRESS))
                yield return new ValidationResult("CONSIGNEE_ADDRESS必填。");
            #endregion

            #region 非必填判断
            //收货地址行政区划
            if (!String.IsNullOrWhiteSpace(CONSIGNEE_DITRICT))
            {
                var paraDistrict = ParaCacheManager.Instance.ParaDistrict.Find(o => o.CODE == CONSIGNEE_DITRICT);
                if (paraDistrict == null)
                    yield return new ValidationResult(String.Format("CONSIGNEE_DITRICT【{0}】不存在,", CONSIGNEE_DITRICT));
            }

            //支付企业
            if (!String.IsNullOrWhiteSpace(PAY_CODE))
            {
                string payName = "";
                ValidationResult payResult = CheckPara.CheckCuCompanyCode(PAY_CODE, COP_TYPE.COP_TYPE_P, IE_FLAG.IE_FLAG_I, "PAY_CODE", ref payName);
                if (payResult != null)
                    yield return payResult;
                else
                    PAY_NAME = payName;
            }
            #endregion

            #region 验证商品信息
            int i = 1;
            foreach (CuOrderList l in CuOrderLists)
            {
                l.G_NUM = i++;
                StringBuilder validResult = new StringBuilder();

                //企业商品名称
                if (string.IsNullOrEmpty(l.ITEM_NAME))
                    yield return new ValidationResult("GOODS_NAME必填。");
                //原产国
                var paraCountry = ParaCacheManager.Instance.ParaCountry.Find(o => o.CODE == l.COUNTRY);
                if (paraCountry == null)
                    validResult.Append(string.Format("PRODUCTION_MARKETING_COUNTRY【{0}】不存在,", l.UNIT));
                //计量单位
                var paraUnit = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.UNIT);
                if (paraUnit == null)
                    validResult.Append(string.Format("DECLARE_MEASURE_UNIT【{0}】不存在,", l.UNIT));
                //单价
                if (l.PRICE <= 0)
                    yield return new ValidationResult("DECLARE_PRICE应大于0。");
                //数量
                if (l.QTY <= 0)
                    yield return new ValidationResult("DECLARE_COUNT应大于0。");
                //总价
                //币制
                if (!l.CURRENCY.Equals(CU_CURRENCY.CU_CURRENCY_142))
                    validResult.Append("CURR_CODE应为【142】。");

                if (validResult.Length > 0)
                    yield return new ValidationResult(string.Format("第{0}项商品{1}", l.G_NUM.ToString(), validResult.ToString()));
            }

            #endregion

            #region 业务验证
            CuOrderBillService orderBillService = new CuOrderBillService();
            CuPaymentBillService paymentBillService = new CuPaymentBillService();

            if (orderBillService.Exists(ORDER_NO, EBP_CODE))
            {
                yield return new ValidationResult(String.Format("统一版订单号为【{0}】且电商平台代码为【{1}】的订单已存在。", ORDER_NO, EBP_CODE));
            }


            if (!String.IsNullOrWhiteSpace(PAY_TRANSACTION_ID) && !String.IsNullOrWhiteSpace(PAY_CODE))
            {
                //验证支付单信息是否多次绑定
                IList<CuOrderHead> orderTemp = orderBillService.GetOrderBillByPayment(PAY_TRANSACTION_ID, PAY_CODE);
                CuOrderHead selectedOrder = orderTemp.FirstOrDefault(o => o.GUID != GUID);
                if (selectedOrder != null)
                    yield return new ValidationResult(String.Format("统一版该支付单信息【{0}】已绑定给其他订单【{1}】。", PAY_TRANSACTION_ID, selectedOrder.ORDER_NO));

                //验证支付单中的订单信息，与订单中的支付单信息  两者是否一致
                IList<CuPaymentHead> paymentTemp = paymentBillService.GetPaymentByOrder(ORDER_NO, EBP_CODE);
                CuPaymentHead selectedPayment = paymentTemp.FirstOrDefault(o => o.PAY_TRANSACTION_ID != PAY_TRANSACTION_ID || o.PAY_CODE != PAY_CODE);
                if (selectedPayment != null)
                    yield return new ValidationResult(String.Format("统一版订单中的支付单号【{0}】【{1}】，与含有相同订单信息的支付单中的支付单号【{2}】【{3}】不是同一个，", PAY_TRANSACTION_ID, PAY_CODE, selectedPayment.PAY_TRANSACTION_ID, selectedPayment.PAY_CODE));
                
            }
            #endregion
        }
    }
}