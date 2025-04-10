using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.Xml;

namespace WypozyczalniaP2P.Models
{
    public class Wypozyczenie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Klient jest wymagany")]
        public string KlientId { get; set; }
        public virtual Klient? Klient { get; set; }

        [Required(ErrorMessage = "Samochód jest wymagany")]
        public int SamochodId { get; set; }
        public virtual Samochod? Samochod { get; set; }

        [Required(ErrorMessage = "Wypożyczający jest wymagany")]
        public string WypozyczajacyId { get; set; }
        public virtual Klient? Wypozyczajacy { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana")]
        [DataType(DataType.Date)]
        public DateTime DataRozpoczecia { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataZakonczenia { get; set; }

       
    }
}
