using System.Web.Mvc;

namespace XQYC.Web.Areas.InformationBroker
{
    public class InformationBrokerConsoleAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "InformationBrokerConsole";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "InformationBroker_default",
                "InformationBrokerConsole/{controller}/{action}/{id}",
                new { action = "Index", controller = "Home", id = UrlParameter.Optional },
                new string[] { "XQYC.Web.Areas.InformationBrokerConsole.Controllers" }
            );
        }
    }
}
