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
public class AddressesController : ControllerBase
{
    private readonly VbDbContext _dbContext;

    public AddressesController(VbDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
    {
        var addresses = await _dbContext.Addresses.ToListAsync();
        return Ok(addresses);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> GetAddress(int id)
    {
        var address = await _dbContext.Addresses.FindAsync(id);

        if (address == null)
        {
            return NotFound();
        }

        return address;
    }

    [HttpPost]
    public async Task<ActionResult<Address>> PostAddress([FromBody] Address address)
    {
        _dbContext.Addresses.Add(address);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAddress), new { id = address.Id }, address);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAddress(int id, [FromBody] Address address)
    {
        if (id != address.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(address).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AddressExists(id))
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
    public async Task<IActionResult> DeleteAddress(int id)
    {
        var address = await _dbContext.Addresses.FindAsync(id);

        if (address == null)
        {
            return NotFound();
        }

        _dbContext.Addresses.Remove(address);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool AddressExists(int id)
    {
        return _dbContext.Addresses.Any(a => a.Id == id);
    }
}
