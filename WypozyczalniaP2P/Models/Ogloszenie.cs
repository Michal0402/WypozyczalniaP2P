using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaP2P.Models
{
    public class Ogloszenie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Tytuł nie może przekraczać 50 znaków")]
        public string Tytul { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Opis nie może przekraczać 200 znaków")]
        public string Opis { get; set; }
        
        [Required]
        public string KlientId { get; set; }
        public virtual Klient? Klient { get; set; }

        [Required(ErrorMessage = "Cena za dzień jest wymagana")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cena za dzień musi być większa niż 0")]
        [Display(Name = "Cena za dobę")]
        public decimal CenaZaDzien { get; set; }

        [Required]
        public int SamochodId { get; set; }
        public virtual Samochod? Samochod { get; set; }

        public string? Zdjecie { get; set; } = "default.jpg"; //defaultowe zdjecie samochodu

        public DateTime DataUtworzenia { get; set; }
    }
}
