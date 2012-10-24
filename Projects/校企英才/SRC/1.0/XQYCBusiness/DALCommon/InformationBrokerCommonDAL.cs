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
            get { return new string[] { "UserGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "UserGuid"; }
        }

        protected override string PagingSPName
        {
            get { return "usp_XQYC_InformationBroker_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(InformationBrokerEntity entity)
        {
            string commandText = string.Format(@"Insert Into [XQYCInformationBroker] (
			        [UserGuid],
                    [InformationBrokerStatus],
			        [ProviderUserGuid],
			        [ProviderUserName],
			        [RecommendUserGuid],
			        [RecommendUserName],
			        [ServiceUserGuid],
			        [ServiceUserName],
			        [FinanceUserGuid],
			        [FinanceUserName]
                ) 
                Values (
			        {0}UserGuid,
                    {0}InformationBrokerStatus,
			        {0}ProviderUserGuid,
			        {0}ProviderUserName,
			        {0}RecommendUserGuid,
			        {0}RecommendUserName,
			        {0}ServiceUserGuid,
			        {0}ServiceUserName,
			        {0}FinanceUserGuid,
			        {0}FinanceUserName
                 )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(InformationBrokerEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCInformationBroker] Set   
					[UserGuid] = {0}UserGuid,
                    [InformationBrokerStatus]= {0}InformationBrokerStatus,
					[ProviderUserGuid] = {0}ProviderUserGuid,
					[ProviderUserName] = {0}ProviderUserName,
					[RecommendUserGuid] = {0}RecommendUserGuid,
					[RecommendUserName] = {0}RecommendUserName,
					[ServiceUserGuid] = {0}ServiceUserGuid,
					[ServiceUserName] = {0}ServiceUserName,
					[FinanceUserGuid] = {0}FinanceUserGuid,
					[FinanceUserName] = {0}FinanceUserName
             Where [UserGuid] = {0}UserGuid", ParameterNamePrefix);

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
                GenerateParameter("UserGuid",entity.UserGuid),
                GenerateParameter("InformationBrokerStatus",entity.InformationBrokerStatus),
			    GenerateParameter("ProviderUserGuid",entity.ProviderUserGuid),
			    GenerateParameter("ProviderUserName",entity.ProviderUserName?? string.Empty),
			    GenerateParameter("RecommendUserGuid",entity.RecommendUserGuid),
			    GenerateParameter("RecommendUserName",entity.RecommendUserName?? string.Empty),
			    GenerateParameter("ServiceUserGuid",entity.ServiceUserGuid),
			    GenerateParameter("ServiceUserName",entity.ServiceUserName?? string.Empty),
			    GenerateParameter("FinanceUserGuid",entity.FinanceUserGuid),
			    GenerateParameter("FinanceUserName",entity.FinanceUserName?? string.Empty)
            };

            return list.ToArray();
        }


        protected override InformationBrokerEntity Load(IDataReader reader)
        {
            InformationBrokerEntity entity = null;
            if (reader != null && reader.IsClosed == false)
            {
                BusinessUser businessUser = BusinessUserCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>.Load((TDataReader)reader);
                entity = Converter.InheritedEntityConvert<BusinessUser, InformationBrokerEntity>(businessUser);

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerID"))
                {
                    entity.InformationBrokerID = reader.GetInt32(reader.GetOrdinal("InformationBrokerID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "UserGuid"))
                {
                    entity.UserGuid = reader.GetGuid(reader.GetOrdinal("UserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerStatus"))
                {
                    entity.InformationBrokerStatus = (UserStatuses)reader.GetInt32(reader.GetOrdinal("InformationBrokerStatus"));
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
            }

            return entity;
        }
        #endregion
    }
}
