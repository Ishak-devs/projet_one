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
            return RedirectToAction("Index", "Home");

        if (await _context.Users.AnyAsync(u => u.Nom_enseigne == user.Nom_enseigne))
        {
            TempData["wrong_message"] = "Cette enseigne a déja saisi une demande.";
            return RedirectToAction("Index", "Home");
        }

        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            TempData["email_used"] = "Une personne avec cet Email a déja envoyé une demande.";
            return RedirectToAction("Index", "Home");
        }

        try
        {
            // Enregistrement en DB
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            TempData["succesmessage"] = "Votre demande a bien été enregistrée.";
            return RedirectToAction("Index", "Home");
}
        catch (Exception ex)
        {
            TempData["wrong_message"] = "Erreur lors de l'enregistrement : " + ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }
}