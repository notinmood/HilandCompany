using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using XQYC.Business.BLL;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    public class LaborContractEntity : BaseModel<LaborContractEntity>
    {
        public override string[] BusinessKeyNames
        {
            get { return new string[] { "LaborContractGuid" }; }
        }

        #region 基本信息

        private int laborContractID;
        public int LaborContractID
        {
            get { return laborContractID; }
            set { laborContractID = value; }
        }

        private Guid laborContractGuid = Guid.Empty;
        public Guid LaborContractGuid
        {
            get { return laborContractGuid; }
            set { laborContractGuid = value; }
        }

        private Guid laborUserGuid = Guid.Empty;
        public Guid LaborUserGuid
        {
            get { return laborUserGuid; }
            set { laborUserGuid = value; }
        }

        private string laborCode = String.Empty;
        public string LaborCode
        {
            get { return laborCode; }
            set { laborCode = value; }
        }

        private Guid enterpriseGuid = Guid.Empty;
        public Guid EnterpriseGuid
        {
            get { return enterpriseGuid; }
            set { enterpriseGuid = value; }
        }

        private Guid enterpriseContractGuid = Guid.Empty;
        public Guid EnterpriseContractGuid
        {
            get { return enterpriseContractGuid; }
            set { enterpriseContractGuid = value; }
        }

        private LaborWorkStatuses laborContractStatus = LaborWorkStatuses.Worked;
        public LaborWorkStatuses LaborContractStatus
        {
            get { return laborContractStatus; }
            set { laborContractStatus = value; }
        }

        private Logics laborContractIsCurrent = Logics.True;
        public Logics LaborContractIsCurrent
        {
            get { return laborContractIsCurrent; }
            set { laborContractIsCurrent = value; }
        }

        private string laborDepartment = String.Empty;
        public string LaborDepartment
        {
            get { return laborDepartment; }
            set { laborDepartment = value; }
        }

        private string laborWorkShop = String.Empty;
        public string LaborWorkShop
        {
            get { return laborWorkShop; }
            set { laborWorkShop = value; }
        }

        private DateTime laborContractStartDate = DateTimeHelper.Min;
        public DateTime LaborContractStartDate
        {
            get { return laborContractStartDate; }
            set { laborContractStartDate = value; }
        }

        private DateTime laborContractStopDate = DateTimeHelper.Min;
        public DateTime LaborContractStopDate
        {
            get { return laborContractStopDate; }
            set { laborContractStopDate = value; }
        }

        private string laborContractDetails = String.Empty;
        public string LaborContractDetails
        {
            get { return laborContractDetails; }
            set { laborContractDetails = value; }
        }

        private DateTime laborContractDiscontinueDate = DateTimeHelper.Min;
        public DateTime LaborContractDiscontinueDate
        {
            get { return laborContractDiscontinueDate; }
            set { laborContractDiscontinueDate = value; }
        }

        private string laborContractDiscontinueDesc = String.Empty;
        public string LaborContractDiscontinueDesc
        {
            get { return laborContractDiscontinueDesc; }
            set { laborContractDiscontinueDesc = value; }
        }

        private string enterpriseInsuranceFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的保险企业应该担负部分的计算公式Key
        /// </summary>
        public string EnterpriseInsuranceFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.enterpriseInsuranceFormularKey))
                {
                    this.enterpriseInsuranceFormularKey = Enterprise.DefaultEnterpriseInsuranceFormularKey;
                }
                return enterpriseInsuranceFormularKey;
            }
            set { enterpriseInsuranceFormularKey = value; }
        }

        private string enterpriseReserveFundFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的公积金企业应该担负部分的计算公式Key
        /// </summary>
        public string EnterpriseReserveFundFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.enterpriseReserveFundFormularKey))
                {
                    this.enterpriseReserveFundFormularKey = Enterprise.DefaultEnterpriseReserveFundFormularKey;
                }
                return enterpriseReserveFundFormularKey;
            }
            set { enterpriseReserveFundFormularKey = value; }
        }

        private string enterpriseManageFeeFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的管理费企业应该担负部分的计算公式Key
        /// </summary>
        public string EnterpriseManageFeeFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.enterpriseManageFeeFormularKey))
                {
                    this.enterpriseManageFeeFormularKey = Enterprise.DefaultEnterpriseManageFeeFormularKey;
                }
                return enterpriseManageFeeFormularKey;
            }
            set { enterpriseManageFeeFormularKey = value; }
        }

        private string enterpriseMixCostFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的混合费用企业应该担负部分的计算公式Key
        /// </summary>
        public string EnterpriseMixCostFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.enterpriseMixCostFormularKey))
                {
                    this.enterpriseMixCostFormularKey = Enterprise.DefaultEnterpriseMixCostFormularKey;
                }
                return enterpriseMixCostFormularKey;
            }
            set { enterpriseMixCostFormularKey = value; }
        }

        private string enterpriseOtherCostFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的其他费用企业应该担负部分的计算公式Key
        /// </summary>
        public string EnterpriseOtherCostFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.enterpriseOtherCostFormularKey))
                {
                    this.enterpriseOtherCostFormularKey = Enterprise.DefaultEnterpriseOtherCostFormularKey;
                }
                return enterpriseOtherCostFormularKey;
            }
            set { enterpriseOtherCostFormularKey = value; }
        }

        private string personInsuranceFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的保险个人应该担负部分的计算公式Key
        /// </summary>
        public string PersonInsuranceFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.personInsuranceFormularKey))
                {
                    this.personInsuranceFormularKey = Enterprise.DefaultPersonInsuranceFormularKey;
                }
                return personInsuranceFormularKey;
            }
            set { personInsuranceFormularKey = value; }
        }

        private string personReserveFundFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的公积金个人应该担负部分的计算公式Key
        /// </summary>
        public string PersonReserveFundFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.personReserveFundFormularKey))
                {
                    this.personReserveFundFormularKey = Enterprise.DefaultPersonReserveFundFormularKey;
                }
                return personReserveFundFormularKey;
            }
            set { personReserveFundFormularKey = value; }
        }

        private string personManageFeeFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的管理费个人应该担负部分的计算公式Key
        /// </summary>
        public string PersonManageFeeFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.personManageFeeFormularKey))
                {
                    this.personManageFeeFormularKey = Enterprise.DefaultPersonManageFeeFormularKey;
                }
                return personManageFeeFormularKey;
            }
            set { personManageFeeFormularKey = value; }
        }

        private string personMixCostFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的混合费用个人应该担负部分的计算公式Key
        /// </summary>
        public string PersonMixCostFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.personMixCostFormularKey))
                {
                    this.personMixCostFormularKey = Enterprise.DefaultPersonMixCostFormularKey;
                }
                return personMixCostFormularKey;
            }
            set { personMixCostFormularKey = value; }
        }

        private string personOtherCostFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的其他费用个人应该担负部分的计算公式Key
        /// </summary>
        public string PersonOtherCostFormularKey
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.personOtherCostFormularKey))
                {
                    this.personOtherCostFormularKey = Enterprise.DefaultPersonOtherCostFormularKey;
                }
                return personOtherCostFormularKey;
            }
            set { personOtherCostFormularKey = value; }
        }

        private string enterpriseOtherInsuranceFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的其他保险企业应该担负部分的计算公式Key
        /// </summary>
        public string EnterpriseOtherInsuranceFormularKey
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(this.enterpriseOtherInsuranceFormularKey))
                {
                    this.enterpriseOtherInsuranceFormularKey = Enterprise.DefaultEnterpriseOtherInsuranceFormularKey;
                }
                return enterpriseOtherInsuranceFormularKey; 
            }
            set { enterpriseOtherInsuranceFormularKey = value; }
        }

        private string enterpriseTaxFeeFormularKey = String.Empty;
        /// <summary>
        /// 劳务合同中的各种税费企业应该担负部分的计算公式Key
        /// </summary>
        public string EnterpriseTaxFeeFormularKey
        {
            get 
            {
                if (string.IsNullOrWhiteSpace(this.enterpriseTaxFeeFormularKey))
                {
                    this.enterpriseTaxFeeFormularKey = Enterprise.DefaultEnterpriseTaxFeeFormularKey;
                }
                return enterpriseTaxFeeFormularKey;
            }
            set { enterpriseTaxFeeFormularKey = value; }
        }

        private Guid operateUserGuid = Guid.Empty;
        public Guid OperateUserGuid
        {
            get { return operateUserGuid; }
            set { operateUserGuid = value; }
        }

        private DateTime operateDate = DateTimeHelper.Min;
        public DateTime OperateDate
        {
            get { return operateDate; }
            set { operateDate = value; }
        }
        #endregion

        #region 延迟属性
        private EnterpriseEntity enterprise = null;
        public EnterpriseEntity Enterprise
        {
            get
            {
                if (this.enterprise == null)
                {
                    this.enterprise = EnterpriseBLL.Instance.Get(this.enterpriseGuid);
                }

                return this.enterprise;
            }
        }

        private EnterpriseContractEntity enterpriseContract = null;
        public EnterpriseContractEntity EnterpriseContract
        {
            get
            {
                if (this.enterpriseContract == null)
                {
                    this.enterpriseContract = EnterpriseContractBLL.Instance.Get(this.enterpriseContractGuid);
                }
                return this.enterpriseContract;
            }
        }

        private LaborEntity labor = null;
        public LaborEntity Labor
        {
            get
            {
                if (this.labor == null)
                {
                    this.labor = LaborBLL.Instance.Get(this.LaborUserGuid);
                }

                return this.labor;
            }
        }
        #endregion
    }
}
