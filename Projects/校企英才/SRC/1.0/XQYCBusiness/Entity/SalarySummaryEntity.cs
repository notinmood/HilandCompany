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
            各种工资实付，应付名称直接的关系： 1.扣除保险等各种费用中个人应该承担的部分 2.扣除个税 3.先扣除罚款 4.扣除各种借款
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
        /// 扣减的各种费用直接用负值表示;税后扣减的扣费项目
        /// </remarks>
        public decimal SalaryRebate
        {
            get { return salaryRebate; }
            set { salaryRebate = value; }
        }

        private decimal salaryRebateBeforeTax;
        /// <summary>
        /// 各种费用扣减（各种罚款等，不包括保险公积金等）
        /// </summary>
        /// <remarks>
        /// 扣减的各种费用直接用负值表示；税前扣减的扣费项目
        /// </remarks>
        public decimal SalaryRebateBeforeTax
        {
            get { return salaryRebateBeforeTax; }
            set { salaryRebateBeforeTax = value; }
        }

        /// <summary>
        /// 应付工资（扣除保险公积金等前的应付工资）
        /// </summary>
        public decimal SalaryNeedPayBeforeCost
        {
            get
            {
                return SalaryGrossPay + SalaryRebateBeforeTax;
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
        /// <remarks>
        /// 因为SalaryRebate已经用负值表示表示了，所以其相加即可
        /// </remarks>
        public decimal SalaryNeedPayToLabor
        {
            get
            {
                return SalaryNeedPayBeforeTax - SalaryTax + SalaryRebate - PersonBorrow;
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
                if (enterpriseManageFeeCalculatedFix.HasValue == true)
                {
                    return enterpriseManageFeeCalculatedFix.Value;
                }
                else
                {
                    return enterpriseManageFeeCalculated;
                }
            }
            set { enterpriseManageFeeCalculated = value; }
        }

        private decimal? enterpriseManageFeeCalculatedFix = null;
        public decimal? EnterpriseManageFeeCalculatedFix
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
                if (enterpriseInsuranceCalculatedFix.HasValue == true)
                {
                    return enterpriseInsuranceCalculatedFix.Value;
                }
                else
                {
                    return enterpriseInsuranceCalculated;
                }
            }
            set { enterpriseInsuranceCalculated = value; }
        }

        private decimal? enterpriseInsuranceCalculatedFix= null;
        public decimal? EnterpriseInsuranceCalculatedFix
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
                if (enterpriseReserveFundCalculatedFix.HasValue == true)
                {
                    return enterpriseReserveFundCalculatedFix.Value;
                }
                else
                {
                    return enterpriseReserveFundCalculated;
                }
            }
            set { enterpriseReserveFundCalculated = value; }
        }

        private decimal? enterpriseReserveFundCalculatedFix = null;
        public decimal? EnterpriseReserveFundCalculatedFix
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
                if (personManageFeeCalculatedFix.HasValue == true)
                {
                    return personManageFeeCalculatedFix.Value;
                }
                else
                {
                    return personManageFeeCalculated;
                }
            }
            set { personManageFeeCalculated = value; }
        }

        private decimal? personManageFeeCalculatedFix = null;
        public decimal? PersonManageFeeCalculatedFix
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
                if (personInsuranceCalculatedFix.HasValue == true)
                {
                    return personInsuranceCalculatedFix.Value;
                }
                else
                {
                    return personInsuranceCalculated;
                }
            }
            set { personInsuranceCalculated = value; }
        }

        private decimal? personInsuranceCalculatedFix = null;
        public decimal? PersonInsuranceCalculatedFix
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
                if (personReserveFundCalculatedFix.HasValue == true)
                {
                    return personReserveFundCalculatedFix.Value;
                }
                else
                {
                    return personReserveFundCalculated;
                }
            }
            set { personReserveFundCalculated = value; }
        }

        private decimal? personReserveFundCalculatedFix = null;
        public decimal? PersonReserveFundCalculatedFix
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
                if (enterpriseMixCostCalculatedFix.HasValue == true)
                {
                    return enterpriseMixCostCalculatedFix.Value;
                }
                else
                {
                    return enterpriseMixCostCalculated;
                }
            }
            set { enterpriseMixCostCalculated = value; }
        }

        private decimal? enterpriseMixCostCalculatedFix = null;
        public decimal? EnterpriseMixCostCalculatedFix
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
                if (personMixCostCalculatedFix.HasValue == true)
                {
                    return personMixCostCalculatedFix.Value;
                }
                else
                {
                    return personMixCostCalculated;
                }
            }
            set { personMixCostCalculated = value; }
        }

        private decimal? personMixCostCalculatedFix = null;
        public decimal? PersonMixCostCalculatedFix
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
                if (enterpriseOtherCostCalculatedFix.HasValue == true)
                {
                    return enterpriseOtherCostCalculatedFix.Value;
                }
                else
                {
                    return enterpriseOtherCostCalculated;
                }
            }
            set { enterpriseOtherCostCalculated = value; }
        }

        private decimal? enterpriseOtherCostCalculatedFix = null;
        public decimal? EnterpriseOtherCostCalculatedFix
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
                if (personOtherCostCalculatedFix.HasValue == true)
                {
                    return personOtherCostCalculatedFix.Value;
                }
                else
                {
                    return personOtherCostCalculated;
                }
            }
            set { personOtherCostCalculated = value; }
        }

        private decimal? personOtherCostCalculatedFix = null;
        public decimal? PersonOtherCostCalculatedFix
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
