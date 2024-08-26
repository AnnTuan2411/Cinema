using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public int? MembershipId { get; set; }

    public int? AccId { get; set; }

    public virtual Account? Acc { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Membership? Membership { get; set; }

    public virtual ICollection<MovieRating> MovieRatings { get; set; } = new List<MovieRating>();

    public virtual ICollection<ServiceFeedBack> ServiceFeedBacks { get; set; } = new List<ServiceFeedBack>();
}
