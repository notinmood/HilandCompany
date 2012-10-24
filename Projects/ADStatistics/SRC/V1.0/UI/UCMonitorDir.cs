using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HiLand.General.Entity;
using HiLand.Utility.Native;
using HiLand.General.BLL;
using HiLand.Utility.Enums;

namespace Hiland.Project.ADStatistics.UI
{
    public partial class UCMonitorDir : UCBase
    {
        public UCMonitorDir()
        {
            InitializeComponent();
            this.DisplayOriginalDir();
        }

        private string originalDir = string.Empty;
        private string newDir = string.Empty;

        private string adMonitorDIRName = "ADMonitorDIR";

        private void DisplayOriginalDir()
        {
            BasicSettingEntity entity = BasicSettingBLL.Instance.GetBySettingKey(adMonitorDIRName);
            this.lblMessage.Text = entity.SettingValue;
            this.btnSave.Enabled = false;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = this.folderBrowserDialog.ShowDialog();
            if (DialogMisc.ConfirmIsEffectButton(dialogResult) == true)
            {
                this.newDir = this.folderBrowserDialog.SelectedPath;

                if (this.newDir != this.originalDir)
                {
                    this.btnSave.Enabled = true;
                    this.lblMessage.Text = this.newDir;
                }
                else
                {
                    this.btnSave.Enabled = false;
                }
            }
        }

        protected override void OnSave(HiLand.Utility.Event.EventArgs<EventControlEntity> eventArgs)
        {
            base.OnSave(eventArgs);

            this.originalDir = this.newDir;

            BasicSettingEntity entity = new BasicSettingEntity();
            entity.CanUsable = Logics.True;
            entity.DisplayName = "监控目录";
            entity.SettingID = 1;
            entity.SettingKey = adMonitorDIRName;
            entity.SettingValue = this.newDir;

            bool isSuccessful= BasicSettingBLL.Instance.Update(entity);
            eventArgs.Data.Token = isSuccessful;
            eventArgs.Data.DelayOperationSecond = 3;
        }

        protected override void OnBeforeClose(HiLand.Utility.Event.EventArgs<EventControlEntity> eventArgs)
        {
            base.OnBeforeClose(eventArgs);

            if (this.newDir != this.originalDir)
            {
                bool isEffectButtion= MessageBoxHelper.Show("您改变的路径尚未保存，是否要退出？", "警告", MessageBoxIcon.Question);
                eventArgs.Data.Token = isEffectButtion;
            }
        }
    }
}
