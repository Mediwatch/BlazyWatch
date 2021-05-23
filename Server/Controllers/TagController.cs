using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Mediwatch.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server;

namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController: ControllerBase
    {
        private readonly DbContextMediwatch _context;

        public TagController(DbContextMediwatch context)
        {
            _context = context;
        }

        //GET: Formation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tag>>> GetTag()
        {
            /// <summary>
            /// Get all tag
            /// </summary>
            /// <returns>Return the list of the available formation</returns>
            return await _context.tags.ToListAsync();
        }

        //GET: /tag/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<tag>> GetTag(int id)
        {
            /// <summary>
            /// Get a specific tag by is ID
            /// </summary>
            /// <returns>Return the formation information</returns>
            var tags = await _context.tags.FindAsync(id);
            if (tags == null)
            {
                return NotFound();
            }

            return tags;
        }

        // POST: /tag
        [HttpPost]
        public async Task<ActionResult<formation>> PostTag(tag tag)
        {
            /// <summary>
            /// Create a new Tag
            /// </summary>
            _context.tags.Add(tag);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTag), new { id = tag.id }, tag);
        }

                //Put: /Formation/5
        [HttpPut("{id}")]
        public async Task<ActionResult<formation>> PostTag(int id, tag tag)
        {
            /// <summary>
            /// Update the Tag specific by is ID
            /// </summary>
            tag.id = id;
            if (!TagExists(id))
            {
                return NotFound();
            }
            _context.Entry(tag).State = EntityState.Modified;
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return CreatedAtAction(nameof(GetTag), new { id = tag.id }, tag);
        }

        private bool TagExists(int id) {
            return _context.tags.Any(e => e.id == id);
        }

    }
}