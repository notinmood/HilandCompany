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
    public class BoothForeOrderCommonDAL<TTransaction, TConnection, TCommand, TDataReader, TParameter>
        : BaseDAL<BoothForeOrderView, TTransaction, TConnection, TCommand, TDataReader, TParameter>
        where TConnection : class,IDbConnection, new()
        where TCommand : IDbCommand, new()
        where TTransaction : IDbTransaction
        where TDataReader : class, IDataReader
        where TParameter : IDataParameter, IDbDataParameter, new()
    {
        #region 基本信息
        protected override string TableName
        {
            get { return "VXQYCBoothForeOrder"; }
        }

        protected override string[] KeyNames
        {
            get { return new string[] { "OwnerKey" }; }
        }

        /// <summary>
        /// Guid主键名称
        /// </summary>
        protected override string GuidKeyName
        {
            get { return "OwnerKey"; }
        }

        /// <summary>
        /// 获取按企业名称分页的预定和使用情况
        /// </summary>
        protected override string PagingSPName
        {
            get { return "usp_XQYC_BoothForeOrder_SelectPaging"; }
        }
        #endregion

        #region 逻辑操作
        /// <summary>
        /// 视图不提供添加功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Create(BoothForeOrderView model)
        {
            //视图不提供添加功能;
            return true;
        }

        /// <summary>
        /// 视图不提供更新功能
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override bool Update(BoothForeOrderView model)
        {
            //视图不提供更新功能;
            return true;
        }
        #endregion

        #region 辅助方法
        protected override BoothForeOrderView Load(IDataReader reader)
        {
            BoothForeOrderView entity = new BoothForeOrderView();
            if (reader != null && reader.IsClosed == false)
            {
                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "OwnerKey"))
                {
                    entity.OwnerKey = GuidHelper.TryConvert(reader.GetOrdinal("OwnerKey").ToString());
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "OwnerName"))
                {
                    entity.OwnerName = reader.GetString(reader.GetOrdinal("OwnerName"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ForeCount"))
                {
                    entity.ForeCount = reader.GetInt32(reader.GetOrdinal("ForeCount"));
                }

                if (DataReaderHelper.IsExistFieldAndNotNull(reader, "ExcutedCount"))
                {
                    entity.ExcutedCount = reader.GetInt32(reader.GetOrdinal("ExcutedCount"));
                }
            }

            return entity;
        }
        #endregion
    }
}
