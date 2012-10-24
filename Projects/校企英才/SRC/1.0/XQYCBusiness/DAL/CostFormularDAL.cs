using System.Data.SqlClient;
using XQYC.Business.DALCommon;

namespace XQYC.Business.DAL
{
    public class CostFormularDAL : InsuranceFundCommonDAL<SqlTransaction, SqlConnection, SqlCommand, SqlDataReader, SqlParameter>
    {
    }
}
