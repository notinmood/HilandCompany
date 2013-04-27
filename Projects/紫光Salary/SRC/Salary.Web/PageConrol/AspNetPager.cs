using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace Salary.Core.PageConrol
{
    [ToolboxData("<{0}:AspNetPager runat='server' PageSize='25' FirstPageText='首页' PrePageText='上一页' NextPageText='下一页' EndPageText='末页' ButtonText='GO'></{0}:AspNetPager>")]
    public class AspNetPager : WebControl, INamingContainer
    {
        #region 属性块
        private object baseState = null;
        private object buttonStyleState = null;
        private object textBoxStyleState = null;
        private object labelStyleState = null;
        private object linkButtonStyleState = null;
        private LinkButton _lnkbtnFrist;
        private LinkButton _lnkbtnPre;
        private LinkButton _lnkbtnNext;
        private LinkButton _lnkbtnLast;
        private Label _lblCurrentPage;
        private Label _lblRecodeCount;
        private Label _lblPageCount;
        private Label _lblPageSize;
        private TextBox _txtPageIndex;
        private LinkButton _btnChangePage;
        private static readonly object EventPageChange = new object();
        [Category("Pagination"), Description("每页显示的记录数"), DefaultValue("25")]
        public virtual int PageSize
        {
            get
            {
                EnsureChildControls();
                return _lblPageSize.Text.Trim() != "" ? int.Parse(_lblPageSize.Text.Trim()) : 25;
            }
            set
            {
                EnsureChildControls();
                _lblPageSize.Text = value.ToString();
            }
        }
        [Category("Pagination"), Description("总记录数"),
        DefaultValue("0"), Bindable(true)]
        public virtual int RecordCount
        {
            get
            {
                EnsureChildControls();
                return _lblRecodeCount.Text.Trim() != "" ? int.Parse(_lblRecodeCount.Text.Trim()) : 0;
            }
            set
            {
                EnsureChildControls();
                if (value >= 0)
                {
                    int recodeCount = value;
                    _lblPageCount.Text = (value % PageSize == 0 ? value / PageSize : value / PageSize + 1).ToString();//计算总页数
                }
                _lblRecodeCount.Text = value.ToString();
            }
        }
        [Category("Pagination"), Description("当前页码"),
        DefaultValue("1"), Bindable(true)]
        public virtual int PageIndex
        {
            get
            {
                EnsureChildControls();
                return _lblCurrentPage.Text.Trim() != "" ? int.Parse(_lblCurrentPage.Text.Trim()) : 1;
            }
            set
            {
                EnsureChildControls();
                _lblCurrentPage.Text = value.ToString();
            }
        }
        [Category("Appearance"), Description("设置第一页的文本"),
        DefaultValue("首页"), Bindable(true)]
        public virtual string FirstPageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnFrist.Text.Trim() != "" ? _lnkbtnFrist.Text.Trim() : "首页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnFrist.Text = value;
            }
        }
        [Category("Appearance"), Description("设置上一页的文本"),
        DefaultValue("上一页"), Bindable(true)]
        public virtual string PrePageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnPre.Text.Trim() != "" ? _lnkbtnPre.Text.Trim() : "上一页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnPre.Text = value;
            }
        }
        [Category("Appearance"), Description("设置下一页的文本"),
        DefaultValue("下一页"), Bindable(true)]
        public virtual string NextPageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnNext.Text.Trim() != "" ? _lnkbtnNext.Text.Trim() : "下一页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnNext.Text = value;
            }
        }
        [Category("Appearance"), Description("设置末页的文本"),
        DefaultValue("末页"), Bindable(true)]
        public virtual string EndPageText
        {
            get
            {
                EnsureChildControls();
                return _lnkbtnLast.Text.Trim() != "" ? _lnkbtnLast.Text.Trim() : "末页";
            }
            set
            {
                EnsureChildControls();
                _lnkbtnLast.Text = value;
            }
        }
        [Category("Appearance"), Description("设置跳转按钮的文本"),
        DefaultValue(":"), Bindable(true)]
        public virtual string ButtonText
        {
            get
            {
                EnsureChildControls();
                return _btnChangePage.Text.Trim() != "" ? _btnChangePage.Text.Trim() : "GO";
            }
            set
            {
                EnsureChildControls();
                _btnChangePage.Text = value;
            }
        }
        #endregion

        #region 分页事件相关
        public event EventHandler PageChanged
        {
            add
            {
                Events.AddHandler(EventPageChange, value);
            }
            remove
            {
                Events.RemoveHandler(EventPageChange, value);
            }

        }
        protected void OnPageChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)Events[EventPageChange];
            if (handler != null)
            {
                handler(this, e);
            }
        }
        #endregion

        #region 样式属性
        private Style _buttonStyle;
        private Style _textBoxStyle;

        private Style _linkButtonStyle;
        [Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description("应用于按钮的样式")]
        public virtual Style ButtonStyle
        {
            get
            {
                if (_buttonStyle == null)
                {
                    _buttonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_buttonStyle).TrackViewState();
                    }
                }
                return _buttonStyle;
            }
        }
        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
        DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description("应用于链接按钮的样式")]
        public virtual Style LinkButtonStyle
        {
            get
            {
                if (_linkButtonStyle == null)
                {
                    _linkButtonStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_linkButtonStyle).TrackViewState();
                    }
                }
                return _linkButtonStyle;
            }
        }

        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
        DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description("应用于文本框的样式")]
        public virtual Style TextBoxStyle
        {
            get
            {
                if (_textBoxStyle == null)
                {
                    _textBoxStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_textBoxStyle).TrackViewState();
                    }
                }
                return _textBoxStyle;
            }
        }
        private Style _labelStyle;
        [
        Category("Styles"),
        DefaultValue(null),
        DesignerSerializationVisibility(
        DesignerSerializationVisibility.Content),
        PersistenceMode(PersistenceMode.InnerProperty),
        Description("应用于标签的样式")]
        public virtual Style LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new Style();
                    if (IsTrackingViewState)
                    {
                        ((IStateManager)_labelStyle).TrackViewState();
                    }
                }
                return _labelStyle;
            }
        }
        #endregion

        #region 自定义视图状态
        protected override void LoadViewState(object savedState)
        {
            if (savedState == null)
            {
                base.LoadViewState(null);
                return;
            }
            else
            {
                Triplet t = savedState as Triplet;

                if (t != null)
                {
                    base.LoadViewState(baseState);

                    if ((t.Second) != null)
                    {
                        ((IStateManager)ButtonStyle).LoadViewState(buttonStyleState);
                    }

                    if ((t.Third) != null)
                    {
                        ((IStateManager)TextBoxStyle).LoadViewState(textBoxStyleState);
                    }
                    if (labelStyleState != null)
                    {
                        ((IStateManager)(_labelStyle)).LoadViewState(labelStyleState);
                    }
                    if (linkButtonStyleState != null)
                    {
                        ((IStateManager)(_linkButtonStyle)).LoadViewState(linkButtonStyleState);
                    }
                }
                else
                {
                    throw new ArgumentException("Invalid view state .");
                }
            }
        }

        protected override object SaveViewState()
        {
            baseState = base.SaveViewState();
            buttonStyleState = null;
            textBoxStyleState = null;
            labelStyleState = null;
            linkButtonStyleState = null;

            if (_buttonStyle != null)
            {
                buttonStyleState =
                ((IStateManager)_buttonStyle).SaveViewState();
            }

            if (_textBoxStyle != null)
            {
                textBoxStyleState =
                ((IStateManager)_textBoxStyle).SaveViewState();
            }
            if (_labelStyle != null)
            {
                labelStyleState = ((IStateManager)_labelStyle).SaveViewState();
            }
            if (_linkButtonStyle != null)
            {
                linkButtonStyleState = ((IStateManager)_linkButtonStyle).SaveViewState();
            }
            return new Triplet(baseState,
            buttonStyleState, textBoxStyleState);

        }

        protected override void TrackViewState()
        {
            base.TrackViewState();
            if (_buttonStyle != null)
            {
                ((IStateManager)_buttonStyle).TrackViewState();
            }
            if (_textBoxStyle != null)
            {
                ((IStateManager)_textBoxStyle).TrackViewState();
            }
            if (_labelStyle != null)
            {
                ((IStateManager)_labelStyle).TrackViewState();
            }
            if (_linkButtonStyle != null)
            {
                ((IStateManager)_linkButtonStyle).TrackViewState();
            }
        }
        #endregion

        #region 生成控件
        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            _btnChangePage = new LinkButton();
            _btnChangePage.ID = "btnChangePage";
            _btnChangePage.Click += new EventHandler(BtnChangePage_Click);
            _lblCurrentPage = new Label();
            _lblCurrentPage.ID = "lblCurrentPage";
            _lblCurrentPage.Text = "1";
            _lblPageCount = new Label();
            _lblPageCount.ID = "lblPageCount";
            _lblRecodeCount = new Label();
            _lblRecodeCount.ID = "lblRecodeCount";
            _lnkbtnFrist = new LinkButton();
            _lnkbtnFrist.ID = "lnkbtnFrist";
            _lnkbtnFrist.Click += new EventHandler(lnkbtnFrist_Click);
            _lnkbtnLast = new LinkButton();
            _lnkbtnLast.ID = "lnkbtnLast";
            _lnkbtnLast.Click += new EventHandler(lnkbtnLast_Click);
            _lnkbtnNext = new LinkButton();
            _lnkbtnNext.ID = "lnkbtnNext";
            _lnkbtnNext.Click += new EventHandler(lnkbtnNext_Click);
            _lnkbtnPre = new LinkButton();
            _lnkbtnPre.ID = "lnkbtnPre";
            _lnkbtnPre.Click += new EventHandler(lnkbtnPre_Click);
            _txtPageIndex = new TextBox();
            _txtPageIndex.ID = "txtPageIndex";
            _lblPageSize = new Label();
            _lblPageSize.ID = "lblPageSize";
            this.Controls.Add(_btnChangePage);
            this.Controls.Add(_lblCurrentPage);
            this.Controls.Add(_lblPageCount);
            this.Controls.Add(_lblRecodeCount);
            this.Controls.Add(_lnkbtnFrist);
            this.Controls.Add(_lnkbtnLast);
            this.Controls.Add(_lnkbtnNext);
            this.Controls.Add(_lnkbtnPre);
            this.Controls.Add(_txtPageIndex);
            base.CreateChildControls();
        }
        #endregion

        #region 按钮点击事件
        #region 翻页相关的事件
        /// <summary>
        /// 处理翻页事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkbtnFrist_Click(object sender, EventArgs e) //第一页
        {
            _lblCurrentPage.Text = "1";
            OnPageChanged(EventArgs.Empty);
        }
        protected void lnkbtnPre_Click(object sender, EventArgs e) //上一页
        {
            int pageIndex = int.Parse(_lblCurrentPage.Text);
            if (pageIndex > 0)
            {
                pageIndex--;
                _lblCurrentPage.Text = pageIndex.ToString();
                OnPageChanged(EventArgs.Empty);
            }
        }
        protected void lnkbtnNext_Click(object sender, EventArgs e)//下一页
        {
            int pageIndex = int.Parse(_lblCurrentPage.Text);
            int pageCount = int.Parse(_lblPageCount.Text);
            if (pageIndex < pageCount)
            {
                pageIndex++;
                _lblCurrentPage.Text = pageIndex.ToString();
            }
            OnPageChanged(EventArgs.Empty);
        }
        protected void lnkbtnLast_Click(object sender, EventArgs e)//末页
        {
            _lblCurrentPage.Text = _lblPageCount.Text;

            OnPageChanged(EventArgs.Empty);
        }
        #endregion
        protected void BtnChangePage_Click(object sender, EventArgs e)//跳转到指定页
        {
            int pageIndex = 0;
            try
            {
                pageIndex = int.Parse(_txtPageIndex.Text);
            }
            catch
            {
                MessageBox("请输入正确的页数!");
                return;
            }
            int pageCount = int.Parse(_lblPageCount.Text);
            if (pageIndex == 0)//如果为0，则提示错误
            {
                MessageBox("请输入正确的页数!");
                return;
            }
            if (pageIndex > pageCount)//如果大于总页数则提示错误
            {
                MessageBox("请输入正确的页数!");
                return;
            }
            _lblCurrentPage.Text = pageIndex.ToString();
            OnPageChanged(EventArgs.Empty);
        }
        #endregion

        #region 重写TagKey
        protected override HtmlTextWriterTag TagKey
        {
            get
            {
                return HtmlTextWriterTag.Table;
            }
        }
        #endregion

        #region 绘制控件
        protected override void RenderContents(HtmlTextWriter writer)
        {
            if (ButtonStyle != null)
            {
                _btnChangePage.ApplyStyle(ButtonStyle);
            }
            if (TextBoxStyle != null)
            {
                _txtPageIndex.ApplyStyle(TextBoxStyle);
            }
            if (LabelStyle != null)
            {
                _lblCurrentPage.ApplyStyle(LabelStyle);
                _lblPageCount.ApplyStyle(LabelStyle);
                _lblRecodeCount.ApplyStyle(LabelStyle);
                _lblPageSize.ApplyStyle(LabelStyle);
            }
            AddAttributesToRender(writer);
            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
            writer.AddAttribute(HtmlTextWriterAttribute.Align, "");
            writer.RenderBeginTag(HtmlTextWriterTag.Td);
            writer.Write("当前第");
            if (_lblCurrentPage != null)
                _lblCurrentPage.RenderControl(writer);
            writer.Write("页,每页");
            if (_lblPageSize != null)
            {
                _lblPageSize.RenderControl(writer);
            }
            writer.Write("条记录,总共");
            if (_lblRecodeCount != null)
                _lblRecodeCount.RenderControl(writer);
            writer.Write("条记录,共");
            if (_lblPageCount != null)
                _lblPageCount.RenderControl(writer);
            writer.Write("页&nbsp;&nbsp;[&nbsp;&nbsp;");
            if (_lnkbtnFrist != null)
            {
                if (PageIndex == 1) //如果是第一页，则第一页灰显，作用是避免不必要的点击造成没必要的数据传输
                {
                    _lnkbtnFrist.Enabled = false;
                }
                else
                {
                    _lnkbtnFrist.Enabled = true;
                }
                _lnkbtnFrist.RenderControl(writer);
            }
            writer.Write("&nbsp;");
            if (_lnkbtnPre != null)
            {
                if (PageIndex > 1) //如果当前页大于1，则上一页显示，否则灰显
                {
                    _lnkbtnPre.Enabled = true;
                }
                else
                {
                    _lnkbtnPre.Enabled = false;
                }
                _lnkbtnPre.RenderControl(writer);
            }
            writer.Write("&nbsp;");
            if (_lnkbtnNext != null)
            {
                if (_lblPageCount == null)
                {
                    _lnkbtnNext.Enabled = false;
                }
                else
                {
                    int pageCount = int.Parse(_lblPageCount.Text); //获取总页数
                    if (PageIndex < pageCount)//如果当前页小于总页数，则下一页显示，否则灰显
                    {
                        _lnkbtnNext.Enabled = true;
                    }
                    else
                    {
                        _lnkbtnNext.Enabled = false;
                    }
                }
                _lnkbtnNext.RenderControl(writer);
            }
            writer.Write("&nbsp;");
            if (_lnkbtnLast != null)
            {
                if (_lblPageCount == null)
                {
                    _lnkbtnLast.Enabled = false;
                }
                else
                {
                    int pageCount = int.Parse(_lblPageCount.Text); //获取总页数
                    if (PageIndex == pageCount || pageCount == 0)//如果当前页为最后一页，则末页灰显
                    {
                        _lnkbtnLast.Enabled = false;
                    }
                    else
                    {
                        _lnkbtnLast.Enabled = true;
                    }
                }
                _lnkbtnLast.RenderControl(writer);
            }
            writer.Write("&nbsp;&nbsp;]&nbsp;&nbsp;跳转到第");
            if (_txtPageIndex != null)
                _txtPageIndex.RenderControl(writer);
            writer.Write("页");
            if (_btnChangePage != null)
                _btnChangePage.RenderControl(writer);
            writer.RenderEndTag();
            writer.RenderEndTag();
            //base.RenderContents(writer);
        }
        #endregion

        #region 页面提示方法
        /// <summary>
        /// 页面提示方法：普通
        /// </summary>
        /// <param name="message">提示的语句</param>
        public static void MessageBox(string message)
        {
            PageMessageBox(message, false, false, false, false);
        }

        /// <summary>
        /// 页面提示方法
        /// </summary>
        /// <param name="message">提示的语句</param>
        private static void PageMessageBox(string message, bool needReloadoPener, bool needCloseCurrentPage, bool isConfirm, bool isReloadParentWindow)
        {
            message = message.Replace("'", "").Replace("\r", "\\r").Replace("\n", "\\n");
            string a = string.Empty;
            if (!string.IsNullOrEmpty(message))
            {
                //alert语句
                a = isConfirm ? string.Format(@"if(confirm('{0}')){{window.open('','_self') ;window.close();}}", message) : string.Format(@"alert('{0}');", message);

                if (isConfirm)
                {
                    needReloadoPener = true;
                }
            }

            //刷新父窗口语句
            string r = needReloadoPener ?
                (isReloadParentWindow ?
                "if(parent!=null){if(parent.opener!=null){parent.opener.location.reload();}} else if(window.opener!=null){window.opener.location.reload();}" :
                "if(parent!=null){if(parent.opener!=null){parent.opener.location.href=parent.opener.location.href;}} else if(window.opener!=null){window.opener.location.href=window.opener.location.href;}") :
                string.Empty;

            //页面关闭语句
            string c = needCloseCurrentPage ? "window.open('','_self') ;window.close();" : string.Empty;

            string javaScript = string.Format(@"{0}{1}{2}", a, r, c);
            InvokeJavaScriptWay(javaScript);
        }

        /// <summary>
        /// 页面提示方法
        /// </summary>
        /// <param name="message">提示的语句</param>
        private static void InvokeJavaScriptWay(string javaScript)
        {
            System.Web.UI.Page page = HttpContext.Current.CurrentHandler as Page;
            Type type = page.GetType();
            FieldInfo[] fieldInfos = type.GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            InvokeJavaScript(javaScript);

        }

        #region 非Ajax处理方法

        /// <summary>
        /// 处理JavaScript脚本
        /// </summary>
        /// <param name="javaScript"></param>
        private static void InvokeJavaScript(string javaScript)
        {
            string key = Guid.NewGuid().ToString();
            InvokeJavaScript(key, javaScript);
        }

        /// <summary>
        /// 处理JavaScript脚本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="javaScript"></param>
        private static void InvokeJavaScript(string key, string javaScript)
        {
            (HttpContext.Current.CurrentHandler as Page).ClientScript.RegisterStartupScript(typeof(System.Web.UI.Page), key, javaScript, true);
        }

        #endregion



        #endregion
    }
}