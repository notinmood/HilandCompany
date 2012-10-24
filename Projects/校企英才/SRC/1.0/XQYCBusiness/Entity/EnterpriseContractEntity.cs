using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;

namespace XQYC.Business.Entity
{
    public class EnterpriseContractEntity : BaseModel<EnterpriseContractEntity>
    {
        public override string[] BusinessKeyNames
        {
            get { return new string[] { "ContractGuid" }; }
        }

        #region 基本信息

        private int contractID;
        public int ContractID
        {
            get { return contractID; }
            set { contractID = value; }
        }

        private Guid contractGuid = Guid.Empty;
        public Guid ContractGuid
        {
            get { return contractGuid; }
            set { contractGuid = value; }
        }

        private Guid enterpriseGuid = Guid.Empty;
        public Guid EnterpriseGuid
        {
            get { return enterpriseGuid; }
            set { enterpriseGuid = value; }
        }

        private string enterpriseInfo = String.Empty;
        public string EnterpriseInfo
        {
            get { return enterpriseInfo; }
            set { enterpriseInfo = value; }
        }

        private string contractTitle = String.Empty;
        public string ContractTitle
        {
            get { return contractTitle; }
            set { contractTitle = value; }
        }

        private string contractDetails = String.Empty;
        public string ContractDetails
        {
            get { return contractDetails; }
            set { contractDetails = value; }
        }

        private DateTime contractStartDate = DateTimeHelper.Min;
        public DateTime ContractStartDate
        {
            get { return contractStartDate; }
            set { contractStartDate = value; }
        }

        private DateTime contractStopDate = DateTimeHelper.Min;
        public DateTime ContractStopDate
        {
            get { return contractStopDate; }
            set { contractStopDate = value; }
        }

        private DateTime contractCreateDate = DateTimeHelper.Min;
        public DateTime ContractCreateDate
        {
            get { return contractCreateDate; }
            set { contractCreateDate = value; }
        }

        private string contractCreateUserKey = String.Empty;
        public string ContractCreateUserKey
        {
            get { return contractCreateUserKey; }
            set { contractCreateUserKey = value; }
        }

        private int contractLaborCount;
        public int ContractLaborCount
        {
            get { return contractLaborCount; }
            set { contractLaborCount = value; }
        }

        private string contractLaborAddon = String.Empty;
        public string ContractLaborAddon
        {
            get { return contractLaborAddon; }
            set { contractLaborAddon = value; }
        }

        private Logics contractStatus = Logics.True;
        public Logics ContractStatus
        {
            get { return contractStatus; }
            set { contractStatus = value; }
        }
        
        #endregion
    }
}
