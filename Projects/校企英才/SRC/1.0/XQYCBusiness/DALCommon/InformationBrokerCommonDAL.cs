using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.DALCommon;
using HiLand.Framework.FoundationLayer;
using HiLand.General;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;
using HiLand.Utility.Enums;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.DALCommon
{
    public class InformationBrokerCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<InformationBrokerEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCInformationBroker"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "InformationBrokerGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "InformationBrokerGuid"; }
        }

        protected override string PagingSPName
        {
            get { return "usp_XQYC_InformationBroker_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(InformationBrokerEntity entity)
        {
            if (entity.InformationBrokerGuid == Guid.Empty)
            {
                entity.InformationBrokerGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCInformationBroker] (
			    [InformationBrokerGuid],
			    [InformationBrokerName],
			    [InformationBrokerNameShort],
			    [CanUsable],
			    [AreaCode],
			    [IndustryKey],
			    [IndustryType],
			    [InformationBrokerType],
			    [InformationBrokerKind],
			    [PrincipleAddress],
			    [ContactPerson],
			    [PostCode],
			    [Telephone],
			    [Fax],
			    [Email],
			    [InformationBrokerWWW],
			    [InformationBrokerLevel],
			    [InformationBrokerRank],
			    [InformationBrokerDescription],
			    [InformationBrokerMemo],
			    [ProviderUserGuid],
			    [ProviderUserName],
			    [RecommendUserGuid],
			    [RecommendUserName],
			    [ServiceUserGuid],
			    [ServiceUserName],
			    [FinanceUserGuid],
			    [FinanceUserName],
			    [CreateUserKey],
			    [CreateDate],
			    [PropertyNames],
			    [PropertyValues]
            ) 
            Values (
			    {0}InformationBrokerGuid,
			    {0}InformationBrokerName,
			    {0}InformationBrokerNameShort,
			    {0}CanUsable,
			    {0}AreaCode,
			    {0}IndustryKey,
			    {0}IndustryType,
			    {0}InformationBrokerType,
			    {0}InformationBrokerKind,
			    {0}PrincipleAddress,
			    {0}ContactPerson,
			    {0}PostCode,
			    {0}Telephone,
			    {0}Fax,
			    {0}Email,
			    {0}InformationBrokerWWW,
			    {0}InformationBrokerLevel,
			    {0}InformationBrokerRank,
			    {0}InformationBrokerDescription,
			    {0}InformationBrokerMemo,
			    {0}ProviderUserGuid,
			    {0}ProviderUserName,
			    {0}RecommendUserGuid,
			    {0}RecommendUserName,
			    {0}ServiceUserGuid,
			    {0}ServiceUserName,
			    {0}FinanceUserGuid,
			    {0}FinanceUserName,
			    {0}CreateUserKey,
			    {0}CreateDate,
			    {0}PropertyNames,
			    {0}PropertyValues
            )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(InformationBrokerEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCInformationBroker] Set   
				    [InformationBrokerGuid] = {0}InformationBrokerGuid,
				    [InformationBrokerName] = {0}InformationBrokerName,
				    [InformationBrokerNameShort] = {0}InformationBrokerNameShort,
				    [CanUsable] = {0}CanUsable,
				    [AreaCode] = {0}AreaCode,
				    [IndustryKey] = {0}IndustryKey,
				    [IndustryType] = {0}IndustryType,
				    [InformationBrokerType] = {0}InformationBrokerType,
				    [InformationBrokerKind] = {0}InformationBrokerKind,
				    [PrincipleAddress] = {0}PrincipleAddress,
				    [ContactPerson] = {0}ContactPerson,
				    [PostCode] = {0}PostCode,
				    [Telephone] = {0}Telephone,
				    [Fax] = {0}Fax,
				    [Email] = {0}Email,
				    [InformationBrokerWWW] = {0}InformationBrokerWWW,
				    [InformationBrokerLevel] = {0}InformationBrokerLevel,
				    [InformationBrokerRank] = {0}InformationBrokerRank,
				    [InformationBrokerDescription] = {0}InformationBrokerDescription,
				    [InformationBrokerMemo] = {0}InformationBrokerMemo,
				    [ProviderUserGuid] = {0}ProviderUserGuid,
				    [ProviderUserName] = {0}ProviderUserName,
				    [RecommendUserGuid] = {0}RecommendUserGuid,
				    [RecommendUserName] = {0}RecommendUserName,
				    [ServiceUserGuid] = {0}ServiceUserGuid,
				    [ServiceUserName] = {0}ServiceUserName,
				    [FinanceUserGuid] = {0}FinanceUserGuid,
				    [FinanceUserName] = {0}FinanceUserName,
				    [CreateUserKey] = {0}CreateUserKey,
				    [CreateDate] = {0}CreateDate,
				    [PropertyNames] = {0}PropertyNames,
				    [PropertyValues] = {0}PropertyValues
            Where [InformationBrokerID] = {0}InformationBrokerID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法

        protected override TParameter[] PrepareParasAll(InformationBrokerEntity entity)
        {
            List<TParameter> list = new List<TParameter>()
            {
                    GenerateParameter("InformationBrokerID",entity.InformationBrokerID),
			        GenerateParameter("InformationBrokerGuid",entity.InformationBrokerGuid),
			        GenerateParameter("InformationBrokerName",entity.InformationBrokerName?? String.Empty),
			        GenerateParameter("InformationBrokerNameShort",entity.InformationBrokerNameShort?? String.Empty),
			        GenerateParameter("CanUsable",entity.CanUsable),
			        GenerateParameter("AreaCode",entity.AreaCode?? String.Empty),
			        GenerateParameter("IndustryKey",entity.IndustryKey?? String.Empty),
			        GenerateParameter("IndustryType",entity.IndustryType),
			        GenerateParameter("InformationBrokerType",entity.InformationBrokerType),
			        GenerateParameter("InformationBrokerKind",entity.InformationBrokerKind?? String.Empty),
			        GenerateParameter("PrincipleAddress",entity.PrincipleAddress?? String.Empty),
			        GenerateParameter("ContactPerson",entity.ContactPerson?? String.Empty),
			        GenerateParameter("PostCode",entity.PostCode?? String.Empty),
			        GenerateParameter("Telephone",entity.Telephone?? String.Empty),
			        GenerateParameter("Fax",entity.Fax?? String.Empty),
			        GenerateParameter("Email",entity.Email?? String.Empty),
			        GenerateParameter("InformationBrokerWWW",entity.InformationBrokerWWW?? String.Empty),
			        GenerateParameter("InformationBrokerLevel",entity.InformationBrokerLevel),
			        GenerateParameter("InformationBrokerRank",entity.InformationBrokerRank?? String.Empty),
			        GenerateParameter("InformationBrokerDescription",entity.InformationBrokerDescription?? String.Empty),
			        GenerateParameter("InformationBrokerMemo",entity.InformationBrokerMemo?? String.Empty),
			        GenerateParameter("ProviderUserGuid",entity.ProviderUserGuid),
			        GenerateParameter("ProviderUserName",entity.ProviderUserName?? String.Empty),
			        GenerateParameter("RecommendUserGuid",entity.RecommendUserGuid),
			        GenerateParameter("RecommendUserName",entity.RecommendUserName?? String.Empty),
			        GenerateParameter("ServiceUserGuid",entity.ServiceUserGuid),
			        GenerateParameter("ServiceUserName",entity.ServiceUserName?? String.Empty),
			        GenerateParameter("FinanceUserGuid",entity.FinanceUserGuid),
			        GenerateParameter("FinanceUserName",entity.FinanceUserName?? String.Empty),
			        GenerateParameter("CreateUserKey",entity.CreateUserKey?? String.Empty),
			        GenerateParameter("CreateDate",entity.CreateDate),
			        GenerateParameter("PropertyNames",entity.PropertyNames?? String.Empty),
			        GenerateParameter("PropertyValues",entity.PropertyValues?? String.Empty)
            };

            return list.ToArray();
        }


        protected override InformationBrokerEntity Load(IDataReader reader)
        {
            InformationBrokerEntity entity = new InformationBrokerEntity();
            if (reader != null && reader.IsClosed == false)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerID"))
                {
                    entity.InformationBrokerID = reader.GetInt32(reader.GetOrdinal("InformationBrokerID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerGuid"))
                {
                    entity.InformationBrokerGuid = reader.GetGuid(reader.GetOrdinal("InformationBrokerGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerName"))
                {
                    entity.InformationBrokerName = reader.GetString(reader.GetOrdinal("InformationBrokerName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerNameShort"))
                {
                    entity.InformationBrokerNameShort = reader.GetString(reader.GetOrdinal("InformationBrokerNameShort"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CanUsable"))
                {
                    entity.CanUsable = (Logics)reader.GetInt32(reader.GetOrdinal("CanUsable"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "AreaCode"))
                {
                    entity.AreaCode = reader.GetString(reader.GetOrdinal("AreaCode"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IndustryKey"))
                {
                    entity.IndustryKey = reader.GetString(reader.GetOrdinal("IndustryKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IndustryType"))
                {
                    entity.IndustryType = (IndustryTypes)reader.GetInt32(reader.GetOrdinal("IndustryType"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerType"))
                {
                    entity.InformationBrokerType = (InformationBrokerTypes)reader.GetInt32(reader.GetOrdinal("InformationBrokerType"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerKind"))
                {
                    entity.InformationBrokerKind = reader.GetString(reader.GetOrdinal("InformationBrokerKind"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PrincipleAddress"))
                {
                    entity.PrincipleAddress = reader.GetString(reader.GetOrdinal("PrincipleAddress"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ContactPerson"))
                {
                    entity.ContactPerson = reader.GetString(reader.GetOrdinal("ContactPerson"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PostCode"))
                {
                    entity.PostCode = reader.GetString(reader.GetOrdinal("PostCode"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Telephone"))
                {
                    entity.Telephone = reader.GetString(reader.GetOrdinal("Telephone"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Fax"))
                {
                    entity.Fax = reader.GetString(reader.GetOrdinal("Fax"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Email"))
                {
                    entity.Email = reader.GetString(reader.GetOrdinal("Email"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerWWW"))
                {
                    entity.InformationBrokerWWW = reader.GetString(reader.GetOrdinal("InformationBrokerWWW"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerLevel"))
                {
                    entity.InformationBrokerLevel = reader.GetInt32(reader.GetOrdinal("InformationBrokerLevel"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerRank"))
                {
                    entity.InformationBrokerRank = reader.GetString(reader.GetOrdinal("InformationBrokerRank"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerDescription"))
                {
                    entity.InformationBrokerDescription = reader.GetString(reader.GetOrdinal("InformationBrokerDescription"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerMemo"))
                {
                    entity.InformationBrokerMemo = reader.GetString(reader.GetOrdinal("InformationBrokerMemo"));
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
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CreateUserKey"))
                {
                    entity.CreateUserKey = reader.GetString(reader.GetOrdinal("CreateUserKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CreateDate"))
                {
                    entity.CreateDate = reader.GetDateTime(reader.GetOrdinal("CreateDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PropertyNames"))
                {
                    entity.PropertyNames = reader.GetString(reader.GetOrdinal("PropertyNames"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PropertyValues"))
                {
                    entity.PropertyValues = reader.GetString(reader.GetOrdinal("PropertyValues"));
                }
            }

            return entity;
        }
        #endregion
    }
}
