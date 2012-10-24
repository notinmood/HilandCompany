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

        private LaborWorkStatuses laborContractStatus= LaborWorkStatuses.Worked;
        public LaborWorkStatuses LaborContractStatus
        {
            get { return laborContractStatus; }
            set { laborContractStatus = value; }
        }

        private Logics laborContractIsCurrent= Logics.True;
        public Logics LaborContractIsCurrent
        {
            get { return laborContractIsCurrent; }
            set { laborContractIsCurrent = value; }
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

        private string insuranceFormularKey = String.Empty;
        public string InsuranceFormularKey
        {
            get { return insuranceFormularKey; }
            set { insuranceFormularKey = value; }
        }

        private string reserveFundFormularKey = String.Empty;
        public string ReserveFundFormularKey
        {
            get { return reserveFundFormularKey; }
            set { reserveFundFormularKey = value; }
        }

        private string manageFeeFormularKey = String.Empty;
        public string ManageFeeFormularKey
        {
            get { return manageFeeFormularKey; }
            set { manageFeeFormularKey = value; }
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
        #endregion

        #endregion
    }
}
