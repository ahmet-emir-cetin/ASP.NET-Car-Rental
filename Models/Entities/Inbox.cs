using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Inbox
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Message { get; set; } = null!;
}
