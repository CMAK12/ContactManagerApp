using ContactManagerApp.Core.Models;
using ContactManagerApp.Core.Stores;
using ContactManagerApp.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContactManagerApp.Server.Controllers;

public class ContactsController : Controller
{
    private readonly IContactService _contactService;
    private readonly IContactStore _contact;

    public ContactsController(IContactService contactService, IContactStore contactStore)
    {
        _contactService = contactService;
        _contact = contactStore;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var contacts = await _contact.GetAllContactsAsync();
        return View(contacts);
    }

    [HttpPost]
    [Route("[controller]")]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        try
        {
            await _contactService.ProcessCsvFileAsync(file);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut]
    [Route("[controller]")]
    public async Task<IActionResult> Edit([FromBody] Contact contact)
    {
        try
        {
            await _contact.UpdateContactAsync(contact);
            return Ok("Contact updated successfully.");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);  // Contact not found
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("[controller]/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _contact.DeleteContactAsync(id);
            return Ok("Contact deleted successfully.");
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);  // Contact not found
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}