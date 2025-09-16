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
        public async Task CanInsertUserIntoDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                var user = new User
                {
                    Nom_enseigne = "TestEnseigne",
                    Nom = "TestNom",
                    Email = "test@email.com",
                    Telephone = "0123456789"
                };
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                Assert.Equal(1, await context.Users.CountAsync(), "L'insertion en base a échoué.");
                var user = await context.Users.FirstAsync();
                Assert.Equal("TestEnseigne", user.Nom_enseigne);
                Assert.Equal("TestNom", user.Nom);
                Assert.Equal("test@email.com", user.Email);
                Assert.Equal("0123456789", user.Telephone);
            
            }
        }
    }
}
