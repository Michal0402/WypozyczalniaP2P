using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaP2P.Models
{
    public class Klient : IdentityUser 
    {
        [Required]
        [StringLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków")]
        public string Imie { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Numer prawa jazdy jest wymagany")]
        [StringLength(20, ErrorMessage = "Numer prawa jazdy nie może przekraczać 20 znaków")]
        public string NumerPrawaJazdy { get; set; }

        [StringLength(100, ErrorMessage = "Adres nie może przekraczać 100 znaków")]
        public string Adres { get; set; }

        [StringLength(100, ErrorMessage = "Miejscowosc nie może przekraczać 100 znaków")]
        public string Miejscowosc { get; set; }

        [StringLength(10, ErrorMessage = "Kod pocztowy nie może przekraczać 10 znaków")]
        public string KodPocztowy { get; set; }

        [Required(ErrorMessage = "Data urodzenia jest wymagana")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Klient), nameof(ValidateAge))]
        public DateTime DataUrodzenia { get; set; }

        // Relacja z wypożyczeniami (wypożyczenia, które klient zrobił)
        public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();

        public virtual ICollection<Wypozyczenie> Wypozyczone { get; set; } = new List<Wypozyczenie>();

        // Relacja z flotą (samochody, które klient oferuje do wypożyczenia)
        public virtual ICollection<Samochod> Flota { get; set; } = new List<Samochod>();

        public virtual ICollection<OpiniaKlienta> MojeOpinieKlientow { get; set; } = new List<OpiniaKlienta>();
        public virtual ICollection<OpiniaSamochodu> MojeOpinieSamochodow { get; set; } = new List<OpiniaSamochodu>();

        // Opinie o kliencie jako tym ktory od kogos wypozyczl auto
        public virtual ICollection<OpiniaKlienta> OpinieWypozyczen { get; set; } = new List<OpiniaKlienta>();

        public virtual ICollection<Ogloszenie> Ogloszenia { get; set; } = new List<Ogloszenie>();

        public virtual ICollection<Wynajem> Wynajmy { get; set; } = new List<Wynajem>();

        public static ValidationResult ValidateAge(DateTime dataUrodzenia, ValidationContext context)
        {
            var today = DateTime.Today;
            var age = today.Year - dataUrodzenia.Year;
            if (dataUrodzenia.Date > today.AddYears(-age)) age--;
            if (age < 18)
            {
                return new ValidationResult("Klient musi mieć ukończone co najmniej 18 lat.");
            }
            return ValidationResult.Success;
        }

    }
}
