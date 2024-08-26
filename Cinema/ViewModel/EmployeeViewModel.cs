using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModel
{
    public class EmployeeViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public int RoleId { get; set; }
        public string Position { get; set; }
        public int? AccId { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int? Status { get; set; }
        public string AccountTypeName { get; set; }
    }
}
