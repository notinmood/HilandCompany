using System;
using System.Collections.Generic;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Enums;
using XQYC.Business.DAL;
using XQYC.Business.DALCommon;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Business.BLL
{

    /*
     * 劳务人员合同处理的几个注意事项：
     * 1、订立新合同的时候，新合同的LaborContractIsCurrent为True，同时需要修改当前劳务人员其他合同的LaborContractIsCurrent为False
     *      1.1、如果LaborCode变化同时也要更新Labor表中LaborCode的值
     *      1.2、订立新合同的时候，需要把新合同的部分信息写入XQYCLabor表，主要体现为表XQYCLabor的a.几个Current开头的字段 b.LaborWorkStatus字段
     *      1.3、更新合同的时候，如果劳务人员表内current记录的是此合同，那么劳务人员表亦需要更新current信息
     * 4、暂时未考虑删除的情况
     * 5、系统任务自动扫描到期合同的时候，修改字段LaborContractStatus的值为NormalStop
    */

    //TODO:xieran20121011 添加系统自动扫描任务，完成上面提到的第五项目
    //TODO:xieran20121005 需要实现一个系统任务调用的，根据当前时间变更劳务合同状态（主要是变更到自然到期）的方法

    /// <summary>
    /// 劳务人员合同业务逻辑类
    /// </summary>
    public class LaborContractBLL : BaseBLL<LaborContractBLL, LaborContractEntity, LaborContractDAL, ILaborContractDAL>
    {
        public override bool Create(LaborContractEntity model)
        {
            bool isSuccessful = base.Create(model);

            if (isSuccessful == true && model.LaborContractIsCurrent == Logics.True)
            {
                UpdateCurrentContractData(model);
            }

            return isSuccessful;
        }

        public override bool Update(LaborContractEntity model)
        {
            bool isSuccessful = base.Update(model);
            if (isSuccessful == true && model.LaborContractIsCurrent == Logics.True)
            {
                UpdateCurrentContractData(model);
            }

            return isSuccessful;
        }

        /// <summary>
        /// 更新当前合同的关联信息
        /// </summary>
        /// <param name="model">当前劳务合同</param>
        private void UpdateCurrentContractData(LaborContractEntity model)
        {
            //1.更新此劳务人员其他的合同是否为当前项为假
            this.SaveDAL.RemoveCurrentStatusOfLaborContract(model.LaborUserGuid, model.LaborContractGuid);

            //2.更新此劳务人员基本信息中当前劳务合同信息的部分
            LaborEntity laborEntity = LaborBLL.Instance.Get(model.LaborUserGuid);
            UpdateLaborCurrentData(laborEntity, model);
        }

        /// <summary>
        /// 更新劳务人员基础信息中当前劳务合同的部分
        /// </summary>
        /// <param name="laborEntity"></param>
        /// <param name="contractEntity"></param>
        private bool UpdateLaborCurrentData(LaborEntity laborEntity, LaborContractEntity contractEntity)
        {
            laborEntity.CurrentEnterpriseKey = contractEntity.EnterpriseGuid.ToString();
            laborEntity.CurrentEnterpriseName = contractEntity.Enterprise.CompanyName;
            laborEntity.CurrentContractKey = contractEntity.LaborContractGuid.ToString();
            laborEntity.CurrentContractStartDate = contractEntity.LaborContractStartDate;
            laborEntity.CurrentContractStopDate = contractEntity.LaborContractStopDate;
            laborEntity.CurrentContractDesc = contractEntity.EnterpriseContract.ContractTitle;

            laborEntity.CurrentContractDiscontinueDate = contractEntity.LaborContractDiscontinueDate;
            laborEntity.CurrentContractDiscontinueDesc = contractEntity.LaborContractDiscontinueDesc;

            laborEntity.LaborWorkStatus = contractEntity.LaborContractStatus;
            laborEntity.LaborCode = contractEntity.LaborCode;

            return LaborBLL.Instance.Update(laborEntity);
        }

        /// <summary>
        /// 获取某人员当前的劳务合同
        /// </summary>
        /// <param name="LaborUserGuid"></param>
        /// <returns></returns>
        public LaborContractEntity GetCurrentContract(Guid LaborUserGuid)
        {
            List<LaborContractEntity> contractList = base.GetList(string.Format(" LaborUserGuid= '{0}' AND LaborContractIsCurrent={1} ",LaborUserGuid,(int)Logics.True));
            if (contractList != null && contractList.Count > 0)
            {
                return contractList[0];
            }
            else
            {
                return LaborContractEntity.Empty;
            }
        }
    }
}