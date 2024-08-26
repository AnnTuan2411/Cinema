using Cinema.Models;

namespace Cinema.Repository_2
{
    public interface ICalendarReposiitory : IGenericRepository<Calendar>
    {
        IEnumerable<Calendar> GetCalendarWithEmps();
        Calendar GetCalendarDetailById(int id);
    }
}
