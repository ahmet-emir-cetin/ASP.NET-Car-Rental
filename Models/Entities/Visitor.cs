using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Visitor
{
    public int Id { get; set; }

    public string Ip { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string Browser { get; set; } = null!;

    public string Date { get; set; } = null!;
}
