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
    public partial class 劳务人员前后两个批次对比 : BaseForm
    {
        public 劳务人员前后两个批次对比()
        {
            //InitializeComponent();
        }

        protected override void InnerDoWork(ref List<string> resultData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //1.获取LaborAll表的所有数据
            DataTable dtForAll = new DataTable();
            string commandStringForAll = "select [姓名],[身份证号码] from [LaborAll]";
            SqlDataAdapter daForAll = new SqlDataAdapter(commandStringForAll, connectionString);

            daForAll.Fill(dtForAll);


            //2.获取LaborImported表所有数据。
            DataTable dtForImported = new DataTable();
            string commandStringForImported = "select [姓名],[身份证号码] from [LaborImported]";
            SqlDataAdapter daForImported = new SqlDataAdapter(commandStringForImported, connectionString);

            daForImported.Fill(dtForImported);

            //3.比较两个表的数据
            int rowCountForAll = dtForAll.Rows.Count;
            for (int j = 0; j < dtForImported.Rows.Count; j++)
            {
                DataRow drForImported = dtForImported.Rows[j];

                bool isMartched = false;
                for (int i = 0; i < rowCountForAll; i++)
                {
                    DataRow drForAll = dtForAll.Rows[i];

                    if (drForAll["姓名"].ToString() == drForImported["姓名"].ToString() &&
                        drForAll["身份证号码"].ToString() == drForImported["身份证号码"].ToString())
                    {
                        isMartched = true;
                        break;
                    }
                }

                if (isMartched == false)
                {
                    resultData.Add(string.Format("{0}-{1}", drForImported["姓名"].ToString(), drForImported["身份证号码"].ToString()));
                }

                base.backgroundWorker1.ReportProgress(100 * j / rowCountForAll);
            }
        }

       

        protected override string exportFileName
        {
            get { return "Import有All没有差异.txt"; }
        }
    }
}
