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
                return RedirectToAction("Index");
            }
            catch
            {
                ModelState.AddModelError("", "Erreur lors de la sauvegarde");
            }
        }
        // Si erreur, on renvoie la vue Index avec le modèle pour afficher les erreurs
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
