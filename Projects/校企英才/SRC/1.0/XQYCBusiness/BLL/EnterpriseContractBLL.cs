using System;
using System.Collections.Generic;
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

namespace XQYC.Business.BLL
{
    public class EnterpriseContractBLL : BaseBLL<EnterpriseContractBLL, EnterpriseContractEntity, EnterpriseContractDAL>
    {
        public override bool Create(EnterpriseContractEntity model)
        {
            bool isSuccessful = base.Create(model);

            OperateLogBLL.RecordOperateLog(string.Format("创建企业合同信息{0}", isSuccessful == true ? "成功" : "失败"), "EnterpriseContract", model.ContractGuid.ToString(), model.EnterpriseInfo, model, null);
            //RecordOperateLog(model, null, string.Format("创建企业合同信息{0}", isSuccessful == true ? "成功" : "失败"));
            return isSuccessful;
        }

        public override bool Update(EnterpriseContractEntity model)
        {
            EnterpriseContractEntity originalModel = Get(model.ContractGuid, true);
            bool isSuccessful = base.Update(model);
            OperateLogBLL.RecordOperateLog(string.Format("修改企业合同信息{0}", isSuccessful == true ? "成功" : "失败"), "EnterpriseContract", model.ContractGuid.ToString(), model.EnterpriseInfo, model, originalModel);
            //RecordOperateLog(model, originalModel, string.Format("修改企业合同信息{0}", isSuccessful == true ? "成功" : "失败"));
            return isSuccessful;
        }

        //private static void RecordOperateLog(EnterpriseContractEntity newModel, EnterpriseContractEntity originalModel, string logTitle)
        //{
        //    if (Config.IsRecordOperateLog == true)
        //    {
        //        try
        //        {
        //            OperateLogEntity logEntity = new OperateLogEntity();
        //            logEntity.CanUsable = Logics.True;
        //            logEntity.LogCategory = "EnterpriseContract";
        //            logEntity.LogDate = DateTime.Now;
        //            if (originalModel == null)
        //            {
        //                logEntity.LogOperateName = OperateTypes.Create.ToString();
        //            }
        //            else
        //            {
        //                logEntity.LogOperateName = OperateTypes.Update.ToString();
        //            }
        //            logEntity.LogStatus = (int)Logics.True;
        //            logEntity.LogType = 0;
        //            logEntity.LogUserKey = BusinessUserBLL.CurrentUserGuid.ToString();
        //            logEntity.LogUserName = BusinessUserBLL.CurrentUser.UserNameDisplay;
        //            logEntity.RelativeKey = newModel.ContractGuid.ToString();
        //            logEntity.RelativeName = newModel.EnterpriseInfo;
        //            logEntity.LogTitle = logTitle;

        //            if (originalModel != null)
        //            {
        //                List<string> compareResult = new List<string>();
        //                string[] excludePropertyArray = new string[]{"LastUpdateUserKey",
        //                    "LastUpdateUserName",
        //                    "LastUpdateDate",
        //                    "PropertyNames",
        //                    "PropertyValues"
        //                };
        //                EntityHelper.Compare(originalModel, newModel, out compareResult, excludePropertyArray);
        //                if (compareResult != null && compareResult.Count > 0)
        //                {
        //                    logEntity.LogMessage = CollectionHelper.Concat(";", compareResult as IEnumerable<String>);
        //                }
        //                else
        //                {
        //                    logEntity.LogMessage = "没有修改任何信息";
        //                }
        //            }

        //            OperateLogBLL.Instance.Create(logEntity);
        //        }
        //        catch
        //        {
        //        }
        //    }
        //}
    }
}