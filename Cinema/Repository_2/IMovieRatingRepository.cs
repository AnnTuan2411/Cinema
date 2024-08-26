using Cinema.Models;

namespace Cinema.Repository_2
{
    public interface IMovieRatingRepository : IGenericRepository<MovieRating>
    {
        IEnumerable<MovieRating> GetMovieRatingWithMovie();
    }
}
