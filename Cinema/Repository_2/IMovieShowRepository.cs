using Cinema.Models;

namespace Cinema.Repository_2
{
    public interface IMovieShowRepository : IGenericRepository<MovieShow>
    {
        IEnumerable<MovieShow> GetMovieShowWithMovieAndRoom();
    }
}
