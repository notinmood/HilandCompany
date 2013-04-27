using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Salary.Web.Utility
{
    public static class MessageHelper
    {









        /// <summary>
        /// 页面提示方法：普通
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageBox(String message)
        {
            PageMessageBox(message, false, false, false, false);
        }

        /// <summary>
        /// 页面提示方法：普通
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageConfirmClose(String message)
        {
            PageMessageBox(message, false, false, true, false);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageBoxAndRefreshParentByReload(String message)
        {
            PageMessageBox(message, true, false, false, true);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageAndRefreshParentByCurrHref(String message)
        {
            PageMessageBox(message, true, false, false, false);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageAndClose(String message)
        {
            PageMessageBox(message, false, true, false, false);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageAndRefreshParentByReloadAndClose(String message)
        {
            PageMessageBox(message, true, true, false, true);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageAndRefreshParentByCurrHrefAndClose(String message)
        {
            PageMessageBox(message, true, true, false, false);
        }

        /// <summary>
        /// 页面提示方法
        /// </summary>
        /// <param name="message">提示的语句</param>
        private static void PageMessageBox(String message, Boolean needReloadoPener, Boolean needCloseCurrentPage, Boolean isConfirm, Boolean isReloadParentWindow)
        {
            message = message.Replace("'", "").Replace("\r", "\\r").Replace("\n", "\\n");
            String a = String.Empty;
            if (!String.IsNullOrEmpty(message))
            {
                //alert语句
                a = isConfirm ? String.Format(@"if(confirm('{0}')){{window.open('','_self') ;window.close();}}", message) : String.Format(@"alert('{0}');", message);

                if (isConfirm)
                {
                    needReloadoPener = true;
                }
            }

            //刷新父窗口语句
            String r = needReloadoPener ?
                (isReloadParentWindow ?
                "if(parent!=null){if(parent.opener!=null){parent.opener.location.reload();}} else if(window.opener!=null){window.opener.location.reload();}" :
                "if(parent!=null){if(parent.opener!=null){parent.opener.location.href=parent.opener.location.href;}} else if(window.opener!=null){window.opener.location.href=window.opener.location.href;}") :
                String.Empty;

            //页面关闭语句
            String c = needCloseCurrentPage ? "window.open('','_self') ;window.close();" : String.Empty;

            String javaScript = String.Format(@"{0}{1}{2}", a, r, c);
            JavascriptHelper.Execute(javaScript);
        }

        public static void Redirect(String url)
        {
            String javaScript = String.Format("window.location.href='{0}';", url);
            JavascriptHelper.Execute(javaScript);
        }

    }
}

