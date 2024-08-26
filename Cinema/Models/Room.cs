using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int NumberOfSeats { get; set; }

    public string? RoomName { get; set; }

    public int? Status { get; set; }

    public virtual ICollection<MovieShow> MovieShows { get; set; } = new List<MovieShow>();

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
