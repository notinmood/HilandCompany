using System;
using System.Collections.Generic;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.FoundationLayer;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Reflection;
using HiLand.Utility.Setting;
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
                OperateLogBLL.RecordOperateLog(string.Format("创建劳务人员信息{0}", isSuccessful == true ? "成功" : "失败"), "Labor", model.UserGuid.ToString(), model.UserNameDisplay, model, null);
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
            LaborEntity originalModel = Get(model.UserGuid, true);
            bool isSuccessful = BusinessUserBLL.UpdateUser(model);
            if (isSuccessful == true)
            {
                //TIP:xieran2012124 保存即加锁锁定
                model.IsProtectedByOwner = Logics.True;
                isSuccessful = base.Update(model);
            }
            OperateLogBLL.RecordOperateLog(string.Format("修改劳务人员信息{0}", isSuccessful == true ? "成功" : "失败"), "Labor", model.UserGuid.ToString(), model.UserNameDisplay, model, originalModel);

            return isSuccessful;
        }

        public override LaborEntity Get(Guid modelID, bool isForceUseNoCache)
        {
            BusinessUser businessUser = BusinessUserBLL.Get(modelID, isForceUseNoCache);
            LaborEntity entity = Converter.InheritedEntityConvert<BusinessUser, LaborEntity>(businessUser);
            LaborEntity entityPartial = base.Get(modelID, isForceUseNoCache);

            entity = ReflectHelper.CopyMemberValue<LaborEntity, LaborEntity>(entityPartial, entity, true);

            return entity;
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
        /// 根据人员姓名，劳工编号，企业Guid信息获取劳工信息
        /// </summary>
        /// <param name="laborName">人员姓名</param>
        /// <param name="laborCode">劳工编号</param>
        /// <param name="laborCardID">劳务人员身份证号</param>
        /// <param name="enterpriseKey">企业Guid信息</param>
        /// <returns></returns>
        public LaborEntity Get(string laborName, string laborCode,string laborCardID, string enterpriseKey)
        {
            string whereClause = string.Format(" UserNameCN='{0}' AND CurrentEnterpriseKey='{1}' ", laborName, enterpriseKey);
            
            if (string.IsNullOrWhiteSpace(laborCode) == false)
            {
                whereClause += string.Format(" AND LaborCode='{0}'  ", laborCode);
            }

            if (string.IsNullOrWhiteSpace(laborCardID) == false)
            {
                whereClause += string.Format(" AND UserCardID='{0}'  ", laborCardID);
            }

            List<LaborEntity> list = this.GetList(whereClause);

            //通过条件进行匹配的时候，没有人员或者多于1个人员都返回匹配失败。
            if (list == null || list.Count == 0 || list.Count > 1)
            {
                return LaborEntity.Empty;
            }
            else
            {
                return list[0];
            }
        }

        /// <summary>
        /// 获取列表（参数paras暂时不支持）
        /// </summary>
        /// <param name="whereClause"></param>
        /// <param name="paras">此参数暂时不支持</param>
        /// <returns></returns>
        public override List<LaborEntity> GetList(string whereClause, params System.Data.IDbDataParameter[] paras)
        {
            return GetList(whereClause, 0, string.Empty);
        }

        public List<LaborEntity> GetList(string whereClause, int topCount, string orderByClause)
        {
            string selectString = " SELECT ";
            if (topCount > 0)
            {
                selectString += string.Format(" TOP {0} ", topCount);
            }

            if (string.IsNullOrWhiteSpace(orderByClause))
            {
                orderByClause = " LaborID DESC ";
            }

            if (string.IsNullOrWhiteSpace(whereClause))
            {
                whereClause = " 1=1 ";
            }

            string sqlClause = string.Format("{0} BIZ.*,CU.* FROM XQYCLabor BIZ LEFT JOIN CoreUser CU ON BIZ.UserGuid= CU.UserGuid WHERE {1} ORDER BY {2}", selectString, whereClause, orderByClause);
            return base.GetListBySQL(sqlClause);
        }

        public override int GetTotalCount(string whereClause)
        {
            string sqlClause = string.Format("SELECT COUNT(1) FROM XQYCLabor BIZ LEFT JOIN CoreUser CU ON BIZ.UserGuid= CU.UserGuid WHERE {0}", whereClause);
            return (int)base.GetScalar(sqlClause);
        }

        /// <summary>
        /// 获取最新录入的人员列表
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public List<LaborEntity> GetListForLastest(int topN)
        {
            string orderByClause = " UserRegisterDate DESC ";
            return GetList(string.Empty, topN, orderByClause);
        }

        /// <summary>
        /// 从某个时间段内总共录入的人数
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetCountForLastestRegister(DateTime startDate, DateTime endDate)
        {
            string whereClause = string.Format(" UserRegisterDate >='{0}' AND UserRegisterDate<='{1}' ", startDate, endDate);
            return GetTotalCount(whereClause);
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
            string sqlClause = string.Format("SELECT BIZ.*,CU.* FROM XQYCLabor BIZ LEFT JOIN CoreUser CU ON BIZ.UserGuid= CU.UserGuid WHERE {0}", whereClause);
            result = base.GetListBySQL(sqlClause);
            return result;
        }
    }
}
