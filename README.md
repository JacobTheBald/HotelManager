Running the tests:

1. Open console
2. Go to "HotelManager.Tests"
3. Run command "dotnet test"

Running the app: Just run the executable located in HotelManager/HotelManager.exe, or launch app via IDE.
Prerequisites: 
- .Net 8.0
- Postgresql server (address, port, login and password are ready to be changed in ConnectionStrings section of appsettings.json file - it was left there for easier setup)

Known issues:
- On launching the app, DB is not created or migrated properly
- While migrating, EF is kind of lost with pending migrations

If any of theese issues exist:
1. Remove DB
2. Remove Migrations folder (we'll start fresh)
3. Open prompt, run these two commands:
  dotnet ef migrations add InitialCreate
  dotnet ef database update

By doing this, on launching the app, DB will be created and data will be seeded with the first launch.
