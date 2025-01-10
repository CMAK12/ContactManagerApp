using ContactManagerApp.Core.Data;
using ContactManagerApp.Core.Models;
using ContactManagerApp.Core.Stores;
using Microsoft.EntityFrameworkCore;

namespace ContactManagerApp.Persistence.Repositories;

public class ContactRepository : IContactStore
{
    private readonly AppDbContext _db;

    public ContactRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Contact>> GetAllContactsAsync()
    {
        return await _db.Contacts.ToListAsync();
    }

    public async Task<Contact> GetContactByIdAsync(Guid id)
    {
        var trackedContact = await _db.Contacts.FindAsync(id);
        
        if (trackedContact == null) throw new NullReferenceException("Contact wasn't found");
        
        return trackedContact;
    }

    public async Task AddContactAsync(Contact contact)
    {
        if (contact == null) throw new NullReferenceException("Contact cannot be null or empty");
        
        _db.Contacts.Add(contact);
        await _db.SaveChangesAsync();
    }
    
    public async Task AddContactsAsync(IEnumerable<Contact> contacts)
    {
        if (contacts == null || !contacts.Any()) throw new NullReferenceException("Contacts cannot be null or empty");
        
        await _db.Contacts.AddRangeAsync(contacts);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateContactAsync(Contact contact)
    {
        if (contact == null) throw new ArgumentNullException(nameof(contact), "Contact cannot be null.");

        var trackedEntity = await _db.Contacts.FindAsync(contact.Id);
        if (trackedEntity == null) throw new InvalidOperationException("Contact not found.");

        _db.Entry(trackedEntity).CurrentValues.SetValues(contact);
        await _db.SaveChangesAsync();
    }


    public async Task DeleteContactAsync(Guid id)
    {
        if (id == Guid.Empty) throw new ArgumentNullException(nameof(id), "Contact ID cannot be empty.");
        
        var contact = await _db.Contacts.FindAsync(id);
        if (contact == null) throw new InvalidOperationException("Contact not found.");
        
        _db.Contacts.Remove(contact);
        await _db.SaveChangesAsync();
    }
}