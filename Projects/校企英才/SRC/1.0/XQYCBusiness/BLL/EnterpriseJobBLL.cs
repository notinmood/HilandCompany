using System;
using System.Collections.Generic;
using System.Data;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Enums;
using XQYC.Business.DAL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.BLL
{
    public class EnterpriseJobBLL : BaseBLL<EnterpriseJobBLL, EnterpriseJobEntity, EnterpriseJobDAL>
    {
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
    }
}