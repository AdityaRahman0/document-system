using Dapper;
using Entities;

namespace Repository
{
    public class AccountRepository : BaseDataAccess
    {
        public User GetValidUSerAndPass(string conncection, string user, string pass)
        {
            using (var Conn = OpenConnection(conncection))
            {
                string query = "Select * From test.Users with(nolock) Where UserName = @Username and Password = @Password";
                return Conn.QuerySingleOrDefault<User>(query, new { Username = user, Password = pass});
            }
        }

        public void RegisterUser(string connection, User model) 
        {
            using (var Conn = OpenConnection(connection)) 
            {
                var insertQuery = "INSERT INTO test.Users (UserName, Email, Role, Password, dtmUpd, usrUpd, Department) VALUES (@UserName, @Email, @Role, @Password, @dtmUpd, @usrUpd, @DepartmentId)";
                Conn.Execute(insertQuery, model);
            }
        }
    }
}
