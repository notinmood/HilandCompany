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
			    [SalaryGrossPay],
			    [SalaryRebate],
			    [IsCostCalculated],
			    [SalaryPayStatus],
			    [EnterpriseManageFeeReal],
			    [EnterpriseManageFeeCalculated],
			    [EnterpriseInsuranceReal],
			    [EnterpriseInsuranceCalculated],
			    [EnterpriseReserveFundReal],
			    [EnterpriseReserveFundCalculated],
			    [PersonManageFeeReal],
			    [PersonManageFeeCalculated],
			    [PersonInsuranceReal],
			    [PersonInsuranceCalculated],
			    [PersonReserveFundReal],
			    [PersonReserveFundCalculated],
			    [EnterpriseMixCostReal],
			    [EnterpriseMixCostCalculated],
			    [PersonMixCostReal],
			    [PersonMixCostCalculated],
			    [EnterpriseOtherCostReal],
			    [EnterpriseOtherCostCalculated],
			    [PersonOtherCostReal],
			    [PersonOtherCostCalculated],
			    [SalaryTaxReal],
			    [SalaryTaxCalculated],			    
                [SalaryMemo],
			    [IsCheckPast],
			    [CheckMemo],
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
			    {0}SalaryGrossPay,
			    {0}SalaryRebate,
			    {0}IsCostCalculated,
			    {0}SalaryPayStatus,
			    {0}EnterpriseManageFeeReal,
			    {0}EnterpriseManageFeeCalculated,
			    {0}EnterpriseInsuranceReal,
			    {0}EnterpriseInsuranceCalculated,
			    {0}EnterpriseReserveFundReal,
			    {0}EnterpriseReserveFundCalculated,
			    {0}PersonManageFeeReal,
			    {0}PersonManageFeeCalculated,
			    {0}PersonInsuranceReal,
			    {0}PersonInsuranceCalculated,
			    {0}PersonReserveFundReal,
			    {0}PersonReserveFundCalculated,
			    {0}EnterpriseMixCostReal,
			    {0}EnterpriseMixCostCalculated,
			    {0}PersonMixCostReal,
			    {0}PersonMixCostCalculated,
			    {0}EnterpriseOtherCostReal,
			    {0}EnterpriseOtherCostCalculated,
			    {0}PersonOtherCostReal,
			    {0}PersonOtherCostCalculated,
			    {0}SalaryTaxReal,
			    {0}SalaryTaxCalculated,
			    {0}SalaryMemo,
			    {0}IsCheckPast,
			    {0}CheckMemo,
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
				[SalaryGrossPay] = {0}SalaryGrossPay,
				[SalaryRebate] = {0}SalaryRebate,
				[IsCostCalculated] = {0}IsCostCalculated,
				[SalaryPayStatus] = {0}SalaryPayStatus,
				[EnterpriseManageFeeReal] = {0}EnterpriseManageFeeReal,
				[EnterpriseManageFeeCalculated] = {0}EnterpriseManageFeeCalculated,
				[EnterpriseInsuranceReal] = {0}EnterpriseInsuranceReal,
				[EnterpriseInsuranceCalculated] = {0}EnterpriseInsuranceCalculated,
				[EnterpriseReserveFundReal] = {0}EnterpriseReserveFundReal,
				[EnterpriseReserveFundCalculated] = {0}EnterpriseReserveFundCalculated,
				[PersonManageFeeReal] = {0}PersonManageFeeReal,
				[PersonManageFeeCalculated] = {0}PersonManageFeeCalculated,
				[PersonInsuranceReal] = {0}PersonInsuranceReal,
				[PersonInsuranceCalculated] = {0}PersonInsuranceCalculated,
				[PersonReserveFundReal] = {0}PersonReserveFundReal,
				[PersonReserveFundCalculated] = {0}PersonReserveFundCalculated,
				[EnterpriseMixCostReal] = {0}EnterpriseMixCostReal,
				[EnterpriseMixCostCalculated] = {0}EnterpriseMixCostCalculated,
				[PersonMixCostReal] = {0}PersonMixCostReal,
				[PersonMixCostCalculated] = {0}PersonMixCostCalculated,
				[EnterpriseOtherCostReal] = {0}EnterpriseOtherCostReal,
				[EnterpriseOtherCostCalculated] = {0}EnterpriseOtherCostCalculated,
				[PersonOtherCostReal] = {0}PersonOtherCostReal,
				[PersonOtherCostCalculated] = {0}PersonOtherCostCalculated,
				[SalaryTaxReal] = {0}SalaryTaxReal,
				[SalaryTaxCalculated] = {0}SalaryTaxCalculated,
				[SalaryMemo] = {0}SalaryMemo,
				[IsCheckPast] = {0}IsCheckPast,
				[CheckMemo] = {0}CheckMemo,
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
			    GenerateParameter("SalaryGrossPay",entity.SalaryGrossPay),
			    GenerateParameter("SalaryRebate",entity.SalaryRebate),
			    GenerateParameter("IsCostCalculated",entity.IsCostCalculated),
			    GenerateParameter("SalaryPayStatus",entity.SalaryPayStatus),
			    GenerateParameter("EnterpriseManageFeeReal",entity.EnterpriseManageFeeReal),
			    GenerateParameter("EnterpriseManageFeeCalculated",entity.EnterpriseManageFeeCalculated),
			    GenerateParameter("EnterpriseInsuranceReal",entity.EnterpriseInsuranceReal),
			    GenerateParameter("EnterpriseInsuranceCalculated",entity.EnterpriseInsuranceCalculated),
			    GenerateParameter("EnterpriseReserveFundReal",entity.EnterpriseReserveFundReal),
			    GenerateParameter("EnterpriseReserveFundCalculated",entity.EnterpriseReserveFundCalculated),
			    GenerateParameter("PersonManageFeeReal",entity.PersonManageFeeReal),
			    GenerateParameter("PersonManageFeeCalculated",entity.PersonManageFeeCalculated),
			    GenerateParameter("PersonInsuranceReal",entity.PersonInsuranceReal),
			    GenerateParameter("PersonInsuranceCalculated",entity.PersonInsuranceCalculated),
			    GenerateParameter("PersonReserveFundReal",entity.PersonReserveFundReal),
			    GenerateParameter("PersonReserveFundCalculated",entity.PersonReserveFundCalculated),
			    GenerateParameter("EnterpriseMixCostReal",entity.EnterpriseMixCostReal),
			    GenerateParameter("EnterpriseMixCostCalculated",entity.EnterpriseMixCostCalculated),
			    GenerateParameter("PersonMixCostReal",entity.PersonMixCostReal),
			    GenerateParameter("PersonMixCostCalculated",entity.PersonMixCostCalculated),
			    GenerateParameter("EnterpriseOtherCostReal",entity.EnterpriseOtherCostReal),
			    GenerateParameter("EnterpriseOtherCostCalculated",entity.EnterpriseOtherCostCalculated),
			    GenerateParameter("PersonOtherCostReal",entity.PersonOtherCostReal),
			    GenerateParameter("PersonOtherCostCalculated",entity.PersonOtherCostCalculated),
                GenerateParameter("SalaryTaxReal",entity.SalaryTaxReal),
			    GenerateParameter("SalaryTaxCalculated",entity.SalaryTaxCalculated),
			    GenerateParameter("SalaryMemo",entity.SalaryMemo?? String.Empty),
			    GenerateParameter("IsCheckPast",entity.IsCheckPast),
			    GenerateParameter("CheckMemo",entity.CheckMemo?? String.Empty)
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
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryGrossPay"))
                {
                    entity.SalaryGrossPay = reader.GetDecimal(reader.GetOrdinal("SalaryGrossPay"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryRebate"))
                {
                    entity.SalaryRebate = reader.GetDecimal(reader.GetOrdinal("SalaryRebate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IsCostCalculated"))
                {
                    entity.IsCostCalculated = (Logics)reader.GetInt32(reader.GetOrdinal("IsCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryPayStatus"))
                {
                    entity.SalaryPayStatus = (SalaryPayStatuses)reader.GetInt32(reader.GetOrdinal("SalaryPayStatus"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseManageFeeReal"))
                {
                    entity.EnterpriseManageFeeReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseManageFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseManageFeeCalculated"))
                {
                    entity.EnterpriseManageFeeCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseManageFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInsuranceReal"))
                {
                    entity.EnterpriseInsuranceReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseInsuranceReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInsuranceCalculated"))
                {
                    entity.EnterpriseInsuranceCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseInsuranceCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseReserveFundReal"))
                {
                    entity.EnterpriseReserveFundReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseReserveFundReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseReserveFundCalculated"))
                {
                    entity.EnterpriseReserveFundCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseReserveFundCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonManageFeeReal"))
                {
                    entity.PersonManageFeeReal = reader.GetDecimal(reader.GetOrdinal("PersonManageFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonManageFeeCalculated"))
                {
                    entity.PersonManageFeeCalculated = reader.GetDecimal(reader.GetOrdinal("PersonManageFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonInsuranceReal"))
                {
                    entity.PersonInsuranceReal = reader.GetDecimal(reader.GetOrdinal("PersonInsuranceReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonInsuranceCalculated"))
                {
                    entity.PersonInsuranceCalculated = reader.GetDecimal(reader.GetOrdinal("PersonInsuranceCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonReserveFundReal"))
                {
                    entity.PersonReserveFundReal = reader.GetDecimal(reader.GetOrdinal("PersonReserveFundReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonReserveFundCalculated"))
                {
                    entity.PersonReserveFundCalculated = reader.GetDecimal(reader.GetOrdinal("PersonReserveFundCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseMixCostReal"))
                {
                    entity.EnterpriseMixCostReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseMixCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseMixCostCalculated"))
                {
                    entity.EnterpriseMixCostCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseMixCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonMixCostReal"))
                {
                    entity.PersonMixCostReal = reader.GetDecimal(reader.GetOrdinal("PersonMixCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonMixCostCalculated"))
                {
                    entity.PersonMixCostCalculated = reader.GetDecimal(reader.GetOrdinal("PersonMixCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherCostReal"))
                {
                    entity.EnterpriseOtherCostReal = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherCostCalculated"))
                {
                    entity.EnterpriseOtherCostCalculated = reader.GetDecimal(reader.GetOrdinal("EnterpriseOtherCostCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonOtherCostReal"))
                {
                    entity.PersonOtherCostReal = reader.GetDecimal(reader.GetOrdinal("PersonOtherCostReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonOtherCostCalculated"))
                {
                    entity.PersonOtherCostCalculated = reader.GetDecimal(reader.GetOrdinal("PersonOtherCostCalculated"));
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
                    entity.IsCheckPast = reader.GetInt32(reader.GetOrdinal("IsCheckPast"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CheckMemo"))
                {
                    entity.CheckMemo = reader.GetString(reader.GetOrdinal("CheckMemo"));
                }
            }
        }
        #endregion
    }
}
