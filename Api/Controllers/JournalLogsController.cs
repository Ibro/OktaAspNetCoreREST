using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalLogsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public JournalLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/JournalLogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JournalLog>>> GetJournalLogs()
        {
            return await _context.JournalLogs.ToListAsync();
        }

        // GET: api/JournalLogs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<JournalLog>> GetJournalLog(string id)
        {
            var journalLog = await _context.JournalLogs.FindAsync(id);

            if (journalLog == null)
            {
                return NotFound();
            }

            return journalLog;
        }

        // PUT: api/JournalLogs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutJournalLog(string id, JournalLog journalLog)
        {
            if (id != journalLog.Id)
            {
                return BadRequest();
            }

            _context.Entry(journalLog).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JournalLogExists(id))
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

        // POST: api/JournalLogs
        [HttpPost]
        public async Task<ActionResult<JournalLog>> PostJournalLog(JournalLog journalLog)
        {
            _context.JournalLogs.Add(journalLog);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetJournalLog", new { id = journalLog.Id }, journalLog);
        }

        // DELETE: api/JournalLogs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<JournalLog>> DeleteJournalLog(string id)
        {
            var journalLog = await _context.JournalLogs.FindAsync(id);
            if (journalLog == null)
            {
                return NotFound();
            }

            _context.JournalLogs.Remove(journalLog);
            await _context.SaveChangesAsync();

            return journalLog;
        }

        private bool JournalLogExists(string id)
        {
            return _context.JournalLogs.Any(e => e.Id == id);
        }
    }
}
