using System.Web.Mvc;

namespace HiLand.Project.SiteWeb.Areas.Manage
{
    public class ManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Manage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Manage_default",
                "Manage/{controller}/{action}/{id}",
                new { controller = "Main", action = "Index", id = UrlParameter.Optional },
                new string[] { "HiLand.Project.SiteWeb.Areas.Manage.Controllers" }
            );
        }
    }
}
