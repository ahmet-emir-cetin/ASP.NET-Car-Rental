using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Campaign
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Subtitle { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Image { get; set; } = null!;

    public sbyte IsView { get; set; }

    public int Hit { get; set; }

    public string SeoUrl { get; set; } = null!;

    public string SeoTitle { get; set; } = null!;
}
