using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Reflection;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

namespace XQYC.Business.BLL
{
    public class InformationBrokerBLL : BaseBLL<InformationBrokerBLL, InformationBrokerEntity, InformationBrokerDAL>
    {
        public override bool Create(InformationBrokerEntity model)
        {
            CreateUserRoleStatuses createStatus;
            BusinessUserBLL.CreateUser(model, out createStatus);
            if (createStatus == CreateUserRoleStatuses.Successful)
            {
                return base.Create(model);
            }

            return false;
        }

        public override bool Update(InformationBrokerEntity model)
        {
            bool isSuccessful = BusinessUserBLL.UpdateUser(model);
            if (isSuccessful == true)
            {
                isSuccessful = base.Update(model);
            }

            return isSuccessful;
        }

        public override InformationBrokerEntity Get(string modelID)
        {
            return Get(new Guid(modelID));
        }

        public override InformationBrokerEntity Get(Guid modelID)
        {
            BusinessUser businessUser = BusinessUserBLL.Get(modelID);
            InformationBrokerEntity entity = Converter.InheritedEntityConvert<BusinessUser, InformationBrokerEntity>(businessUser);
            InformationBrokerEntity entityPartial = base.Get(modelID);

            entity = ReflectHelper.CopyMemberValue<InformationBrokerEntity, InformationBrokerEntity>(entityPartial, entity, true);

            return entity;
        }

        public override List<InformationBrokerEntity> GetList(string whereClause, params IDbDataParameter[] paras)
        {
            //TODO:xieran20121007需要考虑将whereClause中的参数，使用paras替换
            string sqlClause = string.Format("SELECT BIZ.*,CU.* FROM XQYCInformationBroker BIZ LEFT JOIN CoreUser CU ON BIZ.UserGuid= CU.UserGuid WHERE {0} ",whereClause);
            return base.GetListBySQL(sqlClause);
        }
    }
}
