using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework.BusinessCore;
using HiLand.Framework.BusinessCore.BLL;
using HiLand.Utility.Data;
using HiLand.Utility.Entity.Status;
using XQYC.Web.Models;

namespace XQYC.Web.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //List<Guid> guidList1 = BusinessUserBLL.GetUserGuidsByDepartment("管理中心C", false);
            //List<Guid> guidList2 = BusinessUserBLL.GetUserGuidsByDepartment("管理中心C", true);

            //List<BusinessUser> userList1 = BusinessUserBLL.GetUsersByDepartment("管理中心C", false);
            //List<BusinessUser> userList2 = BusinessUserBLL.GetUsersByDepartment("管理中心C", true);
            return View();
        }

        /// <summary>
        /// 自动执行的系统任务测试
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemTaskTest()
        {
            JobContainer.ExecuteJobs(null);

            return View();
        }

        public ActionResult OperationResultTest()
        {
            List<SystemStatusInfo> infoList = new List<SystemStatusInfo>();

            SystemStatusInfo item1 = new SystemStatusInfo();
            item1.SystemStatus = HiLand.Utility.Enums.SystemStatuses.Success;
            item1.Message = "成功信息";
            infoList.Add(item1);

            SystemStatusInfo item2 = new SystemStatusInfo();
            item2.SystemStatus = HiLand.Utility.Enums.SystemStatuses.Failuer;
            item2.Message = "失败信息";
            infoList.Add(item2);

            SystemStatusInfo item3 = new SystemStatusInfo();
            item3.SystemStatus = HiLand.Utility.Enums.SystemStatuses.Warnning;
            item3.Message = "警告信息";
            infoList.Add(item3);

            SystemStatusInfo item4 = new SystemStatusInfo();
            item4.SystemStatus = HiLand.Utility.Enums.SystemStatuses.Tip;
            item4.Message = "提示信息";
            infoList.Add(item4);

            this.TempData.Add("OperationResultData", infoList);
            return RedirectToAction("OperationResults", "System", new {  });
        }

        public ActionResult EnterpriseAutoCompleteTest()
        {
            return View();
        }

        public ActionResult InformationBrokerAutoCompleteTest()
        {
            return View();
        }
    }
}
