using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Carproperty
{
    public int Id { get; set; }

    public int CarId { get; set; }

    public string Title { get; set; } = null!;

    public string Icon { get; set; } = null!;
}
