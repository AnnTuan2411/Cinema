using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class OrderFood
{
    public int? BookingId { get; set; }

    public int? FoodId { get; set; }

    public int FoodQuantity { get; set; }

    public decimal Price { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Food? Food { get; set; }
}
