using Cinema.Models;
using Cinema.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers
{
    public class MovieCategoriesController : Controller
    {
        private IGenericRepository<MovieCategory> _repository;

        public MovieCategoriesController()
        {
            _repository = new GenericRepository<MovieCategory>(new CinemaContext());
        }
        public IActionResult Index(string search = "", string SortColumn = "MovieCategoryName", string IconClass = "fa-sort-asc", int page = 1)
        {

            List<MovieCategory> movieCategories = _repository.GetAll().Where(row => row.MovieCategoryName.Contains(search)).ToList();
            ViewBag.Search = search;
            ViewBag.SortColumn = SortColumn;
            ViewBag.IconClass = IconClass;

            if (SortColumn == "MovieCategoryName")
            {
                if (IconClass == "fa-sort-asc")
                {
                    movieCategories = movieCategories.OrderBy(row => row.MovieCategoryName).ToList();
                }
                else
                {
                    movieCategories = movieCategories.OrderByDescending(row => row.MovieCategoryName).ToList();
                }
            }
            else if (SortColumn == "MovieCategoryID")
            {
                if (IconClass == "fa-sort-asc")
                {
                    movieCategories = movieCategories.OrderBy(row => row.MovieCategoryId).ToList();
                }
                else
                {
                    movieCategories = movieCategories.OrderByDescending(row => row.MovieCategoryId).ToList();
                }
            }

            int NoOfRecordPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(movieCategories.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            movieCategories = movieCategories.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(movieCategories);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MovieCategory movieCategory)
        {
            if (ModelState.IsValid)
            {
                _repository.Insert(movieCategory);
                _repository.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            MovieCategory movieCategory = _repository.GetById(id);
            return View(movieCategory);
        }

        [HttpPost]
        public ActionResult Edit(MovieCategory movCat)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(movCat);
                _repository.Save();
                return RedirectToAction("Index");
            }
            else
            {
                return View(movCat);
            }
        }
    }
}
