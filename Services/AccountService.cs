
using Entities;
using Repository;

namespace Services
{
    public class AccountService
    {
        public User getValidUser(string connection, string user, string pass) 
        {
            AccountRepository AccountRepository = new AccountRepository();
            var result = AccountRepository.GetValidUSerAndPass(connection, user, pass);
            return result;
        }
    }
}
