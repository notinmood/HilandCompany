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
    public class BoothForeOrderView : BaseModel<BoothForeOrderView>
    {
        public override string[] BusinessKeyNames
        {
            get { return new string[] { "OwnerKey" }; }
        }

        #region 基本信息
        private Guid ownerKey = Guid.Empty;
        public Guid OwnerKey
        {
            get { return ownerKey; }
            set { ownerKey = value; }
        }

        private string ownerName = String.Empty;
        public string OwnerName
        {
            get { return ownerName; }
            set { ownerName = value; }
        }

        private int foreCount = 0;
        /// <summary>
        /// 预定（但是未使用）的场次
        /// </summary>
        public int ForeCount
        {
            get { return this.foreCount; }
            set { this.foreCount = value; }
        }

        private int excutedCount = 0;
        /// <summary>
        /// 已使用的场次
        /// </summary>
        public int ExcutedCount
        {
            get { return this.excutedCount; }
            set { this.excutedCount = value; }
        }

        /// <summary>
        /// 总场次
        /// </summary>
        public int TotalCount 
        {
            get { return this.ForeCount + this.ExcutedCount; }
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
                    this.enterprise = EnterpriseBLL.Instance.Get(this.OwnerKey);
                }

                return this.enterprise;
            }
        }
        #endregion
    }
}
