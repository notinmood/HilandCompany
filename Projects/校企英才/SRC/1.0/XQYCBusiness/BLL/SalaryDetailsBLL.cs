using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using XQYC.Business.DAL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.BLL
{
    public class SalaryDetailsBLL : BaseBLL<SalaryDetailsBLL, SalaryDetailsEntity, SalaryDetailsDAL>
    {
        /// <summary>
        /// 根据SalarySummaryGuid物联删除掉salaryDetails数据
        /// </summary>
        /// <param name="salarySummaryGuid"></param>
        /// <returns></returns>
        public bool DeleteList(Guid salarySummaryGuid)
        {
            string whereClause = string.Format(" SalarySummaryKey='{0}' ", salarySummaryGuid);
            return base.DeleteList(whereClause);
        }

        /// <summary>
        /// 创建或者更新某SalarySummary对应的工资税项明细
        /// </summary>
        /// <param name="salarySummaryGuid"></param>
        /// <param name="salaryTax"></param>
        /// <returns></returns>
        public bool CreateOrUpdateSalaryTax(Guid salarySummaryGuid,decimal salaryTax)
        {
            bool isSuccessful = false;
            string whereClause = string.Format(" SalarySummaryKey='{0}' AND SalaryItemKind={1} ", salarySummaryGuid, (int)SalaryItemKinds.SalaryTax);
            List<SalaryDetailsEntity> salaryDetailsList = base.GetList(whereClause);
            if (salaryDetailsList == null || salaryDetailsList.Count == 0)
            {
                SalaryDetailsEntity salaryDetailsEntity = new Entity.SalaryDetailsEntity();
                salaryDetailsEntity.SalaryDetailsGuid = GuidHelper.NewGuid();
                salaryDetailsEntity.SalaryItemKey = "工资税";
                salaryDetailsEntity.SalaryItemKind = SalaryItemKinds.SalaryTax;
                salaryDetailsEntity.SalaryItemValue = salaryTax;
                salaryDetailsEntity.SalarySummaryKey = salarySummaryGuid.ToString();

                isSuccessful = Create(salaryDetailsEntity);
            }
            else
            {
                salaryDetailsList[0].SalaryItemValue = salaryTax;
                isSuccessful= Update(salaryDetailsList[0]);
            }

            return isSuccessful;
        }
    }
}