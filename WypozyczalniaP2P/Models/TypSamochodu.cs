using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaP2P.Models
{
    public class TypSamochodu
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Nazwa typu jest wymagana")]
        [StringLength(50, ErrorMessage = "Nazwa typu nie może przekraczać 50 znaków")]
        public string Nazwa { get; set; }

        [StringLength(200, ErrorMessage = "Opis nie może przekraczać 200 znaków")]
        public string Opis { get; set; }

        public virtual ICollection<Samochod> Samochody { get; set; } = new List<Samochod>();
    }
}
