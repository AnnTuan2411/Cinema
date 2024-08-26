using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Ticket
{
    public string TicketId { get; set; } = null!;

    public int? MovieShowId { get; set; }

    public int? SeatId { get; set; }

    public decimal? TotalPrice { get; set; }

    public Ticket(string ticketId, int? movieShowId, int? seatId, decimal? totalPrice)
    {
        TicketId = ticketId;
        MovieShowId = movieShowId;
        SeatId = seatId;
        TotalPrice = totalPrice;
    }

    public int? Status { get; set; }

    public virtual MovieShow? MovieShow { get; set; }

    public virtual ICollection<OrderTicket> OrderTickets { get; set; } = new List<OrderTicket>();

    public virtual Seat? Seat { get; set; }
}
