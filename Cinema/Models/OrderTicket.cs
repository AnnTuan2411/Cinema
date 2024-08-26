using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class OrderTicket
{
    public decimal Price { get; set; }

    public int BookingId { get; set; }

    public string TicketId { get; set; } = null!;

    public OrderTicket(decimal Price, int bookingId, string ticketId)
    {
        this.Price = Price;
        this.BookingId = bookingId;
        this.TicketId = ticketId;
    }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
