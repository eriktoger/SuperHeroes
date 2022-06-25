Start the api:
dotnet run

You also need to crete a appsettings.json

{
"MongoDB": {
"ConnectionURI": "Your connection URI goes here",
"DatabaseName": "SuperHeroDB",
"CollectionName": "Superheroes"
},
"ConnectionStrings": {
"DefaultConnection": "server=localhost;database=superherodb; trusted_connection=true"
},
"Logging": {
"LogLevel": {
"Default": "Information",
"Microsoft.AspNetCore": "Warning"
}
},
"AllowedHosts": "\*"
}
