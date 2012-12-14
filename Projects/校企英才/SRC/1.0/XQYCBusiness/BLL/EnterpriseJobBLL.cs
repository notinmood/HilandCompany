using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.FoundationLayer;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.Setting;
using XQYC.Business.DAL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.BLL
{
    public class EnterpriseJobBLL : BaseBLL<EnterpriseJobBLL, EnterpriseJobEntity, EnterpriseJobDAL>
    {
        public override bool Create(EnterpriseJobEntity model)
        {
            bool isSuccessful = base.Create(model);

            OperateLogBLL.RecordOperateLog(string.Format("创建企业招工简章信息{0}", isSuccessful == true ? "成功" : "失败"), "EnterpriseJob", model.EnterpriseJobGuid.ToString(), model.EnterpriseName, model, null);
            return isSuccessful;
        }

        public override bool Update(EnterpriseJobEntity model)
        {
            EnterpriseJobEntity originalModel = Get(model.EnterpriseJobGuid, true);
            bool isSuccessful = base.Update(model);
            OperateLogBLL.RecordOperateLog(string.Format("修改企业招工简章信息{0}", isSuccessful == true ? "成功" : "失败"), "EnterpriseJob", model.EnterpriseJobGuid.ToString(), model.EnterpriseName, model, originalModel);
            return isSuccessful;
        }

        /// <summary>
        /// 获取最新列表
        /// </summary>
        /// <param name="topN"></param>
        /// <returns></returns>
        public List<EnterpriseJobEntity> GetListForLastest(int topN)
        {
            string whereClause = string.Format(" CanUsable ={0}", (int)Logics.True);
            string orderbyClause = string.Format(" EnterpriseJobID DESC ");
            return base.GetList(Logics.False, whereClause, topN, orderbyClause);
        }

        /// <summary>
        /// 从某个时间段内总共的数量
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public int GetCountForLastest(DateTime startDate, DateTime endDate)
        {
            string whereClause = string.Format(" CreateTime >='{0}' AND CreateTime<='{1}' ", startDate, endDate);
            return base.GetTotalCount(whereClause);
        }

        /// <summary>
        /// 获取招工简章对应的图片
        /// </summary>
        /// <param name="JobGuid"></param>
        /// <returns></returns>
        public List<ImageEntity> GetImages(Guid JobGuid)
        {
            return ImageBLL.Instance.GetList(Guid.Empty, JobGuid);
        }
    }
}