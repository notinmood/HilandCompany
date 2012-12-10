using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Finance;
using XQYC.Business.BLL;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    public class SalarySummaryEntity : BaseModel<SalarySummaryEntity>
    {
        /*
         * 本类添加两个冗余字段LaborName（劳务人员名称），LaborCode（劳务人员工号）以便于查询
         */

        /*
         * 公积金，管理费，保险统称为常规费用（Cost），Cost有企业部分和个人部分，每个部分都包含2种使用模式（具体使用选择其一）
         * 1.使用MixCost （所有常规费用都在这里，不进行单个细分为保险，公积金等了）
         * 2.使用单个分项目：公积金，管理费，保险和其他费用
         * 两种选用其一，使用提取这些数据的时候，MixCost优先
         */

        /*
            各种工资实付，应付名称直接的关系：1.先扣除罚款 2.扣除保险等各种费用中个人应该承担的部分 3.扣除个税
         * 1.SalaryGrossPay 未扣减费用前的毛工资
         * 2.SalaryNeedPayBeforeCost 
         * 3.SalaryNeedPayBeforeTax
         * 4.SalaryNeedPayToLabor 
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
        /// 未扣减费用前的毛工资
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
        /// 应付工资（扣除保险公积金等前的应付工资）（SalaryGrossPay和SalaryRebate的差额）
        /// </summary>
        /// <remarks>
        /// 因为SalaryRebate已经用负值表示表示了，所以其跟SalaryGrossPay相加即可
        /// </remarks>
        public decimal SalaryNeedPayBeforeCost
        {
            get
            {
                return SalaryGrossPay + SalaryRebate;
            }
        }

        /// <summary>
        /// 扣除工资税前的应付，通常称为应税工资(里面已经去除了保险等各种常规费用的个人承担部分)
        /// </summary>
        public decimal SalaryNeedPayBeforeTax
        {
            get
            {
                return SalaryNeedPayBeforeCost - PersonCostReal;
            }
        }

        /// <summary>
        /// 最后到劳务人员手中的应付费用
        /// </summary>
        public decimal SalaryNeedPayToLabor
        {
            get
            {
                return SalaryNeedPayBeforeTax - SalaryTax - PersonBorrow;
            }
        }

        /// <summary>
        /// 各种常规费用中个人担负的部分（实际）
        /// </summary>
        public decimal PersonCostReal
        {
            get
            {
                return PersonMixCostReal + PersonInsuranceReal + PersonManageFeeReal + PersonReserveFundReal + PersonOtherCostReal;
            }
        }

        /// <summary>
        /// 各种常规费用中个人担负的部分（计算）
        /// </summary>
        public decimal PersonCostCalculated
        {
            get
            {
                return PersonMixCostCalculated + PersonInsuranceCalculated + PersonManageFeeCalculated + PersonReserveFundCalculated + PersonOtherCostCalculated;
            }
        }

        /// <summary>
        /// 各种常规费用中企业担负的部分（实际）
        /// </summary>
        public decimal EnterpriseCostReal
        {
            get
            {
                return EnterpriseMixCostReal + EnterpriseInsuranceReal + EnterpriseManageFeeReal + EnterpriseReserveFundReal + EnterpriseOtherCostReal;
            }
        }

        /// <summary>
        /// 各种常规费用中企业担负的部分（计算）
        /// </summary>
        public decimal EnterpriseCostCalculated
        {
            get
            {
                return EnterpriseMixCostCalculated + EnterpriseInsuranceCalculated + EnterpriseManageFeeCalculated + EnterpriseReserveFundCalculated + EnterpriseOtherCostCalculated;
            }
        }

        private Logics isCostCalculated;
        public Logics IsCostCalculated
        {
            get { return isCostCalculated; }
            set { isCostCalculated = value; }
        }

        private SalaryPayStatuses salaryPayStatus = SalaryPayStatuses.PaidToOrgnization;
        public SalaryPayStatuses SalaryPayStatus
        {
            get { return salaryPayStatus; }
            set { salaryPayStatus = value; }
        }

        private decimal personBorrow;
        /// <summary>
        /// 个人从单位的借款（给人员发放薪资的时候，需要扣除掉）
        /// </summary>
        public decimal PersonBorrow
        {
            get { return personBorrow; }
            set { personBorrow = value; }
        }

        private decimal enterpriseManageFeeReal;
        public decimal EnterpriseManageFeeReal
        {
            get { return enterpriseManageFeeReal; }
            set { enterpriseManageFeeReal = value; }
        }

        private decimal enterpriseManageFeeCalculated;
        public decimal EnterpriseManageFeeCalculated
        {
            get
            {
                if (enterpriseManageFeeCalculatedFix == 0)
                {
                    return enterpriseManageFeeCalculated;
                }
                else
                {
                    return enterpriseManageFeeCalculatedFix;
                }
            }
            set { enterpriseManageFeeCalculated = value; }
        }

        private decimal enterpriseManageFeeCalculatedFix;
        public decimal EnterpriseManageFeeCalculatedFix
        {
            get { return enterpriseManageFeeCalculatedFix; }
            set { enterpriseManageFeeCalculatedFix = value; }
        }

        private decimal enterpriseInsuranceReal;
        public decimal EnterpriseInsuranceReal
        {
            get { return enterpriseInsuranceReal; }
            set { enterpriseInsuranceReal = value; }
        }

        private decimal enterpriseInsuranceCalculated;
        public decimal EnterpriseInsuranceCalculated
        {
            get
            {
                if (enterpriseInsuranceCalculatedFix == 0)
                {
                    return enterpriseInsuranceCalculated;
                }
                else
                {
                    return enterpriseInsuranceCalculatedFix;
                }
            }
            set { enterpriseInsuranceCalculated = value; }
        }

        private decimal enterpriseInsuranceCalculatedFix;
        public decimal EnterpriseInsuranceCalculatedFix
        {
            get { return enterpriseInsuranceCalculatedFix; }
            set { enterpriseInsuranceCalculatedFix = value; }
        }

        private decimal enterpriseReserveFundReal;
        public decimal EnterpriseReserveFundReal
        {
            get { return enterpriseReserveFundReal; }
            set { enterpriseReserveFundReal = value; }
        }

        private decimal enterpriseReserveFundCalculated;
        public decimal EnterpriseReserveFundCalculated
        {
            get
            {
                if (enterpriseReserveFundCalculatedFix == 0)
                {
                    return enterpriseReserveFundCalculated;
                }
                else
                {
                    return enterpriseReserveFundCalculatedFix;
                }
            }
            set { enterpriseReserveFundCalculated = value; }
        }

        private decimal enterpriseReserveFundCalculatedFix;
        public decimal EnterpriseReserveFundCalculatedFix
        {
            get { return enterpriseReserveFundCalculatedFix; }
            set { enterpriseReserveFundCalculatedFix = value; }
        }

        private decimal personManageFeeReal;
        public decimal PersonManageFeeReal
        {
            get { return personManageFeeReal; }
            set { personManageFeeReal = value; }
        }

        private decimal personManageFeeCalculated;
        public decimal PersonManageFeeCalculated
        {
            get
            {
                if (personManageFeeCalculatedFix == 0)
                {
                    return personManageFeeCalculated;
                }
                else
                {
                    return personManageFeeCalculatedFix;
                }
            }
            set { personManageFeeCalculated = value; }
        }

        private decimal personManageFeeCalculatedFix;
        public decimal PersonManageFeeCalculatedFix
        {
            get { return personManageFeeCalculatedFix; }
            set { personManageFeeCalculatedFix = value; }
        }

        private decimal personInsuranceReal;
        public decimal PersonInsuranceReal
        {
            get { return personInsuranceReal; }
            set { personInsuranceReal = value; }
        }

        private decimal personInsuranceCalculated;
        public decimal PersonInsuranceCalculated
        {
            get
            {
                if (personInsuranceCalculatedFix == 0)
                {
                    return personInsuranceCalculated;
                }
                else
                {
                    return personInsuranceCalculatedFix;
                }
            }
            set { personInsuranceCalculated = value; }
        }

        private decimal personInsuranceCalculatedFix;
        public decimal PersonInsuranceCalculatedFix
        {
            get { return personInsuranceCalculatedFix; }
            set { personInsuranceCalculatedFix = value; }
        }

        private decimal personReserveFundReal;
        public decimal PersonReserveFundReal
        {
            get { return personReserveFundReal; }
            set { personReserveFundReal = value; }
        }

        private decimal personReserveFundCalculated;
        public decimal PersonReserveFundCalculated
        {
            get
            {
                if (personReserveFundCalculatedFix == 0)
                {
                    return personReserveFundCalculated;
                }
                else
                {
                    return personReserveFundCalculatedFix;
                }
            }
            set { personReserveFundCalculated = value; }
        }

        private decimal personReserveFundCalculatedFix;
        public decimal PersonReserveFundCalculatedFix
        {
            get { return personReserveFundCalculatedFix; }
            set { personReserveFundCalculatedFix = value; }
        }

        private decimal enterpriseMixCostReal;
        public decimal EnterpriseMixCostReal
        {
            get { return enterpriseMixCostReal; }
            set { enterpriseMixCostReal = value; }
        }

        private decimal enterpriseMixCostCalculated;
        public decimal EnterpriseMixCostCalculated
        {
            get
            {
                if (enterpriseMixCostCalculatedFix == 0)
                {
                    return enterpriseMixCostCalculated;
                }
                else
                {
                    return enterpriseMixCostCalculatedFix;
                }
            }
            set { enterpriseMixCostCalculated = value; }
        }

        private decimal enterpriseMixCostCalculatedFix;
        public decimal EnterpriseMixCostCalculatedFix
        {
            get { return enterpriseMixCostCalculatedFix; }
            set { enterpriseMixCostCalculatedFix = value; }
        }

        private decimal personMixCostReal;
        public decimal PersonMixCostReal
        {
            get { return personMixCostReal; }
            set { personMixCostReal = value; }
        }

        private decimal personMixCostCalculated;
        public decimal PersonMixCostCalculated
        {
            get
            {
                if (personMixCostCalculatedFix == 0)
                {
                    return personMixCostCalculated;
                }
                else
                {
                    return personMixCostCalculatedFix;
                }
            }
            set { personMixCostCalculated = value; }
        }

        private decimal personMixCostCalculatedFix;
        public decimal PersonMixCostCalculatedFix
        {
            get { return personMixCostCalculatedFix; }
            set { personMixCostCalculatedFix = value; }
        }

        private decimal enterpriseOtherCostReal;
        public decimal EnterpriseOtherCostReal
        {
            get { return enterpriseOtherCostReal; }
            set { enterpriseOtherCostReal = value; }
        }

        private decimal enterpriseOtherCostCalculated;
        public decimal EnterpriseOtherCostCalculated
        {
            get
            {
                if (enterpriseOtherCostCalculatedFix == 0)
                {
                    return enterpriseOtherCostCalculated;
                }
                else
                {
                    return enterpriseOtherCostCalculatedFix;
                }
            }
            set { enterpriseOtherCostCalculated = value; }
        }

        private decimal enterpriseOtherCostCalculatedFix;
        public decimal EnterpriseOtherCostCalculatedFix
        {
            get { return enterpriseOtherCostCalculatedFix; }
            set { enterpriseOtherCostCalculatedFix = value; }
        }

        private decimal personOtherCostReal;
        /// <summary>
        /// 补扣保险金，保险滞纳金等信息
        /// </summary>
        public decimal PersonOtherCostReal
        {
            get { return personOtherCostReal; }
            set { personOtherCostReal = value; }
        }

        private decimal personOtherCostCalculated;
        public decimal PersonOtherCostCalculated
        {
            get
            {
                if (personOtherCostCalculatedFix == 0)
                {
                    return personOtherCostCalculated;
                }
                else
                {
                    return personOtherCostCalculatedFix;
                }
            }
            set { personOtherCostCalculated = value; }
        }

        private decimal personOtherCostCalculatedFix;
        public decimal PersonOtherCostCalculatedFix
        {
            get { return personOtherCostCalculatedFix; }
            set { personOtherCostCalculatedFix = value; }
        }

        /// <summary>
        /// 工资税(工资税这个地方,如果有实际值使用实际值，否则使用计算的值)
        /// 为了便于以后修改的方便，这个值向外暴漏，其他两个值SalaryTaxReal、SalaryTaxCalculated不向外暴漏
        /// </summary>
        public decimal SalaryTax
        {
            get
            {
                if (SalaryTaxReal > 0)
                {
                    return SalaryTaxReal;
                }
                else
                {
                    return SalaryTaxCalculated;
                }
            }
        }

        private decimal salaryTaxReal;
        public decimal SalaryTaxReal
        {
            get { return salaryTaxReal; }
            set { salaryTaxReal = value; }
        }

        private decimal salaryTaxCalculated;
        public decimal SalaryTaxCalculated
        {
            internal set { salaryTaxCalculated = value; }
            get
            {
                salaryTaxCalculated = SalaryTaxHelper.GetSalaryTax(SalaryNeedPayBeforeTax);
                return salaryTaxCalculated;
            }
        }

        private string salaryMemo = String.Empty;
        public string SalaryMemo
        {
            get { return salaryMemo; }
            set { salaryMemo = value; }
        }

        private Logics isCheckPast = Logics.True;
        public Logics IsCheckPast
        {
            get { return isCheckPast; }
            set { isCheckPast = value; }
        }

        private string checkMemo = String.Empty;
        public string CheckMemo
        {
            get { return checkMemo; }
            set { checkMemo = value; }
        }

        #endregion

        #region 延迟属性
        private EnterpriseEntity enterprise = null;
        /// <summary>
        /// 当前合同对应的企业信息
        /// </summary>
        public EnterpriseEntity Enterprise
        {
            get
            {
                if (this.enterprise == null)
                {
                    this.enterprise = EnterpriseBLL.Instance.Get(this.EnterpriseKey);
                }

                return this.enterprise;
            }
        }

        private LaborEntity labor = null;
        /// <summary>
        /// 当前合同对应的劳务人员信息
        /// </summary>
        public LaborEntity Labor
        {
            get
            {
                if (this.labor == null)
                {
                    this.labor = LaborBLL.Instance.Get(this.LaborKey);
                }

                return this.labor;
            }
        }
        #endregion
    }
}
