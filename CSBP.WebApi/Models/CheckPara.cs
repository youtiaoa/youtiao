using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CBSP.Web.Framework;

namespace CSBP.WebApi.Models
{
    public class CheckPara
    {
        /// <summary>
        /// 检查电商平台代码
        /// </summary>
        /// <param name="ebPlatId"></param>
        /// <returns></returns>
        public static ValidationResult  CheckEbPlatId(string ebPlatId,ref string ebPlatName)
        {
            if (string.IsNullOrEmpty(ebPlatId))
            {
                return new ValidationResult("EB_PLAT_ID必填。");
            }
            else 
            {
                ebPlatName = ParaCacheManager.Instance.EBPLAT_NAME(ebPlatId);

                if (string.IsNullOrEmpty(ebPlatName))
                {
                    return new ValidationResult("EB_PLAT_ID值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 检查交易平台代码
        /// </summary>
        /// <param name="dealPlatId"></param>
        /// <returns></returns>
        public static ValidationResult CheckDealPlatId(string dealPlatId)
        {
            if (string.IsNullOrEmpty(dealPlatId))
            {
                return new ValidationResult("DEAL_PLAT_ID必填。");
            }
            else
            {
                string dealPlatName = ParaCacheManager.Instance.DEALPLAT_NAME(dealPlatId);

                if (string.IsNullOrEmpty(dealPlatName))
                {
                    return new ValidationResult("DEAL_PLAT_ID值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 检查贸易方式
        /// </summary>
        /// <param name="ebPlatId"></param>
        /// <returns></returns>
        public static ValidationResult CheckTradeMode(string tradeMode)
        {
            if (string.IsNullOrEmpty(tradeMode))
            {
                return new ValidationResult("TRADE_MODE必填。");
            }
            else
            {
                string tradeModeName = ParaCacheManager.Instance.TRADE_MODE_NAME(tradeMode);

                if (string.IsNullOrEmpty(tradeModeName))
                {
                    return new ValidationResult("TRADE_MODE值无效。");
                }
            }

            return null;
        }


        /// <summary>
        /// 检查业务类型
        /// </summary>
        /// <param name="ebPlatId"></param>
        /// <returns></returns>
        public static ValidationResult CheckBusinessType(string businessType)
        {
            if (string.IsNullOrEmpty(businessType))
            {
                return new ValidationResult("BUSINESS_TYPE必填。");
            }
            else
            {
                string tradeModeName = ParaCacheManager.Instance.TRADE_MODE_NAME(businessType);

                if (string.IsNullOrEmpty(tradeModeName))
                {
                    return new ValidationResult("BUSINESS_TYPE值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 检查币制
        /// </summary>
        /// <param name="ebPlatId"></param>
        /// <returns></returns>
        public static ValidationResult CheckCurrCode(string currCode,string fieldName)
        {
            if (string.IsNullOrEmpty(currCode))
            {
                return new ValidationResult(fieldName+"必填。");
            }
            else
            {
                string currCodeName = ParaCacheManager.Instance.CURR_CODE_NAME(currCode);

                if (string.IsNullOrEmpty(currCodeName))
                {
                    return new ValidationResult(fieldName+"值无效。");
                }
            }

            return null;
        }


        /// <summary>
        /// 国家
        /// </summary>
        /// <param name="countryCode"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static ValidationResult CheckCountry(string countryCode,string fieldName)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                return new ValidationResult(fieldName+"必填。");
            }
            else
            {
                string countryName = ParaCacheManager.Instance.COUNTRY_NAME(countryCode);

                if (string.IsNullOrEmpty(countryName))
                {
                    return new ValidationResult(fieldName+"值无效。");
                }
            }

            return null;
        }


        /// <summary>
        /// 单位
        /// </summary>
        /// <param name="unitCode">单位代码</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static ValidationResult CheckUnitCode(string unitCode,string fieldName)
        {
            if (string.IsNullOrEmpty(unitCode))
            {
                return new ValidationResult(fieldName+"必填。");
            }
            else
            {
                string unitName = ParaCacheManager.Instance.UNIT_NAME(unitCode);

                if (string.IsNullOrEmpty(unitName))
                {
                    return new ValidationResult(fieldName+"值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 证件类型
        /// </summary>
        /// <param name="unitCode">证件类型</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static ValidationResult CheckCertType(string certType, string fieldName)
        {
            if (string.IsNullOrEmpty(certType))
            {
                return new ValidationResult(fieldName + "必填。");
            }
            else
            {
                string certTypeName = ParaCacheManager.Instance.CERT_TYPE_NAME(certType);

                if (string.IsNullOrEmpty(certTypeName))
                {
                    return new ValidationResult(fieldName + "值无效。");
                }
            }

            return null;
        }


        /// <summary>
        /// 运输方式
        /// </summary>
        /// <param name="unitCode">运输方式</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static ValidationResult CheckTrafMode(string trafMode, string fieldName)
        {
            if (string.IsNullOrEmpty(trafMode))
            {
                return new ValidationResult(fieldName + "必填。");
            }
            else
            {
                string trafModeName = ParaCacheManager.Instance.TRAF_MODE_NAME(trafMode);

                if (string.IsNullOrEmpty(trafModeName))
                {
                    return new ValidationResult(fieldName + "值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 用途
        /// </summary>
        /// <param name="useTo">用途</param>
        /// <returns></returns>
        public static ValidationResult CheckUseTo(string useTo)
        {
            if (string.IsNullOrEmpty(useTo))
            {
                return new ValidationResult("USE_TO必填。");
            }
            else
            {
                string useToName = ParaCacheManager.Instance.USEDTO_NAME(useTo);

                if (string.IsNullOrEmpty(useToName))
                {
                    return new ValidationResult("USE_TO值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 征免方式
        /// </summary>
        /// <param name="dutyMode">征免</param>
        /// <returns></returns>
        public static ValidationResult CheckDutyMode(string dutyMode)
        {
            if (string.IsNullOrEmpty(dutyMode))
            {
                return new ValidationResult("DUTY_MODE必填。");
            }
            else
            {
                string dutyModeName = ParaCacheManager.Instance.USEDTO_NAME(dutyMode);

                if (string.IsNullOrEmpty(dutyModeName))
                {
                    return new ValidationResult("DUTY_MODE值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 海关口岸
        /// </summary>
        /// <param name="unitCode">口岸代码</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static ValidationResult CheckCustCode(string custCode, string fieldName)
        {
            if (string.IsNullOrEmpty(custCode))
            {
                return new ValidationResult(fieldName + "必填。");
            }
            else
            {
                string customsName = ParaCacheManager.Instance.CUSTOMS_NAME(custCode);

                if (string.IsNullOrEmpty(customsName))
                {
                    return new ValidationResult(fieldName + "值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 检查监管场所
        /// </summary>
        /// <param name="superPlaceCode"></param>
        /// <returns></returns>
        public static ValidationResult CheckSuperPlace(string superPlaceCode)
        {
            if (string.IsNullOrEmpty(superPlaceCode))
            {
                return new ValidationResult("SUPER_PLACE_CODE必填。");
            }
            else
            {
                string superPlaceName = ParaCacheManager.Instance.SUPER_PLACE_NAME(superPlaceCode);

                if (string.IsNullOrEmpty(superPlaceName))
                {
                    return new ValidationResult("SUPER_PLACE_CODE值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 企业信息
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="fieldName"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static ValidationResult CheckCompanyCode(string companyCode, string companyType, string ieFlag, string fieldName,ref string companyName)
        {
            if (string.IsNullOrEmpty(companyCode))
                return new ValidationResult(fieldName+"必填。");
            else
            {
                companyName = ParaCacheManager.Instance.CORP_NAME(companyCode, companyType,ieFlag);

                if (string.IsNullOrEmpty(companyName))
                {
                    return new ValidationResult(fieldName+"值无效。");
                }
            }

            return null;
        }


        /// <summary>
        /// 统一版企业信息
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="fieldName"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static ValidationResult CheckCuCompanyCode(string companyCode, string companyType, string ieFlag, string fieldName, ref string companyName)
        {
            if (string.IsNullOrEmpty(companyCode))
                return new ValidationResult(fieldName + "必填。");
            else
            {
                companyName = ParaCacheManager.Instance.CU_CORP_NAME(companyCode, companyType, ieFlag);

                if (string.IsNullOrEmpty(companyName))
                {
                    return new ValidationResult(fieldName + "值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 国检版企业信息
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="fieldName"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static ValidationResult CheckEciqCompanyCode(string companyCode, string companyType, string ieFlag, string fieldName, ref string companyName)
        {
            if (string.IsNullOrEmpty(companyCode))
                return new ValidationResult(fieldName + "必填。");
            else
            {
                companyName = ParaCacheManager.Instance.ECIQ_CORP_NAME(companyCode, companyType, ieFlag);

                if (string.IsNullOrEmpty(companyName))
                {
                    return new ValidationResult(fieldName + "值无效。");
                }
            }

            return null;
        }

        /// <summary>
        /// 商品信息
        /// </summary>
        /// <param name="companyCode"></param>
        /// <param name="fieldName"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static ValidationResult CheckComplexCode(string complexCode, string fieldName, ref string complexName)
        {
            if (string.IsNullOrEmpty(complexCode))
                return new ValidationResult(fieldName + "必填。");
            else
            {
                complexName = ParaCacheManager.Instance.COMPLEX_NAME(complexCode);

                if (string.IsNullOrEmpty(complexName))
                {
                    return new ValidationResult(fieldName + "值无效。");
                }
            }

            return null;
        }

          


    }
}