using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Mediwatch.Shared.Models;
using Server;

/// <summary>
/// Fichier avec les fonctions relatif aux controleurs des ApplicantSession.
/// A chaque inscription à une formation l'utilisateur créer un ApplicantSession 
/// </summary>
namespace Mediwatch.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantSessionController : ControllerBase
    {
        private readonly DbContextMediwatch _context;

        public ApplicantSessionController(DbContextMediwatch context)
        {
            _context = context;
        }

        //GET: /ApplicantSession
        [HttpGet]
        public async Task<ActionResult<IEnumerable<applicant_session>>> GetApplicantSession(){
            /// <summary>
            /// Obtenir la liste des candidats Session
            /// </summary>
            /// <returns></returns>
                return await _context.applicant_sessions.ToListAsync();
        }

        //GET: /ApplicantSession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<applicant_session>> GetApplicantSession(int id)
        {
            /// <summary>
            /// Obtenez une session de candidature
            /// </summary>
            /// <returns>Retourner la session candidat</returns>
            var applicantSessionResult = await _context.applicant_sessions.FindAsync(id);
            if (applicantSessionResult == null)
            {
                return NotFound();
            }

            return applicantSessionResult;
        }

        //GET: /ApplicantSession/user/{Userid}
        [HttpGet("user/{id}")]
        public async Task<IEnumerable<applicant_session>> GetApplicantSession(string id)
        {
            /// <summary>
            /// Obtenir une session de candidature
            /// </summary>
            /// <returns>Retourner la session candidat</returns>
            Guid tmp = new Guid(id);
            IEnumerable<applicant_session> applicantSessionResult = await _context
            .applicant_sessions
            .Where(elem => elem.idUser == tmp)
            .ToListAsync();
            if (applicantSessionResult == null)
                return null;
            return applicantSessionResult;
        }        

        //Put: /ApplicantSession/{id}
        // AVERTISSEMENT VOUS DEVEZ ENVOYER TOUTES LES DONNÉES CHAMP AVEC MODIFICATION
        [HttpPut("{id}")]
        public async Task<ActionResult<applicant_session>> PutApplicantSession(int id, applicant_session applicantSessionInput)
        {
            /// <summary>
            /// Mettre à jour une session de candidature
            /// </summary>
            applicantSessionInput.id = id;
                if (!ApplicantSessionInputExists(id))
            {
                return NotFound();
            }
            _context.Entry(applicantSessionInput).State = EntityState.Modified;
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

        // POST: /ApplicantSession
        [HttpPost]
        public async Task<ActionResult<applicant_session>> PostApplicantSession(applicant_session applicantSessionBody)
        {
            /// <summary>
            /// Créer une session candidat
            /// </summary>
            applicantSessionBody.createdAt = DateTime.Now;
            _context.applicant_sessions.Add(applicantSessionBody);
            await _context.SaveChangesAsync();
            var result = await GetApplicantSession(applicantSessionBody.id);
            return result;
            // return CreatedAtAction(nameof(GetApplicantSession), new { id = applicantSessionBody.id }, applicantSessionBody);
        }

        // DELETE: /ApplicantSession/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<applicant_session>> DeleteApplicantSession(int id)
        {
            /// <summary>
            /// Supprimer une session de candidature
            /// </summary>
            /// <returns></returns>
            var applicantSessionResult = await _context.applicant_sessions.FindAsync(id);
            if (applicantSessionResult == null)
            {
                return NotFound();
            }

            _context.applicant_sessions.Remove(applicantSessionResult);
            await _context.SaveChangesAsync();

            return applicantSessionResult;
        }

        private bool ApplicantSessionInputExists(long id) {
            return _context.applicant_sessions.Any(e => e.id == id);
        }
    }
}