using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Implementation
{
    public class MovieShowRepository : GenericRepository<MovieShow>, IMovieShowRepository
    {
        public MovieShowRepository(CinemaContext context) : base(context)
        {
          
        }

        public IEnumerable<MovieShow> GetMovieShowWithMovieAndRoom()
        {
            var movieShowWithMovieAndRoom =
                _context.MovieShows
                .Include(m => m.Movie)
                .Include(m => m.Room)
                .ToList();
            return movieShowWithMovieAndRoom;
        }
    }
}
