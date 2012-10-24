using System;
using HiLand.Utility.Enums;

namespace XQYC.Business.Entity
{
    /// <summary>
    /// 信息员实体
    /// </summary>
    public class InformationBrokerEntity : BusinessUserEx<InformationBrokerEntity>
    {
        #region 基本信息

        private int informationBrokerID;
        public int InformationBrokerID
        {
            get { return informationBrokerID; }
            set { informationBrokerID = value; }
        }


        private UserStatuses informationBrokerStatus;
        /// <summary>
        /// （跟UserStatus记录相同的值，此处仅仅为了方便查询）
        /// </summary>
        public UserStatuses InformationBrokerStatus
        {
            get { return informationBrokerStatus; }
            set { informationBrokerStatus = value; }
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

        #endregion
    }
}
