using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;
using System.Web.UI;

namespace Salary.Web.Utility
{
    public static class JavascriptHelper
    {
        /// <summary>
        /// 执行Javascript
        /// </summary>
        /// <param name="Javascript"></param>
        public static void Execute(String javascript)
        {
            InvokeJavascriptWay(javascript);
        }

        /// <summary>
        /// 页面提示方法
        /// </summary>
        /// <param name="message">提示的语句</param>
        private static void InvokeJavascriptWay(String javascript)
        {
            Page page = PageHelper.GetCurrentPage();
            Type type = page.GetType();
            UpdatePanel updatePanel = PageHelper.FindControlsList(page).OfType<UpdatePanel>().FirstOrDefault();

            if (updatePanel == null)
            {
                InvokeJavascript(javascript);
            }
            else
            {
                InvokeJavascript(updatePanel, javascript);
            }

        }

        #region 非Ajax处理方法

        /// <summary>
        /// 处理Javascript脚本
        /// </summary>
        /// <param name="Javascript"></param>
        private static void InvokeJavascript(String javascript)
        {
            string key = Guid.NewGuid().ToString();
            InvokeJavascript(key, javascript);
        }

        /// <summary>
        /// 处理Javascript脚本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="Javascript"></param>
        private static void InvokeJavascript(String key, String javascript)
        {
            (HttpContext.Current.CurrentHandler as Page).ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), key, javascript, true);
        }

        #endregion

        #region Ajax处理方法

        /// <summary>
        /// 处理Javascript脚本
        /// </summary>
        /// <param name="updatePanel"></param>
        /// <param name="Javascript"></param>
        private static void InvokeJavascript(UpdatePanel updatePanel, String javascript)
        {
            string key = Guid.NewGuid().ToString();
            ScriptManager.RegisterStartupScript(updatePanel, typeof(UpdatePanel), key, javascript, true);
        }

        /// <summary>
        /// 处理Javascript脚本
        /// </summary>
        /// <param name="updatePanel"></param>
        /// <param name="key"></param>
        /// <param name="Javascript"></param>
        private static void InvokeJavascript(UpdatePanel updatePanel, String key, String javascript)
        {
            ScriptManager.RegisterStartupScript(updatePanel, typeof(UpdatePanel), key, javascript, true);
        }

        #endregion
    }
}

