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
    public partial class 更新劳务人员对应的工作人员 : BaseForm
    {
        public 更新劳务人员对应的工作人员()
        {
            //InitializeComponent();
        }

        protected override string exportFileName
        {
            get { return "更新劳务人员对应的工作人员Guid.txt"; }
        }

        protected override void InnerDoWork(ref List<string> resultData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //0.1获取所有员工
            DataTable dtForEmployee = new DataTable();
            string commandStringForEmployee = "SELECT UserGuid,UserNameCN FROM CoreUser WHERE UserID<=55";
            SqlDataAdapter daForEmployee = new SqlDataAdapter(commandStringForEmployee, connectionString);
            daForEmployee.Fill(dtForEmployee);

            //0.2获取所有信息员
            DataTable dtForInformationBroker = new DataTable();
            string commandStringForInformationBroker = "select InformationBrokerGuid,InformationBrokerName from XQYCInformationBroker";
            SqlDataAdapter daForInformationBroker = new SqlDataAdapter(commandStringForInformationBroker, connectionString);
            daForInformationBroker.Fill(dtForInformationBroker);

            //1.获取Labor表的所有数据
            DataTable dtForAll = new DataTable();
            string commandStringForAll = "select * from XQYCLabor";
            SqlDataAdapter daForAll = new SqlDataAdapter(commandStringForAll, connectionString);
            daForAll.Fill(dtForAll);

            //2.匹配劳务人员各个服务角色的guid
            for (int i = 0; i < dtForAll.Rows.Count; i++)
            {
                DataRow drForAll = dtForAll.Rows[i];

                Guid userGuid = GuidHelper.Empty;
                if (Convert.IsDBNull(drForAll["UserGuid"]) == false)
                {
                    userGuid = new Guid(drForAll["UserGuid"].ToString());
                }

                if (userGuid != Guid.Empty)
                {
                    string ProviderUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["ProviderUserName"]) == false)
                    {
                        ProviderUserName = drForAll["ProviderUserName"].ToString();
                    }
                    Guid ProviderUserGuid = GetEmployeeGuid(dtForEmployee, ProviderUserName);

                    string RecommendUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["RecommendUserName"]) == false)
                    {
                        RecommendUserName = drForAll["RecommendUserName"].ToString();
                    }
                    Guid RecommendUserGuid = GetEmployeeGuid(dtForEmployee, RecommendUserName);

                    string ServiceUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["ServiceUserName"]) == false)
                    {
                        ServiceUserName = drForAll["ServiceUserName"].ToString();
                    }
                    Guid ServiceUserGuid = GetEmployeeGuid(dtForEmployee, ServiceUserName);

                    string FinanceUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["FinanceUserName"]) == false)
                    {
                        FinanceUserName = drForAll["FinanceUserName"].ToString();
                    }
                    Guid FinanceUserGuid = GetEmployeeGuid(dtForEmployee, FinanceUserName);

                    string BusinessUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["BusinessUserName"]) == false)
                    {
                        BusinessUserName = drForAll["BusinessUserName"].ToString();
                    }
                    Guid BusinessUserGuid = GetEmployeeGuid(dtForEmployee, BusinessUserName);

                    string SettleUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["SettleUserName"]) == false)
                    {
                        SettleUserName = drForAll["SettleUserName"].ToString();
                    }
                    Guid SettleUserGuid = GetEmployeeGuid(dtForEmployee, SettleUserName);

                    string InformationBrokerUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["InformationBrokerUserName"]) == false)
                    {
                        InformationBrokerUserName = drForAll["InformationBrokerUserName"].ToString();
                    }
                    Guid InformationBrokerGuid = GetInformationBrokerGuid(dtForInformationBroker, InformationBrokerUserName);

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        string commString = string.Format(@"UPDATE [XQYCLabor]
                           SET [ProviderUserGuid] = '{1}'
                              ,[RecommendUserGuid] = '{2}'
                              ,[ServiceUserGuid] = '{3}'
                              ,[FinanceUserGuid] = '{4}'
                              ,[BusinessUserGuid] = '{5}'
                              ,[SettleUserGuid] = '{6}'
                              ,[InformationBrokerUserGuid] = '{7}'
                         WHERE [UserGuid]= '{0}'",
                        userGuid,
                        ProviderUserGuid,
                        RecommendUserGuid,
                        ServiceUserGuid,
                        FinanceUserGuid,
                        BusinessUserGuid,
                        SettleUserGuid,
                        InformationBrokerGuid);
                        using (SqlCommand comm = new SqlCommand(commString, conn))
                        {
                            conn.Open();
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
