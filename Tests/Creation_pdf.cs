using Xunit;
using System.IO;
using projet_one.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

public class PdfTests
{
    [Fact]
    public void Test_Generation_PDF()
    {
        
        var user = new User
        {
            Nom_enseigne = "TestEnseigne",
            Email = "ishak@test.com",
            Telephone = "0123456789"
        };

        QuestPDF.Settings.License = LicenseType.Community;

        var fileName = $"Test_{user.Nom_enseigne}.pdf";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs", fileName);
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        
        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.Content().Column(col =>
                {
                    col.Item().Text("Test PDF").FontSize(20).Bold();
                    col.Item().Text($"Nom de l'enseigne : {user.Nom_enseigne}").FontSize(14);
                });
            });
        }).GeneratePdf(filePath);

        // Assert
        Assert.True(File.Exists(filePath), "Le fichier PDF n'a pas été généré !");
        
        // Cleanup (optionnel)
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
