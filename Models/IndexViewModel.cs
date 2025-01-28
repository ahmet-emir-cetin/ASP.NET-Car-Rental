using System.ComponentModel.DataAnnotations;
using happylifeluxury.Models.Entities;

namespace happylifeluxury.Models;

public class IndexViewModel
{
    public Slide? Slide { get; set; }
    public IEnumerable<Slide>? Slides { get; set; }

    public int? CarId { get; set; }

    public Car? Car { get; set; }
    public IEnumerable<Car>? Cars { get; set; }
    public IEnumerable<Car>? EkoCars { get; set; }
    public IEnumerable<Car>? KonforCars { get; set; }

    public Site? Site { get; set; }
    public IEnumerable<Site>? Sites { get; set; }

    public Location? LocationPick { get; set; }
    public Location? LocationDrop { get; set; }
    public IEnumerable<Location>? Locations { get; set; }
    public Location? Location { get; set; }


    [Required(ErrorMessage = "Boş geçilemez!")]
    public string? LocationPickup { get; set; }

    [Required(ErrorMessage = "Boş geçilemez!")]
    public string? LocationDropoff { get; set; }

    [Required(ErrorMessage = "Boş geçilemez!")]
    public string? DatePickup { get; set; }

    [Required(ErrorMessage = "Boş geçilemez!")]
    public string? DateDropoff { get; set; }

    public string? RentalDay { get; set; }

    /* CUSTOMERS */
    public Customer? Customer { get; set; }

    public IEnumerable<Customer>? Customers { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? FirstName { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? LastName { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? Tc { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? DriverNo { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? Bday { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? City { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? County { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? Adress { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? Phone { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? Email { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Customer? DriverDate { get; set; }

    public int TotalPrice { get; set; }

    public int? InsurancePrice { get; set; }
    public string? InsuranceTitle { get; set; }
    public int? InsuranceId { get; set; }
    public bool? InsuranceSelect { get; set; } = false;

    public Insurance? Insurance { get; set; }
    public IEnumerable<Insurance>? Insurances { get; set; }

    [Required(ErrorMessage = "Boş geçilemez !")]
    public Reservation? ReservationNo { get; set; }
    public IEnumerable<Reservation>? Reservations { get; set; }
    public Reservation? Reservation { get; set; }

    public Campaign? Campaign { get; set; }
    public IEnumerable<Campaign>? Campaigns { get; set; }

    public IEnumerable<Rentalrequir>? Rentalrequirs { get; set; }

    public IEnumerable<Carproperty>? Carproperties { get; set; }
    public Carproperty? Carproperty{ get; set; }

    public Inbox? Inbox { get; set; }
    public IEnumerable<Inbox>? Inboxes { get; set; }


    [Required(ErrorMessage = "Boş geçilemez!")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "Boş geçilemez!")]
    public string? Password { get; set; }

    public string? Image { get; set; }

    public IEnumerable<Visitor>? Visitors { get; set; }
}