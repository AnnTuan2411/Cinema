using Cinema.Models;
using Cinema.Repository_2;

namespace Cinema.Implementation
{
    public class ServiceFeedBackRepository : GenericRepository<ServiceFeedBack>, IServiceFeedBackRepository
    {
        public ServiceFeedBackRepository(CinemaContext context) : base(context)
        {

        }
    }
}
