using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.DALCommon;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.DALCommon
{
    public class LaborCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<LaborEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCLabor"; }
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
            get { return "usp_XQYC_Labor_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(LaborEntity entity)
        {
            string commandText = string.Format(@"Insert Into [XQYCLabor] (
			        [UserGuid],
                    [LaborCode],
			        [NativePlace],
			        [CurrentPlace],
			        [IDCardPlace],
			        [HouseHoldType],
			        [WorkSkill],
			        [WorkSkillPaper],
			        [WorkSituation],
			        [PreWorkSituation],
			        [HopeWorkSituation],
			        [HopeWorkSalary],
			        [UrgentLinkMan],
			        [UrgentTelephone],
			        [UrgentRelationship],
			        [InformationComeFrom],
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
                    [InformationBrokerUserGuid],
			        [InformationBrokerUserName],
			        [InsureType],
                    [LaborWorkStatus],
			        [CurrentLaborDepartment],
			        [CurrentLaborWorkShop],
                    [CurrentContractStartDate],
                    [CurrentContractStopDate],
                    [CurrentContractDesc],
			        [CurrentContractDiscontinueDate],
			        [CurrentContractDiscontinueDesc],
                    [CurrentEnterpriseKey],
                    [CurrentEnterpriseName],
                    [CurrentContractKey],
			        [Memo1],
			        [Memo2],
			        [Memo3],
			        [Memo4],
			        [Memo5],
			        [PropertyNames],
			        [PropertyValues]
                ) 
                Values (
			        {0}UserGuid,
                    {0}LaborCode,
			        {0}NativePlace,
			        {0}CurrentPlace,
			        {0}IDCardPlace,
			        {0}HouseHoldType,
			        {0}WorkSkill,
			        {0}WorkSkillPaper,
			        {0}WorkSituation,
			        {0}PreWorkSituation,
			        {0}HopeWorkSituation,
			        {0}HopeWorkSalary,
			        {0}UrgentLinkMan,
			        {0}UrgentTelephone,
			        {0}UrgentRelationship,
			        {0}InformationComeFrom,
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
                    {0}InformationBrokerUserGuid,
			        {0}InformationBrokerUserName,
			        {0}InsureType,
                    {0}LaborWorkStatus,
			        {0}CurrentLaborDepartment,
			        {0}CurrentLaborWorkShop,
                    {0}CurrentContractStartDate,
                    {0}CurrentContractStopDate,
                    {0}CurrentContractDesc,
			        {0}CurrentContractDiscontinueDate,
			        {0}CurrentContractDiscontinueDesc,
                    {0}CurrentEnterpriseKey,
                    {0}CurrentEnterpriseName,
                    {0}CurrentContractKey,
			        {0}Memo1,
			        {0}Memo2,
			        {0}Memo3,
			        {0}Memo4,
			        {0}Memo5,
			        {0}PropertyNames,
			        {0}PropertyValues
                )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(LaborEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCLabor] Set   
					[UserGuid] = {0}UserGuid,
                    [LaborCode]={0}LaborCode,
					[NativePlace] = {0}NativePlace,
					[CurrentPlace] = {0}CurrentPlace,
					[IDCardPlace] = {0}IDCardPlace,
					[HouseHoldType] = {0}HouseHoldType,
					[WorkSkill] = {0}WorkSkill,
					[WorkSkillPaper] = {0}WorkSkillPaper,
					[WorkSituation] = {0}WorkSituation,
					[PreWorkSituation] = {0}PreWorkSituation,
					[HopeWorkSituation] = {0}HopeWorkSituation,
					[HopeWorkSalary] = {0}HopeWorkSalary,
					[UrgentLinkMan] = {0}UrgentLinkMan,
					[UrgentTelephone] = {0}UrgentTelephone,
					[UrgentRelationship] = {0}UrgentRelationship,
					[InformationComeFrom] = {0}InformationComeFrom,
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
                    [InformationBrokerUserGuid] = {0}InformationBrokerUserGuid,
                    [InformationBrokerUserName] = {0}InformationBrokerUserName,
					[InsureType] = {0}InsureType,
                    [LaborWorkStatus] = {0}LaborWorkStatus,
				    [CurrentLaborDepartment] = {0}CurrentLaborDepartment,
				    [CurrentLaborWorkShop] = {0}CurrentLaborWorkShop,
                    [CurrentContractStartDate] = {0}CurrentContractStartDate,
                    [CurrentContractStopDate] = {0}CurrentContractStopDate,
                    [CurrentContractDesc] = {0}CurrentContractDesc,
				    [CurrentContractDiscontinueDate] = {0}CurrentContractDiscontinueDate,
				    [CurrentContractDiscontinueDesc] = {0}CurrentContractDiscontinueDesc,
                    [CurrentEnterpriseKey] = {0}CurrentEnterpriseKey,
                    [CurrentEnterpriseName]= {0}CurrentEnterpriseName,
                    [CurrentContractKey] = {0}CurrentContractKey,
					[Memo1] = {0}Memo1,
					[Memo2] = {0}Memo2,
					[Memo3] = {0}Memo3,
					[Memo4] = {0}Memo4,
					[Memo5] = {0}Memo5,
					[PropertyNames] = {0}PropertyNames,
					[PropertyValues] = {0}PropertyValues
             Where [LaborID] = {0}LaborID", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法

        protected override TParameter[] PrepareParasAll(LaborEntity entity)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("LaborID",entity.LaborID),
                GenerateParameter("UserGuid",entity.UserGuid),
			    GenerateParameter("LaborCode",entity.LaborCode?? String.Empty),
                GenerateParameter("NativePlace",entity.NativePlace?? String.Empty),
			    GenerateParameter("CurrentPlace",entity.CurrentPlace?? String.Empty),
			    GenerateParameter("IDCardPlace",entity.IDCardPlace?? String.Empty),
			    GenerateParameter("HouseHoldType",(int)entity.HouseHoldType),
			    GenerateParameter("WorkSkill",entity.WorkSkill?? String.Empty),
			    GenerateParameter("WorkSkillPaper",entity.WorkSkillPaper?? String.Empty),
			    GenerateParameter("WorkSituation",entity.WorkSituation?? String.Empty),
			    GenerateParameter("PreWorkSituation",entity.PreWorkSituation?? String.Empty),
			    GenerateParameter("HopeWorkSituation",entity.HopeWorkSituation?? String.Empty),
			    GenerateParameter("HopeWorkSalary",entity.HopeWorkSalary?? String.Empty),
			    GenerateParameter("UrgentLinkMan",entity.UrgentLinkMan?? String.Empty),
			    GenerateParameter("UrgentTelephone",entity.UrgentTelephone?? String.Empty),
			    GenerateParameter("UrgentRelationship",entity.UrgentRelationship?? String.Empty),
			    GenerateParameter("InformationComeFrom",entity.InformationComeFrom?? String.Empty),
			    GenerateParameter("ProviderUserGuid",entity.ProviderUserGuid),
			    GenerateParameter("ProviderUserName",entity.ProviderUserName?? String.Empty),
			    GenerateParameter("RecommendUserGuid",entity.RecommendUserGuid),
			    GenerateParameter("RecommendUserName",entity.RecommendUserName?? String.Empty),
			    GenerateParameter("ServiceUserGuid",entity.ServiceUserGuid),
			    GenerateParameter("ServiceUserName",entity.ServiceUserName?? String.Empty),
			    GenerateParameter("FinanceUserGuid",entity.FinanceUserGuid),
			    GenerateParameter("FinanceUserName",entity.FinanceUserName?? String.Empty),
                GenerateParameter("BusinessUserGuid",entity.BusinessUserGuid),
			    GenerateParameter("BusinessUserName",entity.BusinessUserName?? String.Empty),
                GenerateParameter("SettleUserGuid",entity.SettleUserGuid),
			    GenerateParameter("SettleUserName",entity.SettleUserName?? String.Empty),
                GenerateParameter("InformationBrokerUserGuid",entity.InformationBrokerUserGuid),
			    GenerateParameter("InformationBrokerUserName",entity.InformationBrokerUserName?? String.Empty),
			    GenerateParameter("InsureType",entity.InsureType),
                GenerateParameter("LaborWorkStatus",entity.LaborWorkStatus),
                GenerateParameter("CurrentLaborDepartment",entity.CurrentLaborDepartment?? String.Empty),
			    GenerateParameter("CurrentLaborWorkShop",entity.CurrentLaborWorkShop?? String.Empty),
                GenerateParameter("CurrentContractStartDate",entity.CurrentContractStartDate),
                GenerateParameter("CurrentContractStopDate",entity.CurrentContractStopDate),
                GenerateParameter("CurrentContractDesc",entity.CurrentContractDesc??String.Empty),
                GenerateParameter("CurrentContractDiscontinueDate",entity.CurrentContractDiscontinueDate),
			    GenerateParameter("CurrentContractDiscontinueDesc",entity.CurrentContractDiscontinueDesc?? String.Empty),
                GenerateParameter("CurrentEnterpriseKey",entity.CurrentEnterpriseKey??String.Empty),
                GenerateParameter("CurrentEnterpriseName",entity.CurrentEnterpriseName??String.Empty),
                GenerateParameter("CurrentContractKey",entity.CurrentContractKey??String.Empty),
			    GenerateParameter("Memo1",entity.Memo1?? String.Empty),
			    GenerateParameter("Memo2",entity.Memo2?? String.Empty),
			    GenerateParameter("Memo3",entity.Memo3?? String.Empty),
			    GenerateParameter("Memo4",entity.Memo4?? String.Empty),
			    GenerateParameter("Memo5",entity.Memo5?? String.Empty),
			    GenerateParameter("PropertyNames",entity.PropertyNames?? String.Empty),
			    GenerateParameter("PropertyValues",entity.PropertyValues?? String.Empty)
            };

            return list.ToArray();
        }


        protected override LaborEntity Load(IDataReader reader)
        {
            LaborEntity entity = null;
            if (reader != null && reader.IsClosed == false)
            {
                BusinessUser businessUser = BusinessUserCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>.Load((TDataReader)reader);
                entity = Converter.InheritedEntityConvert<BusinessUser, LaborEntity>(businessUser);

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborID"))
                {
                    entity.LaborID = reader.GetInt32(reader.GetOrdinal("LaborID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "UserGuid"))
                {
                    entity.UserGuid = reader.GetGuid(reader.GetOrdinal("UserGuid"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborCode"))
                {
                    entity.LaborCode = reader.GetString(reader.GetOrdinal("LaborCode"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "NativePlace"))
                {
                    entity.NativePlace = reader.GetString(reader.GetOrdinal("NativePlace"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentPlace"))
                {
                    entity.CurrentPlace = reader.GetString(reader.GetOrdinal("CurrentPlace"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "IDCardPlace"))
                {
                    entity.IDCardPlace = reader.GetString(reader.GetOrdinal("IDCardPlace"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "HouseHoldType"))
                {
                    entity.HouseHoldType = (HouseHoldTypes)reader.GetInt32(reader.GetOrdinal("HouseHoldType"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "WorkSkill"))
                {
                    entity.WorkSkill = reader.GetString(reader.GetOrdinal("WorkSkill"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "WorkSkillPaper"))
                {
                    entity.WorkSkillPaper = reader.GetString(reader.GetOrdinal("WorkSkillPaper"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "WorkSituation"))
                {
                    entity.WorkSituation = reader.GetString(reader.GetOrdinal("WorkSituation"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "PreWorkSituation"))
                {
                    entity.PreWorkSituation = reader.GetString(reader.GetOrdinal("PreWorkSituation"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "HopeWorkSituation"))
                {
                    entity.HopeWorkSituation = reader.GetString(reader.GetOrdinal("HopeWorkSituation"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "HopeWorkSalary"))
                {
                    entity.HopeWorkSalary = reader.GetString(reader.GetOrdinal("HopeWorkSalary"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "UrgentLinkMan"))
                {
                    entity.UrgentLinkMan = reader.GetString(reader.GetOrdinal("UrgentLinkMan"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "UrgentTelephone"))
                {
                    entity.UrgentTelephone = reader.GetString(reader.GetOrdinal("UrgentTelephone"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "UrgentRelationship"))
                {
                    entity.UrgentRelationship = reader.GetString(reader.GetOrdinal("UrgentRelationship"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationComeFrom"))
                {
                    entity.InformationComeFrom = reader.GetString(reader.GetOrdinal("InformationComeFrom"));
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
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerUserGuid"))
                {
                    entity.InformationBrokerUserGuid = reader.GetGuid(reader.GetOrdinal("InformationBrokerUserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InformationBrokerUserName"))
                {
                    entity.InformationBrokerUserName = reader.GetString(reader.GetOrdinal("InformationBrokerUserName"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "InsureType"))
                {
                    entity.InsureType = reader.GetInt32(reader.GetOrdinal("InsureType"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "LaborWorkStatus"))
                {
                    entity.LaborWorkStatus = (LaborWorkStatuses)reader.GetInt32(reader.GetOrdinal("LaborWorkStatus"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentLaborDepartment"))
                {
                    entity.CurrentLaborDepartment = reader.GetString(reader.GetOrdinal("CurrentLaborDepartment"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentLaborWorkShop"))
                {
                    entity.CurrentLaborWorkShop = reader.GetString(reader.GetOrdinal("CurrentLaborWorkShop"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentContractStartDate"))
                {
                    entity.CurrentContractStartDate = reader.GetDateTime(reader.GetOrdinal("CurrentContractStartDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentContractStopDate"))
                {
                    entity.CurrentContractStopDate = reader.GetDateTime(reader.GetOrdinal("CurrentContractStopDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentContractDesc"))
                {
                    entity.CurrentContractDesc = reader.GetString(reader.GetOrdinal("CurrentContractDesc"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentContractDiscontinueDate"))
                {
                    entity.CurrentContractDiscontinueDate = reader.GetDateTime(reader.GetOrdinal("CurrentContractDiscontinueDate"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentContractDiscontinueDesc"))
                {
                    entity.CurrentContractDiscontinueDesc = reader.GetString(reader.GetOrdinal("CurrentContractDiscontinueDesc"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentEnterpriseKey"))
                {
                    entity.CurrentEnterpriseKey = reader.GetString(reader.GetOrdinal("CurrentEnterpriseKey"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentEnterpriseName"))
                {
                    entity.CurrentEnterpriseName = reader.GetString(reader.GetOrdinal("CurrentEnterpriseName"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "CurrentContractKey"))
                {
                    entity.CurrentContractKey = reader.GetString(reader.GetOrdinal("CurrentContractKey"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Memo1"))
                {
                    entity.Memo1 = reader.GetString(reader.GetOrdinal("Memo1"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Memo2"))
                {
                    entity.Memo2 = reader.GetString(reader.GetOrdinal("Memo2"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Memo3"))
                {
                    entity.Memo3 = reader.GetString(reader.GetOrdinal("Memo3"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Memo4"))
                {
                    entity.Memo4 = reader.GetString(reader.GetOrdinal("Memo4"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Memo5"))
                {
                    entity.Memo5 = reader.GetString(reader.GetOrdinal("Memo5"));
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
