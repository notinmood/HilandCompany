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
    public partial class Logon : HilandBaseForm
    {
        public Logon()
        {
            InitializeComponent();
        }

        private void btnLogon_Click(object sender, EventArgs e)
        {
            //TODO:以后考虑使用数据库的方式对账号口令进行管理
            if (this.tbxUserName.Text == "admin" && this.tbxPassword.Text == "admin")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.lblMessage.Text = "账号或者口令有误，请重新输入。";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}