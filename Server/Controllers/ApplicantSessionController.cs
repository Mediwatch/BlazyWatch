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
            /// Get the list of Applicant Session
            /// </summary>
            /// <returns></returns>
                return await _context.applicant_sessions.ToListAsync();
        }

        //GET: /ApplicantSession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<applicant_session>> GetApplicantSession(int id)
        {
            /// <summary>
            /// Get an applicant session
            /// </summary>
            /// <returns>Return the applicant session</returns>
            var applicantSessionResult = await _context.applicant_sessions.FindAsync(id);
            if (applicantSessionResult == null)
            {
                return NotFound();
            }

            return applicantSessionResult;
        }

        //Put: /ApplicantSession/5
        // WARNING YOU MUST TO SEND ALL DATA CHAMP WITH MODIFICATION 
        [HttpPut("{id}")]
        public async Task<ActionResult<applicant_session>> PutApplicantSession(int id, applicant_session applicantSessionInput)
        {
            /// <summary>
            /// Update an applicant session
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
            /// Create an applicant session
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
            /// Delete an applicant session
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