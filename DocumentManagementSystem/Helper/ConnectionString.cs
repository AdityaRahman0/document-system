using System.Configuration;

namespace DocumentManagementSystem.Helper
{
    public class ConnectionString
    {
        public static string connectionString = ConfigurationManager.AppSettings["ConnString"].ToString();
    }
}