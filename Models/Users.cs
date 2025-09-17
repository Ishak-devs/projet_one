using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace projet_one.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Le nom de l'enseigne est obligatoire")]
        [MinLength(2, ErrorMessage = "Le nom de l'enseigne doit contenir au moins 2 caractères")]
        public string Nom_enseigne { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            ErrorMessage = "Email invalide : doit contenir un @ et un point")]
        public override string Email { get; set; }

        [Required(ErrorMessage = "Le téléphone est obligatoire")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Le téléphone doit commencer par 0 et contenir exactement 10 chiffres")]
        public string Telephone { get; set; }
        public string? Statut { get; set; } = "En attente de traitement";
    }
}