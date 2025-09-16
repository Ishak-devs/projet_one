using Microsoft.AspNetCore.Mvc;
using projet_one.Models;
using projet_one.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;

namespace projet_one.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Enregistrer(User user)
    {
        if (!ModelState.IsValid)
            return View("~/Views/Home/Index.cshtml", user);

        try
        {
            
            // Enregistrement en DB
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Génération PDF
            QuestPDF.Settings.License = LicenseType.Community;
            var fileName = $"Demande_{user.Nom_enseigne}_{user.Nom}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
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
                        col.Item().Text("Demande d'aide vélo - Entreprise").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                        col.Item().Text($"Nom de l'enseigne : {user.Nom_enseigne}").FontSize(14);
                        col.Item().Text($"Nom du contact : {user.Nom}").FontSize(14);
                        col.Item().Text($"Email : {user.Email}").FontSize(14);
                        col.Item().Text($"Téléphone : {user.Telephone}").FontSize(14);
                        col.Item().Text($"Date de la demande : {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(12).Italic();
                        .Fontfamily("Arial");
                    });
                });
            }).GeneratePdf(filePath);

            return View("~/Views/Home/Index.cshtml", user);
        }
      catch (Exception ex)
{
    ModelState.AddModelError("", $"Erreur : {ex.Message}");
    return View("~/Views/Home/Index.cshtml", user);
}

    }
}
