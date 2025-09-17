using Microsoft.AspNetCore.Mvc;
using projet_one.Models;
using projet_one.Data;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;


namespace projet_one.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserController(ApplicationDbContext context)
    {
        _context = context;
    }



    [HttpPost]
    public async Task<IActionResult> Enregistrer_client(User user)
    {
         if (!ModelState.IsValid)
        return View("~/Views/Home/Index.cshtml", user);

        if (await _context.Users.AnyAsync(u => u.Nom_enseigne == user.Nom_enseigne))
        {
            TempData["wrong_message"] = "Cette enseigne a déja saisi une demande.";
            return View("~/Views/Home/Index.cshtml", user);
        }
            try
            {
                // Enregistrement en DB
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                TempData["succesmessage"] = "Votre demande a bien été enregistrée.";

                // Génération PDF
               QuestPDF.Settings.License = LicenseType.Community;
            var fileName = $"Demande_Aide_Velo_{user.Nom_enseigne}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs", fileName);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    page.Header().Element(ComposeHeader);
                    
                    // Contenu principal
                    page.Content().Element(c => ComposeContent(c, user));
                    
                    // Pied de page
                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.CurrentPageNumber().FontSize(10).FontColor(Colors.Grey.Medium);
                        t.Span(" / ");
                        t.TotalPages().FontSize(10).FontColor(Colors.Grey.Medium);
                        t.Span($" - Document généré le {DateTime.Now:dd/MM/yyyy à HH:mm}").FontSize(10).FontColor(Colors.Grey.Medium);
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

    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            row.RelativeItem().Column(column =>
            {
                column.Item().AlignLeft().Text("DEMANDE D'AIDE")
                    .FontSize(16)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);
                
                column.Item().AlignLeft().Text("Programme d'aide à la mobilité durable")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Medium);
            });

        });
    }

    private void ComposeContent(IContainer container, User user)
    {
        container.PaddingVertical(2, Unit.Centimetre).Column(column =>
        {
            // Section Informations Entreprise
            column.Item().Element(e => ComposeSection(e, "INFORMATIONS DE L'ENTREPRISE", Colors.Blue.Darken3));
            
            column.Item().PaddingTop(10).Grid(grid =>
            {
                grid.Columns(2);
                grid.Item().Component(new InfoField("Nom de l'enseigne", user.Nom_enseigne));
                grid.Item().Component(new InfoField("Email", user.Email));
                grid.Item().Component(new InfoField("Téléphone", user.Telephone));
            });

            
        });
    }

    private void ComposeSection(IContainer container, string title, string color)
    {
        container.Background(color).Padding(10).Column(column =>
        {
            column.Item().Text(title)
                .FontSize(12)
                .Bold()
                .FontColor(Colors.White);
        });
    }
}
public class InfoField : IComponent
{
    private readonly string _label;
    private readonly string _value;

    public InfoField(string label, string value)
    {
        _label = label;
        _value = value;
    }

    public void Compose(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text(_label)
                .FontSize(10)
                .Bold()
                .FontColor(Colors.Grey.Darken2);
            
            column.Item().PaddingTop(2).Text(_value)
                .FontSize(11)
                .FontColor(Colors.Grey.Darken3);
            
            column.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
        });
    }
}