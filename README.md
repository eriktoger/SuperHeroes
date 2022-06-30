Start the api:
dotnet run

You also need to crete a appsettings.json

And create a Mongo Atlas DB.
{
"MongoDB": {
"ConnectionURI": "Connection URI",
"DatabaseName": "SuperHeroDB",
"SuperHeroes": "Superheroes",
"Users": "Users"
},
"ConnectionStrings": {
"DefaultConnection": "server=localhost;database=superherodb; trusted_connection=true"
},
"AppSettings": {
"Token": "A secret token",
"Salt": "Some salt"
},
"Logging": {
"LogLevel": {
"Default": "Information",
"Microsoft.AspNetCore": "Warning"
}
},
"AllowedHosts": "\*"
}
