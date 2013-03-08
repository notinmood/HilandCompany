using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore.BLL;

namespace XQYC.Web.Models.Attribute
{
    public class LaborAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            if (BusinessUserBLL.CurrentUser.UserStatus == HiLand.Utility.Enums.UserStatuses.Unactivated)
            { 
                
            }
        }
    }
}