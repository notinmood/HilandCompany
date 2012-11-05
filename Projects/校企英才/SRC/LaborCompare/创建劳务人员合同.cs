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
using HiLand.Utility.DataBase;
using HiLand.Utility.Enums;

namespace LaborCompare
{
    public partial class 创建劳务人员合同 : BaseForm
    {
        public 创建劳务人员合同()
        {

        }

        protected override string exportFileName
        {
            get { return "创建劳务人员合同.txt"; }
        }

        protected override void InnerDoWork(ref List<string> resultData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //1.获取Labor表的所有数据
            DataTable dtForAll = new DataTable();
            string commandStringForAll = "select * from XQYCLabor ";
            SqlDataAdapter daForAll = new SqlDataAdapter(commandStringForAll, connectionString);

            daForAll.Fill(dtForAll);

            //2.创建劳务人员合同
            for (int i = 0; i < dtForAll.Rows.Count; i++)
            {
                DataRow drForAll = dtForAll.Rows[i];

                Guid userGuidString = GuidHelper.Empty;
                if (Convert.IsDBNull(drForAll["UserGuid"]) == false)
                {
                    userGuidString = new Guid(drForAll["UserGuid"].ToString());
                }

                string laborCode = string.Empty;
                if (Convert.IsDBNull(drForAll["LaborCode"]) == false)
                {
                    laborCode = drForAll["LaborCode"].ToString();
                }

                Guid currentEnterpriseKey = Guid.Empty;
                if (Convert.IsDBNull(drForAll["CurrentEnterpriseKey"]) == false)
                {
                    try
                    {
                        currentEnterpriseKey = new Guid(drForAll["CurrentEnterpriseKey"].ToString());
                    }
                    catch { }
                }

                DateTime CurrentContractStartDate = DateTimeHelper.Min;
                if (Convert.IsDBNull(drForAll["CurrentContractStartDate"]) == false)
                {
                    CurrentContractStartDate = Convert.ToDateTime(drForAll["CurrentContractStartDate"].ToString());
                }

                DateTime LaborContractStopDate = DateTimeHelper.Min;
                if (Convert.IsDBNull(drForAll["CurrentContractStopDate"]) == false)
                {
                    LaborContractStopDate = Convert.ToDateTime(drForAll["CurrentContractStopDate"].ToString());
                }

                DateTime LaborContractDiscontinueDate = DateTimeHelper.Min;
                if (Convert.IsDBNull(drForAll["CurrentContractDiscontinueDate"]) == false)
                {
                    LaborContractDiscontinueDate = Convert.ToDateTime(drForAll["CurrentContractDiscontinueDate"].ToString());
                }

                string LaborContractDetails = string.Empty;
                if (Convert.IsDBNull(drForAll["CurrentEnterpriseName"]) == false)
                {
                    LaborContractDetails = string.Format("与[{0}]的劳务合同", drForAll["CurrentEnterpriseName"].ToString());
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string commString = string.Format(@"INSERT INTO [XQYCLaborContract]
                           ([LaborContractGuid]
                           ,[LaborUserGuid]
                           ,[LaborCode]
                           ,[EnterpriseGuid]
                           ,[LaborContractStatus]
                           ,[LaborContractStartDate]
                           ,[LaborContractStopDate]
                           ,[LaborContractDetails]
                           ,[LaborContractDiscontinueDate]
                           ,[LaborContractIsCurrent]
                            )
                         VALUES
                            (
                                '{0}','{1}','{2}','{3}',20,'{4}','{5}','{6}','{7}',1
                            )", GuidHelper.NewGuid(), userGuidString, laborCode, currentEnterpriseKey, CurrentContractStartDate.ToString("yyyy/MM/dd"),
                              LaborContractStopDate.ToString("yyyy/MM/dd"), LaborContractDetails, LaborContractDiscontinueDate.ToString("yyyy/MM/dd"));
                    using (SqlCommand comm = new SqlCommand(commString, conn))
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }

                base.backgroundWorker1.ReportProgress(100 * i / dtForAll.Rows.Count);
            }
        }
    }
}
