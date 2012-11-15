using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    public class EnterpriseJobEntity : BaseModel<EnterpriseJobEntity>
    {
        public override string[] BusinessKeyNames
        {
            get { return new string[] { "EnterpriseJobGuid" }; }
        }

        #region 基本信息
        private int enterpriseJobID;
        public int EnterpriseJobID
        {
            get { return enterpriseJobID; }
            set { enterpriseJobID = value; }
        }

        private Guid enterpriseJobGuid = Guid.Empty;
        public Guid EnterpriseJobGuid
        {
            get { return enterpriseJobGuid; }
            set { enterpriseJobGuid = value; }
        }

        private string enterpriseJobTitle = String.Empty;
        public string EnterpriseJobTitle
        {
            get { return enterpriseJobTitle; }
            set { enterpriseJobTitle = value; }
        }

        private string enterpriseKey = String.Empty;
        public string EnterpriseKey
        {
            get { return enterpriseKey; }
            set { enterpriseKey = value; }
        }

        private string enterpriseName = String.Empty;
        public string EnterpriseName
        {
            get { return enterpriseName; }
            set { enterpriseName = value; }
        }

        private string enterpriseAddress = String.Empty;
        public string EnterpriseAddress
        {
            get { return enterpriseAddress; }
            set { enterpriseAddress = value; }
        }

        private string enterpriseContackInfo = String.Empty;
        public string EnterpriseContackInfo
        {
            get { return enterpriseContackInfo; }
            set { enterpriseContackInfo = value; }
        }

        private string enterpriseDesc = String.Empty;
        public string EnterpriseDesc
        {
            get { return enterpriseDesc; }
            set { enterpriseDesc = value; }
        }

        private int enterpriseJobLaborCount;
        public int EnterpriseJobLaborCount
        {
            get { return enterpriseJobLaborCount; }
            set { enterpriseJobLaborCount = value; }
        }

        private string enterpriseJobDemand = String.Empty;
        public string EnterpriseJobDemand
        {
            get { return enterpriseJobDemand; }
            set { enterpriseJobDemand = value; }
        }

        private string enterpriseJobTreadment = String.Empty;
        public string EnterpriseJobTreadment
        {
            get { return enterpriseJobTreadment; }
            set { enterpriseJobTreadment = value; }
        }

        private string enterpriseJobOther = String.Empty;
        public string EnterpriseJobOther
        {
            get { return enterpriseJobOther; }
            set { enterpriseJobOther = value; }
        }

        private string enterpriseJobDesc = String.Empty;
        public string EnterpriseJobDesc
        {
            get { return enterpriseJobDesc; }
            set { enterpriseJobDesc = value; }
        }

        private int enterpriseJobStatus;
        public int EnterpriseJobStatus
        {
            get { return enterpriseJobStatus; }
            set { enterpriseJobStatus = value; }
        }

        private int enterpriseJobType;
        public int EnterpriseJobType
        {
            get { return enterpriseJobType; }
            set { enterpriseJobType = value; }
        }

        private string enterpriseJobStation = String.Empty;
        public string EnterpriseJobStation
        {
            get { return enterpriseJobStation; }
            set { enterpriseJobStation = value; }
        }

        private DateTime createTime = DateTimeHelper.Min;
        public DateTime CreateTime
        {
            get { return createTime; }
            set { createTime = value; }
        }

        private string createUserKey = String.Empty;
        public string CreateUserKey
        {
            get { return createUserKey; }
            set { createUserKey = value; }
        }

        private Logics canUsable;
        public Logics CanUsable
        {
            get { return canUsable; }
            set { canUsable = value; }
        }
        #endregion
    }
}
