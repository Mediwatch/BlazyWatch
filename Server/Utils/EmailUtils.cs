using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Mediwatch.Shared;
using Microsoft.AspNetCore.Mvc;

using Microsoft.Extensions.Configuration;

namespace Server.Utils
{
    public class EmailUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="configuration"></param>
        /// <returns>Return 0 on success -1 on error</returns>
        public static int SendMail(EmailForm email, IConfiguration configuration)
        {
            string emailAddress = configuration["EmailServ:MailServer"];
            string password = configuration["EmailServ:Password"];
            
            Console.WriteLine(emailAddress);
            
            if (emailAddress == null || password == null)
                return -1;
            SmtpClient smtpClient = new SmtpClient("ssl0.ovh.net", 25);
            smtpClient.Credentials = new System.Net.NetworkCredential(emailAddress, password);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;

            MailMessage mailMessage = new MailMessage(emailAddress, email.EmailAddress);
            mailMessage.Subject = "Notification Médiwatch";
            
            FileStream fs = File.OpenRead("invoice.docx");
            if (fs != null){
                Attachment att = new Attachment( fs, name: "invoice.docx");
                mailMessage.Attachments.Add( att );
            }
            mailMessage.Body = email.Content;

            try
            {
                smtpClient.SendAsync(mailMessage,null);
                Console.WriteLine("mail sent");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
                return -1;
            }
        }
    }
}