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
                return await _context.formations.ToListAsync();
        }

        //GET: /Formation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<formation>> GetFormation(int id)
        {
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
            if (id != formationPut.id) 
            {
                return BadRequest();
            }
            _context.Entry(formationPut).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FormationExists(id))
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

        // POST: /Formation
        [HttpPost]
        public async Task<ActionResult<formation>> PostFormation(formation formationBody)
        {
            // formationBody.createdAt = DateTime.Now;
            _context.formations.Add(formationBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFormation), new { id = formationBody.id }, formationBody);
        }

        // DELETE: api/Formation/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<formation>> DeleteFormation(int id)
        {
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
