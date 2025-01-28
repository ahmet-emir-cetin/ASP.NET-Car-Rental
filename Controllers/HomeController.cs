using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using happylifeluxury.Models;
using happylifeluxury.Models.Entities;

namespace happylifeluxury.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    HappylifeContext db = new HappylifeContext();

    void GetUserInfo()
    {
        string ipAddress = HttpContext.Connection.RemoteIpAddress!.ToString();
        string userAgent = Request.Headers["User-Agent"].ToString();
        string entryUrl = Request.Path.ToString();
        DateTime currentTime = DateTime.Now;
        Visitor toAdd = new Visitor();
        toAdd.Ip = ipAddress;
        toAdd.Browser = userAgent;
        toAdd.Url = entryUrl;
        toAdd.Date = currentTime.ToString();
        db.Add(toAdd);
        db.SaveChanges();
    }

    public IActionResult Index()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Slides = db.Slides!.OrderBy(x => x.Orders).Where(x => x.IsView == true).ToList(),
            Locations = db.Locations!.OrderBy(x => x.Id).ToList(),
            Cars = db.Cars!.OrderBy(x => x.CarPrice).ToList(),
            Reservations = db.Reservations!.ToList(),
            Customers = db.Customers!.ToList(),
            Carproperties = db.Carproperties!.ToList(),
        };
        return View(model);
    }

    [Route("/reservation/cancel")]
    public IActionResult ReservationCancel()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.OrderBy(x => x.Id).First(),
        };
        return View(model);
    }

    [HttpPost]
    [Route("/reservation/pick-car")]
    [ValidateAntiForgeryToken()]
    public IActionResult CarSelect(IndexViewModel postedData)
    {
        GetUserInfo();
        // Formdan gelen StartDateFormatted ve EndDateFormatted verilerini al
        var pickupDate = postedData.DatePickup;
        var dropoffDate = postedData.DateDropoff;
        var rentalDay = postedData.RentalDay;
        var locationPickup = postedData.LocationPickup;
        var locationDropoff = postedData.LocationDropoff;

        // Diğer işlemleri yap ve model'i tekrar doldur
        var model = new IndexViewModel()
        {
            DatePickup = pickupDate,
            DateDropoff = dropoffDate,
            RentalDay = rentalDay,
            LocationPickup = locationPickup,
            LocationDropoff = locationDropoff,
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Cars = db.Cars!.OrderBy(x => x.CarPrice).ToList(),
            EkoCars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "EKONOMİK").ToList(),
            KonforCars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "KONFOR").ToList(),
            LocationPick = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationPickup)).First(),
            LocationDrop = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationDropoff)).First(),
            Rentalrequirs = db.Rentalrequirs!.OrderBy(x => x.Id).ToList(),
            Carproperties = db.Carproperties!.OrderBy(x => x.Id).ToList(),
        };
        return View(model);
    }

    [HttpPost]
    [Route("/reservation/security-package")]
    public IActionResult SecurityPackage(IndexViewModel postedData)
    {
        GetUserInfo();
        var carId = postedData.CarId;
        var pickupDate = postedData.DatePickup;
        var dropoffDate = postedData.DateDropoff;
        var rentalDay = postedData.RentalDay;
        var locationPickup = postedData.LocationPickup;
        var locationDropoff = postedData.LocationDropoff;
        var totalPrice = postedData.TotalPrice;
        var model = new IndexViewModel()
        {
            DatePickup = pickupDate,
            DateDropoff = dropoffDate,
            RentalDay = rentalDay,
            LocationPickup = locationPickup,
            LocationDropoff = locationDropoff,
            CarId = carId,
            TotalPrice = totalPrice,
            Car = db.Cars!.Where(x => x.Id == carId).First(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            LocationPick = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationPickup)).First(),
            LocationDrop = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationDropoff)).First(),
            Insurances = db.Insurances!.ToList()
        };
        return View(model);
    }

    [HttpPost]
    [Route("/reservation/extra-prop")]
    public IActionResult ExtraProp(IndexViewModel postedData)
    {
        GetUserInfo();
        var carId = postedData.CarId;
        var pickupDate = postedData.DatePickup;
        var dropoffDate = postedData.DateDropoff;
        var rentalDay = postedData.RentalDay;
        var locationPickup = postedData.LocationPickup;
        var locationDropoff = postedData.LocationDropoff;
        var totalPrice = postedData.TotalPrice;
        var insuranceSelect = postedData.InsuranceSelect;
        int? insurancePrice = null;
        string? insuranceTitle = null;
        int? insuranceId = null;
        if (insuranceSelect == true)
        {
            insurancePrice = postedData.InsurancePrice;
            insuranceTitle = postedData.InsuranceTitle;
            insuranceId = postedData.InsuranceId;
            totalPrice += Convert.ToInt16(rentalDay) * insurancePrice.GetValueOrDefault();
        }
        var model = new IndexViewModel()
        {
            DatePickup = pickupDate,
            DateDropoff = dropoffDate,
            RentalDay = rentalDay,
            LocationPickup = locationPickup,
            LocationDropoff = locationDropoff,
            CarId = carId,
            TotalPrice = totalPrice,
            InsurancePrice = insurancePrice,
            InsuranceTitle = insuranceTitle,
            InsuranceId = insuranceId,

            Car = db.Cars!.Where(x => x.Id == carId).First(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            LocationPick = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationPickup)).First(),
            LocationDrop = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationDropoff)).First(),
            Insurance = db.Insurances!.Where(x => x.Id == insuranceId).First()
        };
        return View(model);
    }

    [HttpPost]
    [Route("/reservation/driver")]
    public IActionResult DriverDetails(IndexViewModel postedData)
    {
        GetUserInfo();
        var carId = postedData.CarId;
        var pickupDate = postedData.DatePickup;
        var dropoffDate = postedData.DateDropoff;
        var rentalDay = postedData.RentalDay;
        var locationPickup = postedData.LocationPickup;
        var locationDropoff = postedData.LocationDropoff;
        var totalPrice = postedData.TotalPrice;
        var insuranceSelect = postedData.InsuranceSelect;
        int? insurancePrice = null;
        string? insuranceTitle = null;
        int? insuranceId = null;
        if (insuranceSelect == true)
        {
            // insurancePrice = postedData.InsurancePrice;
            // insuranceTitle = postedData.InsuranceTitle;
            insuranceId = postedData.InsuranceId;
            insurancePrice = db.Insurances.FirstOrDefault(x => x.Id == insuranceId)!.Price * Convert.ToInt16(rentalDay);
            insuranceTitle = db.Insurances.FirstOrDefault(x => x.Id == insuranceId)!.Title;
            totalPrice += insurancePrice.GetValueOrDefault();
        }
        var model = new IndexViewModel()
        {
            DatePickup = pickupDate,
            DateDropoff = dropoffDate,
            RentalDay = rentalDay,
            LocationPickup = locationPickup,
            LocationDropoff = locationDropoff,
            CarId = carId,
            TotalPrice = totalPrice,
            InsurancePrice = insurancePrice,
            InsuranceTitle = insuranceTitle,
            InsuranceId = insuranceId,

            Car = db.Cars!.Where(x => x.Id == carId).First(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            LocationPick = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationPickup)).First(),
            LocationDrop = db.Locations!.OrderBy(x => x.Id).Where(x => x.Id == Convert.ToInt16(locationDropoff)).First(),
            Insurance = db.Insurances!.FirstOrDefault(x => x.Id == insuranceId),
        };
        return View(model);
    }

    [HttpPost]
    [Route("/reservation/add")]
    [ValidateAntiForgeryToken()]
    public IActionResult DriverAdd(IndexViewModel postedData, Reservation postedData1, Customer postedData2)
    {
        if (postedData != null && postedData1 != null && postedData2 != null)
        {
            
            Customer toAddCustomer = new Customer();
            int customerId = toAddCustomer.Id;

            var tcCheck = db.Customers.FirstOrDefault(x => x.Tc == postedData2.Tc);
            if (tcCheck != null)
            {
                customerId = tcCheck.Id;
            }
            else
            {
                toAddCustomer.FirstName = postedData2.FirstName;
                toAddCustomer.LastName = postedData2.LastName;
                toAddCustomer.Tc = postedData2.Tc;
                toAddCustomer.DriverNo = postedData2.DriverNo;
                toAddCustomer.DriverDate = postedData2.DriverDate;
                toAddCustomer.Bday = postedData2.Bday;
                toAddCustomer.City = postedData2.City;
                toAddCustomer.County = postedData2.County;
                toAddCustomer.Adress = postedData2.Adress;
                toAddCustomer.Phone = postedData2.Phone;
                toAddCustomer.Email = postedData2.Email;
                db.Add(toAddCustomer);
                db.SaveChanges();
                customerId = toAddCustomer.Id;
            }



            Reservation toAdd = new Reservation();
            string reservationNumber = DateTime.Now.ToString("yyMMddHHmmss") + postedData2.Tc.Substring(postedData2.Tc.Length - 2);
            toAdd.LocationPickup = postedData.LocationPickup!;
            toAdd.LocationDropoff = postedData.LocationDropoff!;
            toAdd.DatePickup = postedData.DatePickup!;
            toAdd.DateDropoff = postedData.DateDropoff!;
            toAdd.RentalDay = Convert.ToInt16(postedData.RentalDay);
            toAdd.TotalPrice = postedData.TotalPrice;
            toAdd.CarId = Convert.ToInt16(postedData1.CarId);
            toAdd.CustomerId = customerId;
            toAdd.InsuranceId = Convert.ToInt16(postedData.InsuranceId);
            // var insurancePrice = db.Insurances.FirstOrDefault(x => x.Id == postedData.InsuranceId)!.Price;
            toAdd.SecurityPrice = Convert.ToInt16(postedData.InsurancePrice);
            toAdd.ReservationNo = reservationNumber;

            db.Add(toAdd);
            db.SaveChanges();
            @TempData["Success"] = "Rezervasyonunuz oluşturulmuştur! Rezervasyon No:" + reservationNumber;
        }

        return Redirect("/");
    }

    [Route("/cars")]
    public IActionResult Cars()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Cars = db.Cars!.OrderBy(x => x.CarPrice).ToList(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Carproperties = db.Carproperties.ToList()
        };
        return View(model);
    }

    [Route("/cars/eco")]
    public IActionResult CarEco()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Cars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "EKONOMİK").ToList(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Carproperties = db.Carproperties.ToList()
        };
        return View(model);
    }

    [Route("/cars/confor")]
    public IActionResult CarConfor()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Cars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "KONFOR").ToList(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Carproperties = db.Carproperties.ToList()
        };
        return View(model);
    }

    [Route("/cars/prestige")]
    public IActionResult CarPrestige()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Cars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "PRESTİJ" || x.CarType == "PRESTIJ").ToList(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Carproperties = db.Carproperties.ToList()
        };
        return View(model);
    }

    [Route("/cars/vip")]
    public IActionResult CarPremium()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Cars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "VİP" || x.CarType == "VIP").ToList(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Carproperties = db.Carproperties.ToList()
        };
        return View(model);
    }

    [Route("/cars/lux")]
    public IActionResult CarLux()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Cars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "LUKS" || x.CarType == "LÜKS").ToList(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Carproperties = db.Carproperties.ToList()
        };
        return View(model);
    }

    [Route("/cars/van")]
    public IActionResult CarVan()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Cars = db.Cars!.OrderBy(x => x.CarPrice).Where(x => x.CarType == "VAN").ToList(),
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Carproperties = db.Carproperties.ToList()
        };
        return View(model);
    }

    [HttpPost]
    [Route("/reservation/cancel")]
    [ValidateAntiForgeryToken]
    public IActionResult ReservationCancel(Reservation postedData, Customer customer)
    {
        if (postedData != null && customer != null)
        {
            var toDelete = db.Reservations!.FirstOrDefault(x => x.ReservationNo == postedData.ReservationNo);
            if (toDelete != null)
            {
                var findCustomer = db.Customers.Find(toDelete.CustomerId);
                if (findCustomer != null)
                {
                    if (findCustomer.LastName == customer.LastName)
                    {
                        toDelete.Status = 3;
                        db.Entry(toDelete).CurrentValues.SetValues(toDelete);
                        db.SaveChanges();
                        TempData["Success"] = "Rezervasyonunuz İptal Edilmiştir.";
                    }
                }

            }
            else
            {
                TempData["Danger"] = "Rezervasyon kaydı bulunamadı.";
            }
        }
        else
        {
            TempData["Danger"] = "Rezervasyon kaydı bulunamadı.";
        }
        return Redirect("/reservation/cancel");
    }

    [Route("/campaign/")]
    public IActionResult Campaign()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Campaigns = db.Campaigns!.Where(x => x.IsView == 1).ToList(),
        };
        return View(model);
    }

    [Route("/campaign/{title}/{id}")]
    public IActionResult CampaignDetails(String title, int id)
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.OrderBy(x => x.Id).First(),
            Campaigns = db.Campaigns!.OrderBy(x => x.IsView).ToList(),
            Campaign = db.Campaigns!.FirstOrDefault(x => x.Id == id && x.IsView == 1),
        };
        Campaign toUpdate = db.Campaigns!.Find(id)!;
        toUpdate.Hit = toUpdate.Hit + 1;
        db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
        db.SaveChanges();
        return View(model);
    }

    [Route("/contact")]
    public IActionResult Contact()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.OrderBy(x => x.Id).First(),
        };
        return View(model);
    }

    [HttpPost]
    [Route("/contact/add")]
    [ValidateAntiForgeryToken]
    public IActionResult ContactAdd(Inbox postedData)
    {
        if (postedData != null)
        {
            Inbox toAdd = new Inbox();
            toAdd.Name = postedData.Name;
            toAdd.Email = postedData.Email;
            toAdd.Phone = postedData.Phone;
            toAdd.Title = postedData.Title;
            toAdd.Message = postedData.Message;

            db.Add(toAdd);
            db.SaveChanges();
            TempData["Success"] = "Mesaj gönderildi";
        }
        else
        {
            TempData["Danger"] = "Mesaj gönderilemedi";
        }
        return Redirect("/contact");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

