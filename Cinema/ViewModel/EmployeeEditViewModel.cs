using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModel
{
    public class EmployeeEditViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public int AccountId { get; set; }
        public string Position { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public int? Status { get; set; }
        public int? RoleID { get; set; }
    }
}
