using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class MovieShow
{
    public int MovieShowId { get; set; }

    public int? MovieId { get; set; }

    public int? RoomId { get; set; }

    public string Status { get; set; } = null!;

    public decimal Price { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual Movie? Movie { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
