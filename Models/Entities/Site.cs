using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Site
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public string Favicon { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Instagram { get; set; } = null!;

    public string Facebook { get; set; } = null!;

    public string Twitter { get; set; } = null!;
}
