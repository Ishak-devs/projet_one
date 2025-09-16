using Microsoft.AspNetCore.Mvc;
using projet_one.Models;
using System.Diagnostics;

namespace projet_one.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(new User());
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }); //Id pour chaque view en cas de bug
    }
}