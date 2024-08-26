using Cinema.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cinema.Controllers
{
    public class MembershipsController : Controller
    {
        private readonly CinemaContext _context;

        public MembershipsController(CinemaContext context)
        {
            _context = context;
        }

        // GET: Memberships
        public IActionResult Index(int? search, int page = 1)
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

            List<Membership> memberships = new List<Membership>();
            if (search != null)
            {
                memberships = _context.Memberships.Where(m => m.MembershipId == search).ToList();
            }
            else
            {
                memberships = _context.Memberships.ToList();
            }
            ViewBag.Search = search;

            // Phân trang
            int NoOfRecordPerPage = 5;
            int NoOfPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(memberships.Count) / Convert.ToDouble(NoOfRecordPerPage)));
            int NoOfRecordToSkip = (page - 1) * NoOfRecordPerPage;
            ViewBag.Page = page;
            ViewBag.NoOfPages = NoOfPages;
            memberships = memberships.Skip(NoOfRecordToSkip).Take(NoOfRecordPerPage).ToList();
            return View(memberships);
        }

        // GET: Memberships/Details/5
        public async Task<IActionResult> Details(int? id)
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
            if (id == null || _context.Memberships == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // GET: Memberships/Create
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
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Membership membership)
        {

            if (ModelState.IsValid)
            {
                _context.Add(membership);
                _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(membership);
        }

        // GET: Memberships/Edit/5


        // GET: Memberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Memberships == null)
            {
                return NotFound();
            }

            var membership = await _context.Memberships
                .FirstOrDefaultAsync(m => m.MembershipId == id);
            if (membership == null)
            {
                return NotFound();
            }

            return View(membership);
        }

        // POST: Memberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Memberships == null)
            {
                return Problem("Entity set 'CinemaContext.Memberships'  is null.");
            }
            var membership = await _context.Memberships.FindAsync(id);
            if (membership != null)
            {
                _context.Memberships.Remove(membership);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MembershipExists(int id)
        {
            return (_context.Memberships?.Any(e => e.MembershipId == id)).GetValueOrDefault();
        }
    }
}
