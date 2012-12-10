using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.Framework4.Permission;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;

namespace XQYC.Business.Entity
{
    public class EnterpriseServiceEntity : EnterpriseEntity ,IResource
    {
        private static EnterpriseServiceEntity empty = null;
        public static new EnterpriseServiceEntity Empty
        {
            get
            {
                if (empty == null)
                {
                    empty = new EnterpriseServiceEntity();
                    empty.isEmpty = true;
                }

                return empty;
            }
        }

        public override string[] BusinessKeyNames
        {
            get { return new string[] { "EnterpriseServiceGuid" }; }
        }

        #region 基本信息

        private int enterpriseServiceID;
        public int EnterpriseServiceID
        {
            get { return enterpriseServiceID; }
            set { enterpriseServiceID = value; }
        }

        private Guid enterpriseServiceGuid = Guid.Empty;
        public Guid EnterpriseServiceGuid
        {
            get { return enterpriseServiceGuid; }
            set { enterpriseServiceGuid = value; }
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

        private int enterpriseServiceType;
        public int EnterpriseServiceType
        {
            get { return enterpriseServiceType; }
            set { enterpriseServiceType = value; }
        }

        private Logics enterpriseServiceStatus;
        public Logics EnterpriseServiceStatus
        {
            get { return enterpriseServiceStatus; }
            set { enterpriseServiceStatus = value; }
        }

        private string enterpriseServiceContactPerson = String.Empty;
        public string EnterpriseServiceContactPerson
        {
            get { return enterpriseServiceContactPerson; }
            set { enterpriseServiceContactPerson = value; }
        }

        private string enterpriseServiceContactTelephone = String.Empty;
        public string EnterpriseServiceContactTelephone
        {
            get { return enterpriseServiceContactTelephone; }
            set { enterpriseServiceContactTelephone = value; }
        }

        private DateTime enterpriseServiceCreateDate = DateTimeHelper.Min;
        public DateTime EnterpriseServiceCreateDate
        {
            get { return enterpriseServiceCreateDate; }
            set { enterpriseServiceCreateDate = value; }
        }

        private string enterpriseServiceCreateUserKey = String.Empty;
        public string EnterpriseServiceCreateUserKey
        {
            get { return enterpriseServiceCreateUserKey; }
            set { enterpriseServiceCreateUserKey = value; }
        }

        private DateTime enterpriseServiceStartDate = DateTimeHelper.Min;
        public DateTime EnterpriseServiceStartDate
        {
            get { return enterpriseServiceStartDate; }
            set { enterpriseServiceStartDate = value; }
        }

        private DateTime enterpriseServiceStopDate = DateTimeHelper.Min;
        public DateTime EnterpriseServiceStopDate
        {
            get { return enterpriseServiceStopDate; }
            set { enterpriseServiceStopDate = value; }
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
        #endregion

        public Guid ResourceGuid
        {
            get { return this.EnterpriseGuid; }
        }

        public string ResourceName
        {
            get { return this.CompanyName; }
        }

        private Logics isProtectedByOwner = Logics.False;
        /// <summary>
        /// 当前资源是否被保护（被保护的数据，仅能所有者修改，其他人仅能查看）
        /// </summary>
        public Logics IsProtectedByOwner
        {
            get { return this.isProtectedByOwner; }
            set { this.isProtectedByOwner = value; }
        }

        private List<string> ownerKeys = new List<string>();
        public List<string> OwnerKeys
        {
            get
            {
                if (ownerKeys.Count == 0)
                {
                    //ownerKeys.Add(CreateUserKey);
                    ownerKeys.Add(ServiceUserGuid.ToString());
                    ownerKeys.Add(BusinessUserGuid.ToString());
                    ownerKeys.Add(SettleUserGuid.ToString());
                }

                return ownerKeys;
            }
        }

        /// <summary>
        /// 当前用户是否拥有资源的控制权（可以是编辑等权限）
        /// </summary>
        public bool IsOwning
        {
            get { return PermissionDataHelper.IsOwning(this); }
        }
    }
}
