using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Tc { get; set; } = null!;

    public string DriverNo { get; set; } = null!;

    public string DriverDate { get; set; } = null!;

    public string Bday { get; set; } = null!;

    public string City { get; set; } = null!;

    public string County { get; set; } = null!;

    public string Adress { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;
}
