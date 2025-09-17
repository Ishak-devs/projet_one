using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;


namespace projet_one.Models
{
    public class User : IdentityUser
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de l'enseigne est obligatoire")]
        [MinLength(2, ErrorMessage = "Le nom de l'enseigne doit contenir au moins 2 caractères")]
        public string Nom_enseigne { get; set; }

        [Required(ErrorMessage = "Le prénom est obligatoire")]
        [MinLength(2, ErrorMessage = "Le prénom doit contenir au moins 2 caractères")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", 
            ErrorMessage = "Email invalide : doit contenir un @ et un point")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le téléphone est obligatoire")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Le téléphone doit contenir exactement 10 chiffres")]
        public string Telephone { get; set; }
    }
}
