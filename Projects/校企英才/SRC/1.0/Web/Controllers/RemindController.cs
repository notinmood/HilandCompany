using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Paging;
using HiLand.Utility.Web;
using Webdiyer.WebControls.Mvc;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    public class RemindController : Controller
    {
        public ActionResult Index(int id = 1)
        {
            int pageIndex = id;
            int pageSize = SystemConst.CountPerPage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = string.Format( " 1=1  AND ReceiverKey='{0}' ",BusinessUserBLL.CurrentUser.UserGuid); //string.Format(" LoanType= {0} AND LoanStatus!={1}  ",                (int)loanType, (int)LoanStatuses.UserUnCompleted);
            string orderClause = "RemindID DESC";

            PagedEntityCollection<RemindEntity> entityList = RemindBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<RemindEntity> pagedExList = new PagedList<RemindEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        public ActionResult Known(string itemKey)
        {
            string returnUrl = RequestHelper.GetValue("returnUrl");

            return new EmptyResult();
        }
    }
}
