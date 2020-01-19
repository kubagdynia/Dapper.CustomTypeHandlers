# Dapper.CustomTypeHandlers
Dapper custom type handlers to serialize/deserialize objects to Xml and Json.

### Installation
Use NuGet Package Manager
> Install-Package Dapper.CustomTypeHandlers

or .NET CLI
> dotnet add package Dapper.CustomTypeHandlers

or just copy into the project file to reference the package
> <PackageReference Include="Dapper.CustomTypeHandlers" Version="0.5.0" />

### How to use
- Create class that implements **IXmlObjectType** or **IJsonObjectType** interface
```csharp
public class Book
{
	public long Id { get; set; }
	public string Title { get; set; }
	public BookDescription Description { get; set; }
}

public class BookDescription : IXmlObjectType
{
	public Learn Learn { get; set; }
	public string About { get; set; }
	public Features Features { get; set; }
}

public class Learn
{
	public List<string> Points { get; set; }
}

public class Features
{
	public List<string> Points { get; set; }
}
```
- Register these new classes in **Startup.cs**
```csharp
services.RegisterDapperCustomTypeHandlers(new[] { typeof(Book).Assembly });
```
- Create table in a database that contains a column of the XML type (SQL Server)
```sql
CREATE TABLE [dbo].[Books](
	[Id] bigint IDENTITY(1,1) NOT NULL,
	[Title] nvarchar(200) NOT NULL,
	[Description] xml NULL
	CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
)
```
To persist object as a JSON,  you can use the nvarchar field (class should implement IJsonObjectType)
```sql
CREATE TABLE [dbo].[Books](
	[Id] bigint IDENTITY(1,1) NOT NULL,
	[Title] nvarchar(200) NOT NULL,
	[Description] nvarchar(max) NULL
	CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED
	(
		[Id] ASC
	)
)
```

- Use Dapper to save object data in the database
```csharp
public async Task SaveBook(Book book)
{
	using (var conn = _connectionFactory.Connection())
	{
		await conn.ExecuteAsync(_@"INSERT INTO Books (Title, Description) VALUES (@Title, @Description)", book);
	}
}
```

### How to Test
Every commit or pull request is built and tested on the Continuous Integration system ([Travis CI](https://travis-ci.com/kubagdynia/Dapper.CustomTypeHandlers/branches)).

To test locally:
- Download and install [.NET Core 3.1 SDK](https://dotnet.microsoft.com/download)
- Clone or download source code
> git clone https://github.com/kubagdynia/Dapper.CustomTypeHandlers.git
- Start tests from the command line
> dotnet test ./DapperMappers/

### Technologies
List of technologies, frameworks and libraries used for implementation:
- [.NET Core 3.1](https://dotnet.microsoft.com/download) (platform)
- [Dapper](https://github.com/StackExchange/Dapper) (micro ORM)
- [System.Text.Json](https://www.nuget.org/packages/System.Text.Json) (JSON serialization/deserialization)
- [NUnit](https://nunit.org/) (testing framework)
- [SQLite](https://www.sqlite.org/) (database for testing purpose)
- [FluentAssertions](https://github.com/fluentassertions/fluentassertions) (fluent API for asserting the result of unit tests)

### License
This project is licensed under the [MIT License](https://opensource.org/licenses/MIT).

### Builds and Tests
| Branch       | Status      |
|--------------|-------------|
| [master](https://travis-ci.com/kubagdynia/Dapper.CustomTypeHandlers/branches)       | [![Build Status](https://travis-ci.com/kubagdynia/Dapper.CustomTypeHandlers.svg?branch=master)](https://travis-ci.com/kubagdynia/CustomTypeHandlers)|