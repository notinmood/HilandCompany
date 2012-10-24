using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using HiLand.Utility.Algorithm;
using HiLand.Utility.Event;
using HiLand.Utility.Native;

namespace Hiland.Project.ADStatistics.UI
{
    public partial class UCBase : UserControl
    {
        public UCBase()
        {
            InitializeComponent();
            this.SetState();
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandle<EventControlEntity> BeforeClose;

        public event EventHandle<EventControlEntity> Close;

        public event EventHandle<EventControlEntity> AfterClose;

        public event EventHandle<EventControlEntity> BeforeSave;

        public event EventHandle<EventControlEntity> Save;

        public event EventHandle<EventControlEntity> AfterSave;

        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        private void SetState()
        {

        }

        protected virtual void OnBeforeClose(EventArgs<EventControlEntity> eventArgs)
        { 
            if (this.BeforeClose != null)
            {
                this.BeforeClose(this, eventArgs);
            }
        }

        protected virtual void OnClose(EventArgs<EventControlEntity> eventArgs)
        {
            if (eventArgs.Data.Token == true)
            {
                if (this.Close != null)
                {
                    this.Close(this, eventArgs);
                }

                Control parentControl = this.Parent;
                if (parentControl != null)
                {
                    ControlHelper.CrossThreadInvoke(this, CloseDetail, parentControl);
                }
            }
        }

        private void CloseDetail(Control parentControl)
        {
            parentControl.Controls.Remove(this);
            this.Dispose();
        }

        protected virtual void OnAfterClose(EventArgs<EventControlEntity> eventArgs)
        {
            if (eventArgs.Data.Token==true&&  this.AfterClose != null)
            {
                this.AfterClose(this, eventArgs);
            }
        }

        protected virtual void OnBeforeSave(EventArgs<EventControlEntity> eventArgs)
        {
            if (this.BeforeSave != null)
            {
                this.BeforeSave(this, eventArgs);
            }
        }

        protected virtual void OnSave(EventArgs<EventControlEntity> eventArgs)
        {
            if (eventArgs.Data.Token == true && this.Save != null)
            {
                Save(this, eventArgs);
            }
        }

        protected virtual void OnAfterSave(EventArgs<EventControlEntity> eventArgs)
        {
            if (eventArgs.Data.Token == true )
            {
                this.btnSave.Enabled = false;
                this.btnSave.Text = "保存成功";

                if(this.AfterSave != null)
                {
                    this.AfterSave(this, eventArgs);
                }

                if (eventArgs.Data.DelayOperationSecond > 0)
                {
                    backgroundWorker.WorkerReportsProgress = true;
                    backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                    backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                    backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
                    backgroundWorker.RunWorkerAsync(eventArgs);
                }
            }
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EventArgs<EventControlEntity> dd = (EventArgs<EventControlEntity>)e.Result;
            this.OnClose(dd);
        }

        void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.btnSave.Text = string.Format("保存成功({0}%)", e.ProgressPercentage);
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            EventArgs<EventControlEntity> dd = (EventArgs<EventControlEntity>)e.Argument;
            
            int delaySeconds= dd.Data.DelayOperationSecond;
            for (int i = 0; i < delaySeconds; i++)
            {
                int percent = (int)AlgorithmMisc.GetPercent(i, delaySeconds);
                backgroundWorker.ReportProgress(percent);
                Thread.Sleep(1000);
            }

            e.Result = dd;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            EventArgs<EventControlEntity> eventArgs = new EventArgs<EventControlEntity>();
            
            OnBeforeClose(eventArgs);

            OnClose(eventArgs);

            OnAfterClose(eventArgs);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            EventArgs<EventControlEntity> eventArgs = new EventArgs<EventControlEntity>();
           
            OnBeforeSave(eventArgs);

            OnSave(eventArgs);

            OnAfterSave(eventArgs);
        }
    }
}
