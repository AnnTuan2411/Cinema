using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? RoomId { get; set; }

    public string SeatName { get; set; } = null!;

    public int? Status { get; set; }

    public string SeatCategory { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
