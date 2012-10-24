using System.Web.Mvc;

namespace XQYC.Web.Areas.EnterpriseConsole
{
    public class EnterpriseConsoleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EnterpriseConsole";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Enterprise_default",
                "EnterpriseConsole/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new string[] { "XQYC.Web.Areas.EnterpriseConsole.Controllers" }
            );
        }
    }
}
