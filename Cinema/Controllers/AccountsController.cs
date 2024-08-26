using Cinema.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using MailKit.Net.Smtp;

namespace Cinema.Controllers
{

    public class AccountsController : Controller
    {

        private readonly CinemaContext _context;

        public AccountsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public IActionResult Index(string search = "", int page = 1)
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
            }
            List<Account> accounts = _context.Accounts.Where(account => account.RoleId == 1 && account.FullName.Contains(search)).ToList();
            ViewBag.SearchAcc = search;
            //Paging
            int NoOfRecordPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(accounts.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            accounts = accounts.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            return View(accounts);
        }

        public ActionResult Register()
        {
            Account account = new Account();
            return View(account);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account account)
        {
            var Emailexited = _context.Accounts.Any(x => x.Email == account.Email);//(Any)có chức năng trả về true nếu tập hợp chứa ít nhất một phần tử thỏa mãn điều kiện được chỉ định.
            if (Emailexited)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.Vui lòng sử dụng Email khác");
                return View();
            }
            if (ModelState.IsValid)
            {

                Account acc = new Account();
                acc.FullName = account.FullName;
                acc.DateOfBirth = account.DateOfBirth;
                acc.Phone = account.Phone;
                acc.Email = account.Email;
                acc.Password = Commoncs.Hash(account.Password);
                acc.Address = account.Address;
                acc.Gender = account.Gender;
                acc.RoleId = 1;
                account.RoleId = acc.RoleId;
                _context.Accounts.Add(acc);
                _context.SaveChanges();
                //-----
                Customer cus = new Customer();
                cus.Membership = null;
                cus.AccId = acc.AccId;
                _context.Customers.Add(cus);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.RegisterFail = "Đăng ký thất bại!!";
            }
            return View();

        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account account)
        {

            CinemaContext context = new CinemaContext();
            var formemail = account.Email;
            var formpass = account.Password;

            var cuscheck = _context.Accounts.SingleOrDefault(x => x.Email.Equals(formemail) && x.Password.Equals(Commoncs.Hash(formpass)));
            if (cuscheck != null)
            {
                HttpContext.Session.SetString("UserName", cuscheck.FullName); // Lưu thông tin người dùng vào Session
                HttpContext.Session.SetInt32("IDAcc", cuscheck.AccId);
                HttpContext.Session.SetInt32("RoleID", (int)cuscheck.RoleId);
                if (cuscheck.RoleId == 2 || cuscheck.RoleId == 3 || cuscheck.RoleId == 4)
                {
                    return RedirectToAction("Index", "AdminHome");
                }
                else if(cuscheck.RoleId == 1)
                {
                    int id = context.Customers.Where(row => row.AccId == cuscheck.AccId).Select(row => row.CustomerId).FirstOrDefault();
                    HttpContext.Session.SetInt32("CusId", id);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    int id = context.Employees.Where(row => row.AccId == cuscheck.AccId).Select(row => row.EmployeeId).FirstOrDefault();
                    HttpContext.Session.SetInt32("EmpId", id);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.LoginFail = "Sai mật khẩu hoặc Email!!";
                return View("Login");
            }
        }
        public IActionResult Logout()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                HttpContext.Session.Remove("UserName");
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Edit()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
                try
                {
                    ViewBag.SeccionCusId = HttpContext.Session.GetInt32("CusId");
                }catch(Exception ex)
                {
                    ViewBag.SeccionEmpId = HttpContext.Session.GetInt32("EmpId");
                }
            }
            if (ViewBag.SessionID == null)
            {
                return NotFound();
            }

            var account = _context.Accounts.Find(ViewBag.SessionID);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Account account)
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
            if (ViewBag.SessionID != account.AccId)
            {
                return NotFound();
            }
            var existingAccount = _context.Accounts.Find(ViewBag.SessionID);
            if (existingAccount == null)
            {
                return NotFound();
            }

            // Chỉ cập nhật các trường được cho phép
            existingAccount.Phone = account.Phone;
            existingAccount.Address = account.Address;
            existingAccount.Gender = account.Gender;
            _context.Update(existingAccount);
            _context.SaveChanges();
            TempData["UpdateSuccess"] = "Cập nhật thành công";
            return RedirectToAction("Edit");

            /*            ViewData["RoleId"] = new SelectList(_context.AccountRoles, "RoleId", "RoleId", account.RoleId);*/
        }

        public ActionResult EditEmployee()
        {
            if (HttpContext.Session.GetString("UserName") != null)
            {
                ViewBag.Session = HttpContext.Session.GetString("UserName").ToString();
                ViewBag.SessionID = HttpContext.Session.GetInt32("IDAcc");
                ViewBag.SessionRoleID = HttpContext.Session.GetInt32("RoleID");
            }
            if (ViewBag.SessionID == null)
            {
                return NotFound();
            }

            var account = _context.Accounts.Find(ViewBag.SessionID);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }


        //Changepassword
        public ActionResult ChangePass(int? id)
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

            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePass(int id, Account account)
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
            if (id != account.AccId)
            {
                return NotFound();
            }


            var existingAccount = _context.Accounts.Find(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            // Chỉ cập nhật các trường được cho phép
            if (existingAccount.Password == Commoncs.Hash(account.Password))
            {
                var NewPass = HttpContext.Request.Form["NewPass"].ToString();
                var confirmNewPass = HttpContext.Request.Form["ConfirmNewPass"].ToString();
                bool areEqual = string.Equals(NewPass, confirmNewPass, StringComparison.OrdinalIgnoreCase);
                if (areEqual)
                {
                    existingAccount.Password = Commoncs.Hash(confirmNewPass);
                    _context.Update(existingAccount);
                    _context.SaveChanges();
                    TempData["ChangePassSucces"] = "Thay đổi mật khẩu thành công";
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ConfirmPass = "Mật khẩu mới không đúng";
                return View("ChangePass");
            }
            else
            {
                ViewBag.ChangePassFail = "Mật khẩu cũ không đúng";
                return View("ChangePass");
            }

        }


        //Membership
        public ActionResult CheckMembership(int? id)
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

            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckMembership(int id, Customer customer)
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
            if (id != customer.AccId)
            {
                return NotFound();
            }
            if (customer.MembershipId == null)
            {
                ViewBag.Displaymembership = "Bạn chưa đăng ký thẻ Membership";
            }
            else
            {
                var membership = _context.Memberships.Find(customer.MembershipId);
                int rp = (int)membership.RewardPoint;
                if (rp == 0)
                {
                    membership.MembershiplevelName = "Chưa có bậc";
                    _context.Memberships.Update(membership);
                    _context.SaveChanges();
                }
                else if (rp >= 5 && rp < 10)
                {
                    membership.MembershiplevelName = "Đồng";
                    _context.Memberships.Update(membership);
                    _context.SaveChanges();
                }
                else if (rp >= 10 && rp < 20)
                {
                    membership.MembershiplevelName = "Vàng";
                    _context.Memberships.Update(membership);
                    _context.SaveChanges();
                }
                else if (rp >= 20)
                {
                    membership.MembershiplevelName = "Kim Cương";
                    _context.Memberships.Update(membership);
                    _context.SaveChanges();
                }

            }
            return View(customer);
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(Account account)
        {
            var formemail = account.Email;
            var cuscheck = _context.Accounts.SingleOrDefault(x => x.Email.Equals(formemail));
            if (cuscheck == null)
            {
                ViewBag.CheckEmail = "Email không tồn tại";
                return View("ForgotPassword");
            }

            //send email
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com");
                client.Authenticate("anhtuan35670@gmail.com", "cddxxbxbmihjlmqm");
                var bodyBuider = new BodyBuilder
                {
                    HtmlBody = $"<a href=\"https://localhost:7143/Accounts/ResetPassword\">Reset Password</a>"
                };
                var message = new MimeMessage
                {
                    Body = bodyBuider.ToMessageBody()
                };
                message.From.Add(new MailboxAddress("Admin", "anhtuan35670@gmail.com"));
                message.To.Add(new MailboxAddress("KhachHang", formemail));
                message.Subject = "Lấy lại mật khẩu";
                client.Send(message);
                client.Disconnect(true);
            }
            ViewBag.CheckEmailSuccess = "Đã gửi link đến email của bạn! Vui lòng kiểm tra email của bạn";
            return View("ForgotPassword");
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(Account account)
        {
            var formemail = account.Email;
            var existingAccount = _context.Accounts.SingleOrDefault(x => x.Email.Equals(formemail));
            if (existingAccount == null)
            {
                ViewBag.CheckEmail = "Email của bạn không tồn tại";
                return View("ResetPassword");
            }
            else
            {
                var NewPass = HttpContext.Request.Form["NewPass"].ToString();
                var confirmNewPass = HttpContext.Request.Form["ConfirmNewPass"].ToString();
                bool areEqual = string.Equals(NewPass, confirmNewPass, StringComparison.OrdinalIgnoreCase);
                if (areEqual)
                {
                    existingAccount.Password = Commoncs.Hash(confirmNewPass);
                    _context.Update(existingAccount);
                    _context.SaveChanges();
                    TempData["ResetPassSucces"] = "Thay đổi mật khẩu thành công";
                    return RedirectToAction("Login", "Accounts");
                }
                TempData["ConfirmPass"] = "Mật khẩu mới không đúng";
                return RedirectToAction("ResetPassword");
            }
        }

        private bool AccountExists(int id)
        {
            return (_context.Accounts?.Any(e => e.AccId == id)).GetValueOrDefault();
        }
    }
}
