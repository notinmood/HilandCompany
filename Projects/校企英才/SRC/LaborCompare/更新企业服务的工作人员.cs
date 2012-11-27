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
    public partial class 更新企业服务的工作人员 : BaseForm
    {
        public 更新企业服务的工作人员()
        {
            //InitializeComponent();
        }

        protected override string exportFileName
        {
            get { return "更新企业服务的工作人员.txt"; }
        }

        protected override void InnerDoWork(ref List<string> resultData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //0.1获取所有员工
            DataTable dtForEmployee = new DataTable();
            string commandStringForEmployee = "SELECT UserGuid,UserNameCN FROM CoreUser WHERE UserType=8 ";
            SqlDataAdapter daForEmployee = new SqlDataAdapter(commandStringForEmployee, connectionString);
            daForEmployee.Fill(dtForEmployee);

            //1.获取Labor表的所有数据
            DataTable dtForAll = new DataTable();
            string commandStringForAll = string.Format("select * from XQYCEnterpriseService BIZ LEFT JOIN [BD_Corp] BC ON BIZ.EnterpriseGuid= BC.NewCorpGuid where EnterpriseServiceType= 4 ", Guid.Empty);
            SqlDataAdapter daForAll = new SqlDataAdapter(commandStringForAll, connectionString);
            daForAll.Fill(dtForAll);

            //2.匹配劳务人员各个服务角色的guid
            for (int i = 0; i < dtForAll.Rows.Count; i++)
            {
                DataRow drForAll = dtForAll.Rows[i];

                int EnterpriseServiceID = 0;
                if (Convert.IsDBNull(drForAll["EnterpriseServiceID"]) == false)
                {
                    EnterpriseServiceID = Convert.ToInt32(drForAll["EnterpriseServiceID"].ToString());
                }

                if (EnterpriseServiceID != 0)
                {
                    string DeveloperName = string.Empty;
                    if (Convert.IsDBNull(drForAll["Developer"]) == false)
                    {
                        DeveloperName = drForAll["Developer"].ToString();
                    }

                    Guid DeveloperKey = GetEmployeeGuid(dtForEmployee, DeveloperName);

                    
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string commString = string.Format(@"UPDATE [XQYCEnterpriseService]
                           SET 
                            [BusinessUserGuid]='{1}',
                            [BusinessUserName]='{2}'
                         WHERE [EnterpriseServiceID]= '{0}' ",
                        EnterpriseServiceID,
                        DeveloperKey,
                        DeveloperName);

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
        /// 获取服务人员的guid
        /// </summary>
        /// <param name="dtForEmployee"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private static string GetEmployeeName(DataTable dtForEmployee, string userGuid)
        {
            string userName = string.Empty;
            if (GuidHelper.IsInvalidOrEmpty(userGuid)==false)
            {
                foreach (DataRow row in dtForEmployee.Rows)
                {
                    if (row["UserGuid"].ToString().ToLower() == userGuid.ToLower())
                    {
                        userName = row["UserNameCN"].ToString();
                        break;
                    }
                }
            }
            return userName;
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
