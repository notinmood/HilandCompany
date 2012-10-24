using System;
using System.Collections.Generic;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Reflection;
using XQYC.Business.DAL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.BLL
{
    /*
     * 劳务人员处理的几个注意事项：
     * 为了查询的方便，表XQYCLabor对XQYCLaborContract做了部分冗余处理，主要体现为几个Current开头的字段
     * 对这个几个字段的处理，分以下情形：
     * 1、劳务人员建立新的合同的时候，向这几个字段写入新合同的数据
     * 2、劳务人员提前终止合同的时候，更改字段LaborWorkStatus为相应的值
     * 3、系统任务定期扫描到期合同的时候，更改字段LaborWorkStatus为NormalStop
     */

    /// <summary>
    /// 劳务人员业务逻辑类
    /// </summary>
    public class LaborBLL : BaseBLL<LaborBLL, LaborEntity, LaborDAL>
    {
        public new CreateUserRoleStatuses Create(LaborEntity model)
        {
            CreateUserRoleStatuses createStatus;
            BusinessUserBLL.CreateUser(model, out createStatus);
            if (createStatus == CreateUserRoleStatuses.Successful)
            {
                bool isSuccessful = base.Create(model);
                if (isSuccessful == true)
                {
                    return CreateUserRoleStatuses.Successful;
                }
                else
                {
                    return CreateUserRoleStatuses.FailureUnknowReason;
                }
            }
            else
            {
                return createStatus;
            }
        }

        public override bool Update(LaborEntity model)
        {
            bool isSuccessful = BusinessUserBLL.UpdateUser(model);
            if (isSuccessful == true)
            {
                isSuccessful = base.Update(model);
            }

            return isSuccessful;
        }

        public override LaborEntity Get(string modelID)
        {
            return Get(new Guid(modelID));
        }

        public override LaborEntity Get(Guid modelID)
        {
            BusinessUser businessUser = BusinessUserBLL.Get(modelID);
            LaborEntity entity = Converter.InheritedEntityConvert<BusinessUser, LaborEntity>(businessUser);
            LaborEntity entityPartial = base.Get(modelID);

            entity = ReflectHelper.CopyMemberValue<LaborEntity, LaborEntity>(entityPartial, entity, true);

            return entity;
        }

        /// <summary>
        /// 获取列表（参数paras暂时不支持）
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="paras">此参数暂时不支持</param>
        /// <returns></returns>
        public override List<LaborEntity> GetList(string whereClause, params System.Data.IDbDataParameter[] paras)
        {
            string sqlClause = string.Format("SELECT BIZ.*,CU.* FROM XQYCLabor BIZ LEFT JOIN CoreUser CU ON BIZ.UserGuid= CU.UserGuid WHERE {0}", whereClause);
            return base.GetListBySQL(sqlClause);
        }

        /// <summary>
        /// 获取外派到某企业的劳务人员集合
        /// </summary>
        /// <param name="enterpriseGuid">企业Guid</param>
        /// <param name="laborWorkStatus">劳务人员的工作状态</param>
        /// <returns></returns>
        public List<LaborEntity> GetLaborsByEnterprise(Guid enterpriseGuid, LaborWorkStatuses? laborWorkStatus = null)
        {
            List<LaborEntity> result = new List<LaborEntity>();
            string whereClause = string.Format(" CurrentEnterpriseKey='{0}' ", enterpriseGuid);
            if (laborWorkStatus.HasValue)
            {
                whereClause += string.Format(" AND LaborWorkStatus={0} ", (int)laborWorkStatus.Value);
            }
            string sqlClause = string.Format("SELECT BIZ.*,CU.* FROM XQYCLabor BIZ LEFT JOIN CoreUser CU ON BIZ.UserGuid= CU.UserGuid WHERE {0}",whereClause);
            result = base.GetListBySQL(sqlClause);
            return result;
        }
    }
}
