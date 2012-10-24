using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    public class SalarySummaryEntity : BaseModel<SalarySummaryEntity>
    {
        /*
         * 本类添加两个冗余字段LaborName（劳务人员名称），LaborCode（劳务人员工号）以便于查询
         */

        public override string[] BusinessKeyNames
        {
            get { return new string[] { "SalarySummaryGuid" }; }
        }

        #region 基本信息

        private int salarySummaryID;
        public int SalarySummaryID
        {
            get { return salarySummaryID; }
            set { salarySummaryID = value; }
        }

        private Guid salarySummaryGuid = Guid.Empty;
        public Guid SalarySummaryGuid
        {
            get { return salarySummaryGuid; }
            set { salarySummaryGuid = value; }
        }

        private DateTime salaryDate = DateTimeHelper.Min;
        public DateTime SalaryDate
        {
            get { return salaryDate; }
            set { salaryDate = value; }
        }

        private string laborKey = String.Empty;
        public string LaborKey
        {
            get { return laborKey; }
            set { laborKey = value; }
        }

        private string laborName = String.Empty;
        public string LaborName
        {
            get { return laborName; }
            set { laborName = value; }
        }

        private string laborCode = String.Empty;
        public string LaborCode
        {
            get { return laborCode; }
            set { laborCode = value; }
        }

        private string enterpriseKey = String.Empty;
        public string EnterpriseKey
        {
            get { return enterpriseKey; }
            set { enterpriseKey = value; }
        }

        private string enterpriseContractKey = String.Empty;
        public string EnterpriseContractKey
        {
            get { return enterpriseContractKey; }
            set { enterpriseContractKey = value; }
        }

        private string createUserKey = String.Empty;
        public string CreateUserKey
        {
            get { return createUserKey; }
            set { createUserKey = value; }
        }

        private DateTime createDate = DateTimeHelper.Min;
        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        private decimal salaryGrossPay;
        /// <summary>
        /// 未扣减费用钱的毛工资
        /// </summary>
        public decimal SalaryGrossPay
        {
            get { return salaryGrossPay; }
            set { salaryGrossPay = value; }
        }

        private decimal salaryRebate;
        /// <summary>
        /// 各种费用扣减（各种罚款等，不包括保险公积金等）
        /// </summary>
        /// <remarks>
        /// 扣减的各种费用直接用负值表示
        /// </remarks>
        public decimal SalaryRebate
        {
            get { return salaryRebate; }
            set { salaryRebate = value; }
        }

        /// <summary>
        /// 应付工资（SalaryGrossPay和SalaryRebate的差额）
        /// </summary>
        /// <remarks>
        /// 因为SalaryRebate已经用负值表示表示了，所以其跟SalaryGrossPay相加即可
        /// </remarks>
        public decimal SalaryNeedPay
        {
            get
            {
                return SalaryGrossPay + SalaryRebate;
            }
        }

        private Logics isCostCalculated;
        public Logics IsCostCalculated
        {
            get { return isCostCalculated; }
            set { isCostCalculated = value; }
        }

        private SalaryPayStatuses salaryPayStatus= SalaryPayStatuses.PaidToOrgnization;
        public SalaryPayStatuses SalaryPayStatus
        {
            get { return salaryPayStatus; }
            set { salaryPayStatus = value; }
        }

        private decimal manageFeeReal;
        public decimal ManageFeeReal
        {
            get { return manageFeeReal; }
            set { manageFeeReal = value; }
        }

        private decimal manageFeeCalculated;
        public decimal ManageFeeCalculated
        {
            get { return manageFeeCalculated; }
            set { manageFeeCalculated = value; }
        }

        private decimal insuranceReal;
        public decimal InsuranceReal
        {
            get { return insuranceReal; }
            set { insuranceReal = value; }
        }

        private decimal insuranceCalculated;
        public decimal InsuranceCalculated
        {
            get { return insuranceCalculated; }
            set { insuranceCalculated = value; }
        }

        private decimal reserveFundReal;
        public decimal ReserveFundReal
        {
            get { return reserveFundReal; }
            set { reserveFundReal = value; }
        }

        private decimal reserveFundCalculated;
        public decimal ReserveFundCalculated
        {
            get { return reserveFundCalculated; }
            set { reserveFundCalculated = value; }
        }

        private string salaryMemo = String.Empty;
        public string SalaryMemo
        {
            get { return salaryMemo; }
            set { salaryMemo = value; }
        }
        
        #endregion
    }
}
