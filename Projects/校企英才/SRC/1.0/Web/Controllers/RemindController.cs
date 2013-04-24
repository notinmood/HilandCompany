using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Utility.Data;
using HiLand.Utility.Paging;
using HiLand.Utility.Web;
using HiLand.Utility4.MVC.Controls;
using Webdiyer.WebControls.Mvc;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    public class RemindController : Controller
    {
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
            string whereClause = string.Format(" 1=1  AND ReceiverKey='{0}' ", BusinessUserBLL.CurrentUser.UserGuid); //string.Format(" LoanType= {0} AND LoanStatus!={1}  ",                (int)loanType, (int)LoanStatuses.UserUnCompleted);
            string orderClause = "RemindID DESC";

            whereClause += " AND " + QueryControlHelper.GetQueryCondition("QueryControl");

            PagedEntityCollection<RemindEntity> entityList = RemindBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<RemindEntity> pagedExList = new PagedList<RemindEntity>(entityList.Records, entityList.PageIndex, entityList.PageSize, entityList.TotalCount);

            return View(pagedExList);
        }

        public ActionResult SetRead(string itemKey)
        {
            RemindBLL.Instance.SetRead(Converter.TryToGuid(itemKey));

            string returnUrl = RequestHelper.GetValue("returnUrl");
            bool isUsingCompress = RequestHelper.GetValue<bool>("isUsingCompress");
            if (isUsingCompress == true && string.IsNullOrWhiteSpace(returnUrl) == false)
            {
                returnUrl = CompressHelper.Decompress(returnUrl);
            }
            if (string.IsNullOrWhiteSpace(returnUrl))
            {
                returnUrl = Url.Action("Index");
            }

            return Redirect(returnUrl);
        }
    }
}
