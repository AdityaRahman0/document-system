using Dapper;
using Entities;
using PublicClass;
using System.Collections.Generic;

namespace Repository
{
    public class UserRepository : BaseDataAccess
    {
        public User GetManager(string conncection)
        {
            using (var Conn = OpenConnection(conncection))
            {
                var sql = "SELECT top 1 * FROM test.Users with (nolock) where Role = 'Manager' ";
                return Conn.QuerySingleOrDefault<User>(sql);
            }
        }

        public User GetUser(string connection, int userId) 
        {
            using (var Conn = OpenConnection(connection)) 
            {
                var query = "Select * from test.Users with (nolock) where UserId = @UserId";
                return Conn.QuerySingleOrDefault<User>(query, new { UserId = userId });
            }
        }

        public IEnumerable<User> GetNextUser(string conncection, string role, int documetnId)
        {
            using (var Conn = OpenConnection(conncection))
            {
                if (role.ToUpper() == Roles.DEPARTMENT.ToString())
                {
                    var query = @"
                            select u.* FROM test.Users u with (nolock) 
                            INNER JOIN test.DocumentRelatedDepartments da with(nolock) on u.Department = da.DepartmentId
                            where u.Role = @Role and da.DocumentId = @DocumentId ";
                    return Conn.Query<User>(query, new { Role = role, DocumentId = documetnId });
                }
                var sql = "SELECT top 1 * FROM test.Users with (nolock) where UPPER(Role) = @Role ";
                return Conn.Query<User>(sql, new { Role = role});
            }
        }
    }
}
