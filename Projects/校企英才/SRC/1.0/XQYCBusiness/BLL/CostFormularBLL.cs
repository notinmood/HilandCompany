using System.Collections.Generic;
using System.Data;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Enums;
using XQYC.Business.DAL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.BLL
{
    public class CostFormularBLL : BaseBLL<CostFormularBLL, CostFormularEntity, CostFormularDAL>
    {
        /// <summary>
        /// 按照类别获取公式集合
        /// </summary>
        /// <param name="costKind"></param>
        /// <param name="whereClause"></param>
        /// <param name="paras"></param>
        /// <returns></returns>
        public List<CostFormularEntity> GetList(CostKinds costKind, Logics onlyDisplayUsable, string whereClause, params IDbDataParameter[] paras)
        {
            if (string.IsNullOrEmpty(whereClause))
            {
                whereClause = " 1=1 ";
            }

            whereClause += string.Format(" AND CostKind={0} ", (int)costKind);

            return base.GetList(onlyDisplayUsable, whereClause, 0, string.Empty, paras);
        }
    }
}