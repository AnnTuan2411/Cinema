using Cinema.Models;

namespace Cinema.Repository_2
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IEnumerable<Employee> GetEmpsWithAccountsForHR();
        IEnumerable<Employee> GetEmpsWithAccountsForAdmin();
        Employee GetEmpWithAccountById(int id);
    }
}
