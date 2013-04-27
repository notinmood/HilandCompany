using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Salary.Core.Utility;
using Salary.Web.Utility;

namespace Salary.Web.BasePage
{
    public class BaseWebForm : System.Web.UI.Page
    {

        #region 页面事件

        /// <summary>
        /// OnPreInit
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            var query = this.Request.QueryString;
            var queryText = query["$"];
            if (string.IsNullOrEmpty(queryText))
            {
                this.DecodedQueryString = query;
            }
            else
            {
                this.DecodedQueryString = HttpUtility.ParseQueryString(
                    CodeHelper.DecodeQueryString(queryText), Encoding.UTF8);
            }
        }

        /// <summary>
        /// OnLoad
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnLoad(EventArgs e)
        {
            //页面不用缓存
            Response.Buffer = true;
            Response.ExpiresAbsolute = System.DateTime.Now.AddSeconds(-1);
            Response.Expires = 0;
            Response.CacheControl = "no-cache";

            this.RegisterCSSAndJavascript();
            base.OnLoad(e);
        }

        #endregion

        #region Url以及Url传递参数处理

        /// <summary>
        /// 加密Url
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>加密后的URL</returns>
        protected string EncodeUrls(string url)
        {
            string resultUrl = this.ResolveUrl(url);
            var prefixIndex = resultUrl.IndexOf('?');
            if (prefixIndex >= 0 && true)
            {
                return string.Concat(resultUrl.Substring(0, prefixIndex), "?$=", CodeHelper.EncodeQueryString(resultUrl.Substring(prefixIndex + 1)));
            }
            return resultUrl;
        }

        /// <summary>
        /// 在URL路径增加 http:// 和 / 
        /// </summary>
        /// <param name="path">URL路径</param>
        /// <returns>增加后的字符串</returns>
        protected string ResolvePath(string path)
        {
            path = path.EndsWith("/") ? path : path + "/";
            return path.StartsWith("http://") ? path : this.ResolveUrl(path);
        }

        /// <summary>
        /// 解码后的QueryString
        /// </summary>
        protected NameValueCollection DecodedQueryString { get; set; }


        #endregion

        #region 注册CSS和JS


        /// <summary>
        /// 
        /// </summary>
        private void RegisterCSSAndJavascript()
        {
            Header.DataBind();
            RegisterJavascript(ResolveUrl(SysConst.CommonJavascript));
            RegisterJavascript(ResolveUrl(SysConst.JQuery));
            RegisterCSS(ResolveUrl(SysConst.BaseCss));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cssPath"></param>
        protected void RegisterCSS(string cssPath)
        {
            var include = new HtmlGenericControl("link");
            include.Attributes.Add("type", "text/css");
            include.Attributes.Add("rel", "stylesheet");
            include.Attributes.Add("href", cssPath);
            this.Header.Controls.AddAt(0, include);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scriptPath"></param>
        protected void RegisterJavascript(string scriptPath)
        {
            var include = new HtmlGenericControl("script");
            include.Attributes.Add("type", "text/javascript");
            include.Attributes.Add("language", "javascript");
            include.Attributes.Add("src", scriptPath);
            this.Header.Controls.AddAt(0, include);
        }

        #endregion

        #region 弹出提示以及执行JS

        /// <summary>
        /// 页面提示方法：普通
        /// </summary>
        /// <param name="message">提示的语句</param>
        public void MessageBox(String message)
        {
            MessageHelper.MessageBox(message);
        }

        /// <summary>
        /// 页面提示方法：普通
        /// </summary>
        /// <param name="message">提示的语句</param>
        public void MessageConfirmClose(String message)
        {
            MessageHelper.MessageConfirmClose(message);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public void MessageBoxAndRefreshParentByReload(String message)
        {
            MessageHelper.MessageBoxAndRefreshParentByReload(message);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public void MessageAndRefreshParentByCurrHref(String message)
        {
            MessageHelper.MessageAndRefreshParentByCurrHref(message);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public void MessageAndClose(String message)
        {
            MessageHelper.MessageAndClose(message);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public void MessageAndRefreshParentByReloadAndClose(String message)
        {
            MessageHelper.MessageAndRefreshParentByReloadAndClose(message);
        }

        /// <summary>
        /// 页面提示方法：刷新父窗口
        /// </summary>
        /// <param name="message">提示的语句</param>
        public void MessageAndRefreshParentByCurrHrefAndClose(String message)
        {
            MessageHelper.MessageAndRefreshParentByCurrHrefAndClose(message);
        }

        public void ExecuteJavascript(String javascript)
        {
            JavascriptHelper.Execute(javascript);
        }

        #endregion
    }
}
