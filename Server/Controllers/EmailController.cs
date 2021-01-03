using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Mediwatch.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmailController : Controller
    {
        [HttpPost]
        public IActionResult SendMail(EmailForm email)
        {
            /// <summary>
            /// Permet d'envoyer un mail
            /// </summary>
            /// <returns></returns>
            string emailAddress = Environment.GetEnvironmentVariable("CONTACT_EMAIL");
            string password = Environment.GetEnvironmentVariable("PASSWORD_EMAIL");
            
            if (emailAddress == null || password == null)
                return Ok("Environment incomplet");
            SmtpClient smtpClient = new SmtpClient("ssl0.ovh.net", 25);
            smtpClient.Credentials = new System.Net.NetworkCredential(emailAddress, password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage("contact@mediwatch.fr", email.EmailAddress);
            mailMessage.Subject = "Notification Médiwatch";
            mailMessage.Body = email.Content;

            try
            {
                smtpClient.Send(mailMessage);
                Console.WriteLine("Mail Envoyé");
                return Ok("Message éléctronique envoyé avec succès");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur: " + ex.ToString());
                return Ok(ex.ToString());
            }
        }
    }
}