using Cinema.Helpers;
using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cinema.Controllers.Admin
{
    public class AdminHomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminHomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {

            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                try
                {
                    ViewBag.SeccionCusId = HttpContext.Session.GetInt32("CusId");
                }
                catch (Exception ex)
                {
                    ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
                }
            }
            //----- Phan tram ve ban duoc trong thang truoc -----
            DateTime today = DateTime.Now;
            DateTime oneMonthAgo = today.AddMonths(-1);
            var tickets = _unitOfWork.Ticket.GetTicketWithMovieShowAndMovie();
            int currentMonthTicketCount = tickets
                .Where(ticket => ticket.Status == 1 && ticket.MovieShow.StartTime >= oneMonthAgo && ticket.MovieShow.StartTime < today)
                .Count();
            int previousMonthTicketCount = tickets
                .Where(ticket => ticket.Status == 1 && ticket.MovieShow.StartTime >= oneMonthAgo.AddMonths(-1) && ticket.MovieShow.StartTime < oneMonthAgo)
                .Count();
            double growthPercen = Math.Round(((double)(currentMonthTicketCount - previousMonthTicketCount) / previousMonthTicketCount) * 100, 2);
            ViewBag.TicketGrowthPercen = growthPercen;
            ViewBag.CurrentMonthTicketCount = currentMonthTicketCount;

            //------- Doanh thu thang truoc ---------
            var bookings = _unitOfWork.Booking.GetAll();
            double currentMonthMovieIncome = (double)bookings
                .Where(booking => booking.Status == 1 && booking.PuchaseDate >= oneMonthAgo && booking.PuchaseDate < today)
                .Sum(booking => booking.TotalPrice);
            double previousMonthMovieIncome = (double)bookings
                .Where(booking => booking.Status == 1 && booking.PuchaseDate >= oneMonthAgo.AddMonths(-1) && booking.PuchaseDate < oneMonthAgo)
                .Sum(booking => booking.TotalPrice);
            double growthMovieIncomePercen = Math.Round(((currentMonthMovieIncome - previousMonthMovieIncome) / previousMonthMovieIncome) * 100, 2);
            ViewBag.MovieIncomeGrowthPercen = growthMovieIncomePercen;
            ViewBag.CurrentMonthMovieIncome = Math.Round(currentMonthMovieIncome, 2);

            //if else session
            //---- listEmployees cho HR ----                                           
            var listEmps = _unitOfWork.Employee.GetEmpsWithAccountsForHR();
            ViewBag.ListEmps = listEmps;

            //---- listEmployees cho Admin ----
            //var listEmps = _unitOfWork.Employee.GetEmpsWithAccountsForAdmin();
            //ViewBag.ListEmps = listEmps;
            return View();
        }

        [HttpGet]
        public JsonResult LoadLineChart(int? selected)
        {
            if (!selected.HasValue)
            {
                selected = DateTime.Now.Year;
            }
            //var movieRatings = _unitOfWork.MovieRating.GetMovieRatingWithMovie();
            var movies = _unitOfWork.Movie.GetAll();
            var movieRatings = _unitOfWork.MovieRating.GetAll();
            var result = new List<ViewStatistics>();

            foreach (var movie in movies)
            {
                int starSum = 0;
                if (movie.StartDate.Year == selected.Value)
                {
                    foreach (var rating in movieRatings)
                    {
                        if (rating.MovieId == movie.MovieId)
                        {
                            starSum += rating.Star;
                        }
                    }
                    result.Add(new ViewStatistics
                    {
                        movieName = movie.Title,
                        percenTicketSold = starSum
                    });
                }
            }
            return Json(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}