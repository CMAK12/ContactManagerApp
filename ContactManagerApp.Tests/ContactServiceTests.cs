using ContactManagerApp.Core.Models;
using ContactManagerApp.Core.Stores;
using ContactManagerApp.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Moq;

public class ContactServiceTests
{
    [Fact]
    public async Task ProcessCsvFileAsync_NullFile_DoesNotAddContacts()
    {
        // Arrange
        var mockContactStore = new Mock<IContactStore>();
        var contactService = new ContactService(mockContactStore.Object);

        // Act
        await contactService.ProcessCsvFileAsync(null);

        // Assert
        mockContactStore.Verify(x => x.AddContactsAsync(It.IsAny<IEnumerable<Contact>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessCsvFileAsync_EmptyFile_DoesNotAddContacts()
    {
        // Arrange
        var mockContactStore = new Mock<IContactStore>();
        var contactService = new ContactService(mockContactStore.Object);
        var fileMock = new Mock<IFormFile>();
        var ms = new MemoryStream();
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns("contacts.csv");
        fileMock.Setup(_ => _.Length).Returns(ms.Length);
        fileMock.Setup(_ => _.ContentType).Returns("text/csv");

        // Act
        await contactService.ProcessCsvFileAsync(fileMock.Object);

        // Assert
        mockContactStore.Verify(x => x.AddContactsAsync(It.IsAny<IEnumerable<Contact>>()), Times.Never);
    }

    [Fact]
    public async Task ProcessCsvFileAsync_InvalidContentType_DoesNotAddContacts()
    {
        // Arrange
        var mockContactStore = new Mock<IContactStore>();
        var contactService = new ContactService(mockContactStore.Object);
        var fileMock = new Mock<IFormFile>();
        var ms = new MemoryStream();
        fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
        fileMock.Setup(_ => _.FileName).Returns("contacts.csv");
        fileMock.Setup(_ => _.Length).Returns(ms.Length);
        fileMock.Setup(_ => _.ContentType).Returns("application/json");

        // Act
        await contactService.ProcessCsvFileAsync(fileMock.Object);

        // Assert
        mockContactStore.Verify(x => x.AddContactsAsync(It.IsAny<IEnumerable<Contact>>()), Times.Never);
    }
}