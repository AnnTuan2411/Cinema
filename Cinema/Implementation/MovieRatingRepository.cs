using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Implementation
{
    public class MovieRatingRepository : GenericRepository<MovieRating>, IMovieRatingRepository
    {
        public MovieRatingRepository(CinemaContext context) : base(context)
        {

        }
        public IEnumerable<MovieRating> GetMovieRatingWithMovie()
        {
            var movieRatingWithMovies =
                _context.MovieRatings
                .Include(t => t.Movie)               
                .ToList();
            return movieRatingWithMovies;
        }
    }
}
