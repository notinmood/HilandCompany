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
    public class SalaryDetailsCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<SalaryDetailsEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCSalaryDetails"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "SalaryDetailsGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "SalaryDetailsGuid"; }
        }

        //TODO:此存储过程尚未实现
        protected override string PagingSPName
        {
            get { return "usp_XQYC_SalaryDetails_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(SalaryDetailsEntity entity)
        {
            //在创建实体时如果实体的Guid尚未指定，那么给其赋初值
            if (entity.SalaryDetailsGuid == Guid.Empty)
            {
                entity.SalaryDetailsGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCSalaryDetails] (
			    [SalaryDetailsGuid],
			    [SalarySummaryKey],
			    [SalaryItemKey],
			    [SalaryItemValue],
			    [SalaryItemKind],
                [SalaryItemCashDate],
			    [PropertyNames],
			    [PropertyValues]
            ) 
            Values (
			    {0}SalaryDetailsGuid,
			    {0}SalarySummaryKey,
			    {0}SalaryItemKey,
			    {0}SalaryItemValue,
			    {0}SalaryItemKind,
                {0}SalaryItemCashDate,
			    {0}PropertyNames,
			    {0}PropertyValues
            )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(SalaryDetailsEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCSalaryDetails] Set   
				    [SalaryDetailsGuid] = {0}SalaryDetailsGuid,
				    [SalarySummaryKey] = {0}SalarySummaryKey,
				    [SalaryItemKey] = {0}SalaryItemKey,
				    [SalaryItemValue] = {0}SalaryItemValue,
				    [SalaryItemKind] = {0}SalaryItemKind,
                    [SalaryItemCashDate] = {0}SalaryItemCashDate,
				    [PropertyNames] = {0}PropertyNames,
				    [PropertyValues] = {0}PropertyValues
            Where [SalaryDetailsID] = {0}SalaryDetailsID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法


        protected override void InnerPrepareParasAll(SalaryDetailsEntity entity, ref List<TParameter> paraList)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("SalaryDetailsID",entity.SalaryDetailsID),
			    GenerateParameter("SalaryDetailsGuid",entity.SalaryDetailsGuid),
			    GenerateParameter("SalarySummaryKey",entity.SalarySummaryKey?? String.Empty),
			    GenerateParameter("SalaryItemKey",entity.SalaryItemKey?? String.Empty),
                GenerateParameter("SalaryItemCashDate",entity.SalaryItemCashDate),
			    GenerateParameter("SalaryItemValue",entity.SalaryItemValue),
			    GenerateParameter("SalaryItemKind",entity.SalaryItemKind)
            };

            paraList.AddRange(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        protected override void InnerLoad(IDataReader reader, ref SalaryDetailsEntity entity)
        {
            if (reader != null && reader.IsClosed == false && entity != null)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryDetailsID"))
                {
                    entity.SalaryDetailsID = reader.GetInt32(reader.GetOrdinal("SalaryDetailsID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryDetailsGuid"))
                {
                    entity.SalaryDetailsGuid = reader.GetGuid(reader.GetOrdinal("SalaryDetailsGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalarySummaryKey"))
                {
                    entity.SalarySummaryKey = reader.GetString(reader.GetOrdinal("SalarySummaryKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryItemKey"))
                {
                    entity.SalaryItemKey = reader.GetString(reader.GetOrdinal("SalaryItemKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryItemValue"))
                {
                    entity.SalaryItemValue = reader.GetDecimal(reader.GetOrdinal("SalaryItemValue"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryItemKind"))
                {
                    entity.SalaryItemKind = (SalaryItemKinds)reader.GetInt32(reader.GetOrdinal("SalaryItemKind"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "SalaryItemCashDate"))
                {
                    entity.SalaryItemCashDate = reader.GetDateTime(reader.GetOrdinal("SalaryItemCashDate"));
                }
            }
        }
        #endregion
    }
}
