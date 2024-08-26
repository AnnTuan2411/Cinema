using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Membership
{
    public int MembershipId { get; set; }

    public string MembershiplevelName { get; set; } = null!;

    public int? RewardPoint { get; set; }

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}
