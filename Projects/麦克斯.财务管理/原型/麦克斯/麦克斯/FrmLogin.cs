using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 麦克斯
{
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (this.txtUsername.Text == "admin" && this.txtPwd.Text == "admin")
            {
                FrmMain fm = new FrmMain();
                fm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("密码错误");
            }
        }
    }
}
