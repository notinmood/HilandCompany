using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    public class CostFormularEntity : BaseModel<CostFormularEntity>
    {
        public override string[] BusinessKeyNames
        {
            get { return new string[] { "CostFormularGuid" }; }
        }

        #region 基本信息

        private int costFormularID;
        public int CostFormularID
        {
            get { return costFormularID; }
            set { costFormularID = value; }
        }

        private Guid costFormularGuid = Guid.Empty;
        public Guid CostFormularGuid
        {
            get { return costFormularGuid; }
            set { costFormularGuid = value; }
        }

        private string costFormularName = String.Empty;
        public string CostFormularName
        {
            get { return costFormularName; }
            set { costFormularName = value; }
        }

        private string costFormularValue = String.Empty;
        public string CostFormularValue
        {
            get { return costFormularValue; }
            set { costFormularValue = value; }
        }

        private string enterpriseKey = String.Empty;
        public string EnterpriseKey
        {
            get { return enterpriseKey; }
            set { enterpriseKey = value; }
        }

        private int costType;
        public int CostType
        {
            get { return costType; }
            set { costType = value; }
        }

        private CostKinds costKind;
        public CostKinds CostKind
        {
            get { return costKind; }
            set { costKind = value; }
        }

        private Guid referanceGuid = Guid.Empty;
        public Guid ReferanceGuid
        {
            get { return referanceGuid; }
            set { referanceGuid = value; }
        }

        private string costFormularDesc = String.Empty;
        public string CostFormularDesc
        {
            get { return costFormularDesc; }
            set { costFormularDesc = value; }
        }

        private Logics canUsable = Logics.True;
        public Logics CanUsable
        {
            get { return canUsable; }
            set { canUsable = value; }
        }
        #endregion
    }
}
