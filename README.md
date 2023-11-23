# Filmotheque

A REST API using C#, EntityFramework and OpenAPI.
Since this uses an in-memory database, ENTITES AREN'T SAVED BETWEEN RELOADS.

## Prerequisites

You'll need :
- Visual Studio 2022 (with "ASP.NET and web development" kit) (Optional, you can use the cmd if you don't need to debug it.)
- DotNET 7 (download the SDK [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))

## How to begin

### Using Visual Studio
Open the solution (.sln file) using Visual Studio. In case there are compiler errors showing up, you might need to get the NuGet packages; right-click the solution and select "Restore NuGet Packages".

### Using cmd
Go to the solution's folder. Use the command `dotnet restore`.

### Using the exe
Download the zip file found [here](https://github.com/Gameplushy/Filmotheque/releases/tag/full-release).

## How to use

### Using Visual Studio
Build and execute the code. A web page should open. This will open a Swagger page where you will be able to test the API.

### Using cmd
In cmd, use `dotnet run`. Follow the given URL (`Now listening on: http://localhost:<port number>`)). Add to the URL `/swagger`. This will open a Swagger page where you will be able to test the API.

### Using the exe
Execute the exe file. Follow the given URL (`Now listening on: http://localhost:<port number>`)). Add to the URL `/swagger`. This will open a Swagger page where you will be able to test the API.