using Cinema.Helpers;
using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Cinema.Controllers.HR
{
    public class CalendarManageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CalendarManageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CalendarManageController
        public ActionResult Index(string strSearch, int page = 1) // View list 
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
            page = page < 1 ? 1 : page;
            int pageSize = 10;
            var calenderList = _unitOfWork.Calendar.GetCalendarWithEmps();

            if (!String.IsNullOrEmpty(strSearch))
            {
                calenderList = calenderList.Where(calendar => calendar.Emloyee.Acc.FullName.ToLower().Contains(strSearch.ToLower()) || calendar.Emloyee.EmployeeId.ToString().Contains(strSearch)).ToList();
            }
            var result = calenderList.ToPagedList(page, pageSize);
            return View(result);
        }

        // GET: CalendarManageController
        public ActionResult ViewAllEmpCalendars() // View list 
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
            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_unitOfWork.Employee.GetAll());
            ViewData["Events"] = JSONListHelper.GetEventListJSONString(_unitOfWork.Calendar.GetCalendarWithEmps());
            return View();
        }


        // GET: CalendarManageController/Details/5
        public ActionResult Details(int? id)
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
            if (id == null)
            {
                return NotFound();
            }
            var calendarDetail = _unitOfWork.Calendar.GetCalendarDetailById(id.Value);
            if (calendarDetail == null)
            {
                return NotFound();
            }
            return View(calendarDetail);
        }

        // GET: CalendarManageController/Create
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
            List<Employee> employees = (List<Employee>)_unitOfWork.Employee.GetEmpsWithAccountsForHR();
            ViewBag.Employees = employees;
            return View();
        }

        // POST: CalendarManageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Calendar calendar)
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
            try
            {
                if (ModelState.IsValid)
                {
                    _unitOfWork.Calendar.Add(calendar);

                    _unitOfWork.Save();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // GET: CalendarManageController/Edit/5
        public ActionResult Edit(int? id)
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
            if (id == null)
            {
                return NotFound();
            }
            CinemaContext context = new CinemaContext();
            var calendar = _unitOfWork.Calendar.GetById(id.Value);
            if (calendar == null)
            {
                return NotFound();
            }
            return View(calendar);
        }

        // POST: CalendarManageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Calendar calendar)
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
            try
            {
                if (id != calendar.CalendarId)
                {
                    return NotFound();
                }
                if (ModelState.IsValid)
                {
                    _unitOfWork.Calendar.Update(calendar);
                    _unitOfWork.Save();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CalendarManageController/Delete/5
        public ActionResult Delete(int? id)
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
            if (id == null)
            {
                return NotFound();
            }
            var calendar = _unitOfWork.Calendar.GetById(id.Value);
            if (calendar == null)
            {
                return NotFound();
            }
            return View(calendar);
        }

        // POST: CalendarManageController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Calendar calendar)
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
            try
            {
                _unitOfWork.Calendar.Delete(calendar);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }
}
