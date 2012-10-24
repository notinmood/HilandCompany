using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Framework4.Permission.Attributes;
using HiLand.General.BLL;
using HiLand.General.Entity;
using HiLand.Project.SiteWeb.Models;
using HiLand.Utility.Data;
using HiLand.Utility.Enums;
using HiLand.Utility.Paging;
using Webdiyer.WebControls.Mvc;

namespace HiLand.Project.SiteWeb.Areas.Manage.Controllers
{
    [UserAuthorize]
    public class NewsController : Controller
    {
        public ActionResult Index(int id = 1)
        {
            return RedirectToAction("NewsList");
        }

        public ActionResult NewsList(int id = 1)
        {
            int pageIndex = id;
            int pageSize = ConfigConst.CountPerPageForManage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = "";
            string orderClause = "NewsID DESC";

            PagedEntityCollection<NewsEntity> coll = NewsBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<NewsEntity> pagedList = new PagedList<NewsEntity>(coll.Records, coll.PageIndex, coll.PageSize, coll.TotalCount);

            return View(pagedList);
        }

        public ActionResult CreateNews()
        {
            GenerateAndPassNewsCategory();
            return View();
        }



        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateNews(NewsEntity model)
        {
            NewsEntity modelToSave = model;
            if (model.NewsDate == DateTimeHelper.Min)
            {
                modelToSave.NewsDate = DateTime.Now;
            }

            modelToSave.NewsGuid = Guid.NewGuid();

            bool isSuccessful = NewsBLL.Instance.Create(modelToSave);
            if (isSuccessful == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                GenerateAndPassNewsCategory();
                return View();
            }
        }

        private void GenerateAndPassNewsCategory()
        {
            List<NewsCategoryEntity> newsCategoryList = NewsCategoryBLL.GetInstance(20).GetList(Logics.True, string.Empty, 0, string.Empty);
            if (newsCategoryList != null)
            {
                List<SelectListItem> itemList = new List<SelectListItem>();
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (var v in newsCategoryList)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = v.NewsCategoryName;
                    item.Value = v.NewsCategoryCode;
                    itemList.Add(item);
                }
                this.ViewData["NewsCategeoryList"] = itemList;
            }
        }

        public ActionResult UpdateNews(Guid newsGUID)
        {
            GenerateAndPassNewsCategory();

            NewsEntity originalEntity = NewsBLL.Instance.Get(newsGUID);
            //originalEntity.NewsDate = originalEntity.NewsDate.Date;
            return View(originalEntity);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult UpdateNews(NewsEntity model)
        {
            bool isSuccessful = NewsBLL.Instance.Update(model);

            if (isSuccessful == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                GenerateAndPassNewsCategory();
                return View(model);
            }
        }


        public ActionResult DeleteNews(Guid newsGUID)
        {
            GenerateAndPassNewsCategory();
            NewsEntity originalEntity = NewsBLL.Instance.Get(newsGUID);
            return View(originalEntity);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DeleteNews(NewsEntity model)
        {
            bool isSuccessful = NewsBLL.Instance.Delete(model);

            if (isSuccessful == true)
            {
                return RedirectToAction("Index");
            }
            else
            {
                GenerateAndPassNewsCategory();
                return View(model);
            }
        }

        public ActionResult Search()
        {
            return View();
        }


        public ActionResult NewsCategoryList(int id = 1)
        {
            int pageIndex = id;
            int pageSize = ConfigConst.CountPerPageForManage;
            int startIndex = (pageIndex - 1) * pageSize + 1;
            string whereClause = "";
            string orderClause = "";
            PagedEntityCollection<NewsCategoryEntity> coll = NewsCategoryBLL.Instance.GetPagedCollection(startIndex, pageSize, whereClause, orderClause);
            PagedList<NewsCategoryEntity> pagedList = new PagedList<NewsCategoryEntity>(coll.Records, coll.PageIndex, coll.PageSize, coll.TotalCount);

            return View(pagedList);
        }
    }
}
