using Cinema.Models;

namespace Cinema.Repository_2
{
    public interface ITicketRepository : IGenericRepository<Ticket>
    {
        IEnumerable<Ticket> GetTicketWithMovieShowAndMovie();
    }
}
