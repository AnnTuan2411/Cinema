using Microsoft.AspNetCore.Mvc;
using Cinema.Models;
using Cinema.Repository;
using Humanizer;
using System.IO;
using System.Runtime.InteropServices;

namespace Cinema.Controllers
{

    public class MovieController : Controller
    {
        private IGenericRepository<Movie> _repository;

        private readonly IHostEnvironment _hostingEnvironment;

        public MovieController(IHostEnvironment environment)
        {
            _repository = new GenericRepository<Movie>(new CinemaContext());
            _hostingEnvironment = environment;
        }

        public ActionResult Index(int CategoryFilter = 0, String Search="")
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
            ViewBag.Search = Search;
            List<MovieCategory> categories = context.MovieCategories.ToList();
            ViewBag.Categories = categories;
            if (CategoryFilter != 0)
            {
                List<int> movieCategoryName = context.MovieCategoryNames.Where(row => row.MovieCategoryId == CategoryFilter).Select(row => row.MovieId).ToList();
                ViewBag.MovieCategoryId = CategoryFilter;
                if(ViewBag.SessionRoleID == 1)
                {
                    List<Movie> movies = _repository.GetAll().Where(row => row.Status == 1 && row.Title.Contains(Search) && movieCategoryName.Contains(row.MovieId) && row.FinishDate > DateTime.Today).ToList();
                    return View(movies);
                }
                else { 
                List<Movie> movies = _repository.GetAll().Where(row => row.Status == 1 && row.Title.Contains(Search) && movieCategoryName.Contains(row.MovieId) ).ToList();
                return View(movies);
                }
            }
            else
            {
                if (ViewBag.SessionRoleID == 1)
                {
                    List<Movie> movies = _repository.GetAll().Where(row => row.Status == 1 && row.Title.Contains(Search) && row.FinishDate > DateTime.Today).ToList();
                    return View(movies);
                }
                else
                {
                    List<Movie> movies = _repository.GetAll().Where(row => row.Status == 1 && row.Title.Contains(Search)).ToList();
                    return View(movies);
                }
            }
        }

        public ActionResult Create()
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
            List<MovieCategory> categories = context.MovieCategories.ToList();
            ViewBag.Categories = categories;
            List<Person> actor = context.People.ToList();
            ViewBag.Person = actor;
            return View();
        }

        public string UploadMovieImage(IFormFile image)
        {
            if (image != null)
            {
                string _FileName = "";
                _FileName = "movie." + image.FileName.Trim();
                string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "Image", "Movie");
                string filePath = Path.Combine(uploads, _FileName);
                string path = "/Image/Movie/" + _FileName;
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
                return path;
            }
            else
            {
                return null;
            }
        }

        public void ActorAndDirector(List<int> actors, int id, int RoleID)
        {
            using (var context = new CinemaContext())
            {
                foreach (int actor in actors)
                {
                    ActorAndDirector actorAndDirector = new ActorAndDirector(id, actor, RoleID);
                    context.ActorAndDirectors.Add(actorAndDirector);
                    context.SaveChanges();
                }
            }
        }

        public void SetCategory(List<int> categories, int id)
        {
            CinemaContext context = new CinemaContext();
            foreach (int cat in categories)
            {
                MovieCategoryName movieCategoryName = new MovieCategoryName(id, cat);
                context.MovieCategoryNames.Add(movieCategoryName);
                context.SaveChanges();
            }
        }

        [HttpPost]
        public ActionResult Create(Movie movie, IFormFile poster, IFormFile background, List<int> Actor, List<int> Director, List<int> Category)
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
            string posterSrc = UploadMovieImage(poster);
            string backgroundSrc = UploadMovieImage(background);
            movie.Background = backgroundSrc;
            movie.Poster = posterSrc;
            _repository.Insert(movie);
            _repository.Save();
            int id = int.Parse(context.Movies.ToList().Last().MovieId.ToString());
            ActorAndDirector(Actor, id, 1);
            ActorAndDirector(Director, id, 2);
            SetCategory(Category, id);
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
            Movie movie = _repository.GetById(id);
            List<ActorAndDirector> actorAndDirectors = context.ActorAndDirectors.Where(row => row.MovieId == id).ToList();
            List<Person> actors = new List<Person>();
            List<Person> directors = new List<Person>();

            foreach (ActorAndDirector actorAndDirector in actorAndDirectors)
            {
                Person person = context.People.Where(row => row.PersonId == actorAndDirector.PersonId).FirstOrDefault();
                if (actorAndDirector.RoleId == 1)
                {
                    actors.Add(person);
                }
                else if (actorAndDirector.RoleId == 2)
                {
                    directors.Add(person);
                }
            }
            List<MovieCategoryName> movieCategoryNames = context.MovieCategoryNames.Where(row => row.MovieId == id).ToList();
            List<MovieCategory> movieCategories = new List<MovieCategory>();
            foreach (MovieCategoryName movieCategoryName in movieCategoryNames)
            {
                MovieCategory movieCategory = context.MovieCategories.Where(row => row.MovieCategoryId == movieCategoryName.MovieCategoryId).FirstOrDefault();
                movieCategories.Add(movieCategory);
            }
            ViewBag.Actors = actors;
            ViewBag.Directors = directors;
            ViewBag.MovieCategories = movieCategories;
            return View(movie);
        }

        public ActionResult Edit(int id)
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
            List<MovieCategory> categories = context.MovieCategories.ToList();
            ViewBag.Categories = categories;
            List<Person> people = context.People.ToList();
            ViewBag.People = people;
            List<int> movieCategorySelecteds = context.MovieCategoryNames.Where(row => row.MovieId == id).Select(row => row.MovieCategoryId).ToList();
            ViewBag.MovieCategorySelecteds = movieCategorySelecteds;
            List<int> actors = context.ActorAndDirectors.Where(row => row.MovieId == id && row.RoleId == 1).Select(row => row.PersonId).ToList();
            ViewBag.Actors = actors;
            List<int> directors = context.ActorAndDirectors.Where(row => row.MovieId == id && row.RoleId == 2).Select(row => row.PersonId).ToList();
            ViewBag.Directors = directors;
            Movie movie = _repository.GetById(id);
            return View(movie);
        }

        [HttpPost]
        public ActionResult Edit(Movie movie, IFormFile PosterURL, IFormFile BackgroundURL)
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
			if (UploadMovieImage(PosterURL) != null)
            {
                movie.Poster = UploadMovieImage(PosterURL);
            }
            if(UploadMovieImage(BackgroundURL) != null)
            {
                movie.Background = UploadMovieImage(BackgroundURL);
            }
            try
            {
                _repository.Update(movie);
                _repository.Save();
                return RedirectToAction("Index");
            }catch (Exception ex)
            {
                CinemaContext context = new CinemaContext();
                List<MovieCategory> categories = context.MovieCategories.ToList();
                ViewBag.Categories = categories;
                List<Person> people = context.People.ToList();
                ViewBag.People = people;
                List<int> movieCategorySelecteds = context.MovieCategoryNames.Where(row => row.MovieId == movie.MovieId).Select(row => row.MovieCategoryId).ToList();
                ViewBag.MovieCategorySelecteds = movieCategorySelecteds;
                List<int> actors = context.ActorAndDirectors.Where(row => row.MovieId == movie.MovieId && row.RoleId == 1).Select(row => row.PersonId).ToList();
                ViewBag.Actors = actors;
                List<int> directors = context.ActorAndDirectors.Where(row => row.MovieId == movie.MovieId && row.RoleId == 2).Select(row => row.PersonId).ToList();
                ViewBag.Directors = directors;
                return View(movie);
            }
        }
    }
}

