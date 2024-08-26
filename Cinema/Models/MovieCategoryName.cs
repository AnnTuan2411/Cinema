using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class MovieCategoryName
{
    public int MovieId { get; set; }

    public int MovieCategoryId { get; set; }

    public MovieCategoryName(int movieId, int movieCategoryId)
    {
        MovieId = movieId;
        MovieCategoryId = movieCategoryId;
    }

    public bool? Status { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual MovieCategory MovieCategory { get; set; } = null!;
}
