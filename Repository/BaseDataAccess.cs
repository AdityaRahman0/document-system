using System.Data;
using System.Data.SqlClient;

namespace Repository
{
    public class BaseDataAccess
    {
        protected static IDbConnection OpenConnection(string connection)
        {
            IDbConnection Connection = new SqlConnection(connection);
            Connection.Open();

            return Connection;
        }
    }
}
