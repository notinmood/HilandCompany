using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.DALCommon;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;
using HiLand.Utility.Enums;
using XQYC.Business.Entity;

namespace XQYC.Business.DALCommon
{
    public class EnterpriseContractCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<EnterpriseContractEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCEnterpriseContract"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "ContractGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "ContractGuid"; }
        }

        //TODO:此存储过程尚未实现
        protected override string PagingSPName
        {
            get { return "usp_XQYC_EnterpriseContract_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(EnterpriseContractEntity entity)
        {
            //在创建实体时如果实体的Guid尚未指定，那么给其赋初值
            if (entity.ContractGuid == Guid.Empty)
            {
                entity.ContractGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCEnterpriseContract] (
			    [ContractGuid],
			    [EnterpriseGuid],
			    [EnterpriseInfo],
			    [ContractTitle],
			    [ContractDetails],
			    [ContractStartDate],
			    [ContractStopDate],
			    [ContractCreateDate],
			    [ContractCreateUserKey],
			    [ContractLaborCount],
			    [ContractLaborAddon],
                [ContractStatus],
			    [PropertyNames],
			    [PropertyValues]
            ) 
            Values (
			    {0}ContractGuid,
			    {0}EnterpriseGuid,
			    {0}EnterpriseInfo,
			    {0}ContractTitle,
			    {0}ContractDetails,
			    {0}ContractStartDate,
			    {0}ContractStopDate,
			    {0}ContractCreateDate,
			    {0}ContractCreateUserKey,
			    {0}ContractLaborCount,
			    {0}ContractLaborAddon,
                {0}ContractStatus,
			    {0}PropertyNames,
			    {0}PropertyValues
            )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(EnterpriseContractEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCEnterpriseContract] Set   
					[ContractGuid] = {0}ContractGuid,
					[EnterpriseGuid] = {0}EnterpriseGuid,
					[EnterpriseInfo] = {0}EnterpriseInfo,
					[ContractTitle] = {0}ContractTitle,
					[ContractDetails] = {0}ContractDetails,
					[ContractStartDate] = {0}ContractStartDate,
					[ContractStopDate] = {0}ContractStopDate,
					[ContractCreateDate] = {0}ContractCreateDate,
					[ContractCreateUserKey] = {0}ContractCreateUserKey,
					[ContractLaborCount] = {0}ContractLaborCount,
					[ContractLaborAddon] = {0}ContractLaborAddon,
                    [ContractStatus]= {0}ContractStatus,
					[PropertyNames] = {0}PropertyNames,
					[PropertyValues] = {0}PropertyValues
             Where [ContractID] = {0}ContractID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法


        protected override void InnerPrepareParasAll(EnterpriseContractEntity entity, ref List<TParameter> paraList)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("ContractID",entity.ContractID),
			    GenerateParameter("ContractGuid",entity.ContractGuid),
			    GenerateParameter("EnterpriseGuid",entity.EnterpriseGuid),
			    GenerateParameter("EnterpriseInfo",entity.EnterpriseInfo?? String.Empty),
			    GenerateParameter("ContractTitle",entity.ContractTitle?? String.Empty),
			    GenerateParameter("ContractDetails",entity.ContractDetails?? String.Empty),
			    GenerateParameter("ContractStartDate",entity.ContractStartDate),
			    GenerateParameter("ContractStopDate",entity.ContractStopDate),
			    GenerateParameter("ContractCreateDate",entity.ContractCreateDate),
			    GenerateParameter("ContractCreateUserKey",entity.ContractCreateUserKey?? String.Empty),
			    GenerateParameter("ContractLaborCount",entity.ContractLaborCount),
			    GenerateParameter("ContractLaborAddon",entity.ContractLaborAddon?? String.Empty),
                GenerateParameter("ContractStatus",entity.ContractStatus)
            };

            paraList.AddRange(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        protected override void InnerLoad(IDataReader reader, ref EnterpriseContractEntity entity)
        {
            if (reader != null && reader.IsClosed == false && entity != null)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractID"))
                {
                    entity.ContractID = reader.GetInt32(reader.GetOrdinal("ContractID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractGuid"))
                {
                    entity.ContractGuid = reader.GetGuid(reader.GetOrdinal("ContractGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseGuid"))
                {
                    entity.EnterpriseGuid = reader.GetGuid(reader.GetOrdinal("EnterpriseGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInfo"))
                {
                    entity.EnterpriseInfo = reader.GetString(reader.GetOrdinal("EnterpriseInfo"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractTitle"))
                {
                    entity.ContractTitle = reader.GetString(reader.GetOrdinal("ContractTitle"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractDetails"))
                {
                    entity.ContractDetails = reader.GetString(reader.GetOrdinal("ContractDetails"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractStartDate"))
                {
                    entity.ContractStartDate = reader.GetDateTime(reader.GetOrdinal("ContractStartDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractStopDate"))
                {
                    entity.ContractStopDate = reader.GetDateTime(reader.GetOrdinal("ContractStopDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractCreateDate"))
                {
                    entity.ContractCreateDate = reader.GetDateTime(reader.GetOrdinal("ContractCreateDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractCreateUserKey"))
                {
                    entity.ContractCreateUserKey = reader.GetString(reader.GetOrdinal("ContractCreateUserKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractLaborCount"))
                {
                    entity.ContractLaborCount = reader.GetInt32(reader.GetOrdinal("ContractLaborCount"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractLaborAddon"))
                {
                    entity.ContractLaborAddon = reader.GetString(reader.GetOrdinal("ContractLaborAddon"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContractStatus"))
                {
                    entity.ContractStatus = (Logics)reader.GetInt32(reader.GetOrdinal("ContractStatus"));
                }
            }
        }
        #endregion
    }
}
