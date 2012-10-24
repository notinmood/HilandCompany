using System.Data.SqlClient;
using XQYC.Business.DALCommon;

namespace XQYC.Business.DAL
{
    public class EnterpriseServiceDAL : EnterpriseServiceCommonDAL<SqlTransaction, SqlConnection, SqlCommand, SqlDataReader, SqlParameter>
    {
    }
}
