using System.Data.SqlClient;
using XQYC.Business.DALCommon;

namespace XQYC.Business.DAL
{
    public class BoothForeOrderDAL : BoothForeOrderCommonDAL<SqlTransaction, SqlConnection, SqlCommand, SqlDataReader, SqlParameter>
    {

    }
}
