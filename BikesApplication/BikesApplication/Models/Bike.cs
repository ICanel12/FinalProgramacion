using System;
using System.Collections.Generic;

namespace BikesApplication.Models;

public partial class Bike
{
    public int IdBike { get; set; }

    public string Name { get; set; } = null!;

    public string? Image { get; set; }

    public string Type { get; set; } = null!;

    public string Brand { get; set; } = null!;

    public decimal? Size { get; set; }

    public int? NumberDishes { get; set; }

    public int? NumberSprockets { get; set; }
}
