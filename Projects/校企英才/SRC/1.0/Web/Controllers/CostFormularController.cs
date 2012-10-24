using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using XQYC.Business.BLL;
using XQYC.Business.Entity;

namespace XQYC.Web.Controllers
{
    public class CostFormularController : Controller
    {
        public ActionResult Index()
        {
            List<CostFormularEntity> list = CostFormularBLL.Instance.GetList(string.Empty);
            return View(list);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        public ActionResult Item(string keyGuid)
        {
            CostFormularEntity entity = CostFormularEntity.Empty;
            if (string.IsNullOrWhiteSpace(keyGuid) == false)
            {
                entity = CostFormularBLL.Instance.Get(keyGuid);
            }

            return View(entity);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Item(string keyGuid, CostFormularEntity entity, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;

            CostFormularEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(keyGuid))
            {
                targetEntity = new CostFormularEntity();
                SetTargetEntityValue(entity, ref targetEntity);
                isSuccessful = CostFormularBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = CostFormularBLL.Instance.Get(keyGuid);
                SetTargetEntityValue(entity, ref targetEntity);

                isSuccessful = CostFormularBLL.Instance.Update(targetEntity);
            }

            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = "数据保存失败";
            }

            return Json(new LogicStatusInfo(isSuccessful, displayMessage));
        }

        /// <summary>
        /// 通过一个实体给另外一个实体赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetEntityValue(CostFormularEntity originalEntity, ref CostFormularEntity targetEntity)
        {
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.CostFormularName = originalEntity.CostFormularName;
            targetEntity.CostType = originalEntity.CostType;
            targetEntity.CostKind = originalEntity.CostKind;
            targetEntity.CostFormularDesc = originalEntity.CostFormularDesc;
            targetEntity.CostFormularValue = originalEntity.CostFormularValue;
        }
    }
}