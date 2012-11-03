using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LaborCompare
{
    public abstract partial class BaseForm : Form
    {
        /// <summary>
        /// 导出文件的名称
        /// </summary>
        protected abstract string exportFileName { get; }

        public BaseForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerAsync();
        }

        void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.label1.Text = e.ProgressPercentage + "%";
        }

        void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.label1.Text = "完成";
            using (FileStream fileStream = new FileStream(exportFileName, FileMode.OpenOrCreate))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    List<string> resultData = (List<string>)e.Result;
                    streamWriter.WriteLine("总的记录数为{0}", resultData.Count);
                    foreach (string item in resultData)
                    {
                        streamWriter.WriteLine(item);
                    }
                    streamWriter.Flush();
                }
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> resultData = new List<string>();

            InnerDoWork(ref resultData);

            e.Result = resultData;
        }

        /// <summary>
        /// 在内部处理两件事情
        ///1、resultData.Add("");
        ///2、backgroundWorker1.ReportProgress(X);
        /// </summary>
        /// <param name="resultData"></param>
        protected virtual void InnerDoWork(ref List<string> resultData)
        {
            
        }
    }
}
