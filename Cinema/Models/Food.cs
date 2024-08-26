using System;
using System.Collections.Generic;

namespace Cinema.Models;

public partial class Food
{
    public int FoodId { get; set; }

    public int? FoodCategoryId { get; set; }

    public string? Size { get; set; }

    public decimal Price { get; set; }

    public string Image { get; set; } = null!;

    public string Detail { get; set; } = null!;

    public virtual FoodCategory? FoodCategory { get; set; }
}
