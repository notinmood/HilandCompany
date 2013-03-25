using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace 麦克斯
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string name = this.txtName.Text;
            string sql = "";
            if (name=="")
            {
                sql = "select * from company";
            }
            else
            {
                sql = "select * from company where name like '" + name + "'";
            }
            FrmSearch fs = new FrmSearch(sql);
            fs.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cboGudong.SelectedIndex = 0;
            this.cboZiHao.SelectedIndex = 0;
            this.cboType.SelectedIndex = 0;
            this.cboArea.SelectedIndex = 0;
            this.cboBusinessType.SelectedIndex = 0;
            this.cboSearchType.SelectedIndex = 0;
        }

        private void dgvProcess_MouseClick(object sender, MouseEventArgs e)
        {
            //if (this.dgvProcess.SelectedRows.Count <= 0)
            //{
            //    return;
            //}
            FrmCompanyManage fcm = new FrmCompanyManage();
            fcm.ShowDialog();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip5_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dgvProcess_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cboZiHao_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox18_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
