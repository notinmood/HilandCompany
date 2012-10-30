using System;
using HiLand.Framework.FoundationLayer;
using HiLand.General;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    /// <summary>
    /// 信息员实体
    /// </summary>
    public class InformationBrokerEntity : BaseModel<InformationBrokerEntity>
    {
        public override string[] BusinessKeyNames
        {
            get { return new string[] { "InformationBrokerGuid" }; }
        }

        #region 基本信息
        private int informationBrokerID;
        public int InformationBrokerID
        {
            get { return informationBrokerID; }
            set { informationBrokerID = value; }
        }

        private Guid informationBrokerGuid = Guid.Empty;
        public Guid InformationBrokerGuid
        {
            get { return informationBrokerGuid; }
            set { informationBrokerGuid = value; }
        }

        private string informationBrokerName = String.Empty;
        public string InformationBrokerName
        {
            get { return informationBrokerName; }
            set { informationBrokerName = value; }
        }

        private string informationBrokerNameShort = String.Empty;
        public string InformationBrokerNameShort
        {
            get { return informationBrokerNameShort; }
            set { informationBrokerNameShort = value; }
        }

        private Logics canUsable = Logics.True;
        public Logics CanUsable
        {
            get { return canUsable; }
            set { canUsable = value; }
        }

        private string areaCode = String.Empty;
        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }

        private string industryKey = String.Empty;
        public string IndustryKey
        {
            get { return industryKey; }
            set { industryKey = value; }
        }

        private IndustryTypes industryType;
        public IndustryTypes IndustryType
        {
            get { return industryType; }
            set { industryType = value; }
        }

        private InformationBrokerTypes informationBrokerType;
        public InformationBrokerTypes InformationBrokerType
        {
            get { return informationBrokerType; }
            set { informationBrokerType = value; }
        }

        private string informationBrokerKind = String.Empty;
        /// <summary>
        /// 信息员的性质（比如国办学校，私立学校等）
        /// </summary>
        public string InformationBrokerKind
        {
            get { return informationBrokerKind; }
            set { informationBrokerKind = value; }
        }

        private string principleAddress = String.Empty;
        public string PrincipleAddress
        {
            get { return principleAddress; }
            set { principleAddress = value; }
        }

        private string contactPerson = String.Empty;
        public string ContactPerson
        {
            get { return contactPerson; }
            set { contactPerson = value; }
        }

        private string postCode = String.Empty;
        public string PostCode
        {
            get { return postCode; }
            set { postCode = value; }
        }

        private string telephone = String.Empty;
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        private string fax = String.Empty;
        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        private string email = String.Empty;
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string informationBrokerWWW = String.Empty;
        public string InformationBrokerWWW
        {
            get { return informationBrokerWWW; }
            set { informationBrokerWWW = value; }
        }

        private int informationBrokerLevel;
        public int InformationBrokerLevel
        {
            get { return informationBrokerLevel; }
            set { informationBrokerLevel = value; }
        }

        private string informationBrokerRank = String.Empty;
        public string InformationBrokerRank
        {
            get { return informationBrokerRank; }
            set { informationBrokerRank = value; }
        }

        private string informationBrokerDescription = String.Empty;
        public string InformationBrokerDescription
        {
            get { return informationBrokerDescription; }
            set { informationBrokerDescription = value; }
        }

        private string informationBrokerMemo = String.Empty;
        public string InformationBrokerMemo
        {
            get { return informationBrokerMemo; }
            set { informationBrokerMemo = value; }
        }

        private Guid providerUserGuid = Guid.Empty;
        /// <summary>
        /// 信息提供人员
        /// </summary>
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
        /// <summary>
        /// 推荐人员
        /// </summary>
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
        /// <summary>
        /// 客服人员
        /// </summary>
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
        /// <summary>
        /// 财务人员
        /// </summary>
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
        #endregion
    }
}
