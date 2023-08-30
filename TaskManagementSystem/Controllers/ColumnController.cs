using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Model;

namespace TaskManagementSystem.Controllers
{

    [ApiController]
    [Route("api/columns")]
    public class ColumnController : Controller
    {
        private readonly AppDbContext _context;

        public ColumnController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/columns
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ColumnModel>>> GetColumns()
        {
            return await _context.Columns.ToListAsync();
        }

        // GET: api/columns/1
        [HttpGet("{id}")]
        public async Task<ActionResult<ColumnModel>> GetColumn(int id)
        {
            var column = await _context.Columns.FindAsync(id);

            if (column == null)
            {
                return NotFound();
            }

            return column;
        }

        // POST: api/columns
        [HttpPost]
        public async Task<ActionResult<ColumnModel>> CreateColumn(ColumnModel column)
        {
            _context.Columns.Add(column);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetColumn), new { id = column.Id }, column);
        }

        // PUT: api/columns/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateColumn(int id, ColumnModel column)
        {
            if (id != column.Id)
            {
                return BadRequest();
            }

            _context.Entry(column).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ColumnExists(id))
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

        // DELETE: api/columns/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteColumn(int id)
        {
            var column = await _context.Columns.FindAsync(id);
            if (column == null)
            {
                return NotFound();
            }

            _context.Columns.Remove(column);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ColumnExists(int id)
        {
            return _context.Columns.Any(e => e.Id == id);
        }
    }
}
