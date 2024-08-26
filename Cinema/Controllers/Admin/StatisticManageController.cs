using Cinema.Helpers;
using Cinema.Repository_2;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Admin
{
    public class StatisticsManageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatisticsManageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                var count = 0;
                if (movie.StartDate.Year == selected.Value)
                {
                    foreach (var rating in movieRatings)
                    {
                        if (rating.MovieId == movie.MovieId)
                        {
                            starSum += rating.Star;
                            count++;
                        }
                    }
                    result.Add(new ViewStatistics
                    {
                        movieName = movie.Title,
                        percenTicketSold = starSum/count
                    });
                }
            }
            return Json(result);
        }

        [HttpGet]
        public JsonResult LoadDoughnutChart(int? selected)
        {
            if (!selected.HasValue)
            {
                selected = DateTime.Now.Year;
            }
            var tickets = _unitOfWork.Ticket.GetAll();
            var movieShows = _unitOfWork.MovieShow.GetMovieShowWithMovieAndRoom();
            var result = new List<ViewStatistics>();

            foreach (var movieShow in movieShows)
            {
                int count = 0;
                if (movieShow.StartTime.Year == selected.Value)
                {
                    foreach (var ticket in tickets)
                    {
                        if (movieShow.MovieShowId == ticket.MovieShowId && ticket.Status == 1)
                        {
                            count++;
                        }
                    }
                    result.Add(new ViewStatistics
                    {
                        movieName = movieShow.Movie.Title,
                        percenTicketSold = Math.Round(((double)count / movieShow.Room.NumberOfSeats) * 100, 2)
                    });
                }
            }
            return Json(result);
        }

        [HttpGet]
        public JsonResult LoadBarChart(int? selected)
        {
            if (!selected.HasValue)
            {
                selected = DateTime.Now.Year;
            }
            var dataForYearNow = GetDataBarChartForYear(selected.Value);
            dataForYearNow.Reverse();
            return Json(dataForYearNow);
        }

        public ActionResult Index()
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
            //Thong ke doanh thu
            var bookings = _unitOfWork.Booking.GetAll();
            var availableYears = bookings.Select(booking => booking.PuchaseDate.Year).Distinct().ToList();
            ViewBag.AvailableYears = availableYears;
            ViewBag.SelectedYear = DateTime.Now.Year;

            //thong ke phan tram ve da ban so voi so voi tong so ve
            var movieShows = _unitOfWork.MovieShow.GetAll();
            var availableMovieShowYears = movieShows.Select(movieShow => movieShow.StartTime.Year).Distinct().ToList();
            ViewBag.AvailableMovieShowYears = availableMovieShowYears;
            ViewBag.SelectedMovieShowYear = DateTime.Now.Year;

            //thong ke feedback ve dich vu rap phim
            var movies = _unitOfWork.Movie.GetAll();
            var availableMoviesYears = movies.Select(movie => movie.StartDate.Year).Distinct().ToList();
            ViewBag.AvailableMoviesYears = availableMoviesYears;
            ViewBag.SelectedMoviesYear = DateTime.Now.Year;
            return View();
        }

        private List<GroupedBookingViewModelHelper> GetDataBarChartForYear(int selectedYear)
        {
            var bookings = _unitOfWork.Booking.GetAll();
            var groupedByMonthYear = bookings
                .Where(booking => booking.Status == 1 && booking.PuchaseDate.Year == selectedYear)
                .GroupBy(booking => new { booking.PuchaseDate.Year, booking.PuchaseDate.Month })
                .Select(group => new GroupedBookingViewModelHelper
                {
                    Month = group.Key.Month,
                    Year = group.Key.Year,
                    TotalAmount = group.Sum(booking => booking.TotalPrice),
                    CurrentYear = selectedYear
                })
                .ToList();
            return groupedByMonthYear;
        }

        // GET: StatisticsManageController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: StatisticsManageController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StatisticsManageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticsManageController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StatisticsManageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: StatisticsManageController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StatisticsManageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
