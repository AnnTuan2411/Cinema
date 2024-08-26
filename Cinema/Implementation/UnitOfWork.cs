using Cinema.Models;
using Cinema.Repository_2;

namespace Cinema.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinemaContext _context;
        //private static unitofwork instance = null;
        //public static readonly object instancelock = new object();

        public UnitOfWork(CinemaContext context) 
        {
            _context = context;
            Employee = new EmployeeRepository(_context);
            Account = new AccountRepository(_context);
            Calendar = new CalendarRepository(_context);
            Booking = new BookingRepository(_context);
            MovieShow = new MovieShowRepository(_context);
            Ticket = new TicketRepository(_context);
            ServiceFeedBack = new ServiceFeedBackRepository(_context);
            MovieRating = new MovieRatingRepository(_context);
            Movie = new MovieRepository(_context);
        }

        public IEmployeeRepository Employee { get; set; }
        public IAccountRepository Account { get; set; }
        public ICalendarReposiitory Calendar {  get; set; }
        public IBookingRepository Booking { get; set; }
        public IMovieShowRepository MovieShow { get; set; }
        public ITicketRepository Ticket { get; set; }
        public IServiceFeedBackRepository ServiceFeedBack { get; set; }
        public IMovieRatingRepository MovieRating { get; set; }
        public IMovieRepository Movie { get; set; }
        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose() 
        {
            _context.Dispose();
        }
    }
}
