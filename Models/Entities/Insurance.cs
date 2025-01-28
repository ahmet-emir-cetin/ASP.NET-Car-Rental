using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Insurance
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public bool TireglassSec { get; set; }

    public bool SuperMiniSec { get; set; }

    public bool MiniSec { get; set; }

    public bool IhtiyariSec { get; set; }

    public bool FerdiSec { get; set; }

    public int Price { get; set; }
}
