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
    public class FormationController : ControllerBase
    {
        private readonly DbContextMediwatch _context;

        public FormationController(DbContextMediwatch context)
        {
            _context = context;
        }

        //GET: /Formation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<formation>>> GetFormation(){
            /// <summary>
            /// Get all formation
            /// </summary>
            /// <returns>Return the list of the available formation</returns>
                return await _context.formations.ToListAsync();
        }

        //GET: /Formation/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<formation>> GetFormation(int id)
        {
            /// <summary>
            /// Get a specific formation by is ID
            /// </summary>
            /// <returns>Return the formation information</returns>
            var formationResult = await _context.formations.FindAsync(id);
            if (formationResult == null)
            {
                return NotFound();
            }

            return formationResult;
        }

        //Put: /Formation/5
        [HttpPut("{id}")]
        public async Task<ActionResult<formation>> PutFormation(int id, formation formationPut)
        {
            /// <summary>
            /// Update the formation specific by is ID
            /// </summary>
            formationPut.id = id;
            if (!FormationExists(id))
            {
                return NotFound();
            }
            _context.Entry(formationPut).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return StatusCode(200);
        }

        // POST: /Formation
        [HttpPost]
        public async Task<ActionResult<formation>> PostFormation(formation formationBody)
        {
            /// <summary>
            /// Create a new formation
            /// </summary>
            _context.formations.Add(formationBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFormation), new { id = formationBody.id }, formationBody);
        }

        // DELETE: api/Formation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<formation>> DeleteFormation(int id)
        {
            /// <summary>
            /// Delete the formation by ID
            /// </summary>
            /// <returns></returns>
            var formationResult = await _context.formations.FindAsync(id);
            if (formationResult == null)
            {
                return NotFound();
            }

            _context.formations.Remove(formationResult);
            await _context.SaveChangesAsync();

            return formationResult;
        }
        
        private bool FormationExists(long id) {
            return _context.formations.Any(e => e.id == id);
        }


    }
}
