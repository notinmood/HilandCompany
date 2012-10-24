using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace Hiland.Project.ADStatistics.UI
{
    public partial class HilandBaseForm : XtraForm
    {
        public HilandBaseForm()
        {
            InitializeComponent();
        }

        #region 窗体状态设置 （请在派生窗体中调用如下方法）
        /// <summary>
        /// 窗体显示最大化
        /// </summary>
        protected void SetWindowMaximized()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// 设置不出现最大最小化按钮
        /// </summary>
        protected void SetWindowLocked()
        {
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        #endregion
    }
}