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

            // Envoi de mail après enregistrement
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("SupportEngineering", "contact.kcc0@gmail.com")); // expéditeur
            message.To.Add(new MailboxAddress("Admin", "kouicicontact@yahoo.com"));    // destinataire
            message.Subject = "Nouvelle demande enregistrée";
            message.Body = new TextPart("plain")

            {
                Text = $"Nouvelle demande de l'enseigne : {user.Nom_enseigne}\nEmail : {user.Email}\nTéléphone : {user.Telephone}"
            };
            try
            {
                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync("contact.kcc0@gmail.com", "ldmljnlqdvxlxefy");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                TempData["succesmessage"] = "Votre demande a été envoyée avec succès !";
            }

            catch (Exception ex)
            {
                Console.WriteLine("SMTP ERROR: " + ex.Message);
                throw;
            }

        }
        
        catch (Exception ex)
        {
            TempData["wrong_message"] = "Erreur lors de l'enregistrement : " + ex.Message;
            return RedirectToAction("Index", "Home");
        }
        return RedirectToAction("Index", "Home");
    }
}