using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Car
{
    public int Id { get; set; }

    public string CarName { get; set; } = null!;

    public string CarModel { get; set; } = null!;

    public string CarPrice { get; set; } = null!;

    public string CarImage { get; set; } = null!;

    public string CarType { get; set; } = null!;

    public string CarEngineType { get; set; } = null!;
}
