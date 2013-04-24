using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Mathes.StringParse;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

namespace XQYC.Business.BLL
{
    public class SalarySummaryBLL : BaseBLL<SalarySummaryBLL, SalarySummaryEntity, SalarySummaryDAL>
    {
        #region 静态信息
        static SalarySummaryBLL()
        {
            BasicSettingBLL.Instance.BasicSettingChanged += Instance_BasicSettingChanged;
        }

        private static void Instance_BasicSettingChanged(object sender, DataForChange<BasicSettingEntity> args)
        {
            //如果某个费用项被修改了，那么则清空所有的费用项集合（费用项集合使用的时候，让其自动从新从数据库获取）
            if (args.NewData.SettingCategory == "CostItem")
            {
                costList = null;
            }
        }

        private static List<BasicSettingEntity> costList = null;
        /// <summary>
        /// 配置表中，费用项列表
        /// </summary>
        public static List<BasicSettingEntity> CostList
        {
            get
            {
                if (costList == null)
                {
                    costList = BasicSettingBLL.Instance.GetListByCategory("CostItem");
                }

                return costList;
            }
        }
        #endregion

        public override bool Create(SalarySummaryEntity model)
        {
            CalculateNeedCost(model);
            Logics isFirstCash = SalarySummaryBLL.Instance.IsFirstCash(model.EnterpriseKey, model.LaborKey);
            model.IsFirstCash = isFirstCash;
            return base.Create(model);
        }

        public override bool Update(SalarySummaryEntity model)
        {
            CalculateNeedCost(model);
            return base.Update(model);
        }

        /// <summary>
        /// 获取某人某个月在某企业的薪资数据
        /// </summary>
        /// <param name="enterpriseKey"></param>
        /// <param name="laborKey"></param>
        /// <param name="salaryMonth"></param>
        /// <returns></returns>
        public SalarySummaryEntity Get(string enterpriseKey, string laborKey, DateTime salaryMonth)
        {
            string whereClause = string.Format(" LaborKey='{0}' ", laborKey);
            whereClause += string.Format(" AND EnterpriseKey='{0}' ", enterpriseKey);
            DateTime salaryDateFirstDay = DateTimeHelper.GetFirstDateOfMonth(salaryMonth);
            DateTime salaryDateLastDay = DateTimeHelper.GetFirstDateOfMonth(salaryMonth.AddMonths(1));
            whereClause += string.Format(" AND SalaryDate>='{0}' AND SalaryDate<'{1}' ", salaryDateFirstDay, salaryDateLastDay);
            List<SalarySummaryEntity> list = base.GetList(whereClause);

            if (list == null || list.Count == 0)
            {
                return SalarySummaryEntity.Empty;
            }
            else
            {
                return list[0];
            }
        }

        /// <summary>
        /// 获取某企业某个月的薪资数据
        /// </summary>
        /// <param name="enterpriseKey"></param>
        /// <param name="salaryMonth"></param>
        /// <returns></returns>
        public List<SalarySummaryEntity> GetList(string enterpriseKey, DateTime salaryMonth)
        {
            string whereClause = string.Format(" EnterpriseKey = '{0}' ", enterpriseKey);

            DateTime salaryDateFirstDay = DateTimeHelper.GetFirstDateOfMonth(salaryMonth);
            DateTime salaryDateLastDay = DateTimeHelper.GetFirstDateOfMonth(salaryMonth.AddMonths(1));
            whereClause += string.Format(" AND SalaryDate>='{0}' AND SalaryDate<'{1}' ", salaryDateFirstDay, salaryDateLastDay);
            return base.GetList(whereClause);
        }

        /// <summary>
        /// 判断某人员在某企业是否为首次回款
        /// </summary>
        /// <param name="enterpriseKey"></param>
        /// <param name="laborKey"></param>
        /// <param name="excudeSalarySummaryKey">排除在外的工资摘要Key</param>
        /// <returns></returns>
        public Logics IsFirstCash(string enterpriseKey, string laborKey, string excudeSalarySummaryKey = "")
        {
            Logics result = Logics.False;
            string sqlClause = string.Format("select count(*) from XQYCSalarySummary where EnterpriseKey='{0}' AND LaborKey='{1}' ", enterpriseKey, laborKey);
            if (string.IsNullOrWhiteSpace(excudeSalarySummaryKey) == false)
            {
                sqlClause += string.Format(" AND SalarySummaryGuid !='{0}' ", excudeSalarySummaryKey);
            }

            int valueCount = Convert.ToInt32(base.GetScalar(sqlClause));
            if (valueCount > 0)
            {
                result = Logics.False;
            }
            else
            {
                result = Logics.True;
            }

            return result;
        }

        /// <summary>
        /// 按照企业和月份删除工资信息
        /// </summary>
        /// <param name="enterpriseKey"></param>
        /// <param name="salaryMonth"></param>
        /// <returns></returns>
        public bool DeleteList(string enterpriseKey,DateTime salaryMonth)
        {
            string whereClause = string.Format(" EnterpriseKey='{0}' AND SalaryDate='{1}' ",enterpriseKey,salaryMonth.ToShortDateString());
            return base.DeleteList(whereClause);
        }

        #region 私有方法
        /// <summary>
        /// 计算某劳务人员各种应付费用（保险，公积金，管理费等）
        /// </summary>
        /// <param name="isUpdateSalaryTaxDetails">是否同时更新工资税的明细记录</param>
        /// <param name="salarySummary"></param>
        /// <returns></returns>
        private static SalarySummaryEntity CalculateNeedCost(SalarySummaryEntity salarySummary, bool isUpdateSalaryTaxDetails = true)
        {
            LaborEntity labor = LaborBLL.Instance.Get(salarySummary.LaborKey);

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.EnterpriseInsuranceFormularKey) == false)
            {
                salarySummary.EnterpriseInsuranceCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.EnterpriseInsuranceFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.EnterpriseReserveFundFormularKey) == false)
            {
                salarySummary.EnterpriseReserveFundCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.EnterpriseReserveFundFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.EnterpriseManageFeeFormularKey) == false)
            {
                salarySummary.EnterpriseManageFeeCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.EnterpriseManageFeeFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.EnterpriseOtherCostFormularKey) == false)
            {
                salarySummary.EnterpriseOtherCostCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.EnterpriseOtherCostFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.EnterpriseMixCostFormularKey) == false)
            {
                salarySummary.EnterpriseMixCostCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.EnterpriseMixCostFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.PersonInsuranceFormularKey) == false)
            {
                salarySummary.PersonInsuranceCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.PersonInsuranceFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.PersonReserveFundFormularKey) == false)
            {
                salarySummary.PersonReserveFundCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.PersonReserveFundFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.PersonManageFeeFormularKey) == false)
            {
                salarySummary.PersonManageFeeCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.PersonManageFeeFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.PersonOtherCostFormularKey) == false)
            {
                salarySummary.PersonOtherCostCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.PersonOtherCostFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.PersonMixCostFormularKey) == false)
            {
                salarySummary.PersonMixCostCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.PersonMixCostFormularKey), salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.EnterpriseOtherInsuranceFormularKey) == false)
            {
                salarySummary.EnterpriseOtherInsuranceCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.EnterpriseOtherInsuranceFormularKey),salarySummary);
            }

            if (GuidHelper.IsInvalidOrEmpty(labor.CurrentLaborContract.EnterpriseTaxFeeFormularKey) == false)
            {
                salarySummary.EnterpriseTaxFeeCalculated = CalculateCostDetails(new Guid(labor.CurrentLaborContract.EnterpriseTaxFeeFormularKey), salarySummary);
            }

            if (isUpdateSalaryTaxDetails == true)
            {
                SalaryDetailsBLL.Instance.CreateOrUpdateSalaryTax(salarySummary.SalarySummaryGuid, salarySummary.SalaryTax);
            }

            return salarySummary;
        }

        /// <summary>
        /// 计算具体的费用
        /// </summary>
        /// <param name="costFormularKey"></param>
        /// <param name="salarySummary"></param>
        /// <returns></returns>
        private static decimal CalculateCostDetails(Guid costFormularKey, SalarySummaryEntity salarySummary)
        {
            decimal result = 0M;
            CostFormularEntity formularEntity = CostFormularBLL.Instance.Get(costFormularKey);
            if (formularEntity == null)
            {
                return 0M;
            }

            string formularValue = formularEntity.CostFormularValue;
            if (string.IsNullOrWhiteSpace(formularValue) == false)
            {
                List<string> costElementList = StringHelper.GetPlaceHolderList(formularValue, "{", "}");
                foreach (string costElement in costElementList)
                {
                    string placeHolderContent = string.Empty;
                    switch (costElement)
                    {
                        case "NeedPaySalary":
                            placeHolderContent = salarySummary.SalaryNeedPayBeforeCost.ToString();
                            break;
                        case "RealPaySalary":
                            placeHolderContent = salarySummary.SalaryNeedPayToLabor.ToString();
                            break;
                        default:
                            {
                                BasicSettingEntity basicSettingEntity = CostList.Find(w => w.SettingKey == costElement);
                                if (basicSettingEntity != null)
                                {
                                    placeHolderContent = basicSettingEntity.SettingValue;
                                }
                                break;
                            }
                    }

                    string placeHolder = "{" + costElement + "}";
                    formularValue = formularValue.Replace(placeHolder, placeHolderContent);
                }

                try
                {
                    RPN rpn = new RPN();
                    if (rpn.Parse(formularValue))
                    {
                        result = Convert.ToDecimal(rpn.Evaluate());
                    }
                }
                catch
                {
                    result = 0;
                }
            }

            return result;
        }
        #endregion
    }
}