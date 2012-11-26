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
    public partial class 导入企业更新企业对应的工作人员 : BaseForm
    {
        public 导入企业更新企业对应的工作人员()
        {
            //InitializeComponent();
        }

        protected override string exportFileName
        {
            get { return "导入企业更新企业对应的工作人员.txt"; }
        }

        protected override void InnerDoWork(ref List<string> resultData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //0.1获取所有员工
            DataTable dtForEmployee = new DataTable();
            string commandStringForEmployee = "SELECT UserGuid,UserNameCN FROM CoreUser WHERE UserID<=55";
            SqlDataAdapter daForEmployee = new SqlDataAdapter(commandStringForEmployee, connectionString);
            daForEmployee.Fill(dtForEmployee);

            //1.获取Labor表的所有数据
            DataTable dtForAll = new DataTable();
            string commandStringForAll = "select * from GeneralEnterprise where EnterpriseID>15149"; //BrokerKey like '%AA+%' or BrokerKey like '%aa+%'";
            SqlDataAdapter daForAll = new SqlDataAdapter(commandStringForAll, connectionString);
            daForAll.Fill(dtForAll);

            //2.匹配劳务人员各个服务角色的guid
            for (int i = 0; i < dtForAll.Rows.Count; i++)
            {
                DataRow drForAll = dtForAll.Rows[i];

                int enterpriseID = 0;
                if (Convert.IsDBNull(drForAll["EnterpriseID"]) == false)
                {
                    enterpriseID = Convert.ToInt32(drForAll["EnterpriseID"].ToString());
                }

                if (enterpriseID != 0)
                {
                    Guid enterpriseGuid= Guid.Empty;
                     if (Convert.IsDBNull(drForAll["EnterpriseGuid"]) == false)
                    {
                        enterpriseGuid = new Guid(drForAll["EnterpriseGuid"].ToString());
                    }

                    string enterprseName= string.Empty;
                     if (Convert.IsDBNull(drForAll["CompanyName"]) == false)
                    {
                        enterprseName = drForAll["CompanyName"].ToString();
                    }

                    string createUserName = string.Empty;
                    if (Convert.IsDBNull(drForAll["CreateUserName"]) == false)
                    {
                        createUserName = drForAll["CreateUserName"].ToString();
                    }
                    Guid createUserKey = GetEmployeeGuid(dtForEmployee, createUserName);

                    DateTime createDate= DateTimeHelper.Min;
                    if (Convert.IsDBNull(drForAll["CreateDate"]) == false)
                    {
                        createDate = Convert.ToDateTime(drForAll["CreateDate"]);
                    }


                    int isProtected = 0;
                    int cooperateStatus = 0;
                    if (Convert.IsDBNull(drForAll["BrokerKey"]) == false)
                    {
                        string rankString = drForAll["BrokerKey"].ToString().ToLower();

                        if (rankString.Contains("aa+"))
                        {
                            cooperateStatus = 4;
                            isProtected = 1;
                        }
                    }

                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        string commString = string.Format(@"UPDATE [GeneralEnterprise]
                           SET [CreateUserKey] = '{1}',
                            [CooperateStatus]={2},
                            [IsProtectedByOwner]={3}
                         WHERE [EnterpriseID]= '{0}' ",
                        enterpriseID,
                        createUserKey,
                        cooperateStatus,
                        isProtected);
                        using (SqlCommand comm = new SqlCommand(commString, conn))
                        {
                            comm.ExecuteNonQuery();
                        }

                        if (cooperateStatus == 4)
                        {
                            string commString2 = string.Format(@"INSERT INTO [XQYCEnterpriseService]
                               ([EnterpriseServiceGuid]
                               ,[EnterpriseGuid]
                               ,[EnterpriseInfo]
                               ,[EnterpriseServiceType]
                               ,[EnterpriseServiceStatus]
                               ,[EnterpriseServiceCreateDate]
                               ,[EnterpriseServiceCreateUserKey]
                               ,[EnterpriseServiceStartDate]
                               ,[IsProtectedByOwner]
                                )
                         VALUES
                               ('{0}',
                                '{1}',
                                '{2}',
                                4,
                                1,
                                '{3}',
                                '{4}',
                                '{3}',
                                1
                                )", GuidHelper.NewGuid(),
                                  enterpriseGuid,
                                  enterprseName,
                                  createDate.ToString("yyyy/MM/dd"),
                                  createUserKey
                                  );
                            using (SqlCommand comm = new SqlCommand(commString2, conn))
                            {
                                comm.ExecuteNonQuery();
                            }
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
