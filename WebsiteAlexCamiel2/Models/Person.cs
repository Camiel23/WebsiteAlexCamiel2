using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebsiteAlexCamiel2.Models
{
    public class Person
    {
        [Required(ErrorMessage = "Voornaam is een verplicht veld")]
        [Display(Name = "Voornaam")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Achternaam is een verplicht veld")]
        [Display(Name = "Achternaam")]
        public string Achternaam { get; set; }

        [Required(ErrorMessage = "Email is een verplicht veld")]
        [EmailAddress(ErrorMessage = "Geen geldig email adres")]
        public string Email { get; set; }

        public string Telefoon { get; set; }

        public string Adres { get; set; }

        [Required(ErrorMessage = "Omschrijving is een verlicht veld")]
        [Display(Name = "Omschrijving")]
        public string Omschrijving { get; set; }
    }
}
