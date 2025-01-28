using System;
using System.Collections.Generic;

namespace happylifeluxury.Models.Entities;

public partial class Reservation
{
    public int Id { get; set; }

    public string ReservationNo { get; set; } = null!;

    public string LocationPickup { get; set; } = null!;

    public string LocationDropoff { get; set; } = null!;

    public string DatePickup { get; set; } = null!;

    public string DateDropoff { get; set; } = null!;

    public int RentalDay { get; set; }

    public int SecurityPrice { get; set; }

    public int TotalPrice { get; set; }

    public int CustomerId { get; set; }

    public int CarId { get; set; }

    public int InsuranceId { get; set; }

    public int AddId { get; set; }

    public int Status { get; set; }
}
