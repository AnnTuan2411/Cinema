using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string Position { get; set; } = null!;

    public int? AccId { get; set; }

    public string? Img { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual ICollection<Calendar> Calendars { get; set; } = new List<Calendar>();
}
