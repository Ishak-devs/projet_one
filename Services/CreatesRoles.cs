using Microsoft.AspNetCore.Identity;
using projet_one.Models;
using System;
using System.Threading.Tasks;

namespace projet_one.Services
{
    public class GestionRole
    {
        public static async Task CreateRoles_and_user(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            string[] roles = { "Admin" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            string adminEmail = "admin@co.com";
            string adminPassword = "Admin123!";
            string adminEnseigne = "Non renseigné";
            string Adminphone = "Non renseigné";


            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new User { UserName = adminEmail, Email = adminEmail, Nom_enseigne = adminEnseigne, Telephone = Adminphone  };

                await userManager.CreateAsync(adminUser, adminPassword); // le mdp est hashé automatiquement
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

        }
    }
}
