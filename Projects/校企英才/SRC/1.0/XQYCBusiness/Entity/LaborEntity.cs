using System;
using HiLand.Utility.Data;
using XQYC.Business.BLL;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    /// <summary>
    /// 劳务人员实体
    /// </summary>
    public class LaborEntity : BusinessUserEx<LaborEntity>
    {
        public new static LaborEntity Empty
        {
            get
            {
                LaborEntity entity = new LaborEntity();
                entity.isEmpty = true;
                return entity;
            }
        }

        /*
            字段UserGuid跟[CoreUser]表中的字段UserGuid是对应的
         */
        #region 属性信息

        private int laborID;
        public int LaborID
        {
            get { return laborID; }
            set { laborID = value; }
        }

        private string laborCode = String.Empty;
        /// <summary>
        /// 劳务人员的工号
        /// </summary>
        /// <remarks>
        /// 劳务人员前后可能被派往多家企业，每家企业都会给其编号，
        /// 这里记录的是最新一家企业给其的编号（这个信息是有LaborContract内的数据同步过来），
        /// 历史企业给其的编号在LaborContract表中。
        /// </remars>     
        public string LaborCode
        {
            get { return laborCode; }
            set { laborCode = value; }
        }

        private string nativePlace = String.Empty;
        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace
        {
            get { return nativePlace; }
            set { nativePlace = value; }
        }

        private string currentPlace = String.Empty;
        /// <summary>
        /// 现住地
        /// </summary>
        public string CurrentPlace
        {
            get { return currentPlace; }
            set { currentPlace = value; }
        }

        private string idCardPlace = String.Empty;
        /// <summary>
        /// 身份证地址
        /// </summary>
        public string IDCardPlace
        {
            get { return idCardPlace; }
            set { idCardPlace = value; }
        }

        private HouseHoldTypes houseHoldType = HouseHoldTypes.Other;
        /// <summary>
        /// 户口类型
        /// </summary>
        public HouseHoldTypes HouseHoldType
        {
            get { return houseHoldType; }
            set { houseHoldType = value; }
        }

        private string workSkill = String.Empty;
        /// <summary>
        /// 工作技能
        /// </summary>
        public string WorkSkill
        {
            get { return workSkill; }
            set { workSkill = value; }
        }

        private string workSkillPaper = String.Empty;
        /// <summary>
        /// 所持证件
        /// </summary>
        public string WorkSkillPaper
        {
            get { return workSkillPaper; }
            set { workSkillPaper = value; }
        }

        private string workSituation = String.Empty;
        /// <summary>
        /// 工作状况
        /// </summary>
        public string WorkSituation
        {
            get { return workSituation; }
            set { workSituation = value; }
        }

        private string preWorkSituation = String.Empty;
        /// <summary>
        /// 上份工作
        /// </summary>
        public string PreWorkSituation
        {
            get { return preWorkSituation; }
            set { preWorkSituation = value; }
        }

        private string hopeWorkSituation = String.Empty;
        /// <summary>
        /// 现希望从事工作
        /// </summary>
        public string HopeWorkSituation
        {
            get { return hopeWorkSituation; }
            set { hopeWorkSituation = value; }
        }

        private string hopeWorkSalary = String.Empty;
        /// <summary>
        /// 希望工资待遇
        /// </summary>
        public string HopeWorkSalary
        {
            get { return hopeWorkSalary; }
            set { hopeWorkSalary = value; }
        }

        private string urgentLinkMan = String.Empty;
        /// <summary>
        /// 紧急联系人
        /// </summary>
        public string UrgentLinkMan
        {
            get { return urgentLinkMan; }
            set { urgentLinkMan = value; }
        }

        private string urgentTelephone = String.Empty;
        /// <summary>
        /// 紧急联系人电话
        /// </summary>
        public string UrgentTelephone
        {
            get { return urgentTelephone; }
            set { urgentTelephone = value; }
        }

        private string urgentRelationship = String.Empty;
        /// <summary>
        /// 紧急联系人关系
        /// </summary>
        public string UrgentRelationship
        {
            get { return urgentRelationship; }
            set { urgentRelationship = value; }
        }

        private string informationComeFrom = String.Empty;
        /// <summary>
        /// 信息来源
        /// </summary>
        public string InformationComeFrom
        {
            get { return informationComeFrom; }
            set { informationComeFrom = value; }
        }

        private Guid providerUserGuid = Guid.Empty;
        public Guid ProviderUserGuid
        {
            get { return providerUserGuid; }
            set { providerUserGuid = value; }
        }

        private string providerUserName = String.Empty;
        public string ProviderUserName
        {
            get { return providerUserName; }
            set { providerUserName = value; }
        }

        private Guid recommendUserGuid = Guid.Empty;
        public Guid RecommendUserGuid
        {
            get { return recommendUserGuid; }
            set { recommendUserGuid = value; }
        }

        private string recommendUserName = String.Empty;
        public string RecommendUserName
        {
            get { return recommendUserName; }
            set { recommendUserName = value; }
        }

        private Guid serviceUserGuid = Guid.Empty;
        public Guid ServiceUserGuid
        {
            get { return serviceUserGuid; }
            set { serviceUserGuid = value; }
        }

        private string serviceUserName = String.Empty;
        public string ServiceUserName
        {
            get { return serviceUserName; }
            set { serviceUserName = value; }
        }

        private Guid financeUserGuid = Guid.Empty;
        public Guid FinanceUserGuid
        {
            get { return financeUserGuid; }
            set { financeUserGuid = value; }
        }

        private string financeUserName = String.Empty;
        public string FinanceUserName
        {
            get { return financeUserName; }
            set { financeUserName = value; }
        }

        private Guid businessUserGuid = Guid.Empty;
        public Guid BusinessUserGuid
        {
            get { return businessUserGuid; }
            set { businessUserGuid = value; }
        }

        private string businessUserName = String.Empty;
        public string BusinessUserName
        {
            get { return businessUserName; }
            set { businessUserName = value; }
        }

        private Guid settleUserGuid = Guid.Empty;
        public Guid SettleUserGuid
        {
            get { return settleUserGuid; }
            set { settleUserGuid = value; }
        }

        private string settleUserName = String.Empty;
        public string SettleUserName
        {
            get { return settleUserName; }
            set { settleUserName = value; }
        }

        private Guid informationBrokerUserGuid = Guid.Empty;
        public Guid InformationBrokerUserGuid
        {
            get { return informationBrokerUserGuid; }
            set { informationBrokerUserGuid = value; }
        }

        private string informationBrokerUserName = String.Empty;
        public string InformationBrokerUserName
        {
            get { return informationBrokerUserName; }
            set { informationBrokerUserName = value; }
        }


        private int insureType;
        public int InsureType
        {
            get { return insureType; }
            set { insureType = value; }
        }

        private LaborWorkStatuses laborWorkStatus = LaborWorkStatuses.NewWorker;
        public LaborWorkStatuses LaborWorkStatus
        {
            get { return laborWorkStatus; }
            set { laborWorkStatus = value; }
        }


        private string currentEnterpriseKey = String.Empty;
        public string CurrentEnterpriseKey
        {
            get { return currentEnterpriseKey; }
            set { currentEnterpriseKey = value; }
        }

        private string currentEnterpriseName = String.Empty;
        /// <summary>
        /// 当前务工企业的名称
        /// </summary>
        public string CurrentEnterpriseName
        {
            get { return currentEnterpriseName; }
            set { currentEnterpriseName = value; }
        }
        

        private string currentContractKey = String.Empty;
        public string CurrentContractKey
        {
            get { return currentContractKey; }
            set { currentContractKey = value; }
        }

        private DateTime currentContractStartDate = DateTimeHelper.Min;
        public DateTime CurrentContractStartDate
        {
            get { return currentContractStartDate; }
            set { currentContractStartDate = value; }
        }

        private DateTime currentContractStopDate = DateTimeHelper.Min;
        public DateTime CurrentContractStopDate
        {
            get { return currentContractStopDate; }
            set { currentContractStopDate = value; }
        }

        private string currentContractDesc = String.Empty;
        public string CurrentContractDesc
        {
            get { return currentContractDesc; }
            set { currentContractDesc = value; }
        }

        private DateTime currentContractDiscontinueDate = DateTimeHelper.Min;
        public DateTime CurrentContractDiscontinueDate
        {
            get { return currentContractDiscontinueDate; }
            set { currentContractDiscontinueDate = value; }
        }

        private string currentContractDiscontinueDesc = String.Empty;
        public string CurrentContractDiscontinueDesc
        {
            get { return currentContractDiscontinueDesc; }
            set { currentContractDiscontinueDesc = value; }
        }

        private string memo1 = String.Empty;
        /// <summary>
        /// 备注1
        /// </summary>
        public string Memo1
        {
            get { return memo1; }
            set { memo1 = value; }
        }

        private string memo2 = String.Empty;
        /// <summary>
        /// 备注2
        /// </summary>
        public string Memo2
        {
            get { return memo2; }
            set { memo2 = value; }
        }

        private string memo3 = String.Empty;
        /// <summary>
        /// 备注3
        /// </summary>
        public string Memo3
        {
            get { return memo3; }
            set { memo3 = value; }
        }

        private string memo4 = String.Empty;
        /// <summary>
        /// 备注4
        /// </summary>
        public string Memo4
        {
            get { return memo4; }
            set { memo4 = value; }
        }

        private string memo5 = String.Empty;
        /// <summary>
        /// 备注5
        /// </summary>
        public string Memo5
        {
            get { return memo5; }
            set { memo5 = value; }
        }
        #endregion


        //#region 以下几个计算公式采用延迟加载的方式，在使用时获取劳务人员当前的劳务合同中的数据
        
        //private string currentEnterpriseInsuranceFormularKey = String.Empty;
        ///// <summary>
        ///// 当前的劳务合同或者最新的劳务合同中的保险企业应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentEnterpriseInsuranceFormularKey
        //{
        //    get { return currentEnterpriseInsuranceFormularKey; }
        //    set { currentEnterpriseInsuranceFormularKey = value; }
        //}

        //private string currentEnterpriseReserveFundFormularKey = String.Empty;
        ///// <summary>
        ///// 当前的劳务合同或者最新的劳务合同中的公积金企业应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentEnterpriseReserveFundFormularKey
        //{
        //    get { return currentEnterpriseReserveFundFormularKey; }
        //    set { currentEnterpriseReserveFundFormularKey = value; }
        //}

        //private string currentEnterpriseManageFeeFormularKey = String.Empty;
        ///// <summary>
        ///// 当前的劳务合同或者最新的劳务合同中的管理费企业应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentEnterpriseManageFeeFormularKey
        //{
        //    get { return currentEnterpriseManageFeeFormularKey; }
        //    set { currentEnterpriseManageFeeFormularKey = value; }
        //}

        //private string currentEnterpriseMixCostFormularKey = String.Empty;
        ///// <summary>
        ///// 当前的劳务合同或者最新的劳务合同中的混合费用企业应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentEnterpriseMixCostFormularKey
        //{
        //    get { return currentEnterpriseMixCostFormularKey; }
        //    set { currentEnterpriseMixCostFormularKey = value; }
        //}

        //private string currentEnterpriseOtherCostFormularKey = String.Empty;
        ///// <summary>
        ///// 劳务合同中的其他费用企业应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentEnterpriseOtherCostFormularKey
        //{
        //    get { return currentEnterpriseOtherCostFormularKey; }
        //    set { currentEnterpriseOtherCostFormularKey = value; }
        //}

        //private string currentPersonInsuranceFormularKey = String.Empty;
        ///// <summary>
        ///// 劳务合同中的保险个人应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentPersonInsuranceFormularKey
        //{
        //    get { return currentPersonInsuranceFormularKey; }
        //    set { currentPersonInsuranceFormularKey = value; }
        //}

        //private string currentPersonReserveFundFormularKey = String.Empty;
        ///// <summary>
        ///// 劳务合同中的公积金个人应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentPersonReserveFundFormularKey
        //{
        //    get { return currentPersonReserveFundFormularKey; }
        //    set { currentPersonReserveFundFormularKey = value; }
        //}

        //private string currentPersonManageFeeFormularKey = String.Empty;
        ///// <summary>
        ///// 劳务合同中的管理费个人应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentPersonManageFeeFormularKey
        //{
        //    get { return currentPersonManageFeeFormularKey; }
        //    set { currentPersonManageFeeFormularKey = value; }
        //}

        //private string currentPersonMixCostFormularKey = String.Empty;
        ///// <summary>
        ///// 劳务合同中的混合费用个人应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentPersonMixCostFormularKey
        //{
        //    get { return currentPersonMixCostFormularKey; }
        //    set { currentPersonMixCostFormularKey = value; }
        //}

        //private string currentPersonOtherCostFormularKey = String.Empty;
        ///// <summary>
        ///// 劳务合同中的其他费用个人应该担负部分的计算公式Key
        ///// </summary>
        //public string CurrentPersonOtherCostFormularKey
        //{
        //    get { return currentPersonOtherCostFormularKey; }
        //    set { currentPersonOtherCostFormularKey = value; }
        //}
        //#endregion

        #region 延迟属性
        private LaborContractEntity currentLaborContract = LaborContractEntity.Empty;
        /// <summary>
        /// 劳务人员当前的劳务合同
        /// </summary>
        public LaborContractEntity CurrentLaborContract
        {
            get
            {
                if (currentLaborContract.IsEmpty == true)
                {
                    currentLaborContract = LaborContractBLL.Instance.GetCurrentContract(this.UserGuid);
                }

                return currentLaborContract;
            }
        }
        #endregion
    }
}
