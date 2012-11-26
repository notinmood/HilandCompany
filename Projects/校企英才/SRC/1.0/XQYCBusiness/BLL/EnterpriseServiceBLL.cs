using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Reflection;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

namespace XQYC.Business.BLL
{
    public class EnterpriseServiceBLL : BaseBLL<EnterpriseServiceBLL, EnterpriseServiceEntity, EnterpriseServiceDAL>
    {
        public override bool Create(EnterpriseServiceEntity model)
        {
            bool result = base.Create(model);
            if (result == true && model.EnterpriseServiceStatus== Logics.True)
            {
                EnterpriseEntity enterpriseEntity = EnterpriseBLL.Instance.Get(model.EnterpriseGuid);
                enterpriseEntity.CooperateStatus = FlagHelper.AddFlag(enterpriseEntity.CooperateStatus, model.EnterpriseServiceType) ;
                EnterpriseBLL.Instance.Update(enterpriseEntity);
            }
            return result;
        }

        public override bool Update(EnterpriseServiceEntity model)
        {
            bool result = base.Update(model);
            if (result == true)
            {
                EnterpriseEntity enterpriseEntity = EnterpriseBLL.Instance.Get(model.EnterpriseGuid);
                if (model.EnterpriseServiceStatus == Logics.True)
                {
                    enterpriseEntity.CooperateStatus = FlagHelper.AddFlag(enterpriseEntity.CooperateStatus, model.EnterpriseServiceType);
                }
                else
                {
                    enterpriseEntity.CooperateStatus = FlagHelper.RemoveFlag(enterpriseEntity.CooperateStatus, model.EnterpriseServiceType);
                }
                EnterpriseBLL.Instance.Update(enterpriseEntity);
            }
            return result;
        }

        public override EnterpriseServiceEntity Get(Guid modelID)
        {
            EnterpriseServiceEntity entityPartial = base.Get(modelID);
            EnterpriseEntity enterpriseEntity = EnterpriseBLL.Instance.Get(entityPartial.EnterpriseGuid);
            EnterpriseServiceEntity entity = Converter.InheritedEntityConvert<EnterpriseEntity, EnterpriseServiceEntity>(enterpriseEntity);

            entity = ReflectHelper.CopyMemberValue<EnterpriseServiceEntity, EnterpriseServiceEntity>(entityPartial, entity, true);

            return entity;
        }

        public List<EnterpriseServiceEntity> GetListByEnterprise(Guid enterpriseGuid)
        {
            return base.GetList(string.Format(" BIZ.[EnterpriseGuid]='{0}' ", enterpriseGuid));
        }

        public override List<EnterpriseServiceEntity> GetList(HiLand.Utility.Enums.Logics onlyDisplayUsable, string whereClause, int topCount, string orderByClause, params System.Data.IDbDataParameter[] paras)
        {
            if (string.IsNullOrWhiteSpace(orderByClause))
            {
                orderByClause = " EnterpriseServiceID DESC ";
            }

            if (string.IsNullOrWhiteSpace(whereClause))
            {
                whereClause = " 1=1 ";
            }

            string sqlClause = string.Format("SELECT BIZ.*,GE.* FROM XQYCEnterpriseService BIZ Left JOIN GeneralEnterprise GE ON BIZ.EnterpriseGuid= GE.EnterpriseGuid WHERE {0} ORDER BY {1}", whereClause, orderByClause);
            return base.GetListBySQL(sqlClause);
        }
    }
}