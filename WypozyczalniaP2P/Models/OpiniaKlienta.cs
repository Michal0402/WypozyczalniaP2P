using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaP2P.Models
{
    public class OpiniaKlienta
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Autor opinii jest wymagany")]
        public string AutorId { get; set; } // Klucz obcy do Uzytkownik (autor opinii)
        public virtual Klient Autor { get; set; }

        // Opcjonalne powiązanie z Klientem
        public string? KlientId { get; set; } // Nullable, jeśli opinia dotyczy klienta
        public virtual Klient Klient { get; set; } // Nullable

        [Required(ErrorMessage = "Ocena jest wymagana")]
        [Range(1, 5, ErrorMessage = "Ocena musi być w przedziale od 1 do 5")]
        public int Ocena { get; set; }

        [Required(ErrorMessage = "Komentarz jest wymagany")]
        [Range(10, 500, ErrorMessage = "Komentarz nie może przekraczać 500 znaków")]
        public string Komentarz { get; set; }

        [Required(ErrorMessage = "Data dodania jest wymagana")]
        [DataType(DataType.DateTime)]
        public DateTime DataDodania { get; set; }
    }
}
