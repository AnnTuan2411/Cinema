using Cinema.Repository;
using Cinema.Models;
using Cinema.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Cinema.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly CinemaContext _context;

        public EmployeeController(CinemaContext context)
        {
            _context = context;
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
            List<EmployeeViewModel> employeeList = (from account in _context.Accounts
                                join employee in _context.Employees
                                on account.AccId equals employee.AccId
                                join accountrole in _context.AccountRoles
                                on account.RoleId equals accountrole.RoleId
                                select new EmployeeViewModel
                                {
                                    EmployeeId = employee.EmployeeId,
                                    Position = employee.Position,
                                    AccId = employee.AccId,
                                    FullName = account.FullName,
                                    DateOfBirth = account.DateOfBirth,
                                    Phone = account.Phone,
                                    Email = account.Email,
                                    Password = account.Password,
                                    Address = account.Address,
                                    Gender = account.Gender,
                                    Status = account.Status,
                                    AccountTypeName = accountrole.AccountTypeName
                                }
                          ).ToList();

            return View(employeeList);
        }

        public IActionResult Create()
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
            var model = new EmployeeCreateViewModel();
            CinemaContext context = new CinemaContext();
            List<AccountRole> accountRoles = context.AccountRoles.ToList();
            var filteredRoles = accountRoles.Where(ar => ar.RoleId > 2).ToList();
            ViewBag.AccountRole = filteredRoles; 
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model, Account account)
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
            if (ModelState.IsValid)
            {
                var newAccount = new Account
                {
                    FullName = model.FullName,
                    DateOfBirth = model.DateOfBirth,
                    Phone = model.Phone,
                    Email = model.Email,
                    Password = model.Password,
                    Address = model.Address,
                    Gender = model.Gender,
                    Status = 1,
                    RoleId = model.RoleID
                };

                var EmailExited = _context.Accounts.Any(x => x.Email == account.Email);
                if (EmailExited)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.Vui lòng sử dụng Email khác");
                    return View();
                }
                var PhoneExited = _context.Accounts.Any(x => x.Phone == account.Phone);
                if (PhoneExited)
                {
                    ModelState.AddModelError("Phone", "Số điện thoại đã tồn tại.Vui lòng sử dụng số khác");
                    return View();
                }

                _context.Accounts.Add(newAccount);
                _context.SaveChanges();

                var newEmployee = new Employee
                {
                    Position = model.Position,
                    AccId = newAccount.AccId
                };

                _context.Employees.Add(newEmployee);
                _context.SaveChanges();

                TempData["success"] = "Thêm nhân viên thành công";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        public IActionResult Edit(int id)
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
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null)
            {
                return NotFound();
            }

            var editModel = new EmployeeEditViewModel
            {
                Position = employee.Position
            };

            var account = _context.Accounts.FirstOrDefault(a => a.AccId == employee.AccId);

            CinemaContext context = new CinemaContext();
            List<AccountRole> accountRoles = context.AccountRoles.ToList();
            var filteredRoles = accountRoles.Where(ar => ar.RoleId > 2).ToList();
            ViewBag.AccountRole = filteredRoles;

            if (account != null)
            {
                editModel.AccountId = account.AccId;
                editModel.EmployeeId = id;
                editModel.FullName = account.FullName;
                editModel.DateOfBirth = account.DateOfBirth;
                editModel.Phone = account.Phone;
                editModel.Email = account.Email;
                editModel.Address = account.Address;
                editModel.Gender = account.Gender;
                editModel.Status = account.Status;
                editModel.RoleID = account.RoleId;
            }
            return View(editModel);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
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
            if (ModelState.IsValid)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var employee = _context.Employees.SingleOrDefault(e => e.EmployeeId == model.EmployeeId);

                        if (employee == null)
                        {
                            return NotFound();
                        }
                        employee.Position = model.Position;


                        _context.SaveChanges();
                        var account = _context.Accounts.FirstOrDefault(a => a.AccId == model.AccountId);

                        if (account == null)
                        {
                            return NotFound();
                        }
						account.FullName = model.FullName;
						account.DateOfBirth = model.DateOfBirth;
						account.Phone = model.Phone;
						account.Email = model.Email;
						account.Address = model.Address;
						account.Gender = model.Gender;
						account.Status = model.Status;
						account.RoleId = model.RoleID;

                        _context.Accounts.Update(account);
                        _context.SaveChanges();

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
                TempData["success"] = "Chỉnh sửa nhân viên thành công";
                return RedirectToAction("Index");
            }

            return View(model.AccountId);
        }
    }
}

