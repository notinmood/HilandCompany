using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

namespace XQYC.Business.BLL
{
    public class EnterpriseServiceBLL : BaseBLL<EnterpriseServiceBLL, EnterpriseServiceEntity, EnterpriseServiceDAL>
    {
        public List<EnterpriseServiceEntity> GetListByEnterprise(Guid enterpriseGuid)
        {
            return base.GetList(string.Format(" [EnterpriseGuid]='{0}' ",enterpriseGuid));
        }
    }
}