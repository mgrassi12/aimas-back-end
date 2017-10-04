# AIMAS API

Using ASP.Net Core


#DataBase Commands

Run commands from the API folder level.

Make sure both "InitializeDB" and "Services" settings in appsettings.json from the API are off (false) to run these commands

Create new schema:

`dotnet ef migrations add {MigrationName}`

Update db to new schema:

`dotnet ef database update`
