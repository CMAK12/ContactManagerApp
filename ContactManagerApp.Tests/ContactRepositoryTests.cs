using Microsoft.EntityFrameworkCore;
using ContactManagerApp.Core.Data;
using ContactManagerApp.Core.Models;
using ContactManagerApp.Persistence.Repositories;

namespace ContactManagerApp.Server.Tests.Repositories
{
    public class ContactRepositoryTests
    {
        private readonly DbContextOptions<AppDbContext> _dbContextOptions;

        public ContactRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [Fact]
        public async Task GetAllContactsAsync_ShouldReturnAllContacts()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            context.Contacts.AddRange(new List<Contact>
            {
                new Contact { Id = Guid.NewGuid(), Name = "John Doe", DateOfBirth = new DateTime(1990, 1, 1), Married = false, Phone = "1234567890", Salary = 50000m },
                new Contact { Id = Guid.NewGuid(), Name = "Jane Doe", DateOfBirth = new DateTime(1985, 5, 15), Married = true, Phone = "0987654321", Salary = 60000m }
            });
            await context.SaveChangesAsync();

            var repository = new ContactRepository(context);

            // Act
            var result = await repository.GetAllContactsAsync();

            // Assert
            Assert.Equal(4, result.Count()); // 4 because the db context init 2 contacts when creates the db
        }

        [Fact]
        public async Task GetContactByIdAsync_ShouldReturnContact_WhenContactExists()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            using var context = new AppDbContext(_dbContextOptions);
            context.Contacts.Add(new Contact { Id = contactId, Name = "John Doe", DateOfBirth = new DateTime(1990, 1, 1), Married = false, Phone = "1234567890", Salary = 50000m });
            await context.SaveChangesAsync();

            var repository = new ContactRepository(context);

            // Act
            var result = await repository.GetContactByIdAsync(contactId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("John Doe", result.Name);
            Assert.Equal(new DateTime(1990, 1, 1), result.DateOfBirth);
            Assert.False(result.Married);
            Assert.Equal("1234567890", result.Phone);
            Assert.Equal(50000m, result.Salary);
        }

        [Fact]
        public async Task GetContactByIdAsync_ShouldThrowException_WhenContactDoesNotExist()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            var repository = new ContactRepository(context);

            // Act
            var exception = await Record.ExceptionAsync(() => repository.GetContactByIdAsync(Guid.NewGuid()));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<NullReferenceException>(exception);
        }

        [Fact]
        public async Task AddContactAsync_ShouldAddContact()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            var repository = new ContactRepository(context);

            var contact = new Contact { Id = Guid.NewGuid(), Name = "John Doe", DateOfBirth = new DateTime(1990, 1, 1), Married = false, Phone = "1234567890", Salary = 50000m };

            // Act
            await repository.AddContactAsync(contact);

            // Assert
            var addedContact = await context.Contacts.FindAsync(contact.Id);
            Assert.NotNull(addedContact);
            Assert.Equal("John Doe", addedContact.Name);
            Assert.Equal(new DateTime(1990, 1, 1), addedContact.DateOfBirth);
            Assert.False(addedContact.Married);
            Assert.Equal("1234567890", addedContact.Phone);
            Assert.Equal(50000m, addedContact.Salary);
        }

        [Fact]
        public async Task AddContactsAsync_ShouldAddContacts()
        {
            // Arrange
            using var context = new AppDbContext(_dbContextOptions);
            var repository = new ContactRepository(context);

            var contacts = new List<Contact>
            {
                new Contact { Id = Guid.NewGuid(), Name = "John Doe", DateOfBirth = new DateTime(1990, 1, 1), Married = false, Phone = "1234567890", Salary = 50000m },
                new Contact { Id = Guid.NewGuid(), Name = "Jane Doe", DateOfBirth = new DateTime(1985, 5, 15), Married = true, Phone = "0987654321", Salary = 60000m }
            };

            // Act
            await repository.AddContactsAsync(contacts);

            // Assert
            var contactsInDb = context.Contacts.ToList();
            Assert.Equal(4, contactsInDb.Count);
        }

        [Fact]
        public async Task UpdateContactAsync_ShouldUpdateContact()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            using var context = new AppDbContext(_dbContextOptions);
            context.Contacts.Add(new Contact { Id = contactId, Name = "John Doe", DateOfBirth = new DateTime(1990, 1, 1), Married = false, Phone = "1234567890", Salary = 50000m });
            await context.SaveChangesAsync();

            var repository = new ContactRepository(context);
            var updatedContact = new Contact { Id = contactId, Name = "John Smith", DateOfBirth = new DateTime(1991, 2, 2), Married = true, Phone = "1112223333", Salary = 55000m };

            // Act
            await repository.UpdateContactAsync(updatedContact);

            // Assert
            var contactInDb = await context.Contacts.FindAsync(contactId);
            Assert.NotNull(contactInDb);
            Assert.Equal("John Smith", contactInDb.Name);
            Assert.Equal(new DateTime(1991, 2, 2), contactInDb.DateOfBirth);
            Assert.True(contactInDb.Married);
            Assert.Equal("1112223333", contactInDb.Phone);
            Assert.Equal(55000m, contactInDb.Salary);
        }

        [Fact]
        public async Task DeleteContactAsync_ShouldRemoveContact_WhenContactExists()
        {
            // Arrange
            var contactId = Guid.NewGuid();
            using var context = new AppDbContext(_dbContextOptions);
            context.Contacts.Add(new Contact { Id = contactId, Name = "John Doe", DateOfBirth = new DateTime(1990, 1, 1), Married = false, Phone = "1234567890", Salary = 50000m });
            await context.SaveChangesAsync();

            var repository = new ContactRepository(context);

            // Act
            await repository.DeleteContactAsync(contactId);

            // Assert
            var deletedContact = await context.Contacts.FindAsync(contactId);
            Assert.Null(deletedContact);
        }
    }
}
