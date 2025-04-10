using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WypozyczalniaP2P.Models;

namespace WypozyczalniaP2P.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Administrator> Administratorzy { get; set; }
    public DbSet<Klient> Klienci { get; set; }
    public DbSet<Ogloszenie> Ogłoszenia { get; set; }
    public DbSet<OpiniaKlienta> OpinieKlientow { get; set; }
    public DbSet<OpiniaSamochodu> OpinieSamochodow { get; set; }
    public DbSet<Samochod> Samochody { get; set; }
    public DbSet<Pracownik> Pracownicy { get; set; }
    public DbSet<TypSamochodu> TypySamochodow { get; set; }
    public DbSet<Wynajem> Wynajmy { get; set; }
    public DbSet<Wypozyczenie> Wypozyczenia { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Klient>().ToTable("Klienci");
        builder.Entity<Administrator>().ToTable("Administratorzy");
        builder.Entity<Pracownik>().ToTable("Pracownicy");

        // Unikalny indeks na PhoneNumber w tabeli AspNetUsers
        builder.Entity<IdentityUser>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique()
            .HasFilter("[PhoneNumber] IS NOT NULL");

        // Unikalny indeks dla NumerPrawaJazdy w Klient
        builder.Entity<Klient>()
            .HasIndex(k => k.NumerPrawaJazdy)
            .IsUnique();

        // Unikalny indeks dla NumerRejestracyjnego w Samochod
        builder.Entity<Samochod>()
            .HasIndex(s => s.NumerRejestracyjny)
            .IsUnique();

        // Unikalny indeks dla VIN w Samochod
        builder.Entity<Samochod>()
            .HasIndex(s => s.Vin)
            .IsUnique();

        // Relacja Klient (właściciel) -> Samochod
        builder.Entity<Samochod>()
            .HasOne(s => s.Wlasciciel)
            .WithMany(k => k.Flota)
            .HasForeignKey(s => s.WlascicielId)
            .OnDelete(DeleteBehavior.Cascade);
            

        // Relacja TypAuta -> Samochod
        builder.Entity<Samochod>()
            .HasOne(s => s.TypSamochodu)
            .WithMany(t => t.Samochody)
            .HasForeignKey(s => s.TypSamochoduId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Samochod>().HasData(
            new Samochod
            {
                Id = 1,
                Marka = "Toyota",
                Model = "Corolla",
                TypSamochoduId = 1, // Sedan
                NumerRejestracyjny = "WWA1234",
                RokProdukcji = 2019,
                Przebieg = 45000,
                MocSilnika = 132,
                PojemnoscSilnika = 2,
                RodzajPaliwa = Samochod.Paliwo.Benzyna,
                Skrzynia = Samochod.SkrzyniaBiegow.Automatyczna,
                LiczbaMiejsc = Samochod.IloscMiejsc.Pięć,
                IloscDrzwi = Samochod.Drzwi.Cztery,
                RodzajNapedu = Samochod.Naped.FWD,
                Vin = "JTDBR32E042013579",
                Kolor = Samochod.Kolory.Czarny,
                WlascicielId = "85b4c380-cb76-41ff-9387-eed59e4040a2",
                Zdjecie = "default.jpg"
            },
                new Samochod
                {
                    Id = 2,
                    Marka = "Volkswagen",
                    Model = "Tiguan",
                    TypSamochoduId = 2, // SUV
                    NumerRejestracyjny = "WRO5678",
                    RokProdukcji = 2020,
                    Przebieg = 30000,
                    MocSilnika = 150,
                    PojemnoscSilnika = 3,
                    RodzajPaliwa = Samochod.Paliwo.Diesel,
                    Skrzynia = Samochod.SkrzyniaBiegow.Manualna,
                    LiczbaMiejsc = Samochod.IloscMiejsc.Pięć,
                    IloscDrzwi = Samochod.Drzwi.Pięć,
                    RodzajNapedu = Samochod.Naped.AWD,
                    Vin = "WVGZZZ5NZLW123456",
                    Kolor = Samochod.Kolory.Srebrny,
                    WlascicielId = "85b4c380-cb76-41ff-9387-eed59e4040a2",
                    Zdjecie = "default.jpg"
                },
                new Samochod
                {
                    Id = 3,
                    Marka = "Ford",
                    Model = "Fiesta",
                    TypSamochoduId = 3, // Hatchback
                    NumerRejestracyjny = "KRA9012",
                    RokProdukcji = 2018,
                    Przebieg = 60000,
                    MocSilnika = 100,
                    PojemnoscSilnika = 1,
                    RodzajPaliwa = Samochod.Paliwo.Benzyna,
                    Skrzynia = Samochod.SkrzyniaBiegow.Manualna,
                    LiczbaMiejsc = Samochod.IloscMiejsc.Pięć,
                    IloscDrzwi = Samochod.Drzwi.Pięć,
                    RodzajNapedu = Samochod.Naped.FWD,
                    Vin = "WF0DXXGAJD1234567",
                    Kolor = Samochod.Kolory.Czerwony,
                    WlascicielId = "85b4c380-cb76-41ff-9387-eed59e4040a2",
                    Zdjecie = "default.jpg"
                }
            );

        // Wartości domyślne dla TypSamochodu
        builder.Entity<TypSamochodu>().HasData(
                new TypSamochodu { Id = 1, Nazwa = "Sedan", Opis = "Rodzinna limuzyna" },
                new TypSamochodu { Id = 2, Nazwa = "SUV", Opis = "Duże auto" },
                new TypSamochodu { Id = 3, Nazwa = "Hatchback", Opis = "Miejskie auto" },
                new TypSamochodu  { Id = 4, Nazwa = "Combi", Opis = "Długie auto" }
            );

        // Relacja Opinia -> Uzytkownik (Autor)
        builder.Entity<OpiniaKlienta>()
            .HasOne(o => o.Autor)
            .WithMany(a => a.MojeOpinieKlientow)
            .HasForeignKey(o => o.AutorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relacja Opinia -> Klient (opcjonalna)
        builder.Entity<OpiniaKlienta>()
            .HasOne(o => o.Klient)
            .WithMany(a => a.OpinieWypozyczen)
            .HasForeignKey(o => o.KlientId)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Relacja Opinia -> Samochod (opcjonalna)
        builder.Entity<OpiniaSamochodu>()
            .HasOne(o => o.Samochod)
            .WithMany(a => a.Opinie)
            .HasForeignKey(o => o.SamochodId)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Relacja Ogloszenie -> Uzytkownik
        builder.Entity<Ogloszenie>()
            .HasOne(o => o.Klient)
            .WithMany(a => a.Ogloszenia)
            .HasForeignKey(o => o.KlientId)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Relacja Ogloszenie -> Samochod
        builder.Entity<Ogloszenie>()
            .HasOne(o => o.Samochod)
            .WithMany(s => s.Ogloszenia)
            .HasForeignKey(o => o.SamochodId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<Wynajem>()
            .HasOne(o => o.Klient)
            .WithMany(a => a.Wynajmy)
            .HasForeignKey(o => o.KlientId)
            .OnDelete(DeleteBehavior.ClientCascade);

        // Relacja Ogloszenie -> Samochod
        builder.Entity<Wynajem>()
            .HasOne(o => o.Samochod)
            .WithMany(s => s.Wynajmy)
            .HasForeignKey(o => o.SamochodId)
            .OnDelete(DeleteBehavior.ClientCascade);

        builder.Entity<Wypozyczenie>()
               .HasOne(w => w.Klient)
               .WithMany(k => k.Wypozyczenia)
               .HasForeignKey(w => w.KlientId)
               .IsRequired()
               .OnDelete(DeleteBehavior.ClientCascade); // Opcjonalnie: blokada kasowania

        // Konfiguracja relacji z Samochodem
        builder.Entity<Wypozyczenie>()
               .HasOne(w => w.Samochod)
               .WithMany(s => s.Wypozyczenia)
               .HasForeignKey(w => w.SamochodId)
               .IsRequired()
               .OnDelete(DeleteBehavior.ClientCascade);

        // Konfiguracja relacji z Wypożyczającym (WypozyczajacyId)
        builder.Entity<Wypozyczenie>()
               .HasOne(w => w.Wypozyczajacy)
               .WithMany(k => k.Wypozyczone)
               .HasForeignKey(w => w.WypozyczajacyId)
               .IsRequired()
               .OnDelete(DeleteBehavior.ClientCascade);

    }
}
