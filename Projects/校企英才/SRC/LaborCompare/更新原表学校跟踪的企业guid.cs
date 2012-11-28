using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HiLand.Utility.Data;

namespace LaborCompare
{
    public partial class 更新原表学校跟踪的企业guid : BaseForm
    {
        public 更新原表学校跟踪的企业guid()
        {
            //InitializeComponent();
        }

        protected override string exportFileName
        {
            get { return "更新原表学校跟踪的企业guid.txt"; }
        }

        protected override void InnerDoWork(ref List<string> resultData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //0.1获取所有员工
            DataTable dtForEmployee = new DataTable();
            string commandStringForEmployee = "SELECT CorpId,NewCorpGuid FROM BD_School";
            SqlDataAdapter daForEmployee = new SqlDataAdapter(commandStringForEmployee, connectionString);
            daForEmployee.Fill(dtForEmployee);

            //1.获取Labor表的所有数据
            DataTable dtForAll = new DataTable();
            string commandStringForAll = "select ReplyId,CorpId from ReplySchool";
            SqlDataAdapter daForAll = new SqlDataAdapter(commandStringForAll, connectionString);
            daForAll.Fill(dtForAll);

            //2.匹配劳务人员各个服务角色的guid
            for (int i = 0; i < dtForAll.Rows.Count; i++)
            {
                DataRow drForAll = dtForAll.Rows[i];

                int ReplyId = 0;
                if (Convert.IsDBNull(drForAll["ReplyId"]) == false)
                {
                    ReplyId = Convert.ToInt32(drForAll["ReplyId"].ToString());
                }

                if (ReplyId != 0)
                {
                    string CorpId = string.Empty;
                    if (Convert.IsDBNull(drForAll["CorpId"]) == false)
                    {
                        CorpId = drForAll["CorpId"].ToString();
                    }

                    Guid NewCorpGuid = Guid.Empty;
                    foreach (DataRow row in dtForEmployee.Rows)
                    {
                        if (row["CorpId"].ToString() == CorpId)
                        {
                            NewCorpGuid = Converter.TryToGuid(row["NewCorpGuid"].ToString());
                            break;
                        }
                    }

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string commString = string.Format(@"UPDATE [ReplySchool]
                           SET [NewCorpGuid] = '{1}'
                          WHERE [ReplyId]= {0} ",
                            ReplyId,
                            NewCorpGuid);

                        using (SqlCommand comm = new SqlCommand(commString, conn))
                        {
                            comm.ExecuteNonQuery();
                        }
                    }
                }

                base.backgroundWorker1.ReportProgress(100 * i / dtForAll.Rows.Count);
            }
        }

        /// <summary>
        /// 获取服务人员的guid
        /// </summary>
        /// <param name="dtForEmployee"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static Guid GetEmployeeGuid(DataTable dtForEmployee, string userName)
        {
            Guid userGuid = Guid.Empty;
            if (string.IsNullOrWhiteSpace(userName) == false)
            {
                foreach (DataRow row in dtForEmployee.Rows)
                {
                    if (row["UserNameCN"].ToString() == userName)
                    {
                        userGuid = Converter.TryToGuid(row["UserGuid"].ToString());
                        break;
                    }
                }
            }
            return userGuid;
        }

        /// <summary>
        /// 获取信息员的guid
        /// </summary>
        /// <param name="dtForInformationBrokerGuid"></param>
        /// <param name="ProviderUserName"></param>
        /// <returns></returns>
        private static Guid GetInformationBrokerGuid(DataTable dtForInformationBrokerGuid, string userName)
        {
            Guid userGuid = Guid.Empty;
            if (string.IsNullOrWhiteSpace(userName) == false)
            {
                foreach (DataRow row in dtForInformationBrokerGuid.Rows)
                {
                    if (row["InformationBrokerName"].ToString() == userName)
                    {
                        userGuid = Converter.TryToGuid(row["InformationBrokerGuid"].ToString());
                        break;
                    }
                }
            }
            return userGuid;
        }
    }
}
