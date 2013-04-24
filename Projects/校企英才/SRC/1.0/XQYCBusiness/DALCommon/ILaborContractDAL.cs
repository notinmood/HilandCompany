using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HiLand.Framework.FoundationLayer;
using XQYC.Business.Entity;

namespace XQYC.Business.DALCommon
{
    public interface ILaborContractDAL : IDAL<LaborContractEntity>
    {
        /// <summary>
        /// 移除劳务人员所有合同的当前状态
        /// </summary>
        /// <param name="laborGuid">劳务人员Guid</param>
        /// <param name="contractGuidExclude">取消当前状态时，需要排除在外的合同Guid</param>
        /// <returns></returns>
        void RemoveCurrentStatusOfLaborContract(Guid laborGuid, Guid contractGuidExclude);
    }
}
