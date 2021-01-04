using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Mediwatch.Shared.Models;
using Server;
using Newtonsoft.Json;
using Server.Utils;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mediwatch.Shared;

public class InvoiceGenerator {
    private string Template;
    private string Data;
    private string Output = "output";
    private string Format = "pdf";
    private Boolean Overwrite = false;

    public InvoiceGenerator(string template) {
        this.Template = template;
    }

    public InvoiceGenerator SetData(object data) {
        string json = JsonConvert.SerializeObject(data);
        this.Data = "\"" + json.Replace("\"", "\\\"") + "\"";
        // Whithout this Replace the shell will interpret the '"' in the JSON.
        // The '"' added so the JSON doesn't get split by internal spaces.
        // Warning: '"' in keys or values do not work.
        return this;
    }

    public InvoiceGenerator SetOuput(string output) {
        this.Output = output;
        return this;
    }

    public InvoiceGenerator SetFormat(string output) {
        this.Output = output;
        return this;
    }

    public InvoiceGenerator SetOverwrite(Boolean overwrite) {
        this.Overwrite = overwrite;
        return this;
    }

    public string GetArgsString() {
        return this.Template
            + " --data " + this.Data
            + " --output " + this.Output
            + " --format " + this.Format
            + (this.Overwrite ? " --overwrite" : "");
    }

    public void Run() {
        Process.Start(new ProcessStartInfo(
            "./invoice_generator",
            this.GetArgsString()
        ));
    }
}

namespace  Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<IdentityUser<Guid>> _userManager;

        private readonly IConfiguration _configuration;

        private readonly DbContextMediwatch _context;

        public OrderController(DbContextMediwatch context, UserManager<IdentityUser<Guid>> userManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _configuration = configuration;
        }

        //GET: /Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<orderInfo>>> GetOrder()
        {
            return await _context.orderInfos.ToListAsync();
        }

        //GET /Order/{id}
        [HttpGet ("{id}")]
        public async Task<ActionResult<orderInfo>> GetOrder(int id)
        {
            var orderResult = await _context.orderInfos.FindAsync(id);
            if (orderResult == null)
            {
                return NotFound();
            }

            return orderResult;
        }
    
        //POST /Order/
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<orderInfo>> PostOrder(orderInfo orderBody){
            new InvoiceGenerator("invoice_template.docx")
                .SetData(new {
                    envoyeur = "Mediwatch",
                    envoyeur_addresse_1 = "24 rue Pasteur",
                    envoyeur_addresse_2 = "94270 Le Kremlin-Bicêtre",
                    envoyeur_addresse_3 = "France",
                    destinataire = "Zaman Kebab",
                    destinataire_addresse_1 = "3 rue des pigeons",
                    destinataire_addresse_2 = "94132 Le Kremlin-Bicêtre",
                    numéro_facture = "FA-2",
                    date_facture = "24/01/2020",
                    description = "Abonnement 1 mois",
                    quantité = "1",
                    unité = "pce.",
                    prix_unitaire_HT = "50000.00",
                    total_TTC = "50000.00",
                    téléphone = "+33 X XX XX XX XX",
                    email = "contact@mediwatch.com",
                    IBAN = "XXXXXXXXXX"
                })
                .SetOuput("invoice.pdf")
                .SetFormat("docx")
                .SetOverwrite(true)
                .Run();
                var info = await _userManager.FindByIdAsync (User.FindFirstValue (ClaimTypes.NameIdentifier));

                EmailForm email = new EmailForm{
                    EmailAddress = info.Email,
                    Content = "Vous avez payé"
                };

                EmailUtils.SendMail(email, _configuration);
                

            // orderInfo info = new orderInfo();
            orderBody.createAt = DateTime.Now;
            _context.orderInfos.Add(orderBody);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = orderBody.id }, orderBody);
        }
    }
}