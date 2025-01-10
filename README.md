# Contact Manager Application

## Features

- **Read Contacts**: View a list of all contacts in the home page.
- **Update Contact**: Modify existing contacts.
- **Delete Contact**: Remove contacts from the database.
- **CSV File Upload**: Process and bulk insert contacts from a CSV file.

## Technologies

- **Backend**: .NET 8 (ASP.NET)
- **Database**: MS SQL Server
- **ORM**: Entity Framework Core 8.0.11
- **Testing**: xUnit, Moq, InMemory DB for testing

## Setup

1. **Installation**:

   ```bash
   git clone https://github.com/cmak12/ContactManagerApp.git
   cd ContactManagerApp
   ```

2. **Backend Setup**

   - Install required NuGet packages

   ```bash
    dotnet restore
   ```

   - Create and then run migration

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

3. **Run the Application**

   - To run backend:

   ```bash
   cd ContactManagerApp.Server
   dotnet run
   ```

4. **Testing**

   - To run tests:

   ```bash
   cd ContactManagerApp.Tests
   dotnet test
   ```
