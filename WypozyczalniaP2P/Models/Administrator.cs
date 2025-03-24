using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace WypozyczalniaP2P.Models
{
    public class Administrator : IdentityUser
    {
        [Required]
        [StringLength(50, ErrorMessage = "Imię nie może przekraczać 50 znaków")]
        public string Imie { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Nazwisko nie może przekraczać 50 znaków")]
        public string Nazwisko { get; set; }
    }
}
