using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace happylifeluxury.Models.Entities;

public partial class HappylifeContext : DbContext
{
    public HappylifeContext()
    {
    }

    public HappylifeContext(DbContextOptions<HappylifeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Campaign> Campaigns { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Carproperty> Carproperties { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Inbox> Inboxes { get; set; }

    public virtual DbSet<Insurance> Insurances { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Rentalrequir> Rentalrequirs { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }

    public virtual DbSet<Site> Sites { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<Visitor> Visitors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("server=localhost;port=3306;user=root;database=happylife", ServerVersion.AutoDetect("server=localhost;port=3306;user=root;database=happylife"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_turkish_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("admin");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");
        });

        modelBuilder.Entity<Campaign>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("campaign");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Hit)
                .HasColumnType("int(11)")
                .HasColumnName("hit");
            entity.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");
            entity.Property(e => e.IsView)
                .HasDefaultValueSql("'1'")
                .HasColumnType("tinyint(4)")
                .HasColumnName("isView");
            entity.Property(e => e.SeoTitle)
                .HasMaxLength(50)
                .HasColumnName("seoTitle");
            entity.Property(e => e.SeoUrl)
                .HasMaxLength(250)
                .HasColumnName("seoUrl");
            entity.Property(e => e.Subtitle)
                .HasMaxLength(250)
                .HasColumnName("subtitle");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("car");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CarEngineType)
                .HasMaxLength(50)
                .HasColumnName("carEngineType");
            entity.Property(e => e.CarImage)
                .HasMaxLength(500)
                .HasColumnName("carImage");
            entity.Property(e => e.CarModel)
                .HasMaxLength(50)
                .HasColumnName("carModel");
            entity.Property(e => e.CarName)
                .HasMaxLength(50)
                .HasColumnName("carName");
            entity.Property(e => e.CarPrice)
                .HasMaxLength(50)
                .HasColumnName("carPrice");
            entity.Property(e => e.CarType)
                .HasMaxLength(50)
                .HasColumnName("carType");
        });

        modelBuilder.Entity<Carproperty>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("carproperties");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("carId");
            entity.Property(e => e.Icon)
                .HasColumnType("text")
                .HasColumnName("icon");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("customer");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Adress)
                .HasMaxLength(250)
                .HasColumnName("adress");
            entity.Property(e => e.Bday)
                .HasMaxLength(50)
                .HasColumnName("bday");
            entity.Property(e => e.City)
                .HasMaxLength(50)
                .HasColumnName("city");
            entity.Property(e => e.County)
                .HasMaxLength(50)
                .HasColumnName("county");
            entity.Property(e => e.DriverDate)
                .HasMaxLength(100)
                .HasColumnName("driverDate");
            entity.Property(e => e.DriverNo)
                .HasMaxLength(50)
                .HasColumnName("driverNo");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("firstName");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("lastName");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Tc)
                .HasMaxLength(11)
                .HasColumnName("tc");
        });

        modelBuilder.Entity<Inbox>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("inbox");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.Message)
                .HasColumnType("text")
                .HasColumnName("message");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Insurance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("insurance");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.FerdiSec).HasColumnName("ferdiSec");
            entity.Property(e => e.IhtiyariSec).HasColumnName("ihtiyariSec");
            entity.Property(e => e.MiniSec).HasColumnName("miniSec");
            entity.Property(e => e.Price)
                .HasColumnType("int(50)")
                .HasColumnName("price");
            entity.Property(e => e.SuperMiniSec).HasColumnName("superMiniSec");
            entity.Property(e => e.TireglassSec).HasColumnName("tireglassSec");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("location");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.LocatioName)
                .HasMaxLength(50)
                .HasColumnName("locatioName");
        });

        modelBuilder.Entity<Rentalrequir>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("rentalrequir");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("carId");
            entity.Property(e => e.Icon)
                .HasColumnType("text")
                .HasColumnName("icon");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("reservation");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.AddId)
                .HasColumnType("int(11)")
                .HasColumnName("addId");
            entity.Property(e => e.CarId)
                .HasColumnType("int(11)")
                .HasColumnName("carId");
            entity.Property(e => e.CustomerId)
                .HasColumnType("int(11)")
                .HasColumnName("customerId");
            entity.Property(e => e.DateDropoff)
                .HasColumnType("text")
                .HasColumnName("dateDropoff");
            entity.Property(e => e.DatePickup)
                .HasColumnType("text")
                .HasColumnName("datePickup");
            entity.Property(e => e.InsuranceId)
                .HasColumnType("int(11)")
                .HasColumnName("insuranceId");
            entity.Property(e => e.LocationDropoff)
                .HasMaxLength(50)
                .HasColumnName("locationDropoff");
            entity.Property(e => e.LocationPickup)
                .HasMaxLength(50)
                .HasColumnName("locationPickup");
            entity.Property(e => e.RentalDay)
                .HasColumnType("int(50)")
                .HasColumnName("rentalDay");
            entity.Property(e => e.ReservationNo)
                .HasColumnType("text")
                .HasColumnName("reservationNo");
            entity.Property(e => e.SecurityPrice)
                .HasColumnType("int(50)")
                .HasColumnName("securityPrice");
            entity.Property(e => e.Status)
                .HasColumnType("int(11)")
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("int(11)")
                .HasColumnName("totalPrice");
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("site");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(500)
                .HasColumnName("address");
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .HasColumnName("description");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .HasColumnName("email");
            entity.Property(e => e.Facebook)
                .HasMaxLength(250)
                .HasColumnName("facebook");
            entity.Property(e => e.Favicon)
                .HasMaxLength(500)
                .HasColumnName("favicon");
            entity.Property(e => e.Instagram)
                .HasMaxLength(250)
                .HasColumnName("instagram");
            entity.Property(e => e.Logo)
                .HasMaxLength(500)
                .HasColumnName("logo");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .HasColumnName("phone");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .HasColumnName("title");
            entity.Property(e => e.Twitter)
                .HasMaxLength(250)
                .HasColumnName("twitter");
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("slide");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .HasColumnName("image");
            entity.Property(e => e.IsTarget)
                .HasMaxLength(50)
                .HasColumnName("isTarget");
            entity.Property(e => e.IsView)
                .HasDefaultValueSql("'0'")
                .HasColumnName("isView");
            entity.Property(e => e.Orders)
                .HasColumnType("int(11)")
                .HasColumnName("orders");
            entity.Property(e => e.Subtitle)
                .HasMaxLength(50)
                .HasColumnName("subtitle");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("visitor");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Browser)
                .HasMaxLength(500)
                .HasColumnName("browser");
            entity.Property(e => e.Date)
                .HasMaxLength(500)
                .HasColumnName("date");
            entity.Property(e => e.Ip)
                .HasMaxLength(250)
                .HasColumnName("ip");
            entity.Property(e => e.Url)
                .HasMaxLength(500)
                .HasColumnName("url");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
