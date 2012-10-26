using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

namespace XQYC.Business.BLL
{
    public class SalarySummaryBLL : BaseBLL<SalarySummaryBLL, SalarySummaryEntity, SalarySummaryDAL>
    {
        /// <summary>
        /// 获取某人某个月的薪资数据
        /// </summary>
        /// <param name="laborKey"></param>
        /// <param name="salaryMonth"></param>
        /// <returns></returns>
        public SalarySummaryEntity Get(string laborKey, DateTime salaryMonth)
        {
            string whereClause = string.Format(" LaborKey='{0}' ", laborKey);
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
    }
}