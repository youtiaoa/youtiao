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
    public class EciqEnterDeclformModel : EciqEnterDeclformHead,IValidatableObject
    {
        /// <summary>
        /// 当前登录账户对应的组织机构代码
        /// </summary>
        public string COP_GB_CODE { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            #region 必填项

            #region 提运单
            if (string.IsNullOrEmpty(CARRIER_NOTE_NO))
                yield return new ValidationResult("CARRIER_NOTE_NO必填。");
            #endregion 

            #region 报检日期
            if (string.IsNullOrEmpty(DECL_DATE))
                yield return new ValidationResult("DECL_DATE必填。");
            else
            {
                bool flag = true ;
               try
                {
                    Convert.ToDateTime(DECL_DATE).ToString("yyyy-MM-dd");
                }
                catch
                {
                    flag = false;
                }
                if(!flag)
                yield return new ValidationResult("DECL_DATE格式错误。");
            }
            #endregion

            #region 电商平台
            if (string.IsNullOrEmpty(ELECTRIC_BUSINESS_NO))
                yield return new ValidationResult("ELECTRIC_BUSINESS_NO必填。");
            else
            {
                string ebpName = "";
                ValidationResult ebpResult = CheckPara.CheckEciqCompanyCode(ELECTRIC_BUSINESS_NO, COP_TYPE.COP_TYPE_EP, CBSP.Core.IE_FLAG.IE_FLAG_I, "EB_PLAT_ID", ref ebpName);
                if (ebpResult != null)
                    yield return ebpResult;
                else
                    ECP_NAME = ebpName;
            }
            #endregion

            #region 电商企业
            if (string.IsNullOrEmpty(CBE_CODE))
                yield return new ValidationResult("CBE_CODE必填。");
            else
            {
                string ebcName = "";
                ValidationResult ebcResult = CheckPara.CheckEciqCompanyCode(CBE_CODE, COP_TYPE.COP_TYPE_E, CBSP.Core.IE_FLAG.IE_FLAG_I, "EB_CODE", ref ebcName);
                if (ebcResult != null)
                    yield return ebcResult;
                else
                    CBE_NAME = ebcName;
            }
            #endregion

            #region 贸易国别
            if (String.IsNullOrWhiteSpace(TRADE_COUNTRY_CODE))
                yield return new ValidationResult("TRADE_COUNTRY_CODE必填。");
            else
            {
                var paraEciqCountryCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == TRADE_COUNTRY_CODE);
                if (paraEciqCountryCode == null)
                    yield return new ValidationResult(String.Format("TRADE_COUNTRY_CODE【{0}】不存在。", TRADE_COUNTRY_CODE));
            }
            #endregion

            #region 进出口标志
            if (String.IsNullOrWhiteSpace(IE_FLAG))
                yield return new ValidationResult("IE_FLAG必填。");
            else
            {
                var paraEciqInOutFlagCode = ParaEciqManager.Instance.ParaEciqInOutFlag.Find(o => o.CODE == IE_FLAG);
                if (paraEciqInOutFlagCode == null)
                    yield return new ValidationResult(String.Format("IE_FLAG【{0}】不存在。", IE_FLAG));
            }
            #endregion

            #region 启运国家
            if (String.IsNullOrWhiteSpace(DESP_COUNTRY_CODE))
                yield return new ValidationResult("DESP_COUNTRY_CODE必填。");
            else
            {
                var paraEciqCountryCode = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == DESP_COUNTRY_CODE);
                if (paraEciqCountryCode == null)
                    yield return new ValidationResult(String.Format("DESP_COUNTRY_CODE【{0}】不存在。", DESP_COUNTRY_CODE));
            }
            #endregion

            #region 启运口岸
            if (String.IsNullOrWhiteSpace(DESP_PORT_CODE))
                yield return new ValidationResult("DESP_PORT_CODE必填。");
            else
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqPort.Find(o => o.CODE == DESP_PORT_CODE);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("DESP_PORT_CODE【{0}】不存在。", DESP_PORT_CODE));
            }
            #endregion

            #region 入境口岸
            if (String.IsNullOrWhiteSpace(ENTRY_PORT_CODE))
                yield return new ValidationResult("ENTRY_PORT_CODE必填。");
            else
            {
                var paraEciqEntryPortCode = ParaEciqManager.Instance.ParaEciqEntryPort.Find(o => o.CODE == ENTRY_PORT_CODE);
                if (paraEciqEntryPortCode == null)
                    yield return new ValidationResult(String.Format("ENTRY_PORT_CODE【{0}】不存在。", ENTRY_PORT_CODE));
            }
            #endregion

            #region 贸易方式
            if (String.IsNullOrWhiteSpace(TRADE_MODE_CODE))
                yield return new ValidationResult("TRADE_MODE_CODE必填。");
            else
            {
                var paraEciqTradeCode = ParaEciqManager.Instance.ParaEciqTrade.Find(o => o.CODE == TRADE_MODE_CODE);
                if (paraEciqTradeCode == null)
                    yield return new ValidationResult(String.Format("TRADE_MODE_CODE【{0}】不存在。", TRADE_MODE_CODE));
            }
            #endregion

            #region 运输工具
            if (String.IsNullOrWhiteSpace(TRANS_TYPE_CODE))
                yield return new ValidationResult("TRANS_TYPE_CODE必填。");
            else
            {
                var paraEciqTransTypeCode = ParaEciqManager.Instance.ParaEciqTransType.Find(o => o.CODE == TRANS_TYPE_CODE);
                if (paraEciqTransTypeCode == null)
                    yield return new ValidationResult(String.Format("TRANS_TYPE_CODE【{0}】不存在。", TRANS_TYPE_CODE));
            }
            #endregion

            #region 运输工具名称
            if (string.IsNullOrEmpty(TRANS_MEANS_NAME))
                yield return new ValidationResult("TRANS_MEANS_NAME必填。");
            #endregion

            #region 运输工具号码
            if (string.IsNullOrEmpty(TRANS_MEANS_CODE))
                yield return new ValidationResult("TRANS_MEANS_CODE必填。");
            #endregion

            #region 施检机构
            if (String.IsNullOrWhiteSpace(INSP_ORG_CODE))
                yield return new ValidationResult("INSP_ORG_CODE必填。");
            else
            {
                var paraEciqInspOrgCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == INSP_ORG_CODE);
                if (paraEciqInspOrgCode == null)
                    yield return new ValidationResult(String.Format("INSP_ORG_CODE【{0}】不存在。", INSP_ORG_CODE));
            }
            #endregion

            #region 随附单据
            if (String.IsNullOrWhiteSpace(SHEET_TYPE_CODES))
                yield return new ValidationResult("SHEET_TYPE_CODES必填。");
            else
            {
                var paraEciqSheetTypeInCode = ParaEciqManager.Instance.ParaEciqSheetTypeIn.Find(o => o.CODE == SHEET_TYPE_CODES);
                if (paraEciqSheetTypeInCode == null)
                    yield return new ValidationResult(String.Format("SHEET_TYPE_CODES【{0}】不存在。", SHEET_TYPE_CODES));
            }
            #endregion

            #region 申请单证编码串
            if (String.IsNullOrWhiteSpace(CERT_TYPE_CODES))
                yield return new ValidationResult("CERT_TYPE_CODES必填。");
            else
            {
                var paraEciqCertTypeCode = ParaEciqManager.Instance.ParaEciqCertType.Find(o => o.CODE == CERT_TYPE_CODES);
                if (paraEciqCertTypeCode == null)
                    yield return new ValidationResult(String.Format("CERT_TYPE_CODES【{0}】不存在。", CERT_TYPE_CODES));
            }
            #endregion

            #region 申请单证正本数串
            if (string.IsNullOrEmpty(CERT_ORIGINALS))
                yield return new ValidationResult("CERT_ORIGINALS必填。");
            #endregion

            #region 申请单证副本数串
            if (string.IsNullOrEmpty(CERT_COPIES))
                yield return new ValidationResult("CERT_COPIES必填。");
            #endregion

            #region 联系人
            if (string.IsNullOrEmpty(CONTACTOR))
                yield return new ValidationResult("CONTACTOR必填。");
            #endregion

            #region 电话
            if (string.IsNullOrEmpty(TELEPHONE))
                yield return new ValidationResult("TELEPHONE必填。");
            #endregion

            #region 货物存放地点代码
            if (string.IsNullOrEmpty(GOODS_PLACE_CODE))
                yield return new ValidationResult("GOODS_PLACE_CODE必填。");
            #endregion

            #region 货物存放地点
            if (string.IsNullOrEmpty(GOODS_PLACE))
                yield return new ValidationResult("GOODS_PLACE必填。");
            #endregion

            #region 发货人代码
            if (string.IsNullOrEmpty(CONSIGNOR_CODE))
                yield return new ValidationResult("CONSIGNOR_CODE必填。");
            #endregion

            #region 发货人名称(中文)
            if (string.IsNullOrEmpty(CONSIGNOR_CNAME))
                yield return new ValidationResult("CONSIGNOR_CNAME必填。");
            #endregion

            #region 发货人名称(外文)
            if (string.IsNullOrEmpty(CONSIGNOR_ENAME))
                yield return new ValidationResult("CONSIGNOR_ENAME必填。");
            #endregion

            #region 收货人代码
            if (string.IsNullOrEmpty(CONSIGNEE_CODE))
                yield return new ValidationResult("CONSIGNEE_CODE必填。");
            #endregion

            #region 收货人名称(中文)
            if (string.IsNullOrEmpty(CONSIGNEE_CNAME))
                yield return new ValidationResult("CONSIGNEE_CNAME必填。");
            #endregion

            #region 收货人名称(外文)
            if (string.IsNullOrEmpty(CONSIGNEE_ENAME))
                yield return new ValidationResult("CONSIGNEE_ENAME必填。");
            #endregion

            #region 报检单类型
            if (String.IsNullOrWhiteSpace(DECL_TYPE))
                yield return new ValidationResult("DECL_TYPE必填。");
            else
            {
                
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqDeclType.Find(o => o.Code == DECL_TYPE);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("DECL_TYPE【{0}】不存在。", DECL_TYPE));
            }
            #endregion



            #endregion

            #region 非必填项

            #region 吨数
            if (!string.IsNullOrEmpty(TONNAGE.ToString()))
            {
                bool flag = true;
                try
                {
                    Convert.ToDecimal(TONNAGE);
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("TONNAGE格式错误。");
            }
            #endregion

            #region 索赔有效期
            if (true)
            {
                bool flag = true;
                try
                {
                    Convert.ToDecimal(COUNTER_CLAIM);
                }
                catch
                {
                    flag = false;
                }
                if (!flag)
                    yield return new ValidationResult("COUNTER_CLAIM格式错误。");
            }
            #endregion

            #region 经停口岸
            if (!String.IsNullOrWhiteSpace(VIA_PORT_CODE))
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqPort.Find(o => o.CODE == VIA_PORT_CODE);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("VIA_PORT_CODE【{0}】不存在。", VIA_PORT_CODE));
            }
            #endregion

            #region 生成企业
            if (!String.IsNullOrWhiteSpace(TECH_REG_CODE))
            {
                var paraEciqCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == TECH_REG_CODE);
                if (paraEciqCode == null)
                    yield return new ValidationResult(String.Format("TECH_REG_CODE【{0}】不存在。", TECH_REG_CODE));
                if(!String.IsNullOrWhiteSpace(TECH_REG_NAME))
                {
                    var paraEciqName = ParaEciqManager.Instance.EciqInspOrgName(TECH_REG_CODE);
                    if (!paraEciqName.Equals(TECH_REG_NAME))
                        yield return new ValidationResult(String.Format("TECH_REG_NAME【{0}】与TECH_REG_CODE【{1}】不匹配。", TECH_REG_NAME, TECH_REG_CODE));
                }
            }
            #endregion

            #region 目的机构
            if (!String.IsNullOrWhiteSpace(DEST_ORG_CODE))
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqInspOrg.Find(o => o.CODE == DEST_ORG_CODE);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("DEST_ORG_CODE【{0}】不存在。", DEST_ORG_CODE));
            }
            #endregion

            #region 企业性质
            if (!String.IsNullOrWhiteSpace(ENT_PROPERTY))
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqEntQuality.Find(o => o.CODE == ENT_PROPERTY);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("ENT_PROPERTY【{0}】不存在。", ENT_PROPERTY));
            }
            #endregion

            #region 是否退运
            if (!String.IsNullOrWhiteSpace(BACK_TRANSPORT_FLAG))
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqIsFlag.Find(o => o.Code == BACK_TRANSPORT_FLAG);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("BACK_TRANSPORT_FLAG【{0}】不存在。", BACK_TRANSPORT_FLAG));
            }
            #endregion

            #region 是否报关地
            if (!String.IsNullOrWhiteSpace(IS_CLEARANCE_FLAG))
            {
                var paraEciqPortCode = ParaEciqManager.Instance.ParaEciqIsFlag.Find(o => o.Code == IS_CLEARANCE_FLAG);
                if (paraEciqPortCode == null)
                    yield return new ValidationResult(String.Format("IS_CLEARANCE_FLAG【{0}】不存在。", IS_CLEARANCE_FLAG));
            }
            #endregion

            #endregion

            #region 商品信息
            #region 验证商品信息

            if (EciqEnterDeclformLists == null || EciqEnterDeclformLists.Count < 0)
                yield return new ValidationResult("货物信息必填");
            else
            {
                int i = 1;
                foreach (EciqEnterDeclformList l in EciqEnterDeclformLists)
                {
                    l.GOODS_NO = (i++).ToString();
                    l.SEQ_NO = i;
                    StringBuilder validResult = new StringBuilder();

                    #region 必填信息

                    #region 国检备案编号
                    if (String.IsNullOrWhiteSpace(l.PRODUCT_RECORD_NO))
                        validResult.Append("PRODUCT_RECORD_NO必填,");
                    #endregion

                    #region HS_CODE
                    //验证商品信息，以及录入的计量单位、法定计量单位是否和备案参数表里的一致
                    var paraComplex = ParaCacheManager.Instance.ParaCuComplex.Find(o => o.HS_G_CODE == l.HS_CODE);
                    if (paraComplex == null)
                        validResult.Append(string.Format("HS_CODE【{0}】不存在,", l.HS_CODE));
                    //else
                    //{
                    //    if (!String.IsNullOrWhiteSpace(paraComplex.HS_UNIT_1) && !paraComplex.HS_UNIT_1.Equals(l.QTY_UNIT_CODE))
                    //        validResult.Append(string.Format("QTY_UNIT_CODE【{0}】与商品备案参数表中的【{1}】不符,", l.QTY_UNIT_CODE, paraComplex.HS_UNIT_1)); 
                    //}
                    #endregion

                    #region 商品名称
                    if (String.IsNullOrWhiteSpace(l.GOODS_CNAME))
                        validResult.Append("GOODS_CNAME必填,");
                    #endregion

                    #region 规格型号
                    if (String.IsNullOrWhiteSpace(l.GOODS_MODEL))
                        validResult.Append("GOODS_MODEL应必填,");
                    #endregion

                    #region 品牌
                    if (String.IsNullOrWhiteSpace(l.PROD_BRD_CN))
                        validResult.Append("PROD_BRD_CN应必填,");
                    #endregion

                    #region 数量
                    if (true)
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.QTY);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("QTY格式错误。");
                    }
                    #endregion

                    #region 单价
                    if (true)
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.PRICE);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("PRICE格式错误。");
                    }
                    #endregion

                    #region 计量单位
                    if (String.IsNullOrWhiteSpace(l.QTY_UNIT_CODE))
                        validResult.Append("QTY_UNIT_CODE必填,");
                    else
                    {
                        var paraUnit = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.QTY_UNIT_CODE);
                        if (paraUnit == null)
                            validResult.Append(string.Format("QTY_UNIT_CODE【{0}】不存在,", l.QTY_UNIT_CODE));
                    }
                    #endregion

                    #region 币制
                    if (String.IsNullOrWhiteSpace(l.CCY))
                        validResult.Append("CCY必填,");
                    else
                    {
                        if(!l.CCY.Equals("156"))
                            validResult.Append("CCY限定为156,");
                    }
                    #endregion

                    #region 原产国
                    if (String.IsNullOrWhiteSpace(l.ORIGIN_COUNTRY_CODE))
                        validResult.Append("ORIGIN_COUNTRY_CODE必填,");
                    else
                    {
                        var paraUnit = ParaEciqManager.Instance.ParaEciqCountry.Find(o => o.CODE == l.ORIGIN_COUNTRY_CODE);
                        if (paraUnit == null)
                            validResult.Append(string.Format("ORIGIN_COUNTRY_CODE【{0}】不存在,", l.ORIGIN_COUNTRY_CODE));
                    }
                    #endregion

                    #region 包装件数
                    if (String.IsNullOrWhiteSpace((l.PACK_NUMBER).ToString()))
                        validResult.Append("PACK_NUMBER必填,");
                    else
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.PACK_NUMBER);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("PACK_NUMBER格式错误。");
                    }
                    #endregion

                    #region 包装种类
                    if (String.IsNullOrWhiteSpace(l.PACK_TYPE_CODE))
                        validResult.Append("PACK_TYPE_CODE必填,");
                    else
                    {
                        var paraUnit = ParaEciqManager.Instance.ParaEciqPackType.Find(o => o.CODE == l.PACK_TYPE_CODE);
                        if (paraUnit == null)
                            validResult.Append(string.Format("PACK_TYPE_CODE【{0}】不存在,", l.PACK_TYPE_CODE));
                    }
                    #endregion

                    #region 标准量
                    if (String.IsNullOrWhiteSpace((l.STD_QUANTITY).ToString()))
                        validResult.Append("STD_QUANTITY必填,");
                    else
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.STD_QUANTITY);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("STD_QUANTITY格式错误。");
                    }
                    #endregion

                    #region 标准量单位
                    if (String.IsNullOrWhiteSpace(l.STD_UNIT_CODE))
                        validResult.Append("STD_UNIT_CODE必填,");
                    else
                    {
                        var paraUnit = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.STD_UNIT_CODE);
                        if (paraUnit == null)
                            validResult.Append(string.Format("STD_UNIT_CODE【{0}】不存在,", l.STD_UNIT_CODE));
                    }
                    #endregion

                    #endregion
                    #region 非必填信息

                    #region 销售标准数量
                    if (!String.IsNullOrWhiteSpace((l.STD_QTY).ToString()))
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.STD_QTY);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("STD_QTY格式错误。");
                    }
                    #endregion

                    #region 销标数单位
                    if (!String.IsNullOrWhiteSpace(l.STD_QTY_UNIT_CODE))
                    {
                        var paraUnit = ParaCacheManager.Instance.ParaUnit.Find(o => o.CODE == l.STD_QTY_UNIT_CODE);
                        if (paraUnit == null)
                            validResult.Append(string.Format("STD_QTY_UNIT_CODE【{0}】不存在,", l.STD_QTY_UNIT_CODE));
                    }
                    #endregion

                    #endregion


                }
            }

            #endregion
            #endregion

            #region 集装箱信息
            #region 验证集装箱信息

            if (EciqEnterDeclformConts == null || EciqEnterDeclformConts.Count < 0)
                yield return new ValidationResult("集装箱信息必填");
            else
            {
                foreach (EciqEnterDeclformCont l in EciqEnterDeclformConts)
                {
                    StringBuilder validResult = new StringBuilder();

                    #region 必填信息

                    #region 规格代码
                    if (String.IsNullOrWhiteSpace(l.CONTAINER_MODEL_CODE))
                        validResult.Append("CONTAINER_MODEL_CODE必填,");
                    else
                    {
                        var paraUnit = ParaEciqManager.Instance.ParaEciqContainer.Find(o => o.CODE == l.CONTAINER_MODEL_CODE);
                        if (paraUnit == null)
                            validResult.Append(string.Format("CONTAINER_MODEL_CODE【{0}】不存在,", l.CONTAINER_MODEL_CODE));
                    }
                    #endregion

                    #region 数量
                    if (true)
                    {
                        bool flag = true;
                        try
                        {
                            Convert.ToDecimal(l.CONTAINER_QTY);
                        }
                        catch
                        {
                            flag = false;
                        }
                        if (!flag)
                            yield return new ValidationResult("CONTAINER_QTY格式错误。");
                    }
                    #endregion

                    #region 号码串
                    if (String.IsNullOrWhiteSpace(l.CONTAINER_CODE))
                        validResult.Append("CONTAINER_CODE必填,");
                    #endregion
                    #endregion       

                }
            }

            #endregion
            #endregion

            #region 业务验证

            EciqEnterDeclformService declformService = new EciqEnterDeclformService();

            if (!String.IsNullOrWhiteSpace(CARRIER_NOTE_NO))
            {
                //从清单表中，验证运单号是否多次绑定
                IList<EciqEnterDeclformHead> declformTemp = declformService.GetDeclformByWaybill(CARRIER_NOTE_NO);
                EciqEnterDeclformHead selectedDeclform = declformTemp.FirstOrDefault(o => o.DECL_DECLAR_CHECK_ID != DECL_DECLAR_CHECK_ID);
                if (selectedDeclform != null)
                    yield return new ValidationResult(String.Format("包含的提运单信息【{0}】已绑定给其他入区清单【{1}】。", CARRIER_NOTE_NO, selectedDeclform.DECL_DECLAR_CHECK_ID));
 
            }

            #endregion 


        }
    }
}