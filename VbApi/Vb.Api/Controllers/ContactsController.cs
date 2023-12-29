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
public class ContactsController : ControllerBase
{
    private readonly VbDbContext _dbContext;

    public ContactsController(VbDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
    {
        var contacts = await _dbContext.Contacts.ToListAsync();
        return Ok(contacts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Contact>> GetContact(int id)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        return contact;
    }

    [HttpPost]
    public async Task<ActionResult<Contact>> PostContact([FromBody] Contact contact)
    {
        _dbContext.Contacts.Add(contact);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetContact), new { id = contact.Id }, contact);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutContact(int id, [FromBody] Contact contact)
    {
        if (id != contact.Id)
        {
            return BadRequest();
        }

        _dbContext.Entry(contact).State = EntityState.Modified;

        try
        {
            await _dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ContactExists(id))
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
    public async Task<IActionResult> DeleteContact(int id)
    {
        var contact = await _dbContext.Contacts.FindAsync(id);

        if (contact == null)
        {
            return NotFound();
        }

        _dbContext.Contacts.Remove(contact);
        await _dbContext.SaveChangesAsync();

        return NoContent();
    }

    private bool ContactExists(int id)
    {
        return _dbContext.Contacts.Any(c => c.Id == id);
    }
}
