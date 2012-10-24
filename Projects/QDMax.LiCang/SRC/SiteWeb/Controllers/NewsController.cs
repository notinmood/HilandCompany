using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Project.SiteWeb.Models;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Paging;
using Webdiyer.WebControls.Mvc;

namespace HiLand.Project.SiteWeb.Controllers
{
    public class NewsController : Controller
    {
        /// <summary>
        /// 新闻列表
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ActionResult Index(int id = 1, string code = StringHelper.Empty)
        {
            int pageIndex = id;
            int pageSize = ConfigConst.CountPerPageForEndUser;
            int startIndex = (pageIndex - 1) * pageSize + 1;

            string whereClause = string.Format(" CanUsable={0} ", (int)Logics.True);
            if (string.IsNullOrWhiteSpace(code) == false)
            {
                whereClause += string.Format("  AND NewsCategoryCode like '{0}%'", code);
            }

            string orderClause = "NewsID DESC";

            PagedEntityCollection<NewsEntity> coll = NewsBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<NewsEntity> pagedList = new PagedList<NewsEntity>(coll.Records, coll.PageIndex, coll.PageSize, coll.TotalCount);

            this.ViewBag.CategoryCode = code;
            return View(pagedList);
        }

        /// <summary>
        /// 展示单条信息
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public ActionResult Item(string itemKey)
        {
            Guid itemGuid = Converter.TryToGuid(itemKey);
            NewsEntity newsEntity = NewsBLL.Instance.Get(itemGuid);
            return View(newsEntity);
        }
    }
}
