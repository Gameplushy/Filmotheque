# Filmotheque

A REST API using C#, EntityFramework and OpenAPI.

## Prerequisites

You'll need :
- Visual Studio 2022 (with "ASP.NET and web development" kit) (Optional, you can use the cmd if you don't need to debug it.)
- DotNET 7 (download the SDK [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))
- SqlExpress (Or any kind of way to get SqlServer. You'll need to chage the connection string in Program.cs if it's not SQLExpress)

You can also have a database viewer like SSMS in order to have a direct view of the tables.

## How to begin

### Using Visual Studio
Open the solution (.sln file) using Visual Studio. In case there are compiler errors showing up, you might need to get the NuGet packages; right-click the solution and select "Restore NuGet Packages".

In the Package Manager Console, execute the command `Update-Database`. This will create the database using the migration file. If the migration file is missing/out of date, execute the command `Add-Migration` beforehand.

### Using cmd
Go to the solution's folder. Use the command `dotnet restore`, then `dotnet tool install --global dotnet-ef --version 7.0.14`. After that, use `dotnet ef database update` to migrate the database to your SQL server.

### Using the exe
You still need to download the source code to create the database beforehand. (Check "Using Visual Studio" or "Using cmd")
Download the zip file found [here](https://github.com/Gameplushy/Filmotheque/releases/tag/full-release)

## How to use

### Using Visual Studio
Build and execute the code. A web page should open. This will open a Swagger page where you will be able to test the API.

### Using cmd
In cmd, use `dotnet run`. Follow the given URL (`Now listening on: http://localhost:<port number>`)). Add to the URL `/swagger`. This will open a Swagger page where you will be able to test the API.
### Using the exe
Execute the exe file. Follow the given URL (`Now listening on: http://localhost:<port number>`)). Add to the URL `/swagger`. This will open a Swagger page where you will be able to test the API.

## Having trouble with the SQL connection?
You might want to use the [in-memory branch](https://github.com/Gameplushy/Filmotheque/tree/in-memory) instead.