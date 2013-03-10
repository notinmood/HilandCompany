using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore.BLL;
using XQYC.Business.BLL;
using XQYC.Business.Entity;

namespace XQYC.Web.Areas.LaborConsole.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// 用户自己修改口令
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// 个人登录信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyProfile()
        {
            return View();
        }

        /// <summary>
        /// 个人基本信息
        /// </summary>
        /// <returns></returns>
        public ActionResult MyBasicInfo()
        {
            LaborEntity entity = LaborEntity.Empty;
            entity = LaborBLL.Instance.Get(BusinessUserBLL.CurrentUserGuid);
            return View(entity);
        }

        public ActionResult MyBasicInfoEdit()
        {
            LaborEntity entity = LaborEntity.Empty;
            entity = LaborBLL.Instance.Get(BusinessUserBLL.CurrentUserGuid);
            return View(entity);
        }

        [HttpPost]
        public ActionResult MyBasicInfoEdit(LaborEntity editedEntity)
        {
            LaborEntity entity = LaborEntity.Empty;
            entity = LaborBLL.Instance.Get(BusinessUserBLL.CurrentUserGuid);

            entity.UserSex = editedEntity.UserSex;
            entity.UserBirthDay = editedEntity.UserBirthDay;
            entity.UserHeight = editedEntity.UserHeight;
            entity.UserWeight = editedEntity.UserWeight;
            entity.UserEducationalBackground = editedEntity.UserEducationalBackground;
            entity.UserEducationalSchool = editedEntity.UserEducationalSchool;
            entity.UserNation = editedEntity.UserNation;
            entity.NativePlace = editedEntity.NativePlace;
            entity.UserMobileNO = editedEntity.UserMobileNO;
            entity.HomeTelephone = editedEntity.HomeTelephone;
            entity.WorkSkill = editedEntity.WorkSkill;
            entity.WorkSkillPaper = editedEntity.WorkSkillPaper;
            entity.WorkSituation = editedEntity.WorkSituation;
            entity.PreWorkSituation = editedEntity.PreWorkSituation;
            entity.HopeWorkSituation = editedEntity.HopeWorkSituation;
            entity.HopeWorkSalary = editedEntity.HopeWorkSalary;
            entity.MaritalStatus = editedEntity.MaritalStatus;
            entity.UrgentLinkMan = editedEntity.UrgentLinkMan;
            entity.UrgentRelationship = editedEntity.UrgentRelationship;
            entity.UrgentTelephone = editedEntity.UrgentTelephone;
            LaborBLL.Instance.Update(entity);

            return RedirectToActionPermanent("MyBasicInfo");
        }
    }
}
