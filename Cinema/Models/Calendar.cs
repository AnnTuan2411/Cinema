using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Calendar
{
    public int CalendarId { get; set; }

    public int? EmloyeeId { get; set; }

    public int Shift { get; set; }

    public DateTime Date { get; set; }

    public string? Description { get; set; }

    public string? Title { get; set; }

    public virtual Employee? Emloyee { get; set; }
}
