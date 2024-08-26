using Cinema.Models;
using Cinema.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cinema.Controllers
{
    public class PeopleController : Controller
    {
        private readonly IHostEnvironment _hostingEnvironment;

        private IGenericRepository<Person> _repository;

        public PeopleController(IHostEnvironment environment)
        {
            _hostingEnvironment = environment;
            _repository = new GenericRepository<Person>(new CinemaContext());
        }

        public IActionResult Index(String search="",  int page = 1)
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
            List<Person> people = _repository.GetAll().Where(row => row.PersonName.Contains(search)).ToList();
            ViewBag.Search = search;

            int NoOfRecordPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(people.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            people = people.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();

            return View(people);
        }

        public string UploadImage(IFormFile image)
        {
            if (image != null)
            {
                string _FileName = "";
                _FileName = "person." + image.FileName;
                string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "Image", "Person");
                string filePath = Path.Combine(uploads, _FileName);
                string path = "/Image/Person/" + _FileName;
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(fileStream);
                }
                return path;
            }
            else { 
                return null;
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
            List<PersonRole> personRoles = context.PersonRoles.ToList();
            ViewBag.PersonRoles = personRoles;
            return View();
        }

        [HttpPost]
        public ActionResult Create(Person person, IFormFile IMAGE)
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
            person.Image = UploadImage(IMAGE);
            _repository.Insert(person);
            _repository.Save();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id) {
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
            Person person = _repository.GetById(id);
            CinemaContext context = new CinemaContext();
            List<PersonRole> personRoles = context.PersonRoles.ToList();
            ViewBag.PersonRole = personRoles;
            return View(person);
        }

        [HttpPost]
        public ActionResult Edit(Person per, IFormFile IMAGEURL)
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
            if (UploadImage(IMAGEURL) != null) {
                per.Image = UploadImage(IMAGEURL); 
            }
            try { 
                _repository.Update(per);
                _repository.Save();
                return RedirectToAction("Index");
            }catch(Exception ex)
            {
                return View(per);
            }
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
            Person person = _repository.GetById(id);
            return View(person);
        }
    }
}
