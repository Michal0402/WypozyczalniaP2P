using System.ComponentModel.DataAnnotations;
namespace WypozyczalniaP2P.Models
{
    public class Wynajem
    {
        [Key]
        public int Id { get; set; }

        public string KlientId { get; set; }
        public virtual Klient Klient { get; set; }
        public int SamochodId { get; set; }
        public virtual Samochod Samochod { get; set; }
        
        public DateTime DataRozpoczecia { get; set; }
        public DateTime DataZakonczenia { get; set; }

        public int IloscDni { get; set; }

        public decimal CalkowityKosztWynajmu { get; set; }
    }
}
