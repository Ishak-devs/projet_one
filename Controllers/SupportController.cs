using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Authorization;


namespace projet_one.Controllers;

public class SupportController : Controller
{

    public IActionResult Espace_perso()
    {
        var docsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "docs");

        if (!Directory.Exists(docsPath))
            Directory.CreateDirectory(docsPath);

        // Récupère tous les fichiers PDF
        var files = Directory.GetFiles(docsPath, "*.pdf")
                             .Select(f => new FileInfo(f))
                             .OrderByDescending(f => f.CreationTime)
                             .ToList();

        return View(files); 
    }
}
