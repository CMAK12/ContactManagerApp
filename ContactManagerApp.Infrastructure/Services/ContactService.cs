using System.Globalization;
using ContactManagerApp.Core.Models;
using ContactManagerApp.Core.Stores;
using ContactManagerApp.Infrastructure.Services.Interfaces;
using CsvHelper;
using Microsoft.AspNetCore.Http;

namespace ContactManagerApp.Infrastructure.Services;

public class ContactService : IContactService
{
    private readonly IContactStore _contactRepository;

    public ContactService(IContactStore contactRepository)
    {
        _contactRepository = contactRepository;
    }

    public async Task ProcessCsvFileAsync(IFormFile file)
    {
        if (file != null && file.Length > 0 && file.ContentType == "application/vnd.ms-excel")
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // Configure CSV reader
                var csvConfig = new CsvHelper.Configuration.CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    HeaderValidated = null,  // Disables header validation
                    MissingFieldFound = null // Optionally disables missing field validation
                };

                using (var csv = new CsvReader(reader, csvConfig))
                {
                    var records = csv.GetRecords<Contact>().ToList(); // Read all records

                    // If your repository supports adding a list of contacts, use AddRangeAsync
                    await _contactRepository.AddContactsAsync(records);
                }
            }
        }
        else throw new Exception("Invalid file type or file is empty");
    }
}