using Entities;
using Repository;
using System.Collections.Generic;

namespace Services
{
    public class AccountService
    {
        private readonly AccountRepository repository = new AccountRepository();
        private readonly DocumentRepository documentRepository = new DocumentRepository();

        public User GetValidUser(string connection, string user, string pass) 
        {
            var result = repository.GetValidUSerAndPass(connection, user, pass);
            return result;
        }

        public void RegisterUser(string connection, User model) 
        {
            repository.RegisterUser(connection, model);
        }

        public IEnumerable<Department> GetDepartments(string connection)
        {
            return documentRepository.GetDepartments(connection);
        }
    }
}
