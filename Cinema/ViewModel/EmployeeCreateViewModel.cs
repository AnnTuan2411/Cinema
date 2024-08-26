using System.ComponentModel.DataAnnotations;

namespace Cinema.ViewModel
{
    public class EmployeeCreateViewModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public int AccountId { get; set; }
        [MaxLength(30)]
        public string Position { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; }
        [MinimumAge(18)]
        public DateTime DateOfBirth { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Trường này bắt buộc")]
        public string Email { get; set; }
        [MaxLength(20)]
        public string Password { get; set; }
        [MaxLength(50)]
        public string Address { get; set; }
        public string Gender { get; set; }
        public int? Status { get; set; }
        public int RoleID { get; set; }
    }
}
