using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.DALCommon;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using HiLand.Utility.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using XQYC.Business.Entity;

namespace XQYC.Business.DALCommon
{
    public class EmployeeCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<EmployeeEntity, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "XQYCEmployee"; }
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
            get { return "usp_XQYC_Employee_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        public override bool Create(EmployeeEntity entity)
        {
            string commandText = string.Format(@"Insert Into [XQYCEmployee] (
                    [UserGuid],
                    [Foo]
                ) 
                Values (
                    {0}UserGuid,			        
                    {0}Foo
                )", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }

        public override bool Update(EmployeeEntity entity)
        {
            string commandText = string.Format(@"Update [XQYCEmployee] Set   
					[Foo] = {0}Foo
             Where [UserGuid] = {0}UserGuid", ParameterNamePrefix);

            TParameter[] sqlParas = PrepareParasAll(entity);

            bool isSuccessful = HelperExInstance.ExecuteSingleRowNonQuery(commandText, sqlParas);
            return isSuccessful;
        }
        #endregion

        #region 辅助方法

        protected override TParameter[] PrepareParasAll(EmployeeEntity entity)
        {
            List<TParameter> list = new List<TParameter>()
            {
                GenerateParameter("EmployeeID",entity.EmployeeID),
                GenerateParameter("UserGuid",entity.UserGuid== Guid.Empty?GuidHelper.NewGuid():entity.UserGuid),
                GenerateParameter("Foo",entity.Foo?? String.Empty)
            };

            return list.ToArray();
        }
       

        protected override EmployeeEntity Load(IDataReader reader)
        {
            EmployeeEntity entity = null;
            if (reader != null && reader.IsClosed == false)
            {
                BusinessUser businessUser = BusinessUserCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>.Load((TDataReader)reader);
                entity = Converter.InheritedEntityConvert<BusinessUser, EmployeeEntity>(businessUser);

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "EmployeeID"))
                {
                    entity.EmployeeID = reader.GetInt32(reader.GetOrdinal("EmployeeID"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "UserGuid"))
                {
                    entity.UserGuid = reader.GetGuid(reader.GetOrdinal("UserGuid"));
                }
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "Foo"))
                {
                    entity.Foo = reader.GetString(reader.GetOrdinal("Foo"));
                }
            }

            return entity;
        }
        #endregion
    }
}
