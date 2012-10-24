using System;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework.FoundationLayer;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Reflection;
using XQYC.Business.DAL;
using XQYC.Business.Entity;

namespace XQYC.Business.BLL
{
    public class EmployeeBLL : BaseBLL<EmployeeBLL, EmployeeEntity, EmployeeDAL>
    {
        public new CreateUserRoleStatuses Create(EmployeeEntity model)
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

        public override bool Update(EmployeeEntity model)
        {
            bool isSuccessful = BusinessUserBLL.UpdateUser(model);
            if (isSuccessful == true)
            {
                isSuccessful = base.Update(model);
            }

            return isSuccessful;
        }

        public override EmployeeEntity Get(string modelID)
        {
            return Get(new Guid(modelID));
        }

        public override EmployeeEntity Get(Guid modelID)
        {
            BusinessUser businessUser = BusinessUserBLL.Get(modelID);
            EmployeeEntity entity = Converter.InheritedEntityConvert<BusinessUser, EmployeeEntity>(businessUser);
            EmployeeEntity entityPartial = base.Get(modelID);

            entity = ReflectHelper.CopyMemberValue<EmployeeEntity, EmployeeEntity>(entityPartial, entity, true);

            return entity;
        }
    }
}
