using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class FoodCategory
{
    public int FoodCategoryId { get; set; }

    public string FoodCategoryName { get; set; } = null!;

    public virtual ICollection<Food> Foods { get; set; } = new List<Food>();
}
