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
			        [ManageFeeReal],
			        [ManageFeeCalculated],
			        [InsuranceReal],
			        [InsuranceCalculated],
			        [ReserveFundReal],
			        [ReserveFundCalculated],
			        [SalaryMemo],
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
			        {0}ManageFeeReal,
			        {0}ManageFeeCalculated,
			        {0}InsuranceReal,
			        {0}InsuranceCalculated,
			        {0}ReserveFundReal,
			        {0}ReserveFundCalculated,
			        {0}SalaryMemo,
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
				    [ManageFeeReal] = {0}ManageFeeReal,
				    [ManageFeeCalculated] = {0}ManageFeeCalculated,
				    [InsuranceReal] = {0}InsuranceReal,
				    [InsuranceCalculated] = {0}InsuranceCalculated,
				    [ReserveFundReal] = {0}ReserveFundReal,
				    [ReserveFundCalculated] = {0}ReserveFundCalculated,
				    [SalaryMemo] = {0}SalaryMemo,
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
			    GenerateParameter("ManageFeeReal",entity.ManageFeeReal),
			    GenerateParameter("ManageFeeCalculated",entity.ManageFeeCalculated),
			    GenerateParameter("InsuranceReal",entity.InsuranceReal),
			    GenerateParameter("InsuranceCalculated",entity.InsuranceCalculated),
			    GenerateParameter("ReserveFundReal",entity.ReserveFundReal),
			    GenerateParameter("ReserveFundCalculated",entity.ReserveFundCalculated),
			    GenerateParameter("SalaryMemo",entity.SalaryMemo?? String.Empty)
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
                
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ManageFeeReal"))
                {
                    entity.ManageFeeReal = reader.GetDecimal(reader.GetOrdinal("ManageFeeReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ManageFeeCalculated"))
                {
                    entity.ManageFeeCalculated = reader.GetDecimal(reader.GetOrdinal("ManageFeeCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InsuranceReal"))
                {
                    entity.InsuranceReal = reader.GetDecimal(reader.GetOrdinal("InsuranceReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InsuranceCalculated"))
                {
                    entity.InsuranceCalculated = reader.GetDecimal(reader.GetOrdinal("InsuranceCalculated"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ReserveFundReal"))
                {
                    entity.ReserveFundReal = reader.GetDecimal(reader.GetOrdinal("ReserveFundReal"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ReserveFundCalculated"))
                {
                    entity.ReserveFundCalculated = reader.GetDecimal(reader.GetOrdinal("ReserveFundCalculated"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryMemo"))
                {
                    entity.SalaryMemo = reader.GetString(reader.GetOrdinal("SalaryMemo"));
                }
            }
        }
        #endregion
    }
}
