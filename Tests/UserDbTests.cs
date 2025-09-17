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
                .UseMySql(
                    "Server=localhost;Database=projet_one_db;User=root;Password=;",
                    new MySqlServerVersion(new Version(8, 0, 43))
                )
                .Options;

            // Contexte pour l'insertion
            using (var context = new ApplicationDbContext(options))
            {
                await context.Database.EnsureCreatedAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == "test-id-123");
                if (existingUser != null)
                {
                    context.Users.Remove(existingUser);
                    await context.SaveChangesAsync();
                }

                var user = new User
                {
                    Id = "test-id-123",
                    UserName = "testuser",
                    Nom_enseigne = "TestEnseigne",
                    Email = "test@email.com",
                    NormalizedEmail = "TEST@EMAIL.COM",
                    Telephone = "0123456789",
                    EmailConfirmed = true,
                    SecurityStamp = "security-stamp"
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            // Contexte pour la vérification
            using (var context = new ApplicationDbContext(options))
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == "test-id-123");
                Assert.NotNull(user);
                Assert.Equal("TestEnseigne", user.Nom_enseigne);
                Assert.Equal("test@email.com", user.Email);
                Assert.Equal("0123456789", user.Telephone);
            }
        }
    }
}
