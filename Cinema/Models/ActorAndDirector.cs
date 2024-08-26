using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class ActorAndDirector
{
    public int MovieId { get; set; }

    public int PersonId { get; set; }

    public int RoleId { get; set; }

    public ActorAndDirector(int movieId, int personId, int roleId)
    {
        MovieId = movieId;
        PersonId = personId;
        RoleId = roleId;
    }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public virtual PersonRole Role { get; set; } = null!;
}
