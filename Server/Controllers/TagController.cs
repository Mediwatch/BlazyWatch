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

        #region CRUD OP
        // //GET: Tag
        // [HttpGet]
        // public async Task<ActionResult<IEnumerable<tag>>> GetTag()
        // {
        //     /// <summary>
        //     /// Get all tag
        //     /// </summary>
        //     /// <returns>Return the list of the available formation</returns>
        //     return await _context.tags.ToListAsync();
        // }

        //GET: /tag/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<tag>> GetTag(Guid id)
        {
            /// <summary>
            /// Obtenir une balise spécifique par son ID
            /// </summary>
            /// <returns>Retourne les informations d'une formation</returns>

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
            /// Créer un nouveau Tag
            /// </summary>
            _context.tags.Add(tag);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTag), new { id = tag.id }, tag);
        }

        //Put: /tag/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<formation>> PostTag(Guid id, tag tag)
        {
            /// <summary>
            /// Mettre à jour un Tag spécifique par son ID
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


        #endregion
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<tag>>> GetTag(
        [FromQuery(Name = "idform")] String strIdform = "")
        {
            //Part get Tag from Formation
            if (strIdform != "")
            {
                //Factorisation INC
                FormationController _formationController = new FormationController(_context);
                List<JoinFormationTag> Join = JoinTagFormationController.GetJoinTagForm(_context, "", strIdform, "")
                .Result
                .Value
                .ToList();
                List<tag> result = new List<tag>();

                foreach (var item in Join)
                {
                    result.Add(GetTag(item.idTag)
                    .Result
                    .Value);
                }
                return result;
            }
            //Part get List
            return await _context.tags.ToListAsync();
        }
        
        private bool TagExists(Guid id) {
            return _context.tags.Any(e => e.id == id);
        }

    }
}