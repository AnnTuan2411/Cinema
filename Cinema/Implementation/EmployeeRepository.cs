using Cinema.Models;
using Cinema.Repository_2;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Cinema.Implementation
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(CinemaContext context) : base (context)
        {
            
        }

        //3. QL Nhân viên
        //4. QL rạp chiếu
        //5. Nhân viên
        public IEnumerable<Employee> GetEmpsWithAccountsForHR()
        {
            var empWithAccounts = _context.Employees.Include(x => x.Acc).Where(e => e.Acc.RoleId == 5).ToList();
            return empWithAccounts;
        }

        public IEnumerable<Employee> GetEmpsWithAccountsForAdmin()
        {

            var empWithAccounts = _context.Employees.Include(x => x.Acc).Where(e => e.Acc.RoleId == 3).ToList();
            var empWithAccounts2 = _context.Employees.Include(x => x.Acc).Where(e => e.Acc.RoleId == 4).ToList();
            var empWithAccounts3 = _context.Employees.Include(x => x.Acc).Where(e => e.Acc.RoleId == 5).ToList();
            var result = empWithAccounts.Union(empWithAccounts2).Union(empWithAccounts3);
            return result;
        }

        public Employee GetEmpWithAccountById(int id)
        {
            var empWithAccountById = _context.Employees.Include(e => e.Acc)
                .Where(e => e.EmployeeId == id) 
                .FirstOrDefault();
            return empWithAccountById;
        }


    }
}
