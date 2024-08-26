using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? CustomerId { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime PuchaseDate { get; set; }

    public int? Status { get; set; }

    public Booking(int? customerId, decimal totalPrice, DateTime puchaseDate)
    {
        CustomerId = customerId;
        TotalPrice = totalPrice;
        PuchaseDate = puchaseDate;
    }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderTicket> OrderTickets { get; set; } = new List<OrderTicket>();
}
