using Cinema.Models;
using Cinema.Repository_2;

namespace Cinema.Implementation
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        public BookingRepository(CinemaContext context) : base(context)
        {

        }
    }
}
