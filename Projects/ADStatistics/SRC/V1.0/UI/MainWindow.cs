using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraEditors;


namespace Hiland.Project.ADStatistics.UI
{
    partial class MainWindow : HilandBaseForm
    {
        public MainWindow()
        {
            //TODO:交付时，将以下屏蔽信息进行取消。
            //if (UserLogin()==false)
            //{
            //    //登录被取消退出系统，这一句很重要 
            //    System.Environment.Exit(0);
            //} 

            InitializeComponent();
            this.SetWindowMaximized();
            InitGrid();
        }

        #region 辅助方法
        /// <summary>
        /// 在容器中设置新控件（同时清除容器内既有的控件）
        /// </summary>
        private void SetNewControlToContainer(Control newControl)
        {
            this.splitContainerControl.Panel2.Controls.Clear();
            newControl.Dock = DockStyle.Fill;
            this.splitContainerControl.Panel2.Controls.Add(newControl);
        }
        #endregion

        void InitGrid()
        {
            BindingList<Person> gridDataList = new BindingList<Person>();
            gridDataList.Add(new Person("John", "Smith"));
            gridDataList.Add(new Person("Gabriel", "Smith"));
            gridDataList.Add(new Person("Ashley", "Smith", "some comment"));
            gridDataList.Add(new Person("Adrian", "Smith", "some comment"));
            gridDataList.Add(new Person("Gabriella", "Smith", "some comment"));
            gridControl.DataSource = gridDataList;
        }

        private bool UserLogin()
        {
            Logon frmLogin = new Logon();
            try
            {
                DialogResult result = frmLogin.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            finally
            {
                frmLogin.Dispose();
            }
        }

        private void itemScanImage_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Control com = new UserControl1();
            this.SetNewControlToContainer(com);
        }



        private void itemMonitorDirSetting_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Control com = new UCMonitorDir();
            this.SetNewControlToContainer(com);
            
            //MonitorDir monitorDir = new MonitorDir();
            //monitorDir.ShowDialog();

        }




        private void outboxItem_LinkClicked(object sender, DevExpress.XtraNavBar.NavBarLinkEventArgs e)
        {
            Control com = new UserControl2();
            this.SetNewControlToContainer(com);
        }
    }
}