using System.Web.Mvc;

namespace XQYC.Web.Areas.UserCenter
{
    public class UserCenterAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserCenter";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "UserCenter_default",
                "UserCenter/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new string[] { "XQYC.Web.Areas.UserCenter.Controllers" }
            );
        }
    }
}
