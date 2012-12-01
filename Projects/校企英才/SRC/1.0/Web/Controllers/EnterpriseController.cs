using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Entity;
using HiLand.Utility.Enums;
using HiLand.Utility.IO;
using HiLand.Utility.Paging;
using HiLand.Utility.Web;
using HiLand.Utility4.MVC.Controls;
using HiLand.Utility4.Office;
using HiLand.Utility4.Web;
using Webdiyer.WebControls.Mvc;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    [PermissionAuthorize]
    public class EnterpriseController : Controller
    {
        #region 企业基本信息
        public ActionResult Index(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("Index", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";
            string orderClause = "EnterpriseID DESC";
            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            PagedEntityCollection<EnterpriseEntity> entityList = EnterpriseBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<EnterpriseEntity> pagedExList = new PagedList<EnterpriseEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        /// <summary>
        /// 劳务派遣企业客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IndexLaborDispatch(int id = 1)
        {
            return InnerIndex(id, 1, "IndexLaborDispatch", "IndexLaborDispatch");//1在GeneralBasicSetting表中表示劳务派遣
        }

        /// <summary>
        /// 代理招聘企业客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IndexLaborHireBroke(int id = 1)
        {
            return InnerIndex(id, 2, "IndexLaborHireBroke", "IndexLaborHireBroke");//2在GeneralBasicSetting表中表示代理招聘
        }

        /// <summary>
        /// 人才会场企业客户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IndexLaborJobFair(int id = 1)
        {
            return InnerIndex(id, 4, "IndexLaborJobFair", "IndexLaborJobFair");//4在GeneralBasicSetting表中表示人才会场
        }

        private ActionResult InnerIndex(int id, int enterpriseServiceType, string viewName, string actionName)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action(actionName, new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = string.Format(" EnterpriseServiceType={0} AND EnterpriseServiceStatus={1} ", enterpriseServiceType, (int)Logics.True);
            string orderClause = "EnterpriseID DESC";
            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            PagedEntityCollection<EnterpriseServiceEntity> entityList = EnterpriseServiceBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<EnterpriseServiceEntity> pagedExList = new PagedList<EnterpriseServiceEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(viewName, pagedExList);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        public ActionResult Item(string keyGuid)
        {
            EnterpriseEntity department = EnterpriseEntity.Empty;
            if (string.IsNullOrWhiteSpace(keyGuid) == false)
            {
                department = EnterpriseBLL.Instance.Get(keyGuid);
            }

            return View(department);
        }

        /// <summary>
        /// 对单条记录进行添加（或者修改）
        /// </summary>
        /// <param name="keyGuid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Item(string keyGuid, EnterpriseEntity entity, bool isOnlyPlaceHolder = true)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;

            EnterpriseEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(keyGuid))
            {
                targetEntity = new EnterpriseEntity();
                SetTargetEntityValue(entity, ref targetEntity);
                targetEntity.EstablishedTime = DateTime.Now;
                isSuccessful = EnterpriseBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = EnterpriseBLL.Instance.Get(keyGuid);
                SetTargetEntityValue(entity, ref targetEntity);

                isSuccessful = EnterpriseBLL.Instance.Update(targetEntity);
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
        private static void SetTargetEntityValue(EnterpriseEntity originalEntity, ref EnterpriseEntity targetEntity)
        {
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.CompanyName = originalEntity.CompanyName;
            targetEntity.ContactPerson = originalEntity.ContactPerson;
            targetEntity.CompanyNameShort = originalEntity.CompanyNameShort;
            targetEntity.PrincipleAddress = originalEntity.PrincipleAddress;
            targetEntity.EnterpriseWWW = originalEntity.EnterpriseWWW;
            targetEntity.BusinessType = originalEntity.BusinessType;
            targetEntity.Email = originalEntity.Email;
            targetEntity.EnterpriseLevel = originalEntity.EnterpriseLevel;
            targetEntity.EnterpriseMemo = originalEntity.EnterpriseMemo;
            targetEntity.EnterpriseDescription = originalEntity.EnterpriseDescription;
            targetEntity.Fax = originalEntity.Fax;
            targetEntity.IndustryKey = originalEntity.IndustryKey;
            targetEntity.PostCode = originalEntity.PostCode;
            targetEntity.StaffScope = originalEntity.StaffScope;
            targetEntity.Telephone = originalEntity.Telephone;
            targetEntity.TelephoneOther = originalEntity.TelephoneOther;
            targetEntity.AreaCode = originalEntity.AreaCode;
            targetEntity.AreaOther = originalEntity.AreaOther;
            targetEntity.EnterpriseMemo = originalEntity.EnterpriseMemo;
        }

        /// <summary>
        /// 企业自动完成的数据
        /// </summary>
        /// <returns></returns>
        public ActionResult AutoCompleteData()
        {
            string userValueInputted = RequestHelper.GetValue("term");
            List<AutoCompleteEntity> itemList = new List<AutoCompleteEntity>();
            string whereClause = string.Format(" ( CompanyName like '%{0}%' OR CompanyNameShort like '%{0}%' ) AND  CanUsable={1}", userValueInputted, (int)Logics.True);
            List<EnterpriseEntity> userList = EnterpriseBLL.Instance.GetList(whereClause);

            foreach (EnterpriseEntity currentLabor in userList)
            {
                AutoCompleteEntity item = new AutoCompleteEntity();
                item.details = "nothing";
                item.key = currentLabor.EnterpriseGuid.ToString();
                item.label = string.Format("{0}({1})", currentLabor.CompanyName, currentLabor.CompanyNameShort);
                item.value = currentLabor.CompanyName;

                itemList.Add(item);
            }

            return Json(itemList, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region 广告张贴
        public ActionResult ADList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<ForeOrderEntity> trackerList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format(" OwnerKey='{0}' ", itemGuid.ToString());
                trackerList = ForeOrderBLL.Instance.GetList(whereClause);
            }

            if (string.IsNullOrWhiteSpace(itemName))
            {
                itemName = EnterpriseBLL.Instance.Get(itemKey).CompanyName;
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            return View(trackerList);
        }

        public ActionResult ADItem(string enterpriseKey, string itemKey = StringHelper.Empty)
        {
            ForeOrderEntity entity = ForeOrderEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = ForeOrderBLL.Instance.Get(itemKey);
            }

            this.ViewBag.EnterpriseKey = enterpriseKey;

            return View(entity);
        }

        [HttpPost]
        public ActionResult ADItem(string enterpriseKey, string itemKey, ForeOrderEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            ForeOrderEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new ForeOrderEntity();

                targetEntity.OwnerKey = enterpriseKey;
                targetEntity.OwnerName = EnterpriseBLL.Instance.Get(enterpriseKey).CompanyNameShort;
                targetEntity.ForeOrderCategory = "AD";
                targetEntity.RelativeKey = GuidHelper.EmptyString;
                targetEntity.RelativeName = "广告张贴";
                targetEntity.CreateTime = DateTime.Now;
                targetEntity.CreateUserKey = BusinessUserBLL.CurrentUser.UserGuid.ToString();

                SetTargetForeOrderEntityValue(originalEntity, ref  targetEntity);

                isSuccessful = ForeOrderBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = ForeOrderBLL.Instance.Get(itemKey);

                SetTargetForeOrderEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = ForeOrderBLL.Instance.Update(targetEntity);
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
        #endregion

        #region 广告张贴查询
        public ActionResult ADQueryList(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("ADQueryList", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = string.Format(" ForeOrderCategory='{0}' ", "AD");

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            string orderClause = "ForeOrderID DESC";

            PagedEntityCollection<ForeOrderEntity> entityList = ForeOrderBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<ForeOrderEntity> pagedExList = new PagedList<ForeOrderEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }
        #endregion

        #region 摊位预定
        public ActionResult BoothList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<ForeOrderEntity> trackerList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format(" OwnerKey='{0}' ", itemGuid.ToString());
                trackerList = ForeOrderBLL.Instance.GetList(whereClause);
            }

            if (string.IsNullOrWhiteSpace(itemName))
            {
                itemName = EnterpriseBLL.Instance.Get(itemKey).CompanyName;
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            return View(trackerList);
        }

        public ActionResult BoothItem(string enterpriseKey, string itemKey = StringHelper.Empty)
        {
            ForeOrderEntity entity = ForeOrderEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = ForeOrderBLL.Instance.Get(itemKey);
            }

            this.ViewBag.EnterpriseKey = enterpriseKey;

            return View(entity);
        }

        [HttpPost]
        public ActionResult BoothItem(string enterpriseKey, string itemKey, ForeOrderEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            ForeOrderEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new ForeOrderEntity();

                targetEntity.OwnerKey = enterpriseKey;
                targetEntity.OwnerName = EnterpriseBLL.Instance.Get(enterpriseKey).CompanyNameShort;
                targetEntity.ForeOrderCategory = "Booth";
                targetEntity.RelativeKey = GuidHelper.EmptyString;
                targetEntity.RelativeName = "摊位";
                targetEntity.CreateTime = DateTime.Now;
                targetEntity.CreateUserKey = BusinessUserBLL.CurrentUser.UserGuid.ToString();

                SetTargetForeOrderEntityValue(originalEntity, ref  targetEntity);

                isSuccessful = ForeOrderBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = ForeOrderBLL.Instance.Get(itemKey);

                SetTargetForeOrderEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = ForeOrderBLL.Instance.Update(targetEntity);
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

        private void SetTargetForeOrderEntityValue(ForeOrderEntity originalEntity, ref ForeOrderEntity targetEntity)
        {
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.ForeOrderCount = originalEntity.ForeOrderCount;
            targetEntity.ForeOrderDate = originalEntity.ForeOrderDate;
            targetEntity.ForeOrderDateEnd = originalEntity.ForeOrderDateEnd;
            targetEntity.ForeOrderDesc = originalEntity.ForeOrderDesc;
            targetEntity.ForeOrderMemo1 = originalEntity.ForeOrderMemo1;
            targetEntity.ForeOrderMemo2 = originalEntity.ForeOrderMemo2;

            targetEntity.ForeOrderPaid = originalEntity.ForeOrderPaid;
            targetEntity.ForeOrderStatus = originalEntity.ForeOrderStatus;
            targetEntity.ForeOrderTitle = originalEntity.ForeOrderTitle;
            targetEntity.ForeOrderType = originalEntity.ForeOrderType;
            targetEntity.ForeOrderUnitFee = originalEntity.ForeOrderUnitFee;

            targetEntity.ForeOrderPayDate = originalEntity.ForeOrderPayDate;
            targetEntity.CommissionDate = originalEntity.CommissionDate;
            targetEntity.CommissionFee = originalEntity.CommissionFee;
            targetEntity.CommissionIsDrawed = originalEntity.CommissionIsDrawed;
            targetEntity.CommissionOther = originalEntity.CommissionOther;
        }
        #endregion

        #region 摊位预定查询
        public ActionResult BoothQueryList(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("BoothQueryList", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = string.Format(" ForeOrderCategory='{0}' ", "Booth");

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            string orderClause = "ForeOrderID DESC";

            PagedEntityCollection<ForeOrderEntity> entityList = ForeOrderBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<ForeOrderEntity> pagedExList = new PagedList<ForeOrderEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        /// <summary>
        /// 企业摊位使用情况查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult BoothQueryList2(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("BoothQueryList2", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            string orderClause = "";

            PagedEntityCollection<BoothForeOrderView> entityList = BoothForeOrderBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<BoothForeOrderView> pagedExList = new PagedList<BoothForeOrderView>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }
        #endregion

        #region 招聘简章
        public ActionResult JobList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<EnterpriseJobEntity> entityList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format(" EnterpriseKey='{0}' ", itemGuid.ToString());
                entityList = EnterpriseJobBLL.Instance.GetList(whereClause);
            }

            if (string.IsNullOrWhiteSpace(itemName))
            {
                itemName = EnterpriseBLL.Instance.Get(itemKey).CompanyName;
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            return View(entityList);
        }

        public ActionResult JobItem(string enterpriseKey, string itemKey = StringHelper.Empty)
        {
            EnterpriseJobEntity entity = EnterpriseJobEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = EnterpriseJobBLL.Instance.Get(itemKey);
            }

            this.ViewBag.EnterpriseKey = enterpriseKey;

            return View(entity);
        }

        [HttpPost]
        public ActionResult JobItem(string enterpriseKey, string itemKey, EnterpriseJobEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            EnterpriseJobEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new EnterpriseJobEntity();

                targetEntity.EnterpriseKey = enterpriseKey;
                targetEntity.EnterpriseName = EnterpriseBLL.Instance.Get(enterpriseKey).CompanyNameShort;
                targetEntity.CreateTime = DateTime.Now;
                targetEntity.CreateUserKey = BusinessUserBLL.CurrentUser.UserGuid.ToString();

                SetTargetEnterpriseJobEntityValue(originalEntity, ref  targetEntity);

                isSuccessful = EnterpriseJobBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = EnterpriseJobBLL.Instance.Get(itemKey);

                SetTargetEnterpriseJobEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = EnterpriseJobBLL.Instance.Update(targetEntity);
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

        private void SetTargetEnterpriseJobEntityValue(EnterpriseJobEntity originalEntity, ref EnterpriseJobEntity targetEntity)
        {
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.EnterpriseAddress = originalEntity.EnterpriseAddress;
            targetEntity.EnterpriseAreaCode = originalEntity.EnterpriseAreaCode;
            targetEntity.EnterpriseContackInfo = originalEntity.EnterpriseContackInfo;
            targetEntity.EnterpriseDesc = originalEntity.EnterpriseDesc;
            targetEntity.EnterpriseJobDemand = originalEntity.EnterpriseJobDemand;
            targetEntity.EnterpriseJobDesc = originalEntity.EnterpriseJobDesc;
            targetEntity.EnterpriseJobLaborCount = originalEntity.EnterpriseJobLaborCount;
            targetEntity.EnterpriseJobOther = originalEntity.EnterpriseJobOther;
            targetEntity.EnterpriseJobStation = originalEntity.EnterpriseJobStation;
            targetEntity.EnterpriseJobTitle = originalEntity.EnterpriseJobTitle;
            targetEntity.EnterpriseJobTreadment = originalEntity.EnterpriseJobTreadment;
            targetEntity.EnterpriseJobType = originalEntity.EnterpriseJobType;
            targetEntity.EnterpriseJobStatus = originalEntity.EnterpriseJobStatus;
            targetEntity.InterviewDateInfo = originalEntity.InterviewDateInfo;
        }

        public ActionResult JobPictureList(string itemKey)
        {
            List<ImageEntity> entityList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format(" RelativeGuid='{0}' ", itemGuid.ToString());
                entityList = ImageBLL.Instance.GetList(whereClause);
            }

            this.ViewBag.EnterpriseJobKey = itemKey;
            return View(entityList);
        }

        public ActionResult JobPictureItem(string enterpriseJobKey, string itemKey = StringHelper.Empty)
        {
            ImageEntity entity = ImageEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = ImageBLL.Instance.Get(itemKey);
            }

            this.ViewBag.EnterpriseJobKey = enterpriseJobKey;

            return View(entity);
        }

        [HttpPost]
        public ActionResult JobPictureItem(string enterpriseJobKey, string itemKey, ImageEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            ImageEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new ImageEntity();

                targetEntity.RelativeGuid = GuidHelper.TryConvert(enterpriseJobKey);
                targetEntity.ImageCategoryCode = "enterpriserJob";
                targetEntity.CreateTime = DateTime.Now;

                SetTargetJobImageEntityValue(originalEntity, ref  targetEntity);

                isSuccessful = ImageBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = ImageBLL.Instance.Get(itemKey);
                originalEntity.CreateTime = targetEntity.CreateTime;
                originalEntity.ImageRelativePath = targetEntity.ImageRelativePath;
                originalEntity.ImageType = targetEntity.ImageType;

                SetTargetJobImageEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = ImageBLL.Instance.Update(targetEntity);
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

        private void SetTargetJobImageEntityValue(ImageEntity originalEntity, ref ImageEntity targetEntity)
        {
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.ImageStatus = originalEntity.CanUsable;
            targetEntity.ImageOrder = originalEntity.ImageOrder;
            targetEntity.ImageName = originalEntity.ImageName;

            HttpPostedFileBase fileInfo = Request.Files["fileInput"];
            if (fileInfo.HasFile())
            {
                string fileExtensionName = Path.GetExtension(fileInfo.FileName);
                string fileName = string.Format("{0}{1}", GuidHelper.NewGuidString(), fileExtensionName);
                string baseVirtualPath = PathHelper.CombineForVirtual(ImageEntity.ImageVirtualBasePath, "enterpriseJob");

                string nativeFullPath = IOHelper.EnsureDatePath(Request.MapPath(baseVirtualPath), DatePathFormaters.Y_MD);
                nativeFullPath = Path.Combine(nativeFullPath, fileName);
                string relativeVirtualPath = IOHelper.GetRelativeVirtualPath(nativeFullPath, ImageEntity.ImageVirtualBasePath);
                fileInfo.SaveAs(nativeFullPath);

                targetEntity.ImageRelativePath = relativeVirtualPath;
                targetEntity.ImageType = FileHelper.GeFileExtensionNameWithoutDot(fileExtensionName);
            }
        }

        public ActionResult JobExport(string itemKey)
        {
            //1.读取模板
            string fileFullPath = Server.MapPath("~/DownFiles/Templet/企业招聘简章模板.docx");
            string targetFullPath = Server.MapPath("~/DownFiles/Templet/EnterpriseJobs/企业招聘简章模板-" + GuidHelper.NewGuidString() + ".docx");
            System.IO.File.Copy(fileFullPath, targetFullPath, true);

            EnterpriseJobEntity jobEntity = EnterpriseJobBLL.Instance.Get(itemKey);
            List<ImageEntity> imageList = EnterpriseJobBLL.Instance.GetImages(jobEntity.EnterpriseJobGuid);

            using (FileStream memStream = new FileStream(targetFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                //2.1替换内容
                WordHelper.ReplaceTextContent(memStream, "/word/document.xml", "公司名称具体内容", jobEntity.EnterpriseName);
                WordHelper.ReplaceTextContent(memStream, "/word/document.xml", "公司简介具体内容", jobEntity.EnterpriseDesc);
                WordHelper.ReplaceTextContent(memStream, "/word/document.xml", "岗位说明具体内容", jobEntity.EnterpriseJobStation);
                WordHelper.ReplaceTextContent(memStream, "/word/document.xml", "工作要求具体内容", jobEntity.EnterpriseJobDemand);
                WordHelper.ReplaceTextContent(memStream, "/word/document.xml", "薪资待遇具体内容", jobEntity.EnterpriseJobTreadment);
                WordHelper.ReplaceTextContent(memStream, "/word/document.xml", "联系方式具体内容", jobEntity.EnterpriseContackInfo);

                //2.2替换图片
                if (imageList != null)
                {
                    for (int i = 0; i < imageList.Count; i++)
                    {
                        ImageEntity image = imageList[i];
                        int j = i + 1;
                        if (j > 3)
                        {
                            break;
                        }
                        string thumbVirtualPath = image.ImageAllVirtualPath; //image.EnsureThumbnailAllVirtualPath(200, 150);
                        string thumbFullPath = Server.MapPath(thumbVirtualPath);

                        Stream imageStream = FileHelper.GetStreamFromFile(thumbFullPath);
                        WordHelper.ReplaceSteamContent(memStream, string.Format("/word/media/image{0}.jpeg", j), imageStream);
                    }
                }

                memStream.Flush();
                memStream.Close();
            }

            //3.输出文件
            return File(targetFullPath, ContentTypes.GetContentType("docx"), string.Format("企业招聘简章-{0}.docx", jobEntity.EnterpriseName));
        }
        #endregion

        #region 招聘简章查询
        public ActionResult JobQueryList(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("JobQueryList", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = string.Format(" 1=1 ");

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            string orderClause = "EnterpriseJobID DESC";

            PagedEntityCollection<EnterpriseJobEntity> entityList = EnterpriseJobBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<EnterpriseJobEntity> pagedExList = new PagedList<EnterpriseJobEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }
        #endregion

        #region 回访跟踪
        public ActionResult TrackerList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<TrackerEntity> trackerList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format(" RelativeKey='{0}' ", itemGuid.ToString());
                trackerList = TrackerBLL.Instance.GetList(whereClause);
            }

            if (string.IsNullOrWhiteSpace(itemName))
            {
                itemName = EnterpriseBLL.Instance.Get(itemKey).CompanyName;
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            return View(trackerList);
        }

        public ActionResult TrackerItem(string enterpriseKey, string itemKey = StringHelper.Empty)
        {
            TrackerEntity entity = TrackerEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = TrackerBLL.Instance.Get(itemKey);
            }

            this.ViewBag.EnterpriseKey = enterpriseKey;

            return View(entity);
        }

        [HttpPost]
        public ActionResult TrackerItem(string enterpriseKey, string itemKey, TrackerEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            TrackerEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new TrackerEntity();

                targetEntity.RelativeKey = enterpriseKey;
                targetEntity.TrackerCategory = "EnterpriseTracker";
                targetEntity.CreateTime = DateTime.Now;
                targetEntity.CreateUserKey = BusinessUserBLL.CurrentUser.UserGuid.ToString();

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);

                isSuccessful = TrackerBLL.Instance.Create(targetEntity);

            }
            else
            {
                targetEntity = TrackerBLL.Instance.Get(itemKey);

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = TrackerBLL.Instance.Update(targetEntity);
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

        private void SetTargetContractEntityValue(TrackerEntity originalEntity, ref TrackerEntity targetEntity)
        {
            targetEntity.CanUsable = originalEntity.CanUsable;
            targetEntity.TrackerDesc = originalEntity.TrackerDesc;
            targetEntity.TrackerTime = originalEntity.TrackerTime;
            targetEntity.TrackerTitle = originalEntity.TrackerTitle;
            targetEntity.TrackerType = originalEntity.TrackerType;
            targetEntity.TrackerUserKey = originalEntity.TrackerUserKey;
        }
        #endregion

        #region 企业合同管理
        public ActionResult ContractList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<EnterpriseContractEntity> userList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format("EnterpriseGuid='{0}'", itemGuid.ToString());
                userList = EnterpriseContractBLL.Instance.GetList(whereClause);
            }

            if (string.IsNullOrWhiteSpace(itemName))
            {
                itemName = EnterpriseBLL.Instance.Get(itemKey).CompanyName;
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            return View(userList);
        }

        public ActionResult ContractItem(string enterpriseKey, string itemKey = StringHelper.Empty)
        {
            EnterpriseContractEntity entity = EnterpriseContractEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                entity = EnterpriseContractBLL.Instance.Get(itemKey);
            }

            this.ViewBag.EnterpriseKey = enterpriseKey;

            return View(entity);
        }

        [HttpPost]
        public ActionResult ContractItem(string enterpriseKey, string itemKey, EnterpriseContractEntity originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            EnterpriseContractEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetEntity = new EnterpriseContractEntity();

                targetEntity.EnterpriseGuid = Converter.ChangeType<Guid>(enterpriseKey);
                targetEntity.EnterpriseInfo = EnterpriseBLL.Instance.Get(targetEntity.EnterpriseGuid).CompanyName;
                targetEntity.ContractCreateDate = DateTime.Now;
                targetEntity.ContractCreateUserKey = BusinessUserBLL.CurrentUser.UserGuid.ToString();

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);


                isSuccessful = EnterpriseContractBLL.Instance.Create(targetEntity);

            }
            else
            {
                targetEntity = EnterpriseContractBLL.Instance.Get(itemKey);

                SetTargetContractEntityValue(originalEntity, ref  targetEntity);
                isSuccessful = EnterpriseContractBLL.Instance.Update(targetEntity);
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

        private void SetTargetContractEntityValue(EnterpriseContractEntity originalEntity, ref EnterpriseContractEntity targetEntity)
        {
            targetEntity.ContractDetails = originalEntity.ContractDetails;
            targetEntity.ContractLaborAddon = originalEntity.ContractLaborAddon;
            targetEntity.ContractLaborCount = originalEntity.ContractLaborCount;
            targetEntity.ContractStartDate = originalEntity.ContractStartDate;
            targetEntity.ContractStopDate = originalEntity.ContractStopDate;
            targetEntity.ContractTitle = originalEntity.ContractTitle;
            targetEntity.ContractStatus = originalEntity.ContractStatus;
        }
        #endregion

        #region 企业合同查询
        public ActionResult ContractQueryList(int id = 1)
        {
            //1.如果是点击查询控件的查询按钮，那么将查询条件作为QueryString附加在地址后面（为了在客户端保存查询条件的状体），重新发起一次请求。
            if (this.Request.HttpMethod.ToLower().Contains("post"))
            {
                string targetUrlWithoutParam = Url.Action("ContractQueryList", new { id = 1 });
                string targetUrl = QueryControlHelper.GetNewQueryUrl("QueryControl", targetUrlWithoutParam);
                return Redirect(targetUrl);
            }

            //2.通常情形下走get查询
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = string.Format(" 1=1 ");

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            string orderClause = "ContractID DESC";

            PagedEntityCollection<EnterpriseContractEntity> entityList = EnterpriseContractBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<EnterpriseContractEntity> pagedExList = new PagedList<EnterpriseContractEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            bool isExportExcel = RequestHelper.GetValue("exportExcel", false);

            return View(pagedExList);
        }
        #endregion

        #region 企业登录用户的管理
        public ActionResult UserList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<BusinessUser> userList = null;

            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                Guid itemGuid = GuidHelper.TryConvert(itemKey);
                string whereClause = string.Format("EnterpriseKey='{0}'", itemGuid.ToString());
                userList = BusinessUserBLL.GetList(whereClause);
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            return View(userList);
        }

        public ActionResult UserItem(string enterpriseKey, string itemKey = StringHelper.Empty)
        {
            BusinessUser user = BusinessUser.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                user = BusinessUserBLL.Get(new Guid(itemKey));
            }

            this.ViewBag.EnterpriseKey = enterpriseKey;

            return View(user);
        }

        [HttpPost]
        public ActionResult UserItem(string enterpriseKey, string itemKey, BusinessUser originalEntity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            BusinessUser targetUser = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == true)
            {
                targetUser = new BusinessUser();

                targetUser.EnterpriseKey = enterpriseKey;
                targetUser.UserType = UserTypes.EnterpriseUser;
                targetUser.Password = SystemConst.InitialUserPassword;

                SetTargetUserEntityValue(originalEntity, ref  targetUser);

                CreateUserRoleStatuses createStatus = CreateUserRoleStatuses.Successful;
                BusinessUserBLL.CreateUser(targetUser, out createStatus);
                if (createStatus == CreateUserRoleStatuses.Successful)
                {
                    isSuccessful = true;
                }
            }
            else
            {
                targetUser = BusinessUserBLL.Get(new Guid(itemKey));

                SetTargetUserEntityValue(originalEntity, ref  targetUser);
                isSuccessful = BusinessUserBLL.UpdateUser(targetUser);
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

        private void SetTargetUserEntityValue(BusinessUser originalEntity, ref BusinessUser targetEntity)
        {
            targetEntity.UserName = originalEntity.UserName;
            targetEntity.UserNameCN = originalEntity.UserNameCN;
            targetEntity.UserSex = originalEntity.UserSex;
            targetEntity.UserMobileNO = originalEntity.UserMobileNO;
            targetEntity.UserStatus = originalEntity.UserStatus;
        }

        /// <summary>
        /// 管理员为企业登录用户进行口令修改
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public ActionResult UserPassword(string userGuid, string userName = "")
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = BusinessUserBLL.Get(new Guid(userGuid)).UserName;
            }

            string returnUrl = RequestHelper.CurrentRequest.AppRelativeCurrentExecutionFilePath;
            return RedirectToAction("_ChangePasswordForManage", "Home", new { area = "UserCenter", userGuid = userGuid, userName = userName, returenUrl = returnUrl });
        }
        #endregion

        #region 企业合作方式
        /// <summary>
        /// 企业选用的服务列表
        /// </summary>
        /// <param name="itemKey">企业Guid字符串</param>
        /// <returns></returns>
        public ActionResult ServiceList(string itemKey, string itemName = StringHelper.Empty)
        {
            List<BasicSettingEntity> allServiceList = BasicSettingBLL.Instance.GetListByCategory("EnterpriseServiceType");
            List<EnterpriseServiceEntity> selectedServiceList = new List<EnterpriseServiceEntity>();

            Guid itemGuid = GuidHelper.TryConvert(itemKey);
            if (itemGuid != Guid.Empty)
            {
                selectedServiceList = EnterpriseServiceBLL.Instance.GetListByEnterprise(itemGuid);
            }

            this.ViewBag.EnterpriseKey = itemKey;
            this.ViewBag.EnterpriseName = itemName;
            this.ViewBag.SelectedServiceList = selectedServiceList;
            return View(allServiceList);
        }

        /// <summary>
        /// 企业选用的服务
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="enterpriseKey"></param>
        /// <param name="itemTypeNumber"></param>
        /// <param name="itemTypeName"></param>
        /// <returns></returns>
        public ActionResult ServiceItem(string itemKey, string enterpriseKey, string itemTypeNumber)
        {
            //
            string returnUrl = RequestHelper.GetValue("returnUrl");
            this.ViewBag.ReturnUrl = returnUrl;

            EnterpriseServiceEntity serviceEntity = EnterpriseServiceEntity.Empty;
            if (GuidHelper.IsInvalidOrEmpty(itemKey) == false)
            {
                serviceEntity = EnterpriseServiceBLL.Instance.Get(itemKey);
            }

            serviceEntity.EnterpriseGuid = GuidHelper.TryConvert(enterpriseKey);
            serviceEntity.EnterpriseInfo = EnterpriseBLL.Instance.Get(serviceEntity.EnterpriseGuid).CompanyName;
            serviceEntity.EnterpriseServiceType = Converter.ChangeType<int>(itemTypeNumber);

            List<BasicSettingEntity> allServiceList = BasicSettingBLL.Instance.GetListByCategory("EnterpriseServiceType");
            string itemTypeName = allServiceList.Find(m => m.SettingValue == itemTypeNumber).DisplayName;
            this.ViewBag.ItemTypeName = itemTypeName;

            return View(serviceEntity);
        }

        [HttpPost]
        public ActionResult ServiceItem(string itemKey, EnterpriseServiceEntity entity)
        {
            bool isSuccessful = false;
            string displayMessage = string.Empty;
            string returnUrl = RequestHelper.GetValue("returnUrl");
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("ServiceList");
            }

            returnUrl = returnUrl.Replace("&amp;", "&");

            EnterpriseServiceEntity targetEntity = null;
            if (GuidHelper.IsInvalidOrEmpty(itemKey))
            {
                targetEntity = new EnterpriseServiceEntity();
                SetTargetServiceEntityValue(entity, ref targetEntity);
                targetEntity.EnterpriseServiceCreateDate = DateTime.Now;
                targetEntity.EnterpriseServiceCreateUserKey = BusinessUserBLL.CurrentUser.UserGuid.ToString();
                isSuccessful = EnterpriseServiceBLL.Instance.Create(targetEntity);
            }
            else
            {
                targetEntity = EnterpriseServiceBLL.Instance.Get(itemKey);
                SetTargetServiceEntityValue(entity, ref targetEntity);

                isSuccessful = EnterpriseServiceBLL.Instance.Update(targetEntity);
            }


            if (isSuccessful == true)
            {
                displayMessage = "数据保存成功";
            }
            else
            {
                displayMessage = "数据保存失败";
            }

            return Redirect(returnUrl);
        }

        /// <summary>
        /// 通过一个实体给另外一个实体赋值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="targetEntity"></param>
        private static void SetTargetServiceEntityValue(EnterpriseServiceEntity originalEntity, ref EnterpriseServiceEntity targetEntity)
        {
            targetEntity.EnterpriseServiceStatus = originalEntity.EnterpriseServiceStatus;
            targetEntity.EnterpriseServiceType = originalEntity.EnterpriseServiceType;
            targetEntity.EnterpriseInfo = originalEntity.EnterpriseInfo;
            targetEntity.EnterpriseGuid = originalEntity.EnterpriseGuid;
            targetEntity.FinanceUserName = RequestHelper.GetValue("FinanceUser");
            targetEntity.FinanceUserGuid = RequestHelper.GetValue<Guid>("FinanceUser_Value");
            targetEntity.ProviderUserName = RequestHelper.GetValue("ProviderUser");
            targetEntity.ProviderUserGuid = RequestHelper.GetValue<Guid>("ProviderUser_Value");
            targetEntity.RecommendUserName = RequestHelper.GetValue("RecommendUser");
            targetEntity.RecommendUserGuid = RequestHelper.GetValue<Guid>("RecommendUser_Value");
            targetEntity.ServiceUserName = RequestHelper.GetValue("ServiceUser");
            targetEntity.ServiceUserGuid = RequestHelper.GetValue<Guid>("ServiceUser_Value");

            targetEntity.BusinessUserName = RequestHelper.GetValue("BusinessUser");
            targetEntity.BusinessUserGuid = RequestHelper.GetValue<Guid>("BusinessUser_Value");
            targetEntity.SettleUserName = RequestHelper.GetValue("SettleUser");
            targetEntity.SettleUserGuid = RequestHelper.GetValue<Guid>("SettleUser_Value");
        }
        #endregion
    }
}
