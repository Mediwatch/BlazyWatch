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
                return await _context.compagnies.ToListAsync();
        }

        //GET: /Compagny/5
        [HttpGet("{id}")]
        public async Task<ActionResult<compagny>> GetCompagny(int id)
        {
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
            compagnyBody.createdAt = DateTime.Now;
            _context.compagnies.Add(compagnyBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompagny), new { id = compagnyBody.id }, compagnyBody);
        }

        // DELETE: /Compagny/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<compagny>> DeleteTodoItem(int id)
        {
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