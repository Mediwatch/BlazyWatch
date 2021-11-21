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
using Mediwatch.Server.Areas.Identity.Data;
using System.IO;

using System.Linq;
using Microsoft.Extensions.Logging;

public class InvoiceGenerator
{
    private string Template;
    private string Data;
    private string Output = "output";
    private string Format = "pdf";
    private Boolean Overwrite = false;

    public InvoiceGenerator(string template)
    {
        this.Template = template;
    }

    public InvoiceGenerator SetData(object data)
    {
        string json = JsonConvert.SerializeObject(data);
        this.Data = "\"" + json.Replace("\"", "\\\"") + "\"";
        // Sans ce remplacement, le shell interprétera le '"' dans le JSON.
        // Le '"' ajouté pour que le JSON ne soit pas divisé par des espaces internes.
        // Attention : '"' dans les clés ou les valeurs ne fonctionne pas.
        return this;
    }

    public InvoiceGenerator SetOuput(string output)
    {
        this.Output = output;
        return this;
    }

    public InvoiceGenerator SetFormat(string output)
    {
        this.Output = output;
        return this;
    }

    public InvoiceGenerator SetOverwrite(Boolean overwrite)
    {
        this.Overwrite = overwrite;
        return this;
    }

    public string GetArgsString()
    {
        return this.Template
            + " --data " + this.Data
            + " --output " + this.Output
            + " --format " + this.Format
            + (this.Overwrite ? " --overwrite" : "");
    }

    public void Run()
    {
        var p = Process.Start(new ProcessStartInfo(
            "./InvoiceGenerator/invoice_generator",
            this.GetArgsString()
        ));
        p.WaitForExit();
    }
}

public class InvoiceArchiver
{
    private string Root;
    private string TimeFormat;

    public InvoiceArchiver(IConfiguration conf)
    {
        Root = conf["InvoiceArchiver:Root"];
        TimeFormat = conf["InvoiceArchiver:TimeFormat"];

        if (!Directory.Exists(Root))
        {
            Directory.CreateDirectory(Root);
        }
    }

    public void ArchiveInvoice(string filePath, string filename, string client)
    {
        if (!Directory.Exists(Root + "/" + client))
        {
            Directory.CreateDirectory(Root + "/" + client);
        }

        // facture_NOM_DATE.pdf
        File.Move(filePath,
            Root + "/" + client + "/"
                + "facture_" + client + "_" + DateTime.Now.ToString(TimeFormat) + ".pdf");
    }

    public string[] Search(string client)
    {
        if (!Directory.Exists(Root + "/" + client))
        {
            return new string[] { };
        }

        return Directory.GetFiles(Root + "/" + client);
    }
}

namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly UserManager<UserCustom> _userManager;

        private readonly IConfiguration _configuration;

        private readonly DbContextMediwatch _context;

        public OrderController(DbContextMediwatch context, UserManager<UserCustom> userManager, IConfiguration configuration)
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
        [HttpGet("{id}")]
        public async Task<ActionResult<orderInfo>> GetOrder(long id)
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
        public async Task<ActionResult<orderInfo>> PostOrder(orderInfo orderBody)
        {
            var userInfo = await _userManager.FindByIdAsync(orderBody.userId);

            var price = orderBody
                .formationId
                .Split(";")
                .ToList()
                .Select(id => _context.formations.FindAsync(Guid.Parse(id)))
                .Select(res => res.Result.Price)
                .Sum();

            orderBody.invoiceId = Guid.NewGuid().ToString();
            orderBody.createAt = DateTime.Now;

            new InvoiceGenerator("./InvoiceGenerator/invoice_template.docx")
                .SetData(new
                {
                    envoyeur = "Mediwatch",
                    envoyeur_addresse_1 = "24 rue Pasteur",
                    envoyeur_addresse_2 = "94270 Le Kremlin-Bicêtre",
                    envoyeur_addresse_3 = "France",
                    destinataire = userInfo.UserName,
                    destinataire_addresse = orderBody.billingAdress,
                    numéro_facture = orderBody.invoiceId,
                    date_facture = orderBody.createAt.ToString("dd/MM/yyyy"),
                    description = "Formation",
                    // description = formationInfo.Name,
                    quantité = "1",
                    unité = "pce.",
                    // prix_unitaire_HT = "100",
                    prix_unitaire_HT = price.ToString(),
                    // prix_unitaire_HT = formationInfo.Price,
                    total_TTC = "-1.00",
                    // total_TTC = formationInfo.Price,
                    téléphone = "+33 X XX XX XX XX",
                    email = "contact@mediwatch.com",
                    IBAN = "XXXXXXXXXX"
                })
                .SetOuput("./InvoiceGenerator/" + orderBody.invoiceId)
                .SetOverwrite(true)
                .Run();

            // EmailForm email = new EmailForm
            // {
            //     EmailAddress = userInfo.Email,
            //     Content = "Vous avez payé"
            // };

            // EmailUtils.SendMail(email, _configuration);

            new InvoiceArchiver(_configuration)
                .ArchiveInvoice("./InvoiceGenerator/" + orderBody.invoiceId, orderBody.invoiceId, userInfo.UserName);

            orderBody.createAt = DateTime.Now;
            _context.orderInfos.Add(orderBody);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = orderBody.id }, orderBody);
        }

        //GET /Order/Archived
        [HttpGet]
        [Route("Archived")]
        [Authorize]
        public async Task<IEnumerable<string>> GetOrderArchived()
        {
            var userInfo = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            return new InvoiceArchiver(_configuration)
                .Search(userInfo.UserName)
                .Select(path => Path.GetFileName(path));
        }

        //Get /Order/Archived/<fileName>
        [HttpGet]
        [Route("Archived/{fileName}")]
        [Authorize]
        public async Task<ActionResult> GetArchived(string fileName)
        {
            var userInfo = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var orders = new InvoiceArchiver(_configuration).Search(userInfo.UserName);

            string filePath;

            try
            {
                filePath = orders.Single(path => Path.GetFileName(path) == fileName);
            }
            catch (System.InvalidOperationException)
            {
                return NotFound();
            }

            Stream sr;

            try
            {
                sr = System.IO.File.OpenRead(filePath);
            }
            catch (System.Exception)
            {
                return StatusCode(500);
            }

            return new FileStreamResult(sr, "application/octet-stream");
        }
    }
}
