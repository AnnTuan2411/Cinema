using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Models;

public partial class Account
{
    public int AccId { get; set; }
    [Required(ErrorMessage = "Trường này bắt buộc")]
    public string FullName { get; set; } = null!;
    [Required(ErrorMessage = "Trường này bắt buộc")]
    public DateTime DateOfBirth { get; set; }
    [Required(ErrorMessage = "Trường này bắt buộc")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại không hợp lệ")]
    public string Phone { get; set; } = null!;
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Trường này bắt buộc")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Trường này bắt buộc")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int? Status { get; set; }

    public int? RoleId { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual AccountRole? Role { get; set; }
}
