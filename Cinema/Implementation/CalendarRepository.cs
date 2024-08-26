using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Cinema.Implementation
{
    public class CalendarRepository : GenericRepository<Calendar>, ICalendarReposiitory
    {
        public CalendarRepository(CinemaContext context) : base(context)
        {

        }

        public IEnumerable<Calendar> GetCalendarWithEmps()
        {
            var calendarWithEmps =
                _context.Calendars
                .Include(c => c.Emloyee)
                .Include(c => c.Emloyee.Acc)
                .ToList().OrderByDescending(x => x.CalendarId);
            return calendarWithEmps;
        }

        public Calendar GetCalendarDetailById(int id)
        {
            var calendarDetail = _context.Calendars.Include(c => c.Emloyee).Include(c => c.Emloyee.Acc)
                .Where(c => c.CalendarId == id)
                .FirstOrDefault();
            return calendarDetail;
        }
    }
}
