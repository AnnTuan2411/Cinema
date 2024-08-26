using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class MovieCategory
{
    public int MovieCategoryId { get; set; }

    public string MovieCategoryName { get; set; } = null!;

    public virtual ICollection<MovieCategoryName> MovieCategoryNames { get; set; } = new List<MovieCategoryName>();
}
