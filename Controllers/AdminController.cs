using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using happylifeluxury.Models;
using happylifeluxury.Models.Entities;
using System.Globalization;

namespace happylifeluxury.Controllers;

public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public AdminController(IWebHostEnvironment webHostEnvironment, ILogger<AdminController> logger)
    {
        _logger = logger;
        _webHostEnvironment = webHostEnvironment;
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

    [Authorize]
    [Route("/admin/")]
    public IActionResult Index()
    {
        GetUserInfo();
        var model = new IndexViewModel
        {
            Customers = db.Customers!.OrderByDescending(x => x.Id).ToList(),
            Reservations = db.Reservations!.OrderBy(x => x.TotalPrice).ToList(),
            Visitors = db.Visitors.ToList(),
        };
        return View(model);
    }

    [Authorize]
    [Route("/admin/settings/site-settings")]
    public IActionResult AdminSettings()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.First(),
        };
        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/settings/site-update")]
    public IActionResult AdminUpdate(Site postedData, IFormFile file)
    {
        var toUpdate = db.Sites!.Find(postedData.Id);
        if (toUpdate != null)
        {
            if (file != null)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/");
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                toUpdate.Logo = fileName;
                toUpdate.Title = postedData.Title;
                toUpdate.Description = postedData.Description;

                db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
                db.SaveChanges();
                TempData["Success"] = "Güncellendi";
            }
            else
            {
                toUpdate.Title = postedData.Title;
                toUpdate.Description = postedData.Description;
                db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
                db.SaveChanges();
                TempData["Success"] = "Güncellendi";
            }
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/settings/site-settings");
    }

    [Authorize]
    [Route("/admin/settings/contact-settings")]
    public IActionResult ContactSettings()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.First(),
        };
        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/settings/contact-update")]
    public IActionResult ContactUpdate(Site postedData)
    {
        var toUpdate = db.Sites!.Find(postedData.Id);
        if (toUpdate != null)
        {
            toUpdate.Email = postedData.Email;
            toUpdate.Phone = postedData.Phone;
            toUpdate.Address = postedData.Address;
            db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
            db.SaveChanges();
            TempData["Success"] = "Güncellendi";
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/settings/contact-settings");
    }

    [Authorize]
    [Route("/admin/settings/social-settings")]
    public IActionResult SocialSettings()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Site = db.Sites!.First(),
        };
        return View(model);
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/settings/social-update")]
    public IActionResult SocialUpdate(Site postedData)
    {
        var toUpdate = db.Sites!.Find(postedData.Id);
        if (toUpdate != null)
        {
            toUpdate.Instagram = postedData.Instagram;
            toUpdate.Facebook = postedData.Facebook;
            toUpdate.Twitter = postedData.Twitter;
            db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
            db.SaveChanges();
            TempData["Success"] = "Güncellendi";
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/settings/social-settings");
    }

    [Authorize]
    [Route("/admin/reservation")]
    public IActionResult Reservation()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
        };
        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/reservation/add")]
    public IActionResult ReservationAdd(Reservation postedData)
    {
        if (postedData != null)
        {
            Reservation toAdd = new Reservation();
            toAdd.CustomerId = postedData.CustomerId;
            var tcNo = db.Customers.FirstOrDefault(x => x.Id == toAdd.CustomerId);
            if (tcNo != null)
            {
                string reservationNumber = DateTime.Now.ToString("yyMMddHHmmss") + tcNo.Tc.Substring(tcNo.Tc.Length - 2);
                toAdd.ReservationNo = reservationNumber;
            }
            toAdd.LocationPickup = postedData.LocationPickup;
            toAdd.LocationDropoff = postedData.LocationDropoff;

            DateTime datePickup = DateTime.ParseExact(postedData.DatePickup, "dd MMMM yyyy", new CultureInfo("tr-TR"));
            DateTime dateDropoff = DateTime.ParseExact(postedData.DateDropoff, "dd MMMM yyyy", new CultureInfo("tr-TR"));

            toAdd.DatePickup = datePickup.ToString("dd-MM-yyyy");
            toAdd.DateDropoff = dateDropoff.ToString("dd-MM-yyyy");

            // toAdd.DatePickup = Convert.ToDateTime(postedData.DatePickup).ToString("dd-MM-yyyy");
            // toAdd.DateDropoff = Convert.ToDateTime(postedData.DateDropoff).ToString("dd-MM-yyyy");
            toAdd.RentalDay = postedData.RentalDay;
            toAdd.CarId = postedData.CarId;
            toAdd.InsuranceId = postedData.InsuranceId;
            var secPrice = db.Insurances.Find(toAdd.InsuranceId);
            if (secPrice != null)
            {
                toAdd.SecurityPrice = toAdd.RentalDay * secPrice.Price;
            }
            else
            {
                toAdd.SecurityPrice = 0;
            }
            var carPrice = db.Cars.Find(toAdd.CarId);
            toAdd.TotalPrice = (toAdd.RentalDay * Convert.ToInt16(carPrice!.CarPrice)) + toAdd.SecurityPrice;
            toAdd.Status = postedData.Status;

            db.Add(toAdd);
            db.SaveChanges();
            TempData["Success"] = "Başarılı bir şekilde rezervasyon oluşturuldu!";
        }
        else
        {
            TempData["Danger"] = "Rezervasyon oluşturulamadı";
        }
        return Redirect("/admin/reservation");
    }

    [Authorize]
    [Route("/admin/reservation/edit/{id:int}")]
    public IActionResult ReservationEdit(int id)
    {
        var model = new IndexViewModel
        {
            Reservation = db.Reservations!.Find(id),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList()
        };
        return PartialView("Admin/_EditReservation", model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/reservation/update")]
    public IActionResult ReservationUpdate(Reservation postedData)
    {
        var toUpdate = db.Reservations!.Find(postedData.Id);
        if (toUpdate != null)
        {
            toUpdate.LocationPickup = postedData.LocationPickup;
            toUpdate.LocationDropoff = postedData.LocationDropoff;
            toUpdate.CustomerId = postedData.CustomerId;
            toUpdate.CarId = postedData.CarId;
            toUpdate.InsuranceId = postedData.InsuranceId;
            var secPrice = db.Insurances.Find(toUpdate.InsuranceId);
            if (secPrice != null)
            {
                toUpdate.SecurityPrice = toUpdate.RentalDay * secPrice.Price;
            }
            else
            {
                toUpdate.SecurityPrice = 0;
            }
            var carPrice = db.Cars.Find(toUpdate.CarId);
            toUpdate.TotalPrice = (toUpdate.RentalDay * Convert.ToInt16(carPrice!.CarPrice)) + toUpdate.SecurityPrice;
            toUpdate.Status = postedData.Status;

            db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
            db.SaveChanges();
            TempData["Success"] = "Güncellendi";
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/reservation");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/reservation/delete")]
    public IActionResult ReservationDelete(Reservation postedData)
    {
        var toDelete = db.Reservations.Find(postedData.Id);
        if (toDelete != null)
        {
            db.Remove(toDelete);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde silindi!";
        }
        else
        {
            @TempData["Danger"] = "İşlem gerçekleştirilemedi";
        }
        return Redirect("/admin/reservation");
    }

    [Authorize]
    [Route("/admin/car")]
    public IActionResult Car()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
        };
        return View(model);
    }

    [Authorize]
    [Route("/admin/car/edit/{id:int}")]
    public IActionResult CarEdit(int id)
    {
        var model = new IndexViewModel
        {
            Car = db.Cars!.Find(id),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList()
        };
        return PartialView("Admin/_EditCar", model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/car/update")]
    public IActionResult CarUpdate(Car postedData, IFormFile file)
    {
        var toUpdate = db.Cars!.Find(postedData.Id);
        if (toUpdate != null)
        {
            if (file != null)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/cars/");
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                toUpdate.CarName = postedData.CarName;
                toUpdate.CarModel = postedData.CarModel;
                toUpdate.CarPrice = postedData.CarPrice;
                toUpdate.CarType = postedData.CarType;
                toUpdate.CarImage = fileName;

                db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
                db.SaveChanges();
                TempData["Success"] = "Güncellendi";
            }
            else
            {
                toUpdate.CarName = postedData.CarName;
                toUpdate.CarModel = postedData.CarModel;
                toUpdate.CarPrice = postedData.CarPrice;
                toUpdate.CarType = postedData.CarType;
                db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
                db.SaveChanges();
                TempData["Success"] = "Güncellendi";
            }
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/car");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/car/add")]
    public IActionResult CarAdd(Car postedData, IFormFile file)
    {
        if (postedData != null)
        {
            if (file != null)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/cars/");
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Car toAdd = new Car();
                toAdd.CarImage = fileName;
                toAdd.CarName = postedData.CarName;
                toAdd.CarModel = postedData.CarModel;
                toAdd.CarPrice = postedData.CarPrice;
                toAdd.CarType = postedData.CarType;
                toAdd.CarEngineType = "Dizel/Benzin";

                db.Add(toAdd);
                db.SaveChanges();
                @TempData["Success"] = "Başarılı bir şekilde Eklendi!";
            }
            else
            {
                @TempData["Danger"] = "Dosya bulunamadı !";
            }
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/car");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/car/delete")]
    public IActionResult CarDelete(Car postedData)
    {
        var toDelete = db.Cars.Find(postedData.Id);
        if (toDelete != null)
        {
            db.Remove(toDelete);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde silindi!";
        }
        else
        {
            @TempData["Danger"] = "İşlem gerçekleştirilemedi";
        }
        return Redirect("/admin/car");
    }

    [Authorize]
    [Route("/admin/slide")]
    public IActionResult Slide()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
            Slides = db.Slides.OrderBy(x => x.Id).ToList(),
        };
        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/slide/add")]
    public IActionResult SlideAdd(Slide postedData, IFormFile file)
    {
        if (postedData != null)
        {
            if (file != null)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/slide/");
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Slide toAdd = new Slide();
                toAdd.Image = fileName;
                toAdd.Title = postedData.Title;
                toAdd.Subtitle = postedData.Subtitle;
                toAdd.Orders = postedData.Orders;
                toAdd.IsView = postedData.IsView;

                db.Add(toAdd);
                db.SaveChanges();
                @TempData["Success"] = "Başarılı bir şekilde Eklendi!";
            }
            else
            {
                @TempData["Danger"] = "Dosya bulunamadı !";
            }
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/slide");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/slide/delete")]
    public IActionResult SlideDelete(Slide postedData)
    {
        var toDelete = db.Slides.Find(postedData.Id);
        if (toDelete != null)
        {
            db.Remove(toDelete);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde silindi!";
        }
        else
        {
            @TempData["Danger"] = "İşlem gerçekleştirilemedi";
        }
        return Redirect("/admin/slide");
    }


    [Authorize]
    [Route("/admin/location")]
    public IActionResult Location()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
            Slides = db.Slides.OrderBy(x => x.Id).ToList(),
        };
        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/location/add")]
    public IActionResult LocationAdd(Location postedData)
    {
        if (postedData != null)
        {
            Location toAdd = new Location();
            toAdd.LocatioName = postedData.LocatioName;
            db.Add(toAdd);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde Eklendi!";
        }
        else
        {
            TempData["Danger"] = "Eklenemedi";
        }
        return Redirect("/admin/location");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/location/delete")]
    public IActionResult LocationDelete(Location postedData)
    {
        var toDelete = db.Locations.Find(postedData.Id);
        if (toDelete != null)
        {
            db.Remove(toDelete);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde silindi!";
        }
        else
        {
            @TempData["Danger"] = "İşlem gerçekleştirilemedi";
        }
        return Redirect("/admin/location");
    }

    [Authorize]
    [Route("/admin/campaign")]
    public IActionResult Campaigns()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
            Slides = db.Slides.OrderBy(x => x.Id).ToList(),
            Campaigns = db.Campaigns.OrderBy(x => x.Id).ToList(),
        };
        return View(model);
    }

    [Authorize]
    [Route("/admin/campaign/edit/{id:int}")]
    public IActionResult CampaignEdit(int id)
    {
        var model = new IndexViewModel
        {
            Campaign = db.Campaigns!.Find(id),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList()
        };
        return PartialView("Admin/_EditCampaign", model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/campaign/update")]
    public IActionResult CampaignUpdate(Campaign postedData, IFormFile file)
    {
        var toUpdate = db.Campaigns!.Find(postedData.Id);
        if (toUpdate != null)
        {
            if (file != null)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/campaigns/");
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                toUpdate.Image = fileName;
                toUpdate.Title = postedData.Title;
                toUpdate.Subtitle = postedData.Subtitle;
                toUpdate.Description = postedData.Description;
                toUpdate.IsView = postedData.IsView;
                toUpdate.SeoUrl = postedData.SeoUrl;
                toUpdate.SeoTitle = postedData.SeoTitle;


                db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
                db.SaveChanges();
                TempData["Success"] = "Güncellendi";
            }
            else
            {
                toUpdate.Title = postedData.Title;
                toUpdate.Subtitle = postedData.Subtitle;
                toUpdate.Description = postedData.Description;
                toUpdate.IsView = postedData.IsView;
                toUpdate.SeoUrl = postedData.SeoUrl;
                toUpdate.SeoTitle = postedData.SeoTitle;
                db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
                db.SaveChanges();
                TempData["Success"] = "Güncellendi";
            }
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/campaign");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/campaign/add")]
    public IActionResult CampaignAdd(Campaign postedData, IFormFile file)
    {
        if (postedData != null)
        {
            if (file != null)
            {
                string extension = System.IO.Path.GetExtension(file.FileName);
                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/images/campaigns/");
                string fileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadFolder, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Campaign toAdd = new Campaign();
                toAdd.Image = fileName;
                toAdd.Title = postedData.Title;
                toAdd.Subtitle = postedData.Subtitle;
                toAdd.Description = postedData.Description;
                toAdd.SeoTitle = postedData.SeoTitle;
                toAdd.SeoUrl = postedData.SeoUrl;
                toAdd.IsView = postedData.IsView;

                db.Add(toAdd);
                db.SaveChanges();
                @TempData["Success"] = "Başarılı bir şekilde Eklendi!";
            }
            else
            {
                @TempData["Danger"] = "Dosya bulunamadı !";
            }
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/campaign");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/campaign/delete")]
    public IActionResult CamapaignDelete(Campaign postedData)
    {
        var toDelete = db.Campaigns.Find(postedData.Id);
        if (toDelete != null)
        {
            db.Remove(toDelete);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde silindi!";
        }
        else
        {
            @TempData["Danger"] = "İşlem gerçekleştirilemedi";
        }
        return Redirect("/admin/campaign");
    }

    [Authorize]
    [Route("/admin/customer")]
    public IActionResult Customer()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
            Slides = db.Slides.OrderBy(x => x.Id).ToList(),
            Campaigns = db.Campaigns.OrderBy(x => x.Id).ToList(),
        };
        return View(model);
    }

    [Authorize]
    [Route("/admin/customer/edit/{id:int}")]
    public IActionResult CustomerEdit(int id)
    {
        var model = new IndexViewModel
        {
            Customer = db.Customers!.Find(id),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList()
        };
        return PartialView("Admin/_EditCustomer", model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/customer/update")]
    public IActionResult CustomerUpdate(Customer postedData)
    {
        var toUpdate = db.Customers!.Find(postedData.Id);
        if (toUpdate != null)
        {
            toUpdate.FirstName = postedData.FirstName;
            toUpdate.LastName = postedData.LastName;
            toUpdate.DriverNo = postedData.DriverNo;
            toUpdate.Phone = postedData.Phone;
            toUpdate.Email = postedData.Email;
            db.Entry(toUpdate).CurrentValues.SetValues(toUpdate);
            db.SaveChanges();
        }
        else
        {
            TempData["Danger"] = "Güncellenemedi";
        }
        return Redirect("/admin/customer");
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/customer/add")]
    public IActionResult CustomerAdd(Customer postedData)
    {
        if (postedData != null)
        {
            Customer toAdd = new Customer();
            toAdd.FirstName = postedData.FirstName;
            toAdd.LastName = postedData.LastName;
            toAdd.Tc = postedData.Tc;
            toAdd.DriverNo = postedData.DriverNo;
            toAdd.DriverDate = postedData.DriverDate;
            toAdd.Bday = postedData.Bday;
            toAdd.City = postedData.City;
            toAdd.County = postedData.County;
            toAdd.Adress = postedData.Adress;
            toAdd.Phone = postedData.Phone;
            toAdd.Email = postedData.Email;

            db.Add(toAdd);
            db.SaveChanges();
            TempData["Success"] = "Başarılı bir şekilde müşteri eklendi!";
        }
        else
        {
            TempData["Danger"] = "Müşteri eklenemedi";
        }
        return Redirect("/admin/customer");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/customer/delete")]
    public IActionResult CustomerDelete(Customer postedData)
    {
        var toDelete = db.Customers.Find(postedData.Id);
        if (toDelete != null)
        {
            db.Remove(toDelete);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde silindi!";
        }
        else
        {
            @TempData["Danger"] = "İşlem gerçekleştirilemedi";
        }
        return Redirect("/admin/customer");
    }

    [Authorize]
    [Route("/admin/carprop")]
    public IActionResult CarProp()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
            Slides = db.Slides.OrderBy(x => x.Id).ToList(),
            Campaigns = db.Campaigns.OrderBy(x => x.Id).ToList(),
            Carproperties = db.Carproperties.OrderBy(x => x.Id).ToList()
        };
        return View(model);
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    [Route("/admin/carprop/add")]
    public IActionResult CarPropAdd(Carproperty postedData)
    {
        if (postedData != null)
        {
            Carproperty toAdd = new Carproperty();
            toAdd.CarId = postedData.CarId;
            toAdd.Title = postedData.Title;
            toAdd.Icon = postedData.Icon;

            db.Add(toAdd);
            db.SaveChanges();
            TempData["Success"] = "Başarılı bir şekilde eklendi!";
        }
        else
        {
            TempData["Danger"] = "Araç çzelliği eklenemedi!";
        }
        return Redirect("/admin/carprop");
    }

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("/admin/carprop/delete")]
    public IActionResult CarPropDelete(Carproperty postedData)
    {
        var toDelete = db.Carproperties.Find(postedData.Id);
        if (toDelete != null)
        {
            db.Remove(toDelete);
            db.SaveChanges();
            @TempData["Success"] = "Başarılı bir şekilde silindi!";
        }
        else
        {
            @TempData["Danger"] = "İşlem gerçekleştirilemedi";
        }
        return Redirect("/admin/carprop");
    }

    [Authorize]
    [Route("/admin/inbox")]
    public IActionResult Inbox()
    {
        GetUserInfo();
        var model = new IndexViewModel()
        {
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
            Slides = db.Slides.OrderBy(x => x.Id).ToList(),
            Campaigns = db.Campaigns.OrderBy(x => x.Id).ToList(),
            Carproperties = db.Carproperties.OrderBy(x => x.Id).ToList(),
            Inboxes = db.Inboxes.OrderBy(x => x.Id).ToList()
        };
        return View(model);
    }

    [Authorize]
    [Route("/admin/inbox/read/{id:int}")]
    public IActionResult InboxRead(int id)
    {
        var model = new IndexViewModel()
        {
            Inbox = db.Inboxes.Find(id),
            Reservations = db.Reservations.OrderByDescending(x => x.Id).ToList(),
            Customers = db.Customers.OrderByDescending(x => x.Id).ToList(),
            Locations = db.Locations.OrderByDescending(x => x.Id).ToList(),
            Cars = db.Cars.OrderByDescending(x => x.Id).ToList(),
            Insurances = db.Insurances.OrderByDescending(x => x.Id).ToList(),
            Slides = db.Slides.OrderBy(x => x.Id).ToList(),
            Campaigns = db.Campaigns.OrderBy(x => x.Id).ToList(),
            Carproperties = db.Carproperties.OrderBy(x => x.Id).ToList()
        };
        return View(model);
    }

    public IActionResult Login()
    {
        GetUserInfo();
        var model = new IndexViewModel
        {
            Site = db.Sites!.OrderBy(x => x.Id).First(),
        };
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(Admin postedData)
    {
        Admin user = db.Admins!.FirstOrDefault(x => x.Username == Crytology.Decryption(postedData.Username.ToString().TrimEnd(), -2) && x.Password == Crytology.Decryption(postedData.Password.ToString().TrimEnd(), -2))!;

        if (user != null)
        {
            var claims = new List<Claim>{
                new Claim("user",user.Id.ToString()),
                new Claim("role","admin")
            };
            var claimsIdenitity = new ClaimsIdentity(claims, "cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdenitity);

            await HttpContext.SignInAsync(claimsPrincipal);
            return Redirect("/admin/");
        }
        else
        {
            TempData["Danger"] = "Girilen Bilgiler Hatalı!";
            return Redirect("/admin/login");
        }
    }

    [Route("/admin/logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        TempData["Success"] = "Güvenli bir şekilde çıkış yapıldı!";
        return Redirect("/admin/login");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}