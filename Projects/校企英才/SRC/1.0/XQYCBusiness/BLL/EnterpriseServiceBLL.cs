using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Reflection;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

namespace XQYC.Business.BLL
{
    public class EnterpriseServiceBLL : BaseBLL<EnterpriseServiceBLL, EnterpriseServiceEntity, EnterpriseServiceDAL>
    {
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