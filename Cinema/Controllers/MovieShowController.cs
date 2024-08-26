using Cinema.Models;
using Cinema.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlTypes;
using System.Runtime.Serialization;

namespace Cinema.Controllers
{
    public class MovieShowController : Controller
    {
        private IGenericRepository<MovieShow> _repository;

        public MovieShowController()
        {
            _repository = new GenericRepository<MovieShow>(new CinemaContext());
        }

        static string GenerateRandomString()
        {

            string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";


            Random random = new Random();

            string randomString = new string(Enumerable.Repeat(characters, 8)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());

            return randomString;
        }

        public ActionResult Index(DateTime date)
        {
            if(date < DateTime.Today)
            {
                date = DateTime.Today;
            }
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
            CinemaContext context = new CinemaContext();
            List<MovieShow> movieShows = _repository.GetAll().Where(row => row.Status == "True" && row.StartTime.Date == date && row.StartTime.AddMinutes(5) >= DateTime.Now).ToList();
            List<Movie> movieList = new List<Movie>();
            foreach (var movieShow in movieShows)
            {
                Movie movie = context.Movies.Where(row => row.MovieId == movieShow.MovieId).FirstOrDefault();
                if (!movieList.Contains(movie))
                {
                    movieList.Add(movie);
                }
            }
            ViewBag.Movies = movieList;
            ViewBag.Date = date;
            return View(movieShows);
        }

        public ActionResult Create()
        {
            CinemaContext context = new CinemaContext();
            List<Movie> movies = context.Movies.Where(row => row.Status == 1 && row.FinishDate > DateTime.Now).ToList();
            ViewBag.Movies = movies;
            List<Room> roomList = context.Rooms.Where(row => row.Status == 1).ToList();
            ViewBag.Rooms = roomList;
            ViewBag.Error = "";
            return View();
        }
        [HttpPost]
        public ActionResult Create(MovieShow movieShow)
        {
            CinemaContext context = new CinemaContext();
            String movieError = "";
            String roomError = "";
            Movie movie = context.Movies.Where(row => row.MovieId == movieShow.MovieId).FirstOrDefault();
            movieShow.EndTime = movieShow.StartTime.AddMinutes(movie.Duration + 15);
            List<Movie> movies = context.Movies.Where(row => row.Status == 1 && row.FinishDate > DateTime.Now).ToList();
            ViewBag.Movies = movies;
            List<Room> roomList = context.Rooms.Where(row => row.Status == 1).ToList();
            ViewBag.Rooms = roomList;
            if (movieShow.StartTime > movie.FinishDate || movieShow.StartTime < movie.StartDate)
            {
                movieError = "Movie is invalid at this Show time!";
                ViewBag.Error = movieError;
                return View(movieShow);
            }
            List<MovieShow> shows = _repository.GetAll().Where(row => row.Status == "True" && row.StartTime >= movieShow.StartTime && row.StartTime < movieShow.EndTime || row.EndTime > movieShow.StartTime && row.EndTime <= movieShow.EndTime).ToList();
            foreach (var show in shows)
            {
                if (show.RoomId == movieShow.RoomId)
                {
                    roomError = "Room is invalid at this Show time!";
                    ViewBag.Error = roomError;
                    return View(movieShow);
                }
            }
            //try
            //{   

            _repository.Insert(movieShow);
            _repository.Save();
            MovieShow movieShow1 = context.MovieShows.OrderByDescending(x => x.MovieShowId).FirstOrDefault();
            List<Seat> seats = context.Seats.Where(row => row.Status == 1).ToList();
            foreach (var seat in seats)
            {
                if (seat.RoomId == movieShow1.RoomId)
                {
                    Ticket tiket = new Ticket(GenerateRandomString(), movieShow1.MovieShowId, seat.SeatId, (movieShow1.Price + seat.Price));
                    context.Tickets.Add(tiket);
                    context.SaveChanges();
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    string Error = "Error!";
            //    ViewBag.Error = Error;
            //    return View(movieShow);
            //}
            return RedirectToAction("Index");
        }

        public ActionResult Detail(int id)
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
  
            CinemaContext context = new CinemaContext();
            MovieShow movieShow = _repository.GetById(id);
            if(movieShow.StartTime.AddMinutes(5) < DateTime.Now)
            {
                return RedirectToAction("Index");
            }
            Movie movie = context.Movies.Where(row => row.MovieId == movieShow.MovieId).FirstOrDefault();
            List<Ticket> tickets = context.Tickets.Where(row => row.MovieShowId == movieShow.MovieShowId && row.Status == 0).ToList();
            List<Seat> seats = new List<Seat>();
            foreach(Ticket ticket in tickets)
            {
                Seat s = context.Seats.Where(row => row.SeatId == ticket.SeatId).FirstOrDefault();
                if(s != null)
                {
                    seats.Add(s);
                }
            }
            ViewBag.Seats = seats;
            ViewBag.Tickets = tickets;
            ViewBag.Movie = movie;
            return View(movieShow);
        }

        public ActionResult MovieShowByMovie(int movieId)
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
            CinemaContext context = new CinemaContext();
            Movie movie = context.Movies.Where(row => row.MovieId == movieId).FirstOrDefault();
            List<MovieShow> movieShows1 = context.MovieShows.Where(row => row.MovieId == movieId && row.StartTime.AddMinutes(5) >= DateTime.Now && row.StartTime.Date == DateTime.Today).OrderBy(row => row.StartTime).ToList();
            ViewBag.MovieShows1 = movieShows1;
            List<MovieShow> movieShows2 = context.MovieShows.Where(row => row.MovieId == movieId && row.StartTime.Date == DateTime.Today.AddDays(1)).OrderBy(row => row.StartTime).ToList();
            ViewBag.MovieShows2 = movieShows2;
            List<MovieShow> movieShows3 = context.MovieShows.Where(row => row.MovieId == movieId && row.StartTime.Date == DateTime.Today.AddDays(2)).OrderBy(row => row.StartTime).ToList();
            ViewBag.MovieShows3 = movieShows3;
            List<MovieShow> movieShows4 = context.MovieShows.Where(row => row.MovieId == movieId && row.StartTime.Date == DateTime.Today.AddDays(3)).OrderBy(row => row.StartTime).ToList();
            ViewBag.MovieShows4 = movieShows4;
            List<MovieShow> movieShows5 = context.MovieShows.Where(row => row.MovieId == movieId && row.StartTime.Date == DateTime.Today.AddDays(4)).OrderBy(row => row.StartTime).ToList();
            ViewBag.MovieShows5 = movieShows5;
            List<MovieShow> movieShows6 = context.MovieShows.Where(row => row.MovieId == movieId && row.StartTime.Date == DateTime.Today.AddDays(5)).OrderBy(row => row.StartTime).ToList();
            ViewBag.MovieShows6 = movieShows6;
            List<MovieShow> movieShows7 = context.MovieShows.Where(row => row.MovieId == movieId && row.StartTime.Date == DateTime.Today.AddDays(6)).OrderBy(row => row.StartTime).ToList();
            ViewBag.MovieShows7 = movieShows7;
            return View(movie);
        }
    }
}
