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
public class AccountTransactionsController : ControllerBase
{
    private readonly VbDbContext _dbContext;

    public AccountTransactionsController(VbDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AccountTransaction>>> GetAccountTransactions()
    {
        var accountTransactions = await _dbContext.AccountTransactions.ToListAsync();
        return Ok(accountTransactions);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AccountTransaction>> GetAccountTransaction(int id)
    {
        var accountTransaction = await _dbContext.AccountTransactions.FindAsync(id);

        if (accountTransaction == null)
        {
            return NotFound();
        }

        return accountTransaction;
    }

    [HttpPost]
    public async Task<ActionResult<AccountTransaction>> PostAccountTransaction([FromBody] AccountTransaction accountTransaction)
    {
        _dbContext.AccountTransactions.Add(accountTransaction);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAccountTransaction), new { id = accountTransaction.Id }, accountTransaction);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAccountTransaction(int id, [FromBody] AccountTransaction accountTransaction)
    {
        if (id != accountTransaction.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(accountTransaction).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AccountTransactionExists(id))
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
    public async Task<IActionResult> DeleteAccountTransaction(int id)
    {
        var accountTransaction = await _dbContext.AccountTransactions.FindAsync(id);

        if (accountTransaction == null)
        {
            return NotFound();
        }

        _dbContext.AccountTransactions.Remove(accountTransaction);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool AccountTransactionExists(int id)
    {
        return _dbContext.AccountTransactions.Any(a => a.Id == id);
    }
}
