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
                return await _context.applicant_sessions.ToListAsync();
        }

        //GET: /ApplicantSession/5
        [HttpGet("{id}")]
        public async Task<ActionResult<applicant_session>> GetApplicantSession(int id)
        {
            var applicantSessionResult = await _context.applicant_sessions.FindAsync(id);
            if (applicantSessionResult == null)
            {
                return NotFound();
            }

            return applicantSessionResult;
        }

        //Put: /ApplicantSession/5
        [HttpPut("{id}")]
        public async Task<ActionResult<applicant_session>> PutApplicantSession(int id, applicant_session applicantSessionInput)
        {
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
            applicantSessionBody.createdAt = DateTime.Now;
            _context.applicant_sessions.Add(applicantSessionBody);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplicantSession), new { id = applicantSessionBody.id }, applicantSessionBody);
        }

        // DELETE: /ApplicantSession/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<applicant_session>> DeleteTodoItem(int id)
        {
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