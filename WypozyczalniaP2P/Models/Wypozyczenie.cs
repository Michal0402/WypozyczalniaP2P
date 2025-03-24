using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaP2P.Models
{
    public class Wypozyczenie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Klient jest wymagany")]
        public string KlientId { get; set; }
        public virtual Klient Klient { get; set; }

        [Required(ErrorMessage = "Samochód jest wymagany")]
        public int SamochodId { get; set; }
        public virtual Samochod Samochod { get; set; }

        [Required(ErrorMessage = "Wypożyczający jest wymagany")]
        public string WypozyczajacyId { get; set; }
        public virtual Klient Wypozyczajacy { get; set; }

        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Wypozyczenie), nameof(ValidateDataRozpoczecia))]
        public DateTime DataRozpoczecia { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(Wypozyczenie), nameof(ValidateDataZakonczenia))]
        public DateTime? DataZakonczenia { get; set; }

        public static ValidationResult ValidateDataRozpoczecia(DateTime dataRozpoczecia, ValidationContext context)
        {
            if (dataRozpoczecia < DateTime.Today)
            {
                return new ValidationResult("Data rozpoczęcia nie może być wcześniejsza niż dzisiejsza data.");
            }
            return ValidationResult.Success;
        }

        public static ValidationResult ValidateDataZakonczenia(DateTime? dataZakonczenia, ValidationContext context)
        {
            var instance = (Wypozyczenie)context.ObjectInstance;
            if (dataZakonczenia.HasValue && dataZakonczenia <= instance.DataRozpoczecia)
            {
                return new ValidationResult("Data zakończenia musi być późniejsza niż data rozpoczęcia.");
            }
            return ValidationResult.Success;
        }
    }
}
