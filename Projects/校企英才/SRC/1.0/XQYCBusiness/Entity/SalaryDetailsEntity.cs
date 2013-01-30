using System;
using HiLand.Framework.FoundationLayer;
using HiLand.Framework.FoundationLayer.Attributes;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using XQYC.Business.Enums;

namespace XQYC.Business.Entity
{
    public class SalaryDetailsEntity : BaseModel<SalaryDetailsEntity>
    {
        public override string[] BusinessKeyNames
        {
            get { return new string[] { "SalaryDetailsGuid" }; }
        }

        #region 基本信息
        private int salaryDetailsID;
        public int SalaryDetailsID
		{
			get {return salaryDetailsID;}
			set {salaryDetailsID = value;}
		}

		private Guid salaryDetailsGuid = Guid.Empty;
        public Guid SalaryDetailsGuid
		{
			get {return salaryDetailsGuid;}
			set {salaryDetailsGuid = value;}
		}

		private string salarySummaryKey = String.Empty;
        public string SalarySummaryKey
		{
			get {return salarySummaryKey;}
			set {salarySummaryKey = value;}
		}

		private string salaryItemKey = String.Empty;
        public string SalaryItemKey
		{
			get {return salaryItemKey;}
			set {salaryItemKey = value;}
		}

		private decimal salaryItemValue;
        public decimal SalaryItemValue
		{
			get {return salaryItemValue;}
			set {salaryItemValue = value;}
		}

        private DateTime salaryItemCashDate = DateTimeHelper.Min;
        public DateTime SalaryItemCashDate
        {
            get { return salaryItemCashDate; }
            set { salaryItemCashDate = value; }
        }

        private SalaryItemKinds salaryItemKind;
        /// <summary>
        /// 
        /// </summary>
        public SalaryItemKinds SalaryItemKind
		{
			get {return salaryItemKind;}
			set {salaryItemKind = value;}
		}
        #endregion
    }
}
