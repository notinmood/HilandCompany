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
using XQYC.Business.Enums;

namespace XQYC.Business.DALCommon
{
    public class EnterpriseJobCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<EnterpriseJobEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCEnterpriseJob"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "EnterpriseJobGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "EnterpriseJobGuid"; }
        }

        /// <summary>
        /// 分页存储过程
        /// </summary>
        protected override string PagingSPName
        {
            get { return "usp_XQYC_EnterpriseJob_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(EnterpriseJobEntity entity)
        {
            //在创建实体时如果实体的Guid尚未指定，那么给其赋初值
            if (entity.EnterpriseJobGuid == Guid.Empty)
            {
                entity.EnterpriseJobGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCEnterpriseJob] (
			    [EnterpriseJobGuid],
			    [EnterpriseJobTitle],
			    [EnterpriseKey],
			    [EnterpriseName],
                [EnterpriseAreaCode],
			    [EnterpriseAddress],
			    [EnterpriseContackInfo],
			    [EnterpriseDesc],
			    [EnterpriseJobLaborCount],
			    [EnterpriseJobDemand],
			    [EnterpriseJobTreadment],
			    [EnterpriseJobOther],
			    [EnterpriseJobDesc],
			    [EnterpriseJobStatus],
			    [EnterpriseJobType],
			    [EnterpriseJobStation],
			    [CreateTime],
			    [CreateUserKey],
			    [CanUsable],
			    [PropertyNames],
			    [PropertyValues]
            ) 
            Values (
			    {0}EnterpriseJobGuid,
			    {0}EnterpriseJobTitle,
			    {0}EnterpriseKey,
			    {0}EnterpriseName,
                {0}EnterpriseAreaCode,
			    {0}EnterpriseAddress,
			    {0}EnterpriseContackInfo,
			    {0}EnterpriseDesc,
			    {0}EnterpriseJobLaborCount,
			    {0}EnterpriseJobDemand,
			    {0}EnterpriseJobTreadment,
			    {0}EnterpriseJobOther,
			    {0}EnterpriseJobDesc,
			    {0}EnterpriseJobStatus,
			    {0}EnterpriseJobType,
			    {0}EnterpriseJobStation,
			    {0}CreateTime,
			    {0}CreateUserKey,
			    {0}CanUsable,
			    {0}PropertyNames,
			    {0}PropertyValues
            )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(EnterpriseJobEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCEnterpriseJob] Set   
				    [EnterpriseJobGuid] = {0}EnterpriseJobGuid,
				    [EnterpriseJobTitle] = {0}EnterpriseJobTitle,
				    [EnterpriseKey] = {0}EnterpriseKey,
				    [EnterpriseName] = {0}EnterpriseName,
                    [EnterpriseAreaCode] = {0}EnterpriseAreaCode,
				    [EnterpriseAddress] = {0}EnterpriseAddress,
				    [EnterpriseContackInfo] = {0}EnterpriseContackInfo,
				    [EnterpriseDesc] = {0}EnterpriseDesc,
				    [EnterpriseJobLaborCount] = {0}EnterpriseJobLaborCount,
				    [EnterpriseJobDemand] = {0}EnterpriseJobDemand,
				    [EnterpriseJobTreadment] = {0}EnterpriseJobTreadment,
				    [EnterpriseJobOther] = {0}EnterpriseJobOther,
				    [EnterpriseJobDesc] = {0}EnterpriseJobDesc,
				    [EnterpriseJobStatus] = {0}EnterpriseJobStatus,
				    [EnterpriseJobType] = {0}EnterpriseJobType,
				    [EnterpriseJobStation] = {0}EnterpriseJobStation,
				    [CreateTime] = {0}CreateTime,
				    [CreateUserKey] = {0}CreateUserKey,
				    [CanUsable] = {0}CanUsable,
				    [PropertyNames] = {0}PropertyNames,
				    [PropertyValues] = {0}PropertyValues
            Where [EnterpriseJobID] = {0}EnterpriseJobID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法


        protected override void InnerPrepareParasAll(EnterpriseJobEntity entity, ref List<TParameter> paraList)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("EnterpriseJobID",entity.EnterpriseJobID),
			    GenerateParameter("EnterpriseJobGuid",entity.EnterpriseJobGuid),
			    GenerateParameter("EnterpriseJobTitle",entity.EnterpriseJobTitle?? String.Empty),
			    GenerateParameter("EnterpriseKey",entity.EnterpriseKey?? String.Empty),
			    GenerateParameter("EnterpriseName",entity.EnterpriseName?? String.Empty),
			    GenerateParameter("EnterpriseAreaCode",entity.EnterpriseAreaCode?? String.Empty),
                GenerateParameter("EnterpriseAddress",entity.EnterpriseAddress?? String.Empty),
			    GenerateParameter("EnterpriseContackInfo",entity.EnterpriseContackInfo?? String.Empty),
			    GenerateParameter("EnterpriseDesc",entity.EnterpriseDesc?? String.Empty),
			    GenerateParameter("EnterpriseJobLaborCount",entity.EnterpriseJobLaborCount),
			    GenerateParameter("EnterpriseJobDemand",entity.EnterpriseJobDemand?? String.Empty),
			    GenerateParameter("EnterpriseJobTreadment",entity.EnterpriseJobTreadment?? String.Empty),
			    GenerateParameter("EnterpriseJobOther",entity.EnterpriseJobOther?? String.Empty),
			    GenerateParameter("EnterpriseJobDesc",entity.EnterpriseJobDesc?? String.Empty),
			    GenerateParameter("EnterpriseJobStatus",entity.EnterpriseJobStatus),
			    GenerateParameter("EnterpriseJobType",entity.EnterpriseJobType),
			    GenerateParameter("EnterpriseJobStation",entity.EnterpriseJobStation?? String.Empty),
			    GenerateParameter("CreateTime",entity.CreateTime),
			    GenerateParameter("CreateUserKey",entity.CreateUserKey?? String.Empty),
			    GenerateParameter("CanUsable",entity.CanUsable)
            };

            paraList.AddRange(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        protected override void InnerLoad(IDataReader reader, ref EnterpriseJobEntity entity)
        {
            if (reader != null && reader.IsClosed == false && entity != null)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobID"))
                {
                    entity.EnterpriseJobID = reader.GetInt32(reader.GetOrdinal("EnterpriseJobID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobGuid"))
                {
                    entity.EnterpriseJobGuid = reader.GetGuid(reader.GetOrdinal("EnterpriseJobGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobTitle"))
                {
                    entity.EnterpriseJobTitle = reader.GetString(reader.GetOrdinal("EnterpriseJobTitle"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseKey"))
                {
                    entity.EnterpriseKey = reader.GetString(reader.GetOrdinal("EnterpriseKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseName"))
                {
                    entity.EnterpriseName = reader.GetString(reader.GetOrdinal("EnterpriseName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseAreaCode"))
                {
                    entity.EnterpriseAreaCode = reader.GetString(reader.GetOrdinal("EnterpriseAreaCode"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseAddress"))
                {
                    entity.EnterpriseAddress = reader.GetString(reader.GetOrdinal("EnterpriseAddress"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseContackInfo"))
                {
                    entity.EnterpriseContackInfo = reader.GetString(reader.GetOrdinal("EnterpriseContackInfo"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseDesc"))
                {
                    entity.EnterpriseDesc = reader.GetString(reader.GetOrdinal("EnterpriseDesc"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobLaborCount"))
                {
                    entity.EnterpriseJobLaborCount = reader.GetInt32(reader.GetOrdinal("EnterpriseJobLaborCount"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobDemand"))
                {
                    entity.EnterpriseJobDemand = reader.GetString(reader.GetOrdinal("EnterpriseJobDemand"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobTreadment"))
                {
                    entity.EnterpriseJobTreadment = reader.GetString(reader.GetOrdinal("EnterpriseJobTreadment"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobOther"))
                {
                    entity.EnterpriseJobOther = reader.GetString(reader.GetOrdinal("EnterpriseJobOther"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobDesc"))
                {
                    entity.EnterpriseJobDesc = reader.GetString(reader.GetOrdinal("EnterpriseJobDesc"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobStatus"))
                {
                    entity.EnterpriseJobStatus = reader.GetInt32(reader.GetOrdinal("EnterpriseJobStatus"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobType"))
                {
                    entity.EnterpriseJobType = reader.GetInt32(reader.GetOrdinal("EnterpriseJobType"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseJobStation"))
                {
                    entity.EnterpriseJobStation = reader.GetString(reader.GetOrdinal("EnterpriseJobStation"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CreateTime"))
                {
                    entity.CreateTime = reader.GetDateTime(reader.GetOrdinal("CreateTime"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CreateUserKey"))
                {
                    entity.CreateUserKey = reader.GetString(reader.GetOrdinal("CreateUserKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CanUsable"))
                {
                    entity.CanUsable = (Logics)reader.GetInt32(reader.GetOrdinal("CanUsable"));
                }
            }
        }
        #endregion
    }
}
