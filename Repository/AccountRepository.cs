using Dapper;
using Dapper.Contrib.Extensions;
using Entities;

namespace Repository
{
    public class AccountRepository : BaseDataAccess
    {
        public User GetValidUSerAndPass(string conncection, string user, string pass)
        {
            using (var Conn = OpenConnection(conncection))
            {
                string query = "Select * From test.Users Where UserName = @Username and Password = @Password";
                return Conn.QuerySingleOrDefault<User>(query, new { Username = user, Password = pass});
            }
        }

        public bool RegisterUser(string connection, User user) 
        {
            using (var Conn = OpenConnection(connection)) 
            {
                return Conn.Insert<User>(user) == 0;
            }
        }
    }
}
