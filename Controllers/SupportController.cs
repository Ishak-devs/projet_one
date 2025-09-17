using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projet_one.Data; // pour ApplicationDbContext
using projet_one.Models; // pour User
using System.Threading.Tasks;

namespace projet_one.Controllers
{
    public class SupportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var demandes = await _context.Users.ToListAsync();
            return View("Index", demandes);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatut(string id, [FromBody] string statut)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.Statut = statut;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
    }

    public class filtres()
    {
        public string? nom_enseigne { get; set; }
        public string? email { get; set; }
        public string? telephone { get; set; }      
    }
