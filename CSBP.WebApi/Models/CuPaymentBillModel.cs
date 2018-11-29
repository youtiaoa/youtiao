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
using System.Web;

namespace CSBP.WebApi.Models
{
    public class CuPaymentBillModel:CuPaymentHead,IValidatableObject
    {
        public string COP_GB_CODE { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            try
            {
                #region 必填项
                //支付交易编号
                if (string.IsNullOrEmpty(PAY_TRANSACTION_ID))
                    yield return new ValidationResult("DEAL_ID必填。");

                #region 支付企业
                if (string.IsNullOrEmpty(PAY_CODE))
                    yield return new ValidationResult("DEAL_PLAT_ID必填。");
                {
                    string payName = "";
                    ValidationResult payResult = CheckPara.CheckCuCompanyCode(PAY_CODE, COP_TYPE.COP_TYPE_P, IE_FLAG.IE_FLAG_I, "DEAL_PLAT_ID", ref payName);
                    if (payResult != null)
                        yield return payResult;
                    else
                        PAY_NAME = payName;
                }
                #endregion

                //订单号
                if (string.IsNullOrEmpty(ORDER_NO))
                    yield return new ValidationResult("ORDER_ID必填。");

                #region 电商平台
                if (string.IsNullOrEmpty(EBP_CODE))
                    yield return new ValidationResult("EB_PLAT_ID必填。");
                {
                    string ebpName = "";
                    ValidationResult ebpResult = CheckPara.CheckCuCompanyCode(EBP_CODE, COP_TYPE.COP_TYPE_EP, IE_FLAG.IE_FLAG_I, "EB_PLAT_ID", ref ebpName);
                    if (ebpResult != null)
                        yield return ebpResult;
                    else
                        EBP_NAME = ebpName;
                }
                #endregion

                //支付人证件类型
                if (!PAYER_ID_TYPE.Equals(CU_CERT_TYPE.CU_CERT_TYPE_1))
                    yield return new ValidationResult("PAYER_CERT_TYPE应为【1】。");
                //支付人证件号
                if (string.IsNullOrEmpty(PAYER_ID_NUMBER))
                    yield return new ValidationResult("PAYER_CERT_ID必填。");
                //支付人姓名
                if (string.IsNullOrEmpty(PAYER_NAME))
                    yield return new ValidationResult("PAYER_NAME必填。");
                //支付金额
                if (AMOUNT_PAID <= 0)
                    yield return new ValidationResult("PAYMENT_AMOUNT应大于0。");
                //支付币制
                if (!CURRENCY.Equals(CU_CURRENCY.CU_CURRENCY_142))
                    yield return new ValidationResult("CURRENCY应为【142】。");
                //支付时间

                #endregion

                #region 非必填项

                #endregion

                #region 业务验证
                CuPaymentBillService paymentService = new CuPaymentBillService();
                CuOrderBillService orderBillService = new CuOrderBillService();

                if (paymentService.Exists(PAY_TRANSACTION_ID, PAY_CODE))
                    yield return new ValidationResult("支付单号为" + PAY_TRANSACTION_ID + "且支付企业代码为" + PAY_CODE + "的支付单已存在。");

                if (!String.IsNullOrWhiteSpace(ORDER_NO) && !String.IsNullOrWhiteSpace(EBP_CODE))
                {
                    //从支付单表中，验证订单是否多次绑定
                    IList<CuPaymentHead> paymentTemp = paymentService.GetPaymentByOrder(ORDER_NO, EBP_CODE);
                    CuPaymentHead selectedPayment = paymentTemp.FirstOrDefault(o => o.GUID != GUID);
                    if (selectedPayment != null)
                        yield return new ValidationResult(String.Format("其订单信息已绑定给其他支付单【{0}】。", selectedPayment.PAY_TRANSACTION_ID));


                    //从订单表中查找已绑定给本支付单的订单，是否和该支付单中的订单信息一致
                    IList<CuOrderHead> orderTemp = orderBillService.GetOrderBillByPayment(PAY_TRANSACTION_ID, PAY_CODE);
                    CuOrderHead selectedOrder = orderTemp.FirstOrDefault(o => o.ORDER_NO != ORDER_NO || o.EBP_CODE != EBP_CODE);
                    if (selectedOrder != null)
                        yield return new ValidationResult(String.Format("支付单中的订单号【{0}】【{1}】，与含有相同支付信息的订单中的订单号【{2}】【{3}】不是同一个，", ORDER_NO, EBP_CODE, selectedOrder.ORDER_NO, selectedOrder.EBP_CODE));
                }

                #endregion

            }
            finally
            {

            }
        }
    }
}