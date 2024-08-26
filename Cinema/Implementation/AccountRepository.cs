using Cinema.Models;
using Cinema.Repository_2;

namespace Cinema.Implementation
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository(CinemaContext context) : base(context)
        {
            
        }
    }
}
