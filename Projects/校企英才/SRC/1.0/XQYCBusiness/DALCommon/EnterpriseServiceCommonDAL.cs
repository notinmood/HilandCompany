using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.DALCommon;
using HiLand.Framework.FoundationLayer;
using HiLand.General.DALCommon;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;
using HiLand.Utility.Enums;
using XQYC.Business.Entity;

namespace XQYC.Business.DALCommon
{
    public class EnterpriseServiceCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<EnterpriseServiceEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCEnterpriseService"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "EnterpriseServiceGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "EnterpriseServiceGuid"; }
        }

        /// <summary>
        /// 分页存储过程
        /// </summary>
        protected override string PagingSPName
        {
            get { return "usp_XQYC_EnterpriseService_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(EnterpriseServiceEntity entity)
        {
            //在创建实体时如果实体的Guid尚未指定，那么给其赋初值
            if (entity.EnterpriseServiceGuid == Guid.Empty)
            {
                entity.EnterpriseServiceGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCEnterpriseService] (
			    [EnterpriseServiceGuid],
			    [EnterpriseGuid],
			    [EnterpriseInfo],
			    [EnterpriseServiceType],
			    [EnterpriseServiceStatus],
                [EnterpriseServiceContactPerson],
			    [EnterpriseServiceContactTelephone],
			    [EnterpriseServiceCreateDate],
			    [EnterpriseServiceCreateUserKey],
                [EnterpriseServiceStartDate],
                [EnterpriseServiceStopDate],
			    [ProviderUserGuid],
			    [ProviderUserName],
			    [RecommendUserGuid],
			    [RecommendUserName],
			    [ServiceUserGuid],
			    [ServiceUserName],
			    [FinanceUserGuid],
			    [FinanceUserName],
			    [BusinessUserGuid],
			    [BusinessUserName],
			    [SettleUserGuid],
			    [SettleUserName],
			    [PropertyNames],
			    [PropertyValues]
            ) 
            Values (
			    {0}EnterpriseServiceGuid,
			    {0}EnterpriseGuid,
			    {0}EnterpriseInfo,
			    {0}EnterpriseServiceType,
			    {0}EnterpriseServiceStatus,
                {0}EnterpriseServiceContactPerson,
			    {0}EnterpriseServiceContactTelephone,
			    {0}EnterpriseServiceCreateDate,
			    {0}EnterpriseServiceCreateUserKey,
                {0}EnterpriseServiceStartDate,
                {0}EnterpriseServiceStopDate,
			    {0}ProviderUserGuid,
			    {0}ProviderUserName,
			    {0}RecommendUserGuid,
			    {0}RecommendUserName,
			    {0}ServiceUserGuid,
			    {0}ServiceUserName,
			    {0}FinanceUserGuid,
			    {0}FinanceUserName,
			    {0}BusinessUserGuid,
			    {0}BusinessUserName,
			    {0}SettleUserGuid,
			    {0}SettleUserName,
			    {0}PropertyNames,
			    {0}PropertyValues
            )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(EnterpriseServiceEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCEnterpriseService] Set   
					[EnterpriseServiceGuid] = {0}EnterpriseServiceGuid,
					[EnterpriseGuid] = {0}EnterpriseGuid,
					[EnterpriseInfo] = {0}EnterpriseInfo,
					[EnterpriseServiceType] = {0}EnterpriseServiceType,
					[EnterpriseServiceStatus] = {0}EnterpriseServiceStatus,
                    [EnterpriseServiceContactPerson] = {0}EnterpriseServiceContactPerson,
				    [EnterpriseServiceContactTelephone] = {0}EnterpriseServiceContactTelephone,
					[EnterpriseServiceCreateDate] = {0}EnterpriseServiceCreateDate,
					[EnterpriseServiceCreateUserKey] = {0}EnterpriseServiceCreateUserKey,
                    [EnterpriseServiceStartDate] = {0}EnterpriseServiceStartDate,
                    [EnterpriseServiceStopDate] = {0}EnterpriseServiceStopDate,
					[ProviderUserGuid] = {0}ProviderUserGuid,
					[ProviderUserName] = {0}ProviderUserName,
					[RecommendUserGuid] = {0}RecommendUserGuid,
					[RecommendUserName] = {0}RecommendUserName,
					[ServiceUserGuid] = {0}ServiceUserGuid,
					[ServiceUserName] = {0}ServiceUserName,
					[FinanceUserGuid] = {0}FinanceUserGuid,
					[FinanceUserName] = {0}FinanceUserName,
                    [BusinessUserGuid] = {0}BusinessUserGuid,
				    [BusinessUserName] = {0}BusinessUserName,
                    [SettleUserGuid] = {0}SettleUserGuid,
				    [SettleUserName] = {0}SettleUserName,
					[PropertyNames] = {0}PropertyNames,
					[PropertyValues] = {0}PropertyValues
             Where [EnterpriseServiceID] = {0}EnterpriseServiceID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法


        protected override void InnerPrepareParasAll(EnterpriseServiceEntity entity, ref List<TParameter> paraList)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("EnterpriseServiceID",entity.EnterpriseServiceID),
			    GenerateParameter("EnterpriseServiceGuid",entity.EnterpriseServiceGuid),
			    GenerateParameter("EnterpriseGuid",entity.EnterpriseGuid),
			    GenerateParameter("EnterpriseInfo",entity.EnterpriseInfo?? String.Empty),
			    GenerateParameter("EnterpriseServiceType",entity.EnterpriseServiceType),
			    GenerateParameter("EnterpriseServiceStatus",(int)entity.EnterpriseServiceStatus),
                GenerateParameter("EnterpriseServiceContactPerson",entity.EnterpriseServiceContactPerson?? String.Empty),
			    GenerateParameter("EnterpriseServiceContactTelephone",entity.EnterpriseServiceContactTelephone?? String.Empty),
			    GenerateParameter("EnterpriseServiceCreateDate",entity.EnterpriseServiceCreateDate),
			    GenerateParameter("EnterpriseServiceCreateUserKey",entity.EnterpriseServiceCreateUserKey?? String.Empty),
                GenerateParameter("EnterpriseServiceStartDate",entity.EnterpriseServiceStartDate),
                GenerateParameter("EnterpriseServiceStopDate",entity.EnterpriseServiceStopDate),
			    GenerateParameter("ProviderUserGuid",entity.ProviderUserGuid),
			    GenerateParameter("ProviderUserName",entity.ProviderUserName?? String.Empty),
			    GenerateParameter("RecommendUserGuid",entity.RecommendUserGuid),
			    GenerateParameter("RecommendUserName",entity.RecommendUserName?? String.Empty),
			    GenerateParameter("ServiceUserGuid",entity.ServiceUserGuid),
			    GenerateParameter("ServiceUserName",entity.ServiceUserName?? String.Empty),
                GenerateParameter("BusinessUserGuid",entity.BusinessUserGuid),
			    GenerateParameter("BusinessUserName",entity.BusinessUserName?? String.Empty),
			    GenerateParameter("SettleUserGuid",entity.SettleUserGuid),
			    GenerateParameter("SettleUserName",entity.SettleUserName?? String.Empty),
                GenerateParameter("FinanceUserGuid",entity.FinanceUserGuid),
			    GenerateParameter("FinanceUserName",entity.FinanceUserName?? String.Empty)
            };

            paraList.AddRange(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        protected override void InnerLoad(IDataReader reader, ref EnterpriseServiceEntity entity)
        {
            if (reader != null && reader.IsClosed == false && entity != null)
            {
                EnterpriseEntity enterpriserEntity = EnterpriseCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>.StaticLoad((TDataReader)reader);
                entity = Converter.InheritedEntityConvert<EnterpriseEntity, EnterpriseServiceEntity>(enterpriserEntity);

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceID"))
                {
                    entity.EnterpriseServiceID = reader.GetInt32(reader.GetOrdinal("EnterpriseServiceID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceGuid"))
                {
                    entity.EnterpriseServiceGuid = reader.GetGuid(reader.GetOrdinal("EnterpriseServiceGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseGuid"))
                {
                    entity.EnterpriseGuid = reader.GetGuid(reader.GetOrdinal("EnterpriseGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInfo"))
                {
                    entity.EnterpriseInfo = reader.GetString(reader.GetOrdinal("EnterpriseInfo"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceType"))
                {
                    entity.EnterpriseServiceType = reader.GetInt32(reader.GetOrdinal("EnterpriseServiceType"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceStatus"))
                {
                    entity.EnterpriseServiceStatus = (Logics)reader.GetInt32(reader.GetOrdinal("EnterpriseServiceStatus"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceContactPerson"))
                {
                    entity.EnterpriseServiceContactPerson = reader.GetString(reader.GetOrdinal("EnterpriseServiceContactPerson"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceContactTelephone"))
                {
                    entity.EnterpriseServiceContactTelephone = reader.GetString(reader.GetOrdinal("EnterpriseServiceContactTelephone"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceCreateDate"))
                {
                    entity.EnterpriseServiceCreateDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseServiceCreateDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceCreateUserKey"))
                {
                    entity.EnterpriseServiceCreateUserKey = reader.GetString(reader.GetOrdinal("EnterpriseServiceCreateUserKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceStartDate"))
                {
                    entity.EnterpriseServiceStartDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseServiceStartDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseServiceStopDate"))
                {
                    entity.EnterpriseServiceStopDate = reader.GetDateTime(reader.GetOrdinal("EnterpriseServiceStopDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ProviderUserGuid"))
                {
                    entity.ProviderUserGuid = reader.GetGuid(reader.GetOrdinal("ProviderUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ProviderUserName"))
                {
                    entity.ProviderUserName = reader.GetString(reader.GetOrdinal("ProviderUserName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "RecommendUserGuid"))
                {
                    entity.RecommendUserGuid = reader.GetGuid(reader.GetOrdinal("RecommendUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "RecommendUserName"))
                {
                    entity.RecommendUserName = reader.GetString(reader.GetOrdinal("RecommendUserName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ServiceUserGuid"))
                {
                    entity.ServiceUserGuid = reader.GetGuid(reader.GetOrdinal("ServiceUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ServiceUserName"))
                {
                    entity.ServiceUserName = reader.GetString(reader.GetOrdinal("ServiceUserName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "FinanceUserGuid"))
                {
                    entity.FinanceUserGuid = reader.GetGuid(reader.GetOrdinal("FinanceUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "FinanceUserName"))
                {
                    entity.FinanceUserName = reader.GetString(reader.GetOrdinal("FinanceUserName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "BusinessUserGuid"))
                {
                    entity.BusinessUserGuid = reader.GetGuid(reader.GetOrdinal("BusinessUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "BusinessUserName"))
                {
                    entity.BusinessUserName = reader.GetString(reader.GetOrdinal("BusinessUserName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SettleUserGuid"))
                {
                    entity.SettleUserGuid = reader.GetGuid(reader.GetOrdinal("SettleUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SettleUserName"))
                {
                    entity.SettleUserName = reader.GetString(reader.GetOrdinal("SettleUserName"));
                }
            }
        }
        #endregion
    }
}
