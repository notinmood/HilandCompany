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

namespace LaborCompare
{
    public partial class 修改劳务人员中关联企业的guid : BaseForm
    {
        public 修改劳务人员中关联企业的guid()
        {
            
        }

        protected override string exportFileName
        {
            get { return "修改劳务人员中关联企业的guid.txt"; }
        }

        protected override void InnerDoWork(ref List<string> resultData)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //1.获取Labor表的所有数据
            DataTable dtForAll = new DataTable();
            string commandStringForAll = "select distinct(CurrentEnterpriseKey) from XQYCLabor";
            SqlDataAdapter daForAll = new SqlDataAdapter(commandStringForAll, connectionString);

            daForAll.Fill(dtForAll);

            //2.更新企业标志的{guid}为guid
            for (int i = 0; i < dtForAll.Rows.Count; i++)
            {
                DataRow drForAll = dtForAll.Rows[i];

                string enterpriseGuidOriginal = drForAll["CurrentEnterpriseKey"].ToString();
                string enterpriseGuidNew = enterpriseGuidOriginal;
                enterpriseGuidNew= enterpriseGuidNew.Trim('{', '}');

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string commString = string.Format("update [XQYCLabor] set [CurrentEnterpriseKey]= '{0}' where [CurrentEnterpriseKey]='{1}' ", enterpriseGuidNew,enterpriseGuidOriginal);
                    using (SqlCommand comm = new SqlCommand(commString, conn))
                    {
                        conn.Open();
                        comm.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
