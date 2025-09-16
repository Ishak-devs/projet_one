using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using projet_one.Models;
using projet_one.Data;



namespace projet_one.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;


    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View(new User());
    }
    
    [HttpPost]
    public async Task<IActionResult> Enregistrer(User user)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                
                QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
                var fileName = $"Demande_{user.Nom_enseigne}_{user.Nom}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                QuestPDF.Fluent.Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Size(PageSizes.A4);
                        page.Content()
                            .Column(col =>
                            {
                                col.Item().Text("Demande d'aide vélo - Entreprise").FontSize(20).Bold().FontColor(Colors.Blue.Medium);
                                col.Item().Text($"Nom de l'enseigne : {user.Nom_enseigne}").FontSize(14);
                                col.Item().Text($"Nom du contact : {user.Nom}").FontSize(14);
                                col.Item().Text($"Email : {user.Email}").FontSize(14);
                                col.Item().Text($"Téléphone : {user.Telephone}").FontSize(14);
                                col.Item().Text($"Date de la demande : {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(12).Italic();
                            });
                    });
                })
                .GeneratePdf(filePath);


                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", $"Erreur lors de la sauvegarde ou de la génération du document : {ex.Message}");
            }
        }
        return View("Index", user);
    }



    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
