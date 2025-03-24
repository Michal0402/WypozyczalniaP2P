using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaP2P.Models
{
    public class Samochod
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Marka jest wymagana")]
        [StringLength(50, ErrorMessage = "Marka nie może przekraczać 50 znaków")]
        public string Marka { get; set; }

        [Required(ErrorMessage = "Model jest wymagany")]
        [StringLength(50, ErrorMessage = "Model nie może przekraczać 50 znaków")]
        public string Model { get; set; }

        [Required(ErrorMessage = "Typ auta jest wymagany")]
        public int TypSamochoduId { get; set; }

        public virtual TypSamochodu? TypSamochodu { get; set; }

        [Required(ErrorMessage = "Numer rejestracyjny jest wymagany")]
        [StringLength(10, ErrorMessage = "Numer rejestracyjny nie może przekraczać 10 znaków")]
        public string NumerRejestracyjny { get; set; }

        [Required(ErrorMessage = "Rok produkcji jest wymagany")]
        [Range(1900, 2030, ErrorMessage = "Rok produkcji musi być między 1900 a 2025")]
        public int RokProdukcji { get; set; }

        [Required(ErrorMessage = "Przebieg jest wymagany")]
        [Range(0, int.MaxValue, ErrorMessage = "Przebieg nie może być ujemny")]
        public int Przebieg { get; set; }

        [Required(ErrorMessage = "Moc silnika jest wymagana")]
        [Range(0, int.MaxValue, ErrorMessage = "Moc silnika musi być większa niż 0")]
        public int MocSilnika { get; set; }

        [Required(ErrorMessage = "Pojemność silnika jest wymagana")]
        [Range(0.01, float.MaxValue, ErrorMessage = "Pojemność silnika musi być większa niż 0")]
        public float PojemnoscSilnika { get; set; }

        [Required(ErrorMessage = "Paliwo jest wymagane")]
        public Paliwo RodzajPaliwa { get; set; }

        [Required(ErrorMessage = "Skrzynia biegów jest wymagana")]
        public SkrzyniaBiegow Skrzynia { get; set; }

        [Required(ErrorMessage = "Ilość miejsc jest wymagana")]
        public IloscMiejsc LiczbaMiejsc { get; set; }

        [Required(ErrorMessage = "Ilość drzwi jest wymagana")]
        public Drzwi IloscDrzwi { get; set; }

        [Required(ErrorMessage = "Napęd jest wymagany")]
        public Naped RodzajNapedu { get; set; }

        [Required(ErrorMessage = "VIN jest wymagany")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "VIN musi mieć 17 znaków")]
        public string Vin { get; set; }

        public Kolory Kolor { get; set; }

        public string? Zdjecie { get; set; } = "default.jpg"; //defaultowe zdjecie samochodu

        public bool CzyDostepny { get; set; } = true;

        public virtual ICollection<OpiniaSamochodu> Opinie { get; set; } = new List<OpiniaSamochodu>();
        public virtual ICollection<Ogloszenie> Ogloszenia { get; set; } = new List<Ogloszenie>();

        // Relacja z właścicielem (Klientem)
        [Required(ErrorMessage = "Właściciel jest wymagany")]
        public string WlascicielId { get; set; } // Klucz obcy do Klienta (string, bo IdentityUser używa string)
        public virtual Klient? Wlasciciel { get; set; }   // Nawigacja do właściciela

        public virtual ICollection<Wynajem> Wynajmy { get; set; } = new List<Wynajem>();

        // Relacja z Wypozyczeniami
        public virtual ICollection<Wypozyczenie> Wypozyczenia { get; set; } = new List<Wypozyczenie>();


        public enum Paliwo
        {
            Benzyna,
            Diesel,
            Elektryczny,
            Hybryda
        }

        public enum SkrzyniaBiegow
        {
            Manualna,
            Automatyczna
        }

        public enum IloscMiejsc
        {
            Dwa,
            Cztery,
            Pięć,
            Siedem,
            Dziewięć
        }

        public enum Drzwi
        {
            Dwa,
            Cztery,
            Pięć
        }

        public enum Naped
        {
            FWD,
            RWD,
            AWD
        }

        public enum Kolory
        {
            Biały,
            Czarny,
            Czerwony,
            Zielony,
            Niebieski,
            Żólty,
            Szary,
            Srebrny,
            Brązowy,
            Pomaranczowy,
            Fioletowy,
            Różowy,
            Inny
        }
    }
}
