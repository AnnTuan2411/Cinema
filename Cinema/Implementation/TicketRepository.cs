using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Implementation
{
    public class TicketRepository : GenericRepository<Ticket>, ITicketRepository
    {
        public TicketRepository(CinemaContext context) : base(context)
        {

        }

        public IEnumerable<Ticket> GetTicketWithMovieShowAndMovie()
        {
            var ticketWithMovieShowAndMovie =
                _context.Tickets
                .Include(t => t.MovieShow)
                .Include(t => t.MovieShow.Movie)
                .ToList();
            return ticketWithMovieShowAndMovie;
        }
    }
}
