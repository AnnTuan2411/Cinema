using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Cinema.Controllers.HR
{
    public class EmployeeManageController : Controller
    {
        private readonly IHostEnvironment _hostingEnvironment;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeManageController(IUnitOfWork unitOfWork, IHostEnvironment environment)
        {
            _hostingEnvironment = environment;
            _unitOfWork = unitOfWork;
        }

        public ActionResult ListEmployeesAndAccounts(string strSearch, int page = 1)
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
            int pageSize = 5;
            var empsWithAccounts = _unitOfWork.Employee.GetEmpsWithAccountsForHR(); // GetEmpsWithAccountsForHR la list chi co employee

            if (!String.IsNullOrEmpty(strSearch))
            {
                empsWithAccounts = empsWithAccounts.Where(emp => emp.Acc.FullName.ToLower().Contains(strSearch.ToLower()) || emp.EmployeeId.ToString().Contains(strSearch)).ToList();
            }
            var result = empsWithAccounts.ToPagedList(page, pageSize);
            return View(result);
        }

        // GET: EmployeeManageController/Details/5
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
            var empFromRepo = _unitOfWork.Employee.GetEmpWithAccountById(id.Value);
            if (empFromRepo == null)
            {
                return NotFound();
            }
            return View(empFromRepo);
        }



        // GET: EmployeeManageController/Create
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
            return View();
        }

        // POST: EmployeeManageController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee employee)
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
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public string UploadImage(IFormFile image)
        {
            if (image != null)
            {
                string _FileName = "";
                _FileName = "employee." + image.FileName;
                string uploads = Path.Combine(_hostingEnvironment.ContentRootPath, "Image", "Employee");
                string filePath = Path.Combine(uploads, _FileName);
                string path = "/Image/Employee/" + _FileName;
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

        // GET: EmployeeManageController/Edit/5
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
            var empFromRepo = _unitOfWork.Employee.GetEmpWithAccountById(id.Value);
            if (empFromRepo == null)
            {
                return NotFound();
            }
            return View(empFromRepo);
        }

        // POST: EmployeeManageController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Employee employee, IFormFile IMAGEURL)
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
            if (UploadImage(IMAGEURL) != null)
            {
                employee.Img = UploadImage(IMAGEURL);
            }

            try
            {
                Employee _employee = new Employee
                {
                    Img = employee.Img,
                    AccId = employee.Acc.AccId,
                    EmployeeId = employee.EmployeeId,
                    Position = employee.Position
                };
                _unitOfWork.Employee.Update(_employee);
                _unitOfWork.Save();

                Account _account = _unitOfWork.Account.GetById(employee.Acc.AccId);
                _account.FullName = employee.Acc.FullName;
                _account.DateOfBirth = employee.Acc.DateOfBirth;
                _account.Phone = employee.Acc.Phone;
                _account.Email = employee.Acc.Email;
                _account.Address = employee.Acc.Address;
                _account.Gender = employee.Acc.Gender;

                _unitOfWork.Account.Update(_account);
                _unitOfWork.Save();
                return RedirectToAction(nameof(ListEmployeesAndAccounts));
            }
            catch (Exception e)
            {
                ViewBag.Message = e.Message;
                return View();
            }
        }

        // GET: EmployeeManageController/DisableEmp/5
        [HttpGet]
        public ActionResult DisableEmp(int? id)
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
            var emp = _unitOfWork.Employee.GetEmpWithAccountById(id.Value);
            if (emp == null)
            {
                return NotFound();
            }
            return PartialView("DisableEmp", emp);
        }

        // POST: EmployeeManageController/DisableEmp/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisableEmp(Employee _employee)
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
                Employee employee = _unitOfWork.Employee.GetEmpWithAccountById(_employee.EmployeeId);
                Account account = _unitOfWork.Account.GetById(employee.Acc.AccId);
                account.Status = 2;
                _unitOfWork.Account.Update(account);
                _unitOfWork.Save();
                return PartialView("DisableEmp", employee);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }

        // GET: EmployeeManageController/EnableEmp/5
        [HttpGet]
        public ActionResult EnableEmp(int? id)
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
            var emp = _unitOfWork.Employee.GetEmpWithAccountById(id.Value);
            if (emp == null)
            {
                return NotFound();
            }
            return PartialView("EnableEmp", emp);
        }

        // POST: EmployeeManageController/EnableEmp/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnableEmp(Employee _employee)
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
                Employee employee = _unitOfWork.Employee.GetEmpWithAccountById(_employee.EmployeeId);
                Account account = _unitOfWork.Account.GetById(employee.Acc.AccId);
                account.Status = 1;
                _unitOfWork.Account.Update(account);
                _unitOfWork.Save();
                return PartialView("EnableEmp", employee);
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View();
            }
        }
    }

}
