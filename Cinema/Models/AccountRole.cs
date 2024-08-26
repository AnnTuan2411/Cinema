using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class AccountRole
{
    public int RoleId { get; set; }

    public string AccountTypeName { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
