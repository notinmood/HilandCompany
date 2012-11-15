using System.Data.SqlClient;
using XQYC.Business.DALCommon;

namespace XQYC.Business.DAL
{
    public class EnterpriseJobDAL : EnterpriseJobCommonDAL<SqlTransaction, SqlConnection, SqlCommand, SqlDataReader, SqlParameter>
    {
    }
}
