using Cinema.Models;
using Cinema.Repository_2;

namespace Cinema.Implementation
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(CinemaContext context) : base(context)
        {

        }
    }
}
