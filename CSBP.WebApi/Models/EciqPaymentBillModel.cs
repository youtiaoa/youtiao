using CBSP.Core;
using CBSP.Core.Domain.PaymentBill;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CSBP.WebApi.Models
{
    public class EciqPaymentBillModel : EciqPaymentHead, IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //交易号
            if (string.IsNullOrEmpty(PAY_TRANSACTION_NO))
                yield return new ValidationResult("PAY_TRANSACTION_NO必填。");
            //订单号
            if (string.IsNullOrEmpty(ORDER_NO))
                yield return new ValidationResult("ORDER_NO必填。");

            //交易流水号
            if (string.IsNullOrEmpty(ORDER_NO))
                yield return new ValidationResult("ORDER_NO必填。");

            #region 支付企业
            if (string.IsNullOrEmpty(PAY_ENTERPRISE_CODE))
                yield return new ValidationResult("PAY_ENTERPRISE_CODE必填。");
            else
            {
                string ebpName = "";
                ValidationResult ebpResult = CheckPara.CheckEciqCompanyCode(PAY_ENTERPRISE_CODE, COP_TYPE.COP_TYPE_P, CBSP.Core.IE_FLAG.IE_FLAG_I, "PAY_ENTERPRISE_CODE", ref ebpName);
                if (ebpResult != null)
                    yield return ebpResult;
                else
                    PAY_ENTERPRISE_NAME = ebpName;
            }
            #endregion

            #region 支付证件类型
            if (String.IsNullOrWhiteSpace(PAYER_DOCUMENT_TYPE))
                yield return new ValidationResult("PAYER_DOCUMENT_TYPE必填。");
            else
            {
                if (!PAYER_DOCUMENT_TYPE.Equals("01"))
                    yield return new ValidationResult("PAYER_DOCUMENT_TYPE限定为01。");
            }
            #endregion

            //支付证件号
            if (string.IsNullOrEmpty(PAYER_DOCUMENT_NUMBER))
                yield return new ValidationResult("PAYER_DOCUMENT_NUMBER必填。");

            //支付人姓名
            if (string.IsNullOrEmpty(PAYER_NAME))
                yield return new ValidationResult("PAYER_NAME必填。");

            //支付人电话
            if (string.IsNullOrEmpty(TELEPHONE))
                yield return new ValidationResult("TELEPHONE必填。");

            //支付金额
            if (string.IsNullOrEmpty(AMOUNT_PAID.ToString()))
                yield return new ValidationResult("AMOUNT_PAID必填。");
            else
            {
                bool flag = true;
                try
                {
                    Convert.ToDecimal(AMOUNT_PAID);
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("AMOUNT_PAID格式错误。");
            }

            #region 支付币制
            if (String.IsNullOrWhiteSpace(CURRENCY))
                yield return new ValidationResult("CURRENCY必填。");
            else
            {
                if (!CURRENCY.Equals("156"))
                    yield return new ValidationResult("CURRENCY限定为156。");
            }
            #endregion

            #region 支付时间
            if (string.IsNullOrEmpty(PAY_TIME.ToString()))
                yield return new ValidationResult("PAY_TIME必填。");
            else
            {
                bool flag = true;
                try
                {
                    Convert.ToDateTime(PAY_TIME).ToString("yyyy-MM-dd");
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("PAY_TIME格式错误。");
            }
            #endregion

            #region 验证选填项的代码有效性


            //判断参数是否符合要求
            #region 创建时间
            if (!string.IsNullOrEmpty(CREATE_TIME.ToString()))
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

            #region 业务验证

            //验证对应的订单未有支付单对应
            if (!string.IsNullOrEmpty(ORDER_NO))
            {
                var bill = new CBSP.Services.PaymentBill.EciqPaymentBillService().GetPaymentByOrder(ORDER_NO);
                if (bill != null && bill.Count>0)
                {
                    yield return new ValidationResult(string.Format("支付单【{0}】中的订单【{1}】已绑定给其他支付单。", ORDER_NO, ORDER_NO));
                }
            }
            #endregion
        }
    }
}