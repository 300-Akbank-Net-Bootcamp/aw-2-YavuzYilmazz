using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vb.Data;
using Vb.Data.Entity;


namespace VbApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EftTransactionsController : ControllerBase
{
    private readonly VbDbContext _dbContext;

    public EftTransactionsController(VbDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EftTransaction>>> GetEftTransactions()
    {
        var eftTransactions = await _dbContext.EftTransactions.ToListAsync();
        return Ok(eftTransactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EftTransaction>> GetEftTransaction(int id)
    {
        var eftTransaction = await _dbContext.EftTransactions.FindAsync(id);

        if (eftTransaction == null)
        {
            return NotFound();
        }

        return eftTransaction;
    }

    [HttpPost]
    public async Task<ActionResult<EftTransaction>> PostEftTransaction([FromBody] EftTransaction eftTransaction)
    {
        _dbContext.EftTransactions.Add(eftTransaction);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetEftTransaction), new { id = eftTransaction.Id }, eftTransaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutEftTransaction(int id, [FromBody] EftTransaction eftTransaction)
    {
        if (id != eftTransaction.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(eftTransaction).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EftTransactionExists(id))
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEftTransaction(int id)
    {
        var eftTransaction = await _dbContext.EftTransactions.FindAsync(id);

        if (eftTransaction == null)
        {
            return NotFound();
        }

        _dbContext.EftTransactions.Remove(eftTransaction);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool EftTransactionExists(int id)
    {
        return _dbContext.EftTransactions.Any(e => e.Id == id);
    }
}
