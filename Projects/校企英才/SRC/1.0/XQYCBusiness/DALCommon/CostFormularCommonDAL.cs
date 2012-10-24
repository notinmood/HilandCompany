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
    public class InsuranceFundCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<CostFormularEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCCostFormular"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "CostFormularGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "CostFormularGuid"; }
        }

        /// <summary>
        /// 分页存储过程（未实现）
        /// </summary>
        protected override string PagingSPName
        {
            get { return "usp_XQYC_CostFormular_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(CostFormularEntity entity)
        {
            //在创建实体时如果实体的Guid尚未指定，那么给其赋初值
            if (entity.CostFormularGuid == Guid.Empty)
            {
                entity.CostFormularGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCCostFormular] (
			        [CostFormularGuid],
			        [CostFormularName],
			        [CostFormularValue],
			        [EnterpriseKey],
			        [CostType],
			        [CostKind],
			        [ReferanceGuid],
			        [CostFormularDesc],
			        [CanUsable],
			        [PropertyNames],
			        [PropertyValues]
                ) 
                Values (
			        {0}CostFormularGuid,
			        {0}CostFormularName,
			        {0}CostFormularValue,
			        {0}EnterpriseKey,
			        {0}CostType,
			        {0}CostKind,
			        {0}ReferanceGuid,
			        {0}CostFormularDesc,
			        {0}CanUsable,
			        {0}PropertyNames,
			        {0}PropertyValues
                )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(CostFormularEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCCostFormular] Set   
				    [CostFormularGuid] = {0}CostFormularGuid,
				    [CostFormularName] = {0}CostFormularName,
				    [CostFormularValue] = {0}CostFormularValue,
				    [EnterpriseKey] = {0}EnterpriseKey,
				    [CostType] = {0}CostType,
				    [CostKind] = {0}CostKind,
				    [ReferanceGuid] = {0}ReferanceGuid,
				    [CostFormularDesc] = {0}CostFormularDesc,
				    [CanUsable] = {0}CanUsable,
				    [PropertyNames] = {0}PropertyNames,
				    [PropertyValues] = {0}PropertyValues
            Where [CostFormularID] = {0}CostFormularID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法


        protected override void InnerPrepareParasAll(CostFormularEntity entity, ref List<TParameter> paraList)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("CostFormularID",entity.CostFormularID),
			    GenerateParameter("CostFormularGuid",entity.CostFormularGuid),
			    GenerateParameter("CostFormularName",entity.CostFormularName?? String.Empty),
			    GenerateParameter("CostFormularValue",entity.CostFormularValue?? String.Empty),
			    GenerateParameter("EnterpriseKey",entity.EnterpriseKey?? String.Empty),
			    GenerateParameter("CostType",entity.CostType),
			    GenerateParameter("CostKind",entity.CostKind),
			    GenerateParameter("ReferanceGuid",entity.ReferanceGuid),
			    GenerateParameter("CostFormularDesc",entity.CostFormularDesc?? String.Empty),
			    GenerateParameter("CanUsable",entity.CanUsable)
            };

            paraList.AddRange(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        protected override void InnerLoad(IDataReader reader, ref CostFormularEntity entity)
        {
            if (reader != null && reader.IsClosed == false && entity != null)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CostFormularID"))
                {
                    entity.CostFormularID = reader.GetInt32(reader.GetOrdinal("CostFormularID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CostFormularGuid"))
                {
                    entity.CostFormularGuid = reader.GetGuid(reader.GetOrdinal("CostFormularGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CostFormularName"))
                {
                    entity.CostFormularName = reader.GetString(reader.GetOrdinal("CostFormularName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CostFormularValue"))
                {
                    entity.CostFormularValue = reader.GetString(reader.GetOrdinal("CostFormularValue"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseKey"))
                {
                    entity.EnterpriseKey = reader.GetString(reader.GetOrdinal("EnterpriseKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CostType"))
                {
                    entity.CostType = reader.GetInt32(reader.GetOrdinal("CostType"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CostKind"))
                {
                    entity.CostKind = (CostKinds)reader.GetInt32(reader.GetOrdinal("CostKind"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ReferanceGuid"))
                {
                    entity.ReferanceGuid = reader.GetGuid(reader.GetOrdinal("ReferanceGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CostFormularDesc"))
                {
                    entity.CostFormularDesc = reader.GetString(reader.GetOrdinal("CostFormularDesc"));
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
