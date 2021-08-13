using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Mediwatch.Shared.Models;
using Server;

namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompagnyController : ControllerBase
    {
        private readonly DbContextMediwatch _context;

        public CompagnyController(DbContextMediwatch context)
        {
            _context = context;
        }

        //GET: /Compagny
        [HttpGet]
        public async Task<ActionResult<IEnumerable<compagny>>> GetCompagny(){
            /// <summary>
            /// Obtenez toute les compagnies dans la base de donnée
            /// </summary>
            /// <returns>une liste Json brute de l'entreprise</returns>
            return await _context.compagnies.ToListAsync();
        }

        //GET: /Compagny/5
        [HttpGet("{id}")]
        public async Task<ActionResult<compagny>> GetCompagny(int id)
        {
            /// <summary>
            /// Obtenir une entreprise par son id
            /// </summary>
            /// <returns>Renvoyer les données de l'entreprise</returns>
            var compagnyResult = await _context.compagnies.FindAsync(id);
            if (compagnyResult == null)
            {
                return NotFound();
            }

            return compagnyResult;
        }

        //Put: /Compagny/5
        [HttpPut("{id}")]
        public async Task<ActionResult<compagny>> PutCompagny(int id, compagny compagnyInput)
        {
            /// <summary>
            /// Modifier les informations de l'entreprise spécifiées par son identifiant
            /// </summary> 
            if (id != compagnyInput.id) 
            {
                return BadRequest();
            }
            _context.Entry(compagnyInput).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompagnyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // POST: /Compagny
        [HttpPost]
        public async Task<ActionResult<compagny>> PostCompagny(compagny compagnyBody)
        {
            /// <summary>
            /// Créer une nouvelle entreprise
            /// </summary>
            compagnyBody.createdAt = DateTime.Now;
            _context.compagnies.Add(compagnyBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompagny), new { id = compagnyBody.id }, compagnyBody);
        }

        // DELETE: /Compagny/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<compagny>> DeleteTodoItem(int id)
        {
            /// <summary>
            /// Supprimer l'entreprise avec l'ID
            /// </summary>
            /// <returns></returns>
            var compagnyResult = await _context.compagnies.FindAsync(id);
            if (compagnyResult == null)
            {
                return NotFound();
            }

            _context.compagnies.Remove(compagnyResult);
            await _context.SaveChangesAsync();

            return compagnyResult;
        }

        private bool CompagnyExists(long id) {
            return _context.compagnies.Any(e => e.id == id);
        }
    }
}