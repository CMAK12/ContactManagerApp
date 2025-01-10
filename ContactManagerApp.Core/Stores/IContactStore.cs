
using ContactManagerApp.Core.Models;

namespace ContactManagerApp.Core.Stores;

public interface IContactStore
{
    /// <summary>
    /// Retrieves all contacts.
    /// </summary>
    /// <returns>The task result contains an enumerable collection of contacts.</returns>
    public Task<IEnumerable<Contact>> GetAllContactsAsync();

    /// <summary>
    /// Retrieves a contact by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the contact to retrieve.</param>
    /// <returns>The task result contains the contact with the specified identifier.</returns>
    public Task<Contact> GetContactByIdAsync(Guid id);

    /// <summary>
    /// Adds a new contact.
    /// </summary>
    /// <param name="contact">The contact to add.</param>
    public Task AddContactAsync(Contact contact);
    /// <summary>
    /// Add new contacts.
    /// </summary>
    /// <param name="contacts">The list of contacts to add.</param>
    /// <returns></returns>
    public Task AddContactsAsync(IEnumerable<Contact> contacts);

    /// <summary>
    /// Updates an existing contact.
    /// </summary>
    /// <param name="contact">The contact to update.</param>
    public Task UpdateContactAsync(Contact contact);

    /// <summary>
    /// Deletes a contact by its identifier.
    /// </summary>
    /// <param name="id">The identifier of the contact to delete.</param>
    public Task DeleteContactAsync(Guid id);
}