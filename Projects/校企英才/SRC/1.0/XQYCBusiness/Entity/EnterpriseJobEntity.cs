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

        private string enterpriseAreaCode = String.Empty;
        public string EnterpriseAreaCode
        {
            get { return enterpriseAreaCode; }
            set { enterpriseAreaCode = value; }
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

        private string interviewDateInfo = String.Empty;
        /// <summary>
        /// 面试时间信息
        /// </summary>
        public string InterviewDateInfo
        {
            get { return interviewDateInfo; }
            set { interviewDateInfo = value; }
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

        private Logics enterpriseJobType= Logics.False;
        /// <summary>
        /// 简章类型（是否为热门简章）
        /// </summary>
        public Logics EnterpriseJobType
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

        private DateTime refreshTime = DateTimeHelper.Min;
        public DateTime RefreshTime
        {
            get { return refreshTime; }
            set { refreshTime = value; }
        }
        

        private string createUserKey = String.Empty;
        public string CreateUserKey
        {
            get { return createUserKey; }
            set { createUserKey = value; }
        }

        private Logics canUsable= Logics.True;
        public Logics CanUsable
        {
            get { return canUsable; }
            set { canUsable = value; }
        }
        #endregion
    }
}
