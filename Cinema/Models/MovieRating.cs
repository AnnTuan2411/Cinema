using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class MovieRating
{
    public int MovieRatingId { get; set; }

    public int? MovieId { get; set; }

    public int? CustomerId { get; set; }

    public string? Comment { get; set; }

    public int Star { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Movie? Movie { get; set; }
}
