using CBSP.Core.Domain.PaymentBill;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSBP.WebApi.Models
{
    public class PaymentBillModel : PaymentHead, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {

            if (string.IsNullOrEmpty(DEAL_ID))
                yield return new ValidationResult("DEAL_ID必填。");

            //交易平台
            ValidationResult dealPlatIdResult = CheckPara.CheckDealPlatId(DEAL_PLAT_ID);
            if (dealPlatIdResult != null)
                yield return dealPlatIdResult;

            //支付日期
            if (PAYMENT_DATE == null)
                yield return new ValidationResult("PAYMENT_DATE必填。");
            //支付币制
            PAYMENT_CURR = "142";
            //支付金额
            if (PAYMENT_AMOUNT <= 0)
                yield return new ValidationResult("PAYMENT_AMOUNT必须大于0。");
            //支付人姓名
            if (string.IsNullOrEmpty(PAYER_NAME))
                yield return new ValidationResult("PAYER_NAME必填。");
            //支付证件类型
            PAYER_CERT_TYPE = "1";
            //支付证件号
            if (string.IsNullOrEmpty(PAYER_CERT_ID))
                yield return new ValidationResult("PAYER_CERT_ID必填。");
            //订单号
            if (string.IsNullOrEmpty(ORDER_ID))
                yield return new ValidationResult("ORDER_ID必填。");
            //电商交易平台
            string ebPlatName = "";
            ValidationResult ebPlatIdResult = CheckPara.CheckEbPlatId(EB_PLAT_ID,ref ebPlatName);
            if (ebPlatIdResult != null)
                yield return ebPlatIdResult;

            #region 验证选填项的代码有效性


            //判断参数是否符合要求
            if (!string.IsNullOrEmpty(PAYEE_CERT_TYPE))
            {
                ValidationResult certTypeResult = CheckPara.CheckCertType(PAYEE_CERT_TYPE, "PAYER_CERT_TYPE");
                if (certTypeResult != null)
                    yield return certTypeResult;
            }
            else
            {
                PAYEE_CERT_TYPE = "1";
            }

            #endregion

            #region 业务验证
            //单据是否存在
            if (!string.IsNullOrEmpty(DEAL_ID) && !string.IsNullOrEmpty(DEAL_PLAT_ID))
            {
                var bill = new CBSP.Services.PaymentBill.PaymentBillService().GetPaymentBillByKeys(DEAL_ID, DEAL_PLAT_ID);
                if (bill != null)
                {
                    yield return new ValidationResult(string.Format("支付单【{0}】、【{1}】已存在。", DEAL_ID, DEAL_PLAT_ID));
                }
            }

            //验证对应的订单未有支付单对应
            if (!string.IsNullOrEmpty(ORDER_ID) && !string.IsNullOrEmpty(EB_PLAT_ID))
            {
                var bill = new CBSP.Services.PaymentBill.PaymentBillService().GetPaymentByOrder(ORDER_ID, EB_PLAT_ID);
                if (bill != null && bill.Count>0)
                {
                    yield return new ValidationResult(string.Format("支付单【{0}】、【{1}】中的订单【{2}】、【{3}】已绑定给其他支付单。", DEAL_ID, DEAL_PLAT_ID,ORDER_ID, EB_PLAT_ID));
                }
            }
            #endregion
        }
    }
}