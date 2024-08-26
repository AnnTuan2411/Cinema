using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{

    public class CustomersController : Controller
    {

        private readonly CinemaContext _context;

        public CustomersController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            var cinemaContext = _context.Customers.Include(c => c.Acc).Include(c => c.Membership);
            return View(await cinemaContext.ToListAsync());
        }

        // GET: Customers/Details/5
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
            var customer = _context.Customers.FirstOrDefault(c => c.AccId == id);
            if (customer == null)
            {
                return NotFound();
            }
            if (customer.MembershipId == null)
            {
                ViewBag.Displaymembership = "Bạn chưa đăng ký thẻ Membership";
                return View();
            }
            int memberID = (int)customer.MembershipId;
            var _membership = _context.Memberships.FirstOrDefault(m => m.MembershipId == memberID);
            var rp = _membership.RewardPoint;
            if (rp >= 0 && rp < 5)
            {
                _membership.MembershiplevelName = "Chưa có";
                _context.Memberships.Update(_membership);
                _context.SaveChanges();
                ViewBag.Repoint = rp.ToString();
                ViewBag.Membershiplevel = "Chưa có";
            }
            if (rp >= 5 && rp < 10)
            {
                _membership.MembershiplevelName = "Đồng";
                _context.Memberships.Update(_membership);
                _context.SaveChanges();
                ViewBag.Repoint = rp.ToString();
                ViewBag.Membershiplevel = "Đồng";
            }
            else if (rp >= 10 && rp < 20)
            {
                _membership.MembershiplevelName = "Vàng";
                _context.Memberships.Update(_membership);
                _context.SaveChanges();
                ViewBag.Repoint = rp.ToString();
                ViewBag.Membershiplevel = "Vàng";
            }
            else if (rp >= 20)
            {
                _membership.MembershiplevelName = "Kim Cương";
                _context.Memberships.Update(_membership);
                _context.SaveChanges();
                ViewBag.Repoint = rp.ToString();
                ViewBag.Membershiplevel = "Kim Cương";
            }

            return View();
        }

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

            var customer = _context.Customers.FirstOrDefault(c => c.AccId == id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Accounts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, Customer customer)
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
            var existingcustomer = _context.Customers.FirstOrDefault(c => c.AccId == id);

            if (existingcustomer == null)
            {
                return NotFound();
            }

            // Chỉ cập nhật các trường được cho phép
            int membershipID = (int)customer.MembershipId;
            var isMembershipIDUnique = _context.Customers
                        .Any(c => c.MembershipId == membershipID && c.AccId != id);
            if (isMembershipIDUnique)
            {
                // Xử lý khi membershipID đã tồn tại cho một khách hàng khác
                TempData["CheckDuplicated"] = "Đã có khách hàng khác dùng";
                return RedirectToAction("Index", "Accounts");
            }

            var isMembershipIDExists = _context.Memberships.Any(m => m.MembershipId == membershipID);

            if (!isMembershipIDExists)
            {
                // Xử lý khi membershipID không tồn tại trong danh sách Membership
                TempData["CheckExits"] = "Mã thẻ không tồn tại";
                return RedirectToAction("Index", "Accounts");
            }
            existingcustomer.MembershipId = customer.MembershipId;
            _context.Update(existingcustomer);
            _context.SaveChanges();
            TempData["UpdateSuccess"] = "Cập nhật thành công";
            return RedirectToAction("Index", "Accounts");
        }
        private bool CustomerExists(int id)
        {
            return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}

