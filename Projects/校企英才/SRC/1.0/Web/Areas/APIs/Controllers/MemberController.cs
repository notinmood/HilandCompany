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
        public ActionResult WebCreate()
        {
            ComeFromTypes comeFromType = ComeFromTypes.WebRegister;
            return Create(comeFromType);
        }

        public ActionResult Create(ComeFromTypes comeFromType)
        {
            LaborEntity labor = new LaborEntity();
            labor.ComeFromType = comeFromType;

            labor.UserNameCN = RequestHelper.GetValue("UserNameCN");
            labor.UserGuid = GuidHelper.NewGuid();
            labor.UserName = labor.UserGuid.ToString();
            labor.UserSex = (Sexes)RequestHelper.GetValue("UserSex", (int)Sexes.UnSet);
            labor.UserBirthDay = RequestHelper.GetValue("UserBirthday",DateTimeHelper.Min);
            labor.NativePlace = RequestHelper.GetValue("NativePlace");
            labor.UserMobileNO = RequestHelper.GetValue("UserMobileNO");

            labor.UserMobileNO = RequestHelper.GetValue("UserMobileNO");
            labor.HomeTelephone = RequestHelper.GetValue("HomeTelephone");
            labor.HopeWorkSalary = RequestHelper.GetValue("HopeWorkSalary");
            labor.UserEducationalBackground = (EducationalBackgrounds)RequestHelper.GetValue("UserEducationalBackground", (int)EducationalBackgrounds.NoSetting);
            labor.WorkSkill = RequestHelper.GetValue("WorkSkill");

            CreateUserRoleStatuses status= LaborBLL.Instance.Create(labor);
            if (status == CreateUserRoleStatuses.Successful)
            {
                return Json(true,JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
