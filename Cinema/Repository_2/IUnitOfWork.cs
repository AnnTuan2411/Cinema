namespace Cinema.Repository_2
{
    public interface IUnitOfWork : IDisposable
    {
        IEmployeeRepository Employee { get; }
        IAccountRepository Account { get; }
        ICalendarReposiitory Calendar { get; }
        IBookingRepository Booking { get; }
        IMovieShowRepository MovieShow { get; }
        ITicketRepository Ticket { get; }
        IServiceFeedBackRepository ServiceFeedBack { get; }
        IMovieRatingRepository MovieRating { get; }
        IMovieRepository Movie { get; }
        int Save();
    }
}
