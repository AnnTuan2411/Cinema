using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Person
{
    public int PersonId { get; set; }

    public string PersonName { get; set; } = null!;

    public string? Image { get; set; }

    public string? Information { get; set; }

    public DateTime? BirthDate { get; set; }

    public string? WikipediaUrl { get; set; }

    public virtual ICollection<ActorAndDirector> ActorAndDirectors { get; set; } = new List<ActorAndDirector>();
}
