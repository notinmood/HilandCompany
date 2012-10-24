using System;
using HiLand.Framework.FoundationLayer;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

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
    }
}