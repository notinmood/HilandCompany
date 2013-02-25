using System.Web.Mvc;
using HiLand.Framework.BusinessCore.Enum;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Web;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;

namespace XQYC.Web.Areas.APIs.Controllers
{
    public class MemberController : Controller
    {
        /// <summary>
        /// 为校企英才官方网站提供人员注册功能
        /// </summary>
        /// <returns></returns>
        public ActionResult WebCreate()
        {
            ComeFromTypes comeFromType = ComeFromTypes.WebRegister;
            return Create(comeFromType,"ctl00$ContentPlaceHolder1$");
        }

        public ActionResult Create(ComeFromTypes comeFromType, string controlNamePrefix = "")
        {
            LaborEntity labor = new LaborEntity();
            labor.ComeFromType = comeFromType;

            labor.UserNameCN = RequestHelper.GetValue(controlNamePrefix + "txbUserName");
            labor.UserGuid = GuidHelper.NewGuid();
            labor.UserName = labor.UserGuid.ToString();
            labor.UserSex = (Sexes)RequestHelper.GetValue(controlNamePrefix + "drpSex", (int)Sexes.UnSet);
            labor.UserBirthDay = RequestHelper.GetValue(controlNamePrefix + "txbBirthday", DateTimeHelper.Min);
            labor.NativePlace = RequestHelper.GetValue(controlNamePrefix + "txbUserCountry");
            labor.UserEmail = RequestHelper.GetValue(controlNamePrefix + "txbEmail");

            labor.UserMobileNO = RequestHelper.GetValue(controlNamePrefix + "txbUserMobiNumber");
            labor.HomeTelephone = RequestHelper.GetValue(controlNamePrefix + "txbHomeTelephone");
            labor.HopeWorkSalary = RequestHelper.GetValue(controlNamePrefix + "txbHopeWorkSalary");
            labor.UserEducationalSchool = RequestHelper.GetValue(controlNamePrefix + "txbUserEducationalBackground");
            labor.WorkSkill = RequestHelper.GetValue(controlNamePrefix + "txbWorkSkill");

            CreateUserRoleStatuses status = LaborBLL.Instance.Create(labor);
            string requestUrl = RequestHelper.GetValue(controlNamePrefix + "txbPostBackURL");//"http://192.168.10.15:8001/DispatchServices/OnlineJobHunting.aspx";

            if (requestUrl.Contains("?"))
            {
                requestUrl += "&regStatus=";
            }
            else
            {
                requestUrl += "?regStatus=";
            }

            if (status == CreateUserRoleStatuses.Successful)
            {
                return RedirectPermanent(requestUrl + "true");
            }
            else
            {
                return RedirectPermanent(requestUrl + "false");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userAccount">其可以是用户的UserName,也可以是其EMail</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ActionResult Login(string userAccount, string password)
        {
            return Json(true);
        }
    }
}
