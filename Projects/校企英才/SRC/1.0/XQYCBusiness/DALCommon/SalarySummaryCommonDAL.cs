using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;
using HiLand.Utility.Enums;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.DALCommon
{
    public class SalarySummaryCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<SalarySummaryEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCSalarySummary"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "SalarySummaryGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "SalarySummaryGuid"; }
        }

        /// <summary>
        /// 分页存储过程
        /// </summary>
        protected override string PagingSPName
        {
            get { return "usp_XQYC_SalarySummary_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(SalarySummaryEntity entity)
        {
            //在创建实体时如果实体的Guid尚未指定，那么给其赋初值
            if (entity.SalarySummaryGuid == Guid.Empty)
            {
                entity.SalarySummaryGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCSalarySummary] (
			[SalarySummaryGuid],
			[SalaryDate],
			[LaborKey],
			[LaborName],
			[LaborCode],
			[EnterpriseKey],
			[EnterpriseContractKey],
			[CreateUserKey],
			[CreateDate],
			[SalaryPayStatus],
			[SalaryGrossPay],
			[SalaryRebate],
			[SalaryRebateBeforeTax],
			[PersonBorrow],
			[IsCostCalculated],
			[SalaryCashDate],
			[InsuranceCashDate],
			[ReserveFundCashDate],
			[EnterpriseManageFeeReal],
			[EnterpriseManageFeeCalculated],
			[EnterpriseManageFeeCalculatedFix],
			[EnterpriseManageFeeCashDate],
			[EnterpriseGeneralRecruitFeeReal],
			[EnterpriseGeneralRecruitFeeCalculated],
			[EnterpriseGeneralRecruitFeeCalculatedFix],
			[EnterpriseGeneralRecruitFeeCashDate],
			[EnterpriseOnceRecruitFeeReal],
			[EnterpriseOnceRecruitFeeCalculated],
			[EnterpriseOnceRecruitFeeCalculatedFix],
			[EnterpriseOnceRecruitFeeCashDate],
			[EnterpriseInsuranceReal],
			[EnterpriseInsuranceCalculated],
			[EnterpriseInsuranceCalculatedFix],
			[EnterpriseReserveFundReal],
			[EnterpriseReserveFundCalculated],
			[EnterpriseReserveFundCalculatedFix],
			[PersonManageFeeReal],
			[PersonManageFeeCalculated],
			[PersonManageFeeCalculatedFix],
			[PersonInsuranceReal],
			[PersonInsuranceCalculated],
			[PersonInsuranceCalculatedFix],
			[PersonReserveFundReal],
			[PersonReserveFundCalculated],
			[PersonReserveFundCalculatedFix],
			[EnterpriseMixCostReal],
			[EnterpriseMixCostCalculated],
			[EnterpriseMixCostCalculatedFix],
			[PersonMixCostReal],
			[PersonMixCostCalculated],
			[PersonMixCostCalculatedFix],
			[EnterpriseOtherCostReal],
			[EnterpriseOtherCostCalculated],
			[EnterpriseOtherCostCalculatedFix],
			[PersonOtherCostReal],
			[PersonOtherCostCalculated],
			[PersonOtherCostCalculatedFix],
			[EnterpriseTaxFeeReal],
			[EnterpriseTaxFeeCalculated],
			[EnterpriseTaxFeeCalculatedFix],
			[EnterpriseTaxFeeCashDate],
			[EnterpriseOtherInsuranceReal],
			[EnterpriseOtherInsuranceCalculated],
			[EnterpriseOtherInsuranceCalculatedFix],
			[EnterpriseOtherInsuranceCashDate],
			[SalaryTaxReal],
			[SalaryTaxCalculated],
			[SalaryMemo],
			[IsCheckPast],
			[CheckMemo],
			[IsLocked],
			[SalarySettlementStartDate],
			[SalarySettlementEndDate],
			[IsFirstCash],
			[IsAdvanceCash],
			[PropertyNames],
			[PropertyValues]
        ) 
        Values (
			{0}SalarySummaryGuid,
			{0}SalaryDate,
			{0}LaborKey,
			{0}LaborName,
			{0}LaborCode,
			{0}EnterpriseKey,
			{0}EnterpriseContractKey,
			{0}CreateUserKey,
			{0}CreateDate,
			{0}SalaryPayStatus,
			{0}SalaryGrossPay,
			{0}SalaryRebate,
			{0}SalaryRebateBeforeTax,
			{0}PersonBorrow,
			{0}IsCostCalculated,
			{0}SalaryCashDate,
			{0}InsuranceCashDate,
			{0}ReserveFundCashDate,
			{0}EnterpriseManageFeeReal,
			{0}EnterpriseManageFeeCalculated,
			{0}EnterpriseManageFeeCalculatedFix,
			{0}EnterpriseManageFeeCashDate,
			{0}EnterpriseGeneralRecruitFeeReal,
			{0}EnterpriseGeneralRecruitFeeCalculated,
			{0}EnterpriseGeneralRecruitFeeCalculatedFix,
			{0}EnterpriseGeneralRecruitFeeCashDate,
			{0}EnterpriseOnceRecruitFeeReal,
			{0}EnterpriseOnceRecruitFeeCalculated,
			{0}EnterpriseOnceRecruitFeeCalculatedFix,
			{0}EnterpriseOnceRecruitFeeCashDate,
			{0}EnterpriseInsuranceReal,
			{0}EnterpriseInsuranceCalculated,
			{0}EnterpriseInsuranceCalculatedFix,
			{0}EnterpriseReserveFundReal,
			{0}EnterpriseReserveFundCalculated,
			{0}EnterpriseReserveFundCalculatedFix,
			{0}PersonManageFeeReal,
			{0}PersonManageFeeCalculated,
			{0}PersonManageFeeCalculatedFix,
			{0}PersonInsuranceReal,
			{0}PersonInsuranceCalculated,
			{0}PersonInsuranceCalculatedFix,
			{0}PersonReserveFundReal,
			{0}PersonReserveFundCalculated,
			{0}PersonReserveFundCalculatedFix,
			{0}EnterpriseMixCostReal,
			{0}EnterpriseMixCostCalculated,
			{0}EnterpriseMixCostCalculatedFix,
			{0}PersonMixCostReal,
			{0}PersonMixCostCalculated,
			{0}PersonMixCostCalculatedFix,
			{0}EnterpriseOtherCostReal,
			{0}EnterpriseOtherCostCalculated,
			{0}EnterpriseOtherCostCalculatedFix,
			{0}PersonOtherCostReal,
			{0}PersonOtherCostCalculated,
			{0}PersonOtherCostCalculatedFix,
			{0}EnterpriseTaxFeeReal,
			{0}EnterpriseTaxFeeCalculated,
			{0}EnterpriseTaxFeeCalculatedFix,
			{0}EnterpriseTaxFeeCashDate,
			{0}EnterpriseOtherInsuranceReal,
			{0}EnterpriseOtherInsuranceCalculated,
			{0}EnterpriseOtherInsuranceCalculatedFix,
			{0}EnterpriseOtherInsuranceCashDate,
			{0}SalaryTaxReal,
			{0}SalaryTaxCalculated,
			{0}SalaryMemo,
			{0}IsCheckPast,
			{0}CheckMemo,
			{0}IsLocked,
			{0}SalarySettlementStartDate,
			{0}SalarySettlementEndDate,
			{0}IsFirstCash,
			{0}IsAdvanceCash,
			{0}PropertyNames,
			{0}PropertyValues
        )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(SalarySummaryEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCSalarySummary] Set   
				[SalarySummaryGuid] = {0}SalarySummaryGuid,
				[SalaryDate] = {0}SalaryDate,
				[LaborKey] = {0}LaborKey,
				[LaborName] = {0}LaborName,
				[LaborCode] = {0}LaborCode,
				[EnterpriseKey] = {0}EnterpriseKey,
				[EnterpriseContractKey] = {0}EnterpriseContractKey,
				[CreateUserKey] = {0}CreateUserKey,
				[CreateDate] = {0}CreateDate,
				[SalaryPayStatus] = {0}SalaryPayStatus,
				[SalaryGrossPay] = {0}SalaryGrossPay,
				[SalaryRebate] = {0}SalaryRebate,
				[SalaryRebateBeforeTax] = {0}SalaryRebateBeforeTax,
				[PersonBorrow] = {0}PersonBorrow,
				[IsCostCalculated] = {0}IsCostCalculated,
				[SalaryCashDate] = {0}SalaryCashDate,
				[InsuranceCashDate] = {0}InsuranceCashDate,
				[ReserveFundCashDate] = {0}ReserveFundCashDate,
				[EnterpriseManageFeeReal] = {0}EnterpriseManageFeeReal,
				[EnterpriseManageFeeCalculated] = {0}EnterpriseManageFeeCalculated,
				[EnterpriseManageFeeCalculatedFix] = {0}EnterpriseManageFeeCalculatedFix,
				[EnterpriseManageFeeCashDate] = {0}EnterpriseManageFeeCashDate,
				[EnterpriseGeneralRecruitFeeReal] = {0}EnterpriseGeneralRecruitFeeReal,
				[EnterpriseGeneralRecruitFeeCalculated] = {0}EnterpriseGeneralRecruitFeeCalculated,
				[EnterpriseGeneralRecruitFeeCalculatedFix] = {0}EnterpriseGeneralRecruitFeeCalculatedFix,
				[EnterpriseGeneralRecruitFeeCashDate] = {0}EnterpriseGeneralRecruitFeeCashDate,
				[EnterpriseOnceRecruitFeeReal] = {0}EnterpriseOnceRecruitFeeReal,
				[EnterpriseOnceRecruitFeeCalculated] = {0}EnterpriseOnceRecruitFeeCalculated,
				[EnterpriseOnceRecruitFeeCalculatedFix] = {0}EnterpriseOnceRecruitFeeCalculatedFix,
				[EnterpriseOnceRecruitFeeCashDate] = {0}EnterpriseOnceRecruitFeeCashDate,
				[EnterpriseInsuranceReal] = {0}EnterpriseInsuranceReal,
				[EnterpriseInsuranceCalculated] = {0}EnterpriseInsuranceCalculated,
				[EnterpriseInsuranceCalculatedFix] = {0}EnterpriseInsuranceCalculatedFix,
				[EnterpriseReserveFundReal] = {0}EnterpriseReserveFundReal,
				[EnterpriseReserveFundCalculated] = {0}EnterpriseReserveFundCalculated,
				[EnterpriseReserveFundCalculatedFix] = {0}EnterpriseReserveFundCalculatedFix,
				[PersonManageFeeReal] = {0}PersonManageFeeReal,
				[PersonManageFeeCalculated] = {0}PersonManageFeeCalculated,
				[PersonManageFeeCalculatedFix] = {0}PersonManageFeeCalculatedFix,
				[PersonInsuranceReal] = {0}PersonInsuranceReal,
				[PersonInsuranceCalculated] = {0}PersonInsuranceCalculated,
				[PersonInsuranceCalculatedFix] = {0}PersonInsuranceCalculatedFix,
				[PersonReserveFundReal] = {0}PersonReserveFundReal,
				[PersonReserveFundCalculated] = {0}PersonReserveFundCalculated,
				[PersonReserveFundCalculatedFix] = {0}PersonReserveFundCalculatedFix,
				[EnterpriseMixCostReal] = {0}EnterpriseMixCostReal,
				[EnterpriseMixCostCalculated] = {0}EnterpriseMixCostCalculated,
				[EnterpriseMixCostCalculatedFix] = {0}EnterpriseMixCostCalculatedFix,
				[PersonMixCostReal] = {0}PersonMixCostReal,
				[PersonMixCostCalculated] = {0}PersonMixCostCalculated,
				[PersonMixCostCalculatedFix] = {0}PersonMixCostCalculatedFix,
				[EnterpriseOtherCostReal] = {0}EnterpriseOtherCostReal,
				[EnterpriseOtherCostCalculated] = {0}EnterpriseOtherCostCalculated,
				[EnterpriseOtherCostCalculatedFix] = {0}EnterpriseOtherCostCalculatedFix,
				[PersonOtherCostReal] = {0}PersonOtherCostReal,
				[PersonOtherCostCalculated] = {0}PersonOtherCostCalculated,
				[PersonOtherCostCalculatedFix] = {0}PersonOtherCostCalculatedFix,
				[EnterpriseTaxFeeReal] = {0}EnterpriseTaxFeeReal,
				[EnterpriseTaxFeeCalculated] = {0}EnterpriseTaxFeeCalculated,
				[EnterpriseTaxFeeCalculatedFix] = {0}EnterpriseTaxFeeCalculatedFix,
				[EnterpriseTaxFeeCashDate] = {0}EnterpriseTaxFeeCashDate,
				[EnterpriseOtherInsuranceReal] = {0}EnterpriseOtherInsuranceReal,
				[EnterpriseOtherInsuranceCalculated] = {0}EnterpriseOtherInsuranceCalculated,
				[EnterpriseOtherInsuranceCalculatedFix] = {0}EnterpriseOtherInsuranceCalculatedFix,
				[EnterpriseOtherInsuranceCashDate] = {0}EnterpriseOtherInsuranceCashDate,
				[SalaryTaxReal] = {0}SalaryTaxReal,
				[SalaryTaxCalculated] = {0}SalaryTaxCalculated,
				[SalaryMemo] = {0}SalaryMemo,
				[IsCheckPast] = {0}IsCheckPast,
				[CheckMemo] = {0}CheckMemo,
				[IsLocked] = {0}IsLocked,
				[SalarySettlementStartDate] = {0}SalarySettlementStartDate,
				[SalarySettlementEndDate] = {0}SalarySettlementEndDate,
				[IsFirstCash] = {0}IsFirstCash,
				[IsAdvanceCash] = {0}IsAdvanceCash,
				[PropertyNames] = {0}PropertyNames,
				[PropertyValues] = {0}PropertyValues
        Where [SalarySummaryID] = {0}SalarySummaryID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法


        protected override void InnerPrepareParasAll(SalarySummaryEntity entity, ref List<TParameter> paraList)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("SalarySummaryID",entity.SalarySummaryID),
			    GenerateParameter("SalarySummaryGuid",entity.SalarySummaryGuid),
			    GenerateParameter("SalaryDate",entity.SalaryDate),
			    GenerateParameter("LaborKey",entity.LaborKey?? String.Empty),
			    GenerateParameter("LaborName",entity.LaborName?? String.Empty),
			    GenerateParameter("LaborCode",entity.LaborCode?? String.Empty),
			    GenerateParameter("EnterpriseKey",entity.EnterpriseKey?? String.Empty),
			    GenerateParameter("EnterpriseContractKey",entity.EnterpriseContractKey?? String.Empty),
			    GenerateParameter("CreateUserKey",entity.CreateUserKey?? String.Empty),
			    GenerateParameter("CreateDate",entity.CreateDate),
			    GenerateParameter("SalaryPayStatus",entity.SalaryPayStatus),
			    GenerateParameter("SalaryGrossPay",entity.SalaryGrossPay),
			    GenerateParameter("SalaryRebate",entity.SalaryRebate),
			    GenerateParameter("SalaryRebateBeforeTax",entity.SalaryRebateBeforeTax),
			    GenerateParameter("PersonBorrow",entity.PersonBorrow),
			    GenerateParameter("IsCostCalculated",entity.IsCostCalculated),
			    GenerateParameter("SalaryCashDate",entity.SalaryCashDate),
			    GenerateParameter("InsuranceCashDate",entity.InsuranceCashDate),
			    GenerateParameter("ReserveFundCashDate",entity.ReserveFundCashDate),
			    GenerateParameter("EnterpriseManageFeeReal",entity.EnterpriseManageFeeReal),
			    GenerateParameter("EnterpriseManageFeeCalculated",entity.EnterpriseManageFeeCalculated),
			    GenerateParameter("EnterpriseManageFeeCalculatedFix",entity.EnterpriseManageFeeCalculatedFix),
			    GenerateParameter("EnterpriseManageFeeCashDate",entity.EnterpriseManageFeeCashDate),
			    GenerateParameter("EnterpriseGeneralRecruitFeeReal",entity.EnterpriseGeneralRecruitFeeReal),
			    GenerateParameter("EnterpriseGeneralRecruitFeeCalculated",entity.EnterpriseGeneralRecruitFeeCalculated),
			    GenerateParameter("EnterpriseGeneralRecruitFeeCalculatedFix",entity.EnterpriseGeneralRecruitFeeCalculatedFix),
			    GenerateParameter("EnterpriseGeneralRecruitFeeCashDate",entity.EnterpriseGeneralRecruitFeeCashDate),
			    GenerateParameter("EnterpriseOnceRecruitFeeReal",entity.EnterpriseOnceRecruitFeeReal),
			    GenerateParameter("EnterpriseOnceRecruitFeeCalculated",entity.EnterpriseOnceRecruitFeeCalculated),
			    GenerateParameter("EnterpriseOnceRecruitFeeCalculatedFix",entity.EnterpriseOnceRecruitFeeCalculatedFix),
			    GenerateParameter("EnterpriseOnceRecruitFeeCashDate",entity.EnterpriseOnceRecruitFeeCashDate),
			    GenerateParameter("EnterpriseInsuranceReal",entity.EnterpriseInsuranceReal),
			    GenerateParameter("EnterpriseInsuranceCalculated",entity.EnterpriseInsuranceCalculated),
			    GenerateParameter("EnterpriseInsuranceCalculatedFix",entity.EnterpriseInsuranceCalculatedFix),
			    GenerateParameter("EnterpriseReserveFundReal",entity.EnterpriseReserveFundReal),
			    GenerateParameter("EnterpriseReserveFundCalculated",entity.EnterpriseReserveFundCalculated),
			    GenerateParameter("EnterpriseReserveFundCalculatedFix",entity.EnterpriseReserveFundCalculatedFix),
			    GenerateParameter("PersonManageFeeReal",entity.PersonManageFeeReal),
			    GenerateParameter("PersonManageFeeCalculated",entity.PersonManageFeeCalculated),
			    GenerateParameter("PersonManageFeeCalculatedFix",entity.PersonManageFeeCalculatedFix),
			    GenerateParameter("PersonInsuranceReal",entity.PersonInsuranceReal),
			    GenerateParameter("PersonInsuranceCalculated",entity.PersonInsuranceCalculated),
			    GenerateParameter("PersonInsuranceCalculatedFix",entity.PersonInsuranceCalculatedFix),
			    GenerateParameter("PersonReserveFundReal",entity.PersonReserveFundReal),
			    GenerateParameter("PersonReserveFundCalculated",entity.PersonReserveFundCalculated),
			    GenerateParameter("PersonReserveFundCalculatedFix",entity.PersonReserveFundCalculatedFix),
			    GenerateParameter("EnterpriseMixCostReal",entity.EnterpriseMixCostReal),
			    GenerateParameter("EnterpriseMixCostCalculated",entity.EnterpriseMixCostCalculated),
			    GenerateParameter("EnterpriseMixCostCalculatedFix",entity.EnterpriseMixCostCalculatedFix),
			    GenerateParameter("PersonMixCostReal",entity.PersonMixCostReal),
			    GenerateParameter("PersonMixCostCalculated",entity.PersonMixCostCalculated),
			    GenerateParameter("PersonMixCostCalculatedFix",entity.PersonMixCostCalculatedFix),
			    GenerateParameter("EnterpriseOtherCostReal",entity.EnterpriseOtherCostReal),
			    GenerateParameter("EnterpriseOtherCostCalculated",entity.EnterpriseOtherCostCalculated),
			    GenerateParameter("EnterpriseOtherCostCalculatedFix",entity.EnterpriseOtherCostCalculatedFix),
			    GenerateParameter("PersonOtherCostReal",entity.PersonOtherCostReal),
			    GenerateParameter("PersonOtherCostCalculated",entity.PersonOtherCostCalculated),
			    GenerateParameter("PersonOtherCostCalculatedFix",entity.PersonOtherCostCalculatedFix),
			    GenerateParameter("EnterpriseTaxFeeReal",entity.EnterpriseTaxFeeReal),
			    GenerateParameter("EnterpriseTaxFeeCalculated",entity.EnterpriseTaxFeeCalculated),
			    GenerateParameter("EnterpriseTaxFeeCalculatedFix",entity.EnterpriseTaxFeeCalculatedFix),
			    GenerateParameter("EnterpriseTaxFeeCashDate",entity.EnterpriseTaxFeeCashDate),
			    GenerateParameter("EnterpriseOtherInsuranceReal",entity.EnterpriseOtherInsuranceReal),
			    GenerateParameter("EnterpriseOtherInsuranceCalculated",entity.EnterpriseOtherInsuranceCalculated),
			    GenerateParameter("EnterpriseOtherInsuranceCalculatedFix",entity.EnterpriseOtherInsuranceCalculatedFix),
			    GenerateParameter("EnterpriseOtherInsuranceCashDate",entity.EnterpriseOtherInsuranceCashDate),
			    GenerateParameter("SalaryTaxReal",entity.SalaryTaxReal),
			    GenerateParameter("SalaryTaxCalculated",entity.SalaryTaxCalculated),
			    GenerateParameter("SalaryMemo",entity.SalaryMemo?? String.Empty),
			    GenerateParameter("IsCheckPast",entity.IsCheckPast),
			    GenerateParameter("CheckMemo",entity.CheckMemo?? String.Empty),
			    GenerateParameter("IsLocked",entity.IsLocked),
			    GenerateParameter("SalarySettlementStartDate",entity.SalarySettlementStartDate),
			    GenerateParameter("SalarySettlementEndDate",entity.SalarySettlementEndDate),
			    GenerateParameter("IsFirstCash",entity.IsFirstCash),
			    GenerateParameter("IsAdvanceCash",entity.IsAdvanceCash)
            };

            paraList.AddRange(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        protected override void InnerLoad(IDataReader reader, ref SalarySummaryEntity entity)
        {
            if (reader != null && reader.IsClosed == false && entity != null)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalarySummaryID"))
                {
                    entity.SalarySummaryID = reader.GetInt32(reader.GetOrdinal("SalarySummaryID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalarySummaryGuid"))
                {
                    entity.SalarySummaryGuid = reader.GetGuid(reader.GetOrdinal("SalarySummaryGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryDate"))
                {
                    entity.SalaryDate = reader.GetDateTime(reader.GetOrdinal("SalaryDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborKey"))
                {
                    entity.LaborKey = reader.GetString(reader.GetOrdinal("LaborKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborName"))
                {
                    entity.LaborName = reader.GetString(reader.GetOrdinal("LaborName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborCode"))
                {
                    entity.LaborCode = reader.GetString(reader.GetOrdinal("LaborCode"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseKey"))
                {
                    entity.EnterpriseKey = reader.GetString(reader.GetOrdinal("EnterpriseKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseContractKey"))
                {
                    entity.EnterpriseContractKey = reader.GetString(reader.GetOrdinal("EnterpriseContractKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CreateUserKey"))
                {
                    entity.CreateUserKey = reader.GetString(reader.GetOrdinal("CreateUserKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CreateDate"))
                {
                    entity.CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryPayStatus"))
                {
                    entity.SalaryPayStatus = (SalaryPayStatuses)reader.GetInt32(reader.GetOrdinal("SalaryPayStatus"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryGrossPay"))
                {
                    entity.SalaryGrossPay = reader.GetDecimal(reader.GetOrdinal("SalaryGrossPay"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryRebate"))
                {
                    entity.SalaryRebate = reader.GetDecimal(reader.GetOrdinal("SalaryRebate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryRebateBeforeTax"))
                {
                    entity.SalaryRebateBeforeTax = reader.GetDecimal(reader.GetOrdinal("SalaryRebateBeforeTax"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonBorrow"))
                {
                    entity.PersonBorrow = reader.GetDecimal(reader.GetOrdinal("PersonBorrow"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IsCostCalculated"))
                {
                    entity.IsCostCalculated = (Logics)reader.GetInt32(reader.GetOrdinal("IsCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryCashDate"))
                {
                    entity.SalaryCashDate = reader.GetDateTime(reader.GetOrdinal("SalaryCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InsuranceCashDate"))
                {
                    entity.InsuranceCashDate = reader.GetDateTime(reader.GetOrdinal("InsuranceCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ReserveFundCashDate"))
                {
                    entity.ReserveFundCashDate = reader.GetDateTime(reader.GetOrdinal("ReserveFundCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseManageFeeReal"))
                {
                    entity.EnterpriseManageFeeReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseManageFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseManageFeeCalculated"))
                {
                    entity.EnterpriseManageFeeCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseManageFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseManageFeeCalculatedFix"))
                {
                    entity.EnterpriseManageFeeCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseManageFeeCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseManageFeeCashDate"))
                {
                    entity.EnterpriseManageFeeCashDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseManageFeeCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseGeneralRecruitFeeReal"))
                {
                    entity.EnterpriseGeneralRecruitFeeReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseGeneralRecruitFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseGeneralRecruitFeeCalculated"))
                {
                    entity.EnterpriseGeneralRecruitFeeCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseGeneralRecruitFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseGeneralRecruitFeeCalculatedFix"))
                {
                    entity.EnterpriseGeneralRecruitFeeCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseGeneralRecruitFeeCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseGeneralRecruitFeeCashDate"))
                {
                    entity.EnterpriseGeneralRecruitFeeCashDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseGeneralRecruitFeeCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOnceRecruitFeeReal"))
                {
                    entity.EnterpriseOnceRecruitFeeReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseOnceRecruitFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOnceRecruitFeeCalculated"))
                {
                    entity.EnterpriseOnceRecruitFeeCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseOnceRecruitFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOnceRecruitFeeCalculatedFix"))
                {
                    entity.EnterpriseOnceRecruitFeeCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseOnceRecruitFeeCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOnceRecruitFeeCashDate"))
                {
                    entity.EnterpriseOnceRecruitFeeCashDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseOnceRecruitFeeCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInsuranceReal"))
                {
                    entity.EnterpriseInsuranceReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseInsuranceReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInsuranceCalculated"))
                {
                    entity.EnterpriseInsuranceCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseInsuranceCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInsuranceCalculatedFix"))
                {
                    entity.EnterpriseInsuranceCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseInsuranceCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseReserveFundReal"))
                {
                    entity.EnterpriseReserveFundReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseReserveFundReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseReserveFundCalculated"))
                {
                    entity.EnterpriseReserveFundCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseReserveFundCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseReserveFundCalculatedFix"))
                {
                    entity.EnterpriseReserveFundCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseReserveFundCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonManageFeeReal"))
                {
                    entity.PersonManageFeeReal = reader.GetDecimal(reader.GetOrdinal("PersonManageFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonManageFeeCalculated"))
                {
                    entity.PersonManageFeeCalculated = reader.GetDecimal(reader.GetOrdinal("PersonManageFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonManageFeeCalculatedFix"))
                {
                    entity.PersonManageFeeCalculatedFix = reader.GetDecimal(reader.GetOrdinal("PersonManageFeeCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonInsuranceReal"))
                {
                    entity.PersonInsuranceReal = reader.GetDecimal(reader.GetOrdinal("PersonInsuranceReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonInsuranceCalculated"))
                {
                    entity.PersonInsuranceCalculated = reader.GetDecimal(reader.GetOrdinal("PersonInsuranceCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonInsuranceCalculatedFix"))
                {
                    entity.PersonInsuranceCalculatedFix = reader.GetDecimal(reader.GetOrdinal("PersonInsuranceCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonReserveFundReal"))
                {
                    entity.PersonReserveFundReal = reader.GetDecimal(reader.GetOrdinal("PersonReserveFundReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonReserveFundCalculated"))
                {
                    entity.PersonReserveFundCalculated = reader.GetDecimal(reader.GetOrdinal("PersonReserveFundCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonReserveFundCalculatedFix"))
                {
                    entity.PersonReserveFundCalculatedFix = reader.GetDecimal(reader.GetOrdinal("PersonReserveFundCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseMixCostReal"))
                {
                    entity.EnterpriseMixCostReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseMixCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseMixCostCalculated"))
                {
                    entity.EnterpriseMixCostCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseMixCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseMixCostCalculatedFix"))
                {
                    entity.EnterpriseMixCostCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseMixCostCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonMixCostReal"))
                {
                    entity.PersonMixCostReal = reader.GetDecimal(reader.GetOrdinal("PersonMixCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonMixCostCalculated"))
                {
                    entity.PersonMixCostCalculated = reader.GetDecimal(reader.GetOrdinal("PersonMixCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonMixCostCalculatedFix"))
                {
                    entity.PersonMixCostCalculatedFix = reader.GetDecimal(reader.GetOrdinal("PersonMixCostCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherCostReal"))
                {
                    entity.EnterpriseOtherCostReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherCostCalculated"))
                {
                    entity.EnterpriseOtherCostCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherCostCalculatedFix"))
                {
                    entity.EnterpriseOtherCostCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherCostCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonOtherCostReal"))
                {
                    entity.PersonOtherCostReal = reader.GetDecimal(reader.GetOrdinal("PersonOtherCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonOtherCostCalculated"))
                {
                    entity.PersonOtherCostCalculated = reader.GetDecimal(reader.GetOrdinal("PersonOtherCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonOtherCostCalculatedFix"))
                {
                    entity.PersonOtherCostCalculatedFix = reader.GetDecimal(reader.GetOrdinal("PersonOtherCostCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseTaxFeeReal"))
                {
                    entity.EnterpriseTaxFeeReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseTaxFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseTaxFeeCalculated"))
                {
                    entity.EnterpriseTaxFeeCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseTaxFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseTaxFeeCalculatedFix"))
                {
                    entity.EnterpriseTaxFeeCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseTaxFeeCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseTaxFeeCashDate"))
                {
                    entity.EnterpriseTaxFeeCashDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseTaxFeeCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherInsuranceReal"))
                {
                    entity.EnterpriseOtherInsuranceReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherInsuranceReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherInsuranceCalculated"))
                {
                    entity.EnterpriseOtherInsuranceCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherInsuranceCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherInsuranceCalculatedFix"))
                {
                    entity.EnterpriseOtherInsuranceCalculatedFix = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherInsuranceCalculatedFix"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherInsuranceCashDate"))
                {
                    entity.EnterpriseOtherInsuranceCashDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseOtherInsuranceCashDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryTaxReal"))
                {
                    entity.SalaryTaxReal = reader.GetDecimal(reader.GetOrdinal("SalaryTaxReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryTaxCalculated"))
                {
                    entity.SalaryTaxCalculated = reader.GetDecimal(reader.GetOrdinal("SalaryTaxCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryMemo"))
                {
                    entity.SalaryMemo = reader.GetString(reader.GetOrdinal("SalaryMemo"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IsCheckPast"))
                {
                    entity.IsCheckPast = (Logics)reader.GetInt32(reader.GetOrdinal("IsCheckPast"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CheckMemo"))
                {
                    entity.CheckMemo = reader.GetString(reader.GetOrdinal("CheckMemo"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IsLocked"))
                {
                    entity.IsLocked = (Logics)reader.GetInt32(reader.GetOrdinal("IsLocked"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalarySettlementStartDate"))
                {
                    entity.SalarySettlementStartDate = reader.GetDateTime(reader.GetOrdinal("SalarySettlementStartDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalarySettlementEndDate"))
                {
                    entity.SalarySettlementEndDate = reader.GetDateTime(reader.GetOrdinal("SalarySettlementEndDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IsFirstCash"))
                {
                    entity.IsFirstCash = (Logics)reader.GetInt32(reader.GetOrdinal("IsFirstCash"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IsAdvanceCash"))
                {
                    entity.IsAdvanceCash = (Logics)reader.GetInt32(reader.GetOrdinal("IsAdvanceCash"));
                }
            }
        }
        #endregion
    }
}
