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

        // Relacja Klient (właściciel) -> Samochod
        builder.Entity<Samochod>()
            .HasOne(s => s.Wlasciciel)
            .WithMany(k => k.Flota)
            .HasForeignKey(s => s.WlascicielId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja TypAuta -> Samochod
        builder.Entity<Samochod>()
            .HasOne(s => s.TypSamochodu)
            .WithMany(t => t.Samochody)
            .HasForeignKey(s => s.TypSamochoduId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja Opinia -> Uzytkownik (Autor)
        builder.Entity<OpiniaKlienta>()
            .HasOne(o => o.Autor)
            .WithMany(a => a.MojeOpinieKlientow)
            .HasForeignKey(o => o.AutorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja Opinia -> Klient (opcjonalna)
        builder.Entity<OpiniaKlienta>()
            .HasOne(o => o.Klient)
            .WithMany(a => a.OpinieWypozyczen)
            .HasForeignKey(o => o.KlientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja Opinia -> Samochod (opcjonalna)
        builder.Entity<OpiniaSamochodu>()
            .HasOne(o => o.Samochod)
            .WithMany(a => a.Opinie)
            .HasForeignKey(o => o.SamochodId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja Ogloszenie -> Uzytkownik
        builder.Entity<Ogloszenie>()
            .HasOne(o => o.Klient)
            .WithMany(a => a.Ogloszenia)
            .HasForeignKey(o => o.KlientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja Ogloszenie -> Samochod
        builder.Entity<Ogloszenie>()
            .HasOne(o => o.Samochod)
            .WithMany(s => s.Ogloszenia)
            .HasForeignKey(o => o.SamochodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Wynajem>()
            .HasOne(o => o.Klient)
            .WithMany(a => a.Wynajmy)
            .HasForeignKey(o => o.KlientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacja Ogloszenie -> Samochod
        builder.Entity<Wynajem>()
            .HasOne(o => o.Samochod)
            .WithMany(s => s.Wynajmy)
            .HasForeignKey(o => o.SamochodId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Wypozyczenie>()
               .HasOne(w => w.Klient)
               .WithMany(k => k.Wypozyczenia)
               .HasForeignKey(w => w.KlientId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict); // Opcjonalnie: blokada kasowania

        // Konfiguracja relacji z Samochodem
        builder.Entity<Wypozyczenie>()
               .HasOne(w => w.Samochod)
               .WithMany(s => s.Wypozyczenia)
               .HasForeignKey(w => w.SamochodId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

        // Konfiguracja relacji z Wypożyczającym (WypozyczajacyId)
        builder.Entity<Wypozyczenie>()
               .HasOne(w => w.Wypozyczajacy)
               .WithMany(k => k.Wypozyczone)
               .HasForeignKey(w => w.WypozyczajacyId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);

    }
}
