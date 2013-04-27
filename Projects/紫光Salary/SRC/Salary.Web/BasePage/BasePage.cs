using System;
using System.Linq;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Salary.Core.Utility;
using Salary.Web.Utility;
using System.Web.UI.WebControls;
using Salary.Biz;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Salary.Web.BasePage
{
    public class BasePage : System.Web.UI.Page
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
            GridViewInit();
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
        protected void RegisterCSS(String href)
        {
            HtmlLink link = new HtmlLink();
            link.Href = href;
            link.Attributes.Add("rel", "stylesheet");
            link.Attributes.Add("type", "text/css");
            Header.Controls.Add(link);
        }

        public void RemoveCss(String href)
        {
            href = ResolveUrl(href);
            var findedCss = Header.Controls.OfType<HtmlLink>().FirstOrDefault(css => css.Href.Equals(href, StringComparison.InvariantCultureIgnoreCase));
            if (findedCss != null)
            {
                Page.Header.Controls.Remove(findedCss);
            }
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

        public void Redirect(String url)
        {
            String javaScript = String.Format("window.location.href = '{0}';", EncodeUrls(url));
            JavascriptHelper.Execute(javaScript);
        }

        public void ExecuteJavascript(String javascript)
        {
            JavascriptHelper.Execute(javascript);
        }

        public void RefreahParentPage(String relativeUrl)
        {
            String js = String.Format(@"
                var p = window.parent;
                if(p)
                {{
                    p.location.href = '{0}';}}
                else{{
                    window.location.href = '{0}';}}", ResolveUrl(relativeUrl));
            ExecuteJavascript(js);
        }

        #endregion

        #region OpenPage

        public const String OnClientClickString = "onclick";
        public const String OpenPageWithDimensionString = "openPageWithDimension";
        public const String OpenPageString = "openPage";
        public const String ShowPageWithDimensionString = "showPageWithDimension";
        public const String ShowPageString = "showPage";

        public void ControlClientClickBindShow<TControl>(TControl control, Int32 width, Int32 height, String relativeUrl)
        {
            ControlClientClickBind(control, ShowPageWithDimensionString, width, height, relativeUrl);
        }
        public void ControlClientClickBindShow<TControl>(TControl control, String relativeUrl)
        {
            ControlClientClickBind(control, ShowPageString, relativeUrl);
        }
        public void ControlClientClickBindOpen<TControl>(TControl control, Int32 width, Int32 height, String relativeUrl)
        {
            ControlClientClickBind(control, OpenPageWithDimensionString, width, height, relativeUrl);
        }
        public void ControlClientClickBindOpen<TControl>(TControl control, String relativeUrl)
        {
            ControlClientClickBind(control, OpenPageString, relativeUrl);
        }
        public void ControlClientClickBind<TControl>(TControl control, String javascriptKey, Int32 width, Int32 height, String relativeUrl)
        {
            String onclick = String.Format("{0}({1},{2},'{3}');return false;", javascriptKey, width, height, EncodeUrls(relativeUrl));
            WebControl webControl = control as WebControl;
            if (webControl != null)
            {
                webControl.Attributes.Add(OnClientClickString, onclick);
                return;
            }
            HtmlControl htmlControl = control as HtmlControl;
            if (htmlControl != null)
            {
                htmlControl.Attributes.Add(OnClientClickString, onclick);
                return;
            }
            throw new Exception("控件不包含Attributes属性。");
        }

        public void ControlClientClickBind<TControl>(TControl control, String javascriptKey, String relativeUrl)
        {
            String onclick = String.Format("{0}('{1}');return false;", javascriptKey, EncodeUrls(relativeUrl));
            WebControl webControl = control as WebControl;
            if (webControl != null)
            {
                webControl.Attributes.Add(OnClientClickString, onclick);
                return;
            }
            HtmlControl htmlControl = control as HtmlControl;
            if (htmlControl != null)
            {
                htmlControl.Attributes.Add(OnClientClickString, onclick);
                return;
            }
            throw new Exception("控件不包含Attributes属性。");
        }

        #endregion

        #region FindControls

        public List<T> FindControlsList<T>()
        {
            return Salary.Web.Utility.PageHelper.FindControlsList(this).OfType<T>().ToList();
        }

        #endregion

        #region Session

        public String GetSessionString(String name)
        {
            object session = Session[name];
            if (session == null)
            {
                return null;
            }
            return session.ToString();
        }

        #endregion

        #region UserInfo

        public UserInfo CurrentUserInfo
        {
            get
            {
                if (_CurrentUserInfo == null)
                {
                    _CurrentUserInfo = Session[UserInfoConst.ClassName] as UserInfo;
                    if (_CurrentUserInfo == null)
                    {
                        Response.Redirect(SalaryConst.TurnToLoginUrl);
                        //MessageHelper.Redirect(ResolveUrl(AccreditConst.LoginPageUrl));
                    }
                }
                return _CurrentUserInfo;
            }
            set
            {
                Session[UserInfoConst.ClassName] = _CurrentUserInfo = value;
            }
        }
        private UserInfo _CurrentUserInfo = null;

        /// <summary>
        /// 重新登录
        /// </summary>
        public void ReLogonSystem()
        {
            CurrentUserInfo = null;
            RefreahParentPage(SalaryConst.LoginPageUrl);
        }

        #endregion

        #region

        private void GridViewInit()
        {
            FindControlsList<GridView>().ToList().ForEach(gridview =>
            {
                gridview.CssClass = "gridView";
                gridview.HeaderStyle.CssClass = "gridHead";
                gridview.RowStyle.CssClass = "gridRow";
                gridview.EmptyDataRowStyle.CssClass = "gridEmptyData";
                //gridview.Attributes.Add("bordercolor", "#000000");
                //gridview.BorderColor = "#ECE9D8".to;
                //gridview.EmptyDataText = "无数据";
            });            
        }

        #endregion

        private void SetControlTextRTL(Control ctl)
        {
            if (ctl.GetType().Name == "TextBox")
            {
                TextBox tb = (TextBox)ctl;
                tb.Style.Add("direction", "rtl");
            }
            for (int i = 0; i < ctl.Controls.Count; i++)
            {
                Control ctlC = ctl.Controls[i];
                if (ctlC.GetType().Name == "TextBox")
                {
                    TextBox tb = (TextBox)ctlC;
                    tb.Style.Add("direction", "rtl");
                }
            }
        }

        public string ShowCorrectDateTime(string timeString)
        {
            if (timeString == string.Empty)
            {
                return string.Empty;
            }
            string dt = Convert.ToDateTime(timeString).ToString("yyyy-MM-dd");
            if (dt == "1900-01-01" || dt == "0001-01-01")
            {
                return string.Empty;
            }
            else
            {
                return dt;
            }
        }
    }
}
