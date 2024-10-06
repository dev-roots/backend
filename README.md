# devRoots Backend

Welcome to the ASP.NET Core Web API backend repository for devRoots. This project serves as the backend of the devRoots application, handling data access, business logic, and API endpoints to support the frontend client.

## Getting Started

Follow the steps below to set up and run the project locally.

### Prerequisites

Make sure you have the following tools installed:

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/)
- SQL Server or any other compatible database (depending on the project configuration)
- (Optional) [Postman](https://www.postman.com/) for API testing
- Once it is installed migrate the database locally in package nuget console:

### Environment Setup

If you're using Entity Framework Core for database management, run the following commands to apply migrations and update your local database:

- Ensure the database exists: In Visual Studio, open the Package Manager Console and run:

```cmd
Update-Database
```

This will apply the existing migrations to your database.

- If you need to create a new migration, use:

```cmd
Add-Migration MigrationName
Update-Database
```

### Running the Application

Once the dependencies are installed and the database is set up, you can run the application:

```cmd
dotnet run
```

Test the API: Use Postman or another tool to test the API endpoints. You can start by checking the Swagger documentation available at https://localhost:7061/swagger/index.html.

## Contributors ✨

Thanks goes to these wonderful people:

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%">
        <img
            src="https://avatars.githubusercontent.com/u/108660843?v=4"
            width="100px;"
            alt="Rafa Gómez"
          /><br /><p>Rafa Gómez</p><br />
      </td>
      <td align="center" valign="top" width="14.28%">
        <img
            src="https://avatars.githubusercontent.com/u/118361589?v=4"
            width="100px;"
            alt="Guillermo Afonso"
          /><br /><p>Guillermo Afonso</p><br />
      </td>
    </tr>
  </tbody>
</table>

## Contributing

We welcome contributions! If you'd like to contribute, please follow these steps:

1. Fork the repository on Github
2. Create a named feature branch (like `add_component_x`)
3. Write your change
4. Write tests for your change (if applicable)
5. Run the tests, ensuring they all pass
6. Submit a Pull Request using Github

## License

This project is licensed under the MIT license.
