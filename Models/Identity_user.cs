using Microsoft.AspNetCore.Identity;

public class UserIdentity : IdentityUser
{
    public string Nom { get; set; }
    public string Prenom { get; set; }
    // ajoute d'autres champs si nécessaire
}
