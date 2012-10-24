using System.Web.Mvc;

namespace XQYC.Web.Areas.Labor
{
    public class LaborConsoleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LaborConsole";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "LaborConsole_default",
                "LaborConsole/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new string[] { "XQYC.Web.Areas.LaborConsole.Controllers" }
            );
        }
    }
}
