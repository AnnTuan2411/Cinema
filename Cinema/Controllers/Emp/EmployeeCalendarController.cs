using Cinema.Helpers;
using Cinema.Repository_2;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cinema.Controllers.Emp
{
    public class EmployeeCalendarController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeCalendarController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: EmployeeCalendarController

        public ActionResult ViewCalendar() 
        {

            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
                
            }
            int empID = ViewBag.SeccionEmpId; //sửa lại ID nhân viên lấy từ trên Session xuống

            ViewData["Resources"] = JSONListHelper.GetResourceListJSONString(_unitOfWork.Employee.GetAll());
            ViewData["Events"] = JSONListHelper.GetEventListForOneEmpJSONString(_unitOfWork.Calendar.GetCalendarWithEmps(), empID);
            return View();
        }

        // GET: EmployeeCalendarController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeCalendarController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeCalendarController/Create
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

        // GET: EmployeeCalendarController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeCalendarController/Edit/5
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

        // GET: EmployeeCalendarController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeCalendarController/Delete/5
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
