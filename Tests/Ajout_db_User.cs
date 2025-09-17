using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using projet_one.Data;
using projet_one.Models;
using Xunit;

namespace projet_one.Tests
{
    public class UserDbTests
    {
        [Fact]
        public async Task Ajout_db_User()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                // Créer les tables Identity en mémoire
                await context.Database.EnsureCreatedAsync();

                var user = new User
                {
                    Id = "test-id-123", // Identity utilise string par défaut
                    UserName = "testuser", 
                    Nom_enseigne = "TestEnseigne",
                    Nom = "TestNom",
                    Email = "test@email.com",
                    NormalizedEmail = "TEST@EMAIL.COM", 
                    Telephone = "0123456789",
                    EmailConfirmed = true,
                    SecurityStamp = "security-stamp"
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await context.Users.CountAsync());
                var user = await context.Users.FirstAsync();
                Assert.Equal("TestEnseigne", user.Nom_enseigne);
                Assert.Equal("TestNom", user.Nom);
                Assert.Equal("test@email.com", user.Email);
                Assert.Equal("0123456789", user.Telephone);
                System.Diagnostics.Debug.WriteLine("Insertion utilisateur réussie !");
            }
        }
    }
}