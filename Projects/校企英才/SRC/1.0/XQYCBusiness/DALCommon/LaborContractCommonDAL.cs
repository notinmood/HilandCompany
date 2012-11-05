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
    public class LaborContractCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<LaborContractEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>, ILaborContractDAL
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCLaborContract"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "LaborContractGuid" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "LaborContractGuid"; }
        }

        //TODO:此存储过程尚未实现
        protected override string PagingSPName
        {
            get { return "usp_XQYC_LaborContract_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(LaborContractEntity entity)
        {
            //在创建实体时如果实体的Guid尚未指定，那么给其赋初值
            if (entity.LaborContractGuid == Guid.Empty)
            {
                entity.LaborContractGuid = GuidHelper.NewGuid();
            }

            string commandText = string.Format(@"Insert Into [XQYCLaborContract] (
			    [LaborContractGuid],
			    [LaborUserGuid],
			    [LaborCode],
			    [EnterpriseGuid],
			    [EnterpriseContractGuid],
			    [LaborContractStatus],
			    [LaborContractStartDate],
			    [LaborContractStopDate],
			    [LaborContractDetails],
			    [LaborContractDiscontinueDate],
			    [LaborContractDiscontinueDesc],
			    [LaborContractIsCurrent],
			    [EnterpriseInsuranceFormularKey],
			    [EnterpriseReserveFundFormularKey],
			    [EnterpriseManageFeeFormularKey],
			    [EnterpriseMixCostFormularKey],
			    [EnterpriseOtherCostFormularKey],
			    [PersonInsuranceFormularKey],
			    [PersonReserveFundFormularKey],
			    [PersonManageFeeFormularKey],
			    [PersonMixCostFormularKey],
			    [PersonOtherCostFormularKey],
			    [OperateUserGuid],
			    [OperateDate],
			    [PropertyNames],
			    [PropertyValues]
            ) 
            Values (
			    {0}LaborContractGuid,
			    {0}LaborUserGuid,
			    {0}LaborCode,
			    {0}EnterpriseGuid,
			    {0}EnterpriseContractGuid,
			    {0}LaborContractStatus,
			    {0}LaborContractStartDate,
			    {0}LaborContractStopDate,
			    {0}LaborContractDetails,
			    {0}LaborContractDiscontinueDate,
			    {0}LaborContractDiscontinueDesc,
			    {0}LaborContractIsCurrent,
			    {0}EnterpriseInsuranceFormularKey,
			    {0}EnterpriseReserveFundFormularKey,
			    {0}EnterpriseManageFeeFormularKey,
			    {0}EnterpriseMixCostFormularKey,
			    {0}EnterpriseOtherCostFormularKey,
			    {0}PersonInsuranceFormularKey,
			    {0}PersonReserveFundFormularKey,
			    {0}PersonManageFeeFormularKey,
			    {0}PersonMixCostFormularKey,
			    {0}PersonOtherCostFormularKey,
			    {0}OperateUserGuid,
			    {0}OperateDate,
			    {0}PropertyNames,
			    {0}PropertyValues
            )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(LaborContractEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCLaborContract] Set   
				    [LaborContractGuid] = {0}LaborContractGuid,
				    [LaborUserGuid] = {0}LaborUserGuid,
				    [LaborCode] = {0}LaborCode,
				    [EnterpriseGuid] = {0}EnterpriseGuid,
				    [EnterpriseContractGuid] = {0}EnterpriseContractGuid,
				    [LaborContractStatus] = {0}LaborContractStatus,
				    [LaborContractStartDate] = {0}LaborContractStartDate,
				    [LaborContractStopDate] = {0}LaborContractStopDate,
				    [LaborContractDetails] = {0}LaborContractDetails,
				    [LaborContractDiscontinueDate] = {0}LaborContractDiscontinueDate,
				    [LaborContractDiscontinueDesc] = {0}LaborContractDiscontinueDesc,
				    [LaborContractIsCurrent] = {0}LaborContractIsCurrent,
				    [EnterpriseInsuranceFormularKey] = {0}EnterpriseInsuranceFormularKey,
				    [EnterpriseReserveFundFormularKey] = {0}EnterpriseReserveFundFormularKey,
				    [EnterpriseManageFeeFormularKey] = {0}EnterpriseManageFeeFormularKey,
				    [EnterpriseMixCostFormularKey] = {0}EnterpriseMixCostFormularKey,
				    [EnterpriseOtherCostFormularKey] = {0}EnterpriseOtherCostFormularKey,
				    [PersonInsuranceFormularKey] = {0}PersonInsuranceFormularKey,
				    [PersonReserveFundFormularKey] = {0}PersonReserveFundFormularKey,
				    [PersonManageFeeFormularKey] = {0}PersonManageFeeFormularKey,
				    [PersonMixCostFormularKey] = {0}PersonMixCostFormularKey,
				    [PersonOtherCostFormularKey] = {0}PersonOtherCostFormularKey,
				    [OperateUserGuid] = {0}OperateUserGuid,
				    [OperateDate] = {0}OperateDate,
				    [PropertyNames] = {0}PropertyNames,
				    [PropertyValues] = {0}PropertyValues
            Where [LaborContractID] = {0}LaborContractID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        /// <summary>
        /// 移除劳务人员所有合同的当前状态
        /// </summary>
        /// <param name="laborGuid">劳务人员Guid</param>
        /// <param name="contractGuidExclude">取消当前状态时，需要排除在外的合同Guid</param>
        /// <returns></returns>
        public virtual void RemoveCurrentStatusOfLaborContract(Guid laborGuid, Guid contractGuidExclude)
        {
            string commandText = string.Format(@"Update [XQYCLaborContract] Set   
                    [LaborContractIsCurrent]={0}LaborContractIsCurrent
             Where 
                    [LaborUserGuid] = {0}LaborUserGuid AND 
                    [LaborContractGuid] != {0}LaborContractGuid", ParameterNamePrefix);

            TParameter[] sqlParas = new TParameter[]{
                GenerateParameter("LaborContractIsCurrent",Logics.False),
                GenerateParameter("LaborUserGuid",laborGuid),
                GenerateParameter("LaborContractGuid",contractGuidExclude)
            };

            HelperExInstance.ExecuteNonQuery(commandText, sqlParas);
        }
        #endregion

        #region 辅助方法


        protected override void InnerPrepareParasAll(LaborContractEntity entity, ref List<TParameter> paraList)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("LaborContractID",entity.LaborContractID),
			    GenerateParameter("LaborContractGuid",entity.LaborContractGuid),
			    GenerateParameter("LaborUserGuid",entity.LaborUserGuid),
			    GenerateParameter("LaborCode",entity.LaborCode?? String.Empty),
			    GenerateParameter("EnterpriseGuid",entity.EnterpriseGuid),
			    GenerateParameter("EnterpriseContractGuid",entity.EnterpriseContractGuid),
			    GenerateParameter("LaborContractStatus",entity.LaborContractStatus),
			    GenerateParameter("LaborContractStartDate",entity.LaborContractStartDate),
			    GenerateParameter("LaborContractStopDate",entity.LaborContractStopDate),
			    GenerateParameter("LaborContractDetails",entity.LaborContractDetails?? String.Empty),
			    GenerateParameter("LaborContractDiscontinueDate",entity.LaborContractDiscontinueDate),
			    GenerateParameter("LaborContractDiscontinueDesc",entity.LaborContractDiscontinueDesc?? String.Empty),
			    GenerateParameter("LaborContractIsCurrent",entity.LaborContractIsCurrent),
			    GenerateParameter("EnterpriseInsuranceFormularKey",entity.EnterpriseInsuranceFormularKey?? String.Empty),
			    GenerateParameter("EnterpriseReserveFundFormularKey",entity.EnterpriseReserveFundFormularKey?? String.Empty),
			    GenerateParameter("EnterpriseManageFeeFormularKey",entity.EnterpriseManageFeeFormularKey?? String.Empty),
			    GenerateParameter("EnterpriseMixCostFormularKey",entity.EnterpriseMixCostFormularKey?? String.Empty),
			    GenerateParameter("EnterpriseOtherCostFormularKey",entity.EnterpriseOtherCostFormularKey?? String.Empty),
			    GenerateParameter("PersonInsuranceFormularKey",entity.PersonInsuranceFormularKey?? String.Empty),
			    GenerateParameter("PersonReserveFundFormularKey",entity.PersonReserveFundFormularKey?? String.Empty),
			    GenerateParameter("PersonManageFeeFormularKey",entity.PersonManageFeeFormularKey?? String.Empty),
			    GenerateParameter("PersonMixCostFormularKey",entity.PersonMixCostFormularKey?? String.Empty),
			    GenerateParameter("PersonOtherCostFormularKey",entity.PersonOtherCostFormularKey?? String.Empty),
			    GenerateParameter("OperateUserGuid",entity.OperateUserGuid),
			    GenerateParameter("OperateDate",entity.OperateDate)
            };

            paraList.AddRange(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entity"></param>
        protected override void InnerLoad(IDataReader reader, ref LaborContractEntity entity)
        {
            if (reader != null && reader.IsClosed == false && entity != null)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractID"))
                {
                    entity.LaborContractID = reader.GetInt32(reader.GetOrdinal("LaborContractID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractGuid"))
                {
                    entity.LaborContractGuid = reader.GetGuid(reader.GetOrdinal("LaborContractGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborUserGuid"))
                {
                    entity.LaborUserGuid = reader.GetGuid(reader.GetOrdinal("LaborUserGuid"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborCode"))
                {
                    entity.LaborCode = reader.GetString(reader.GetOrdinal("LaborCode"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseGuid"))
                {
                    entity.EnterpriseGuid = reader.GetGuid(reader.GetOrdinal("EnterpriseGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseContractGuid"))
                {
                    entity.EnterpriseContractGuid = reader.GetGuid(reader.GetOrdinal("EnterpriseContractGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractStatus"))
                {
                    entity.LaborContractStatus = (LaborWorkStatuses)reader.GetInt32(reader.GetOrdinal("LaborContractStatus"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractIsCurrent"))
                {
                    entity.LaborContractIsCurrent = (Logics)reader.GetInt32(reader.GetOrdinal("LaborContractIsCurrent"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractStartDate"))
                {
                    entity.LaborContractStartDate = reader.GetDateTime(reader.GetOrdinal("LaborContractStartDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractStopDate"))
                {
                    entity.LaborContractStopDate = reader.GetDateTime(reader.GetOrdinal("LaborContractStopDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractDetails"))
                {
                    entity.LaborContractDetails = reader.GetString(reader.GetOrdinal("LaborContractDetails"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractDiscontinueDate"))
                {
                    entity.LaborContractDiscontinueDate = reader.GetDateTime(reader.GetOrdinal("LaborContractDiscontinueDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborContractDiscontinueDesc"))
                {
                    entity.LaborContractDiscontinueDesc = reader.GetString(reader.GetOrdinal("LaborContractDiscontinueDesc"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseInsuranceFormularKey"))
                {
                    entity.EnterpriseInsuranceFormularKey = reader.GetString(reader.GetOrdinal("EnterpriseInsuranceFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseReserveFundFormularKey"))
                {
                    entity.EnterpriseReserveFundFormularKey = reader.GetString(reader.GetOrdinal("EnterpriseReserveFundFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseManageFeeFormularKey"))
                {
                    entity.EnterpriseManageFeeFormularKey = reader.GetString(reader.GetOrdinal("EnterpriseManageFeeFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseMixCostFormularKey"))
                {
                    entity.EnterpriseMixCostFormularKey = reader.GetString(reader.GetOrdinal("EnterpriseMixCostFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EnterpriseOtherCostFormularKey"))
                {
                    entity.EnterpriseOtherCostFormularKey = reader.GetString(reader.GetOrdinal("EnterpriseOtherCostFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonInsuranceFormularKey"))
                {
                    entity.PersonInsuranceFormularKey = reader.GetString(reader.GetOrdinal("PersonInsuranceFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonReserveFundFormularKey"))
                {
                    entity.PersonReserveFundFormularKey = reader.GetString(reader.GetOrdinal("PersonReserveFundFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonManageFeeFormularKey"))
                {
                    entity.PersonManageFeeFormularKey = reader.GetString(reader.GetOrdinal("PersonManageFeeFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonMixCostFormularKey"))
                {
                    entity.PersonMixCostFormularKey = reader.GetString(reader.GetOrdinal("PersonMixCostFormularKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PersonOtherCostFormularKey"))
                {
                    entity.PersonOtherCostFormularKey = reader.GetString(reader.GetOrdinal("PersonOtherCostFormularKey"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "OperateUserGuid"))
                {
                    entity.OperateUserGuid = reader.GetGuid(reader.GetOrdinal("OperateUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "OperateDate"))
                {
                    entity.OperateDate = reader.GetDateTime(reader.GetOrdinal("OperateDate"));
                }
            }
        }
        #endregion
    }
}
