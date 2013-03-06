using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Utility.Data;
using HiLand.Utility.Entity.Status;
using HiLand.Utility.Paging;
using Webdiyer.WebControls.Mvc;
using XQYC.Business.BLL;
using XQYC.Business.Entity;
using XQYC.Business.Enums;
using XQYC.Web.Models;

namespace XQYC.Web.Areas.LaborConsole.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Labor/Home/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 劳务合同列表
        /// </summary>
        /// <returns></returns>
        public ActionResult ContractList()
        {
            List<LaborContractEntity> userList = new List<LaborContractEntity>();

            if (BusinessUserBLL.CurrentUserGuid != Guid.Empty)
            {
                string whereClause = string.Format("LaborUserGuid='{0}'", BusinessUserBLL.CurrentUserGuid);
                string orderbyClause = string.Format("LaborContractIsCurrent DESC,LaborContractID DESC");
                userList = LaborContractBLL.Instance.GetList(whereClause, orderbyClause);
            }
            return View(userList);
        }

        /// <summary>
        /// 劳务人员薪资列表
        /// </summary>
        /// <returns></returns>
        public ActionResult SalaryList(int id = 1)
        {
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = " 1=1 ";
            string orderClause = "SalarySummaryID DESC";

            whereClause += string.Format(" AND LaborKey='{0}' ", BusinessUserBLL.CurrentUserGuid);
            PagedEntityCollection<SalarySummaryEntity> entityList = SalarySummaryBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<SalarySummaryEntity> pagedExList = new PagedList<SalarySummaryEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        public ActionResult SalaryDetailsList(string itemKey)
        {
            List<SalaryItemKinds> salaryItemKindList = EnumHelper.GetItems<SalaryItemKinds>("Enterprise");
            string excludeItemString = " 1=1 ";
            for (int i = 0; i < salaryItemKindList.Count; i++)
            {
                excludeItemString += string.Format(" AND SalaryItemKind!={0} ",(int)salaryItemKindList[i]);
            }

            string whereClause = string.Format(" SalarySummaryKey ='{0}' AND {1} ", itemKey, excludeItemString);
            List<SalaryDetailsEntity> salaryDetailsList = SalaryDetailsBLL.Instance.GetList(whereClause);
            return View(salaryDetailsList);
        }
    }
}
