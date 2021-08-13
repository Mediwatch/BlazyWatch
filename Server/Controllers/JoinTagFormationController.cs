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
    [Route("form_tag")]
    public class JoinTagFormationController : ControllerBase
    {
        private readonly DbContextMediwatch _context;

        public JoinTagFormationController(DbContextMediwatch context)
        {
            _context = context;
        }

        #region Opération CRUD basic

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JoinFormationTag>>> GetJoinTagForm()
        {
            /// <summary>
            /// Obtenez la liste de chaque Join
            /// </summary>
            return await _context.joinFormationTags.ToListAsync();
        }

        //POST: /form_tag
        [HttpPost]
        public async Task<ActionResult<JoinFormationTag>> PostJoinTagForm(JoinFormationTag join)
        {
            /// <summary>
            /// Créer une nouvelle balise
            /// </summary>
            _context.joinFormationTags.Add(join);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetJoinTagForm), new { id = join.id }, join);
        }

        //Put: /form_tag/5
        [HttpPut("{id}")]
        public async Task<ActionResult<JoinFormationTag>> PostJoinTagForm(Guid id, JoinFormationTag join)
        {
            /// <summary>
            /// Mettre à jour la jointure spécifique par son ID
            /// </summary>
            join.id = id;
            if (!JoinExists(id))
            {
                return NotFound();
            }
            _context.Entry(join).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return CreatedAtAction(nameof(GetJoinTagForm), new { id = join.id }, join);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<JoinFormationTag>> DeleteJoinTagForm(Guid id)
        {
            /// <summary>
            /// Supprimer la jointure entre la balise et le formulaire
            /// </summary>
            /// <returns></returns>
            var join = await _context.joinFormationTags.FindAsync(id);
            if (join == null)
            {
                return NotFound();
            }

            _context.joinFormationTags.Remove(join);
            await _context.SaveChangesAsync();

            return join;
        }
        private bool JoinExists(Guid id)
        {
            return _context.joinFormationTags.Any(e => e.id == id);
        }
        #endregion


        //Operation for Tag and Formation
        //GET: /form_tag/{?idjoin= or ?idform= or ?idtag=}
        public static async Task<ActionResult<IEnumerable<JoinFormationTag>>> GetJoinTagForm(
        DbContextMediwatch _context,
        String strIdJoin = "",
        String strIdFormation = "",
        String strIdTag = "")
        {
            /// <summary>
            /// Get a specific between Join between Tag and Formation
            /// </summary>
            /// <returns>Return the Join between Tag and Formation</returns>
 

            // String strIdJoin = HttpContext.Request.Query["idjoin"].ToString();
            // String strIdFormation = HttpContext.Request.Query["idform"].ToString();
            // String strIdTag = HttpContext.Request.Query["idtag"].ToString();

            if (strIdJoin != "")
            {
                Guid idJoin = Guid.Parse(strIdJoin);
                JoinFormationTag onlyJoin = await _context.joinFormationTags.FindAsync(idJoin);
                List<JoinFormationTag> joinResult = new List<JoinFormationTag>();
                joinResult.Add(onlyJoin);
                if (joinResult == null)
                {
                    return null;
                }
                return joinResult;
            }
            else if (strIdFormation != "")
            {
                Guid idForm = Guid.Parse(strIdFormation);
                List<JoinFormationTag> JoinFormation = await _context.joinFormationTags
                .Where(elem => elem.idFormation == idForm)
                .ToListAsync();

                return JoinFormation;
            }
            else if (strIdTag != "")
            {
                Guid idTag = Guid.Parse(strIdTag);
                List<JoinFormationTag> JoinTag = await _context.joinFormationTags
                .Where(elem => elem.idTag == idTag)
                .ToListAsync();

                return JoinTag;
            }
            
            return await _context.joinFormationTags.ToListAsync();
        }

    }
}