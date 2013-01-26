using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HiLand.Utility.Data;

namespace XQYC.Web.Areas.APIs.Controllers
{
    public class GeneralServiceController : Controller
    {
        public ActionResult Compress(string data)
        {
            string result = CompressHelper.Compress(data);
            return Content(result);
        }
    }
}