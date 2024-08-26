using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class PersonRole
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual ICollection<ActorAndDirector> ActorAndDirectors { get; set; } = new List<ActorAndDirector>();
}
