using Microsoft.AspNetCore.Http;

namespace ContactManagerApp.Infrastructure.Services.Interfaces;

public interface IContactService
{ 
    /// <summary>
    /// Processes a CSV file and adds the contacts to the database.
    /// </summary>
    /// <param name="file">The CSV file to process.</param>
    public Task ProcessCsvFileAsync(IFormFile file);
}