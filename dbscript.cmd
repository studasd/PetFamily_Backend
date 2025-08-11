:: "dotnet-ef" не является внутренней или внешней командой, исполняемой программой или пакетным файлом. Ставим:
:: dotnet tool install --global dotnet-ef

dotnet-ef database drop -f -c AccountsDbContext -p .\src\Accounts\PetFamily.Accounts.Infrastructure -s .\src\PetFamily.Web
dotnet-ef database drop -f -c SpeciesWriteDbContext -p .\src\Specieses\PetFamily.Specieses.Infrastructure -s .\src\PetFamily.Web
dotnet-ef database drop -f -c VolunteerWriteDbContext -p .\src\Volunteers\PetFamily.Volunteers.Infrastructure -s .\src\PetFamily.Web

dotnet-ef migrations remove -c AccountsDbContext -p .\src\Accounts\PetFamily.Accounts.Infrastructure -s .\src\PetFamily.Web
dotnet-ef migrations remove -c SpeciesWriteDbContext -p .\src\Specieses\PetFamily.Specieses.Infrastructure -s .\src\PetFamily.Web
dotnet-ef migrations remove -c VolunteerWriteDbContext -p .\src\Volunteers\PetFamily.Volunteers.Infrastructure -s .\src\PetFamily.Web

dotnet-ef migrations add Accounts_Init -c AccountsDbContext -p .\src\Accounts\PetFamily.Accounts.Infrastructure -s .\src\PetFamily.Web
dotnet-ef migrations add Specieses_Init -c SpeciesWriteDbContext -p .\src\Specieses\PetFamily.Specieses.Infrastructure -s .\src\PetFamily.Web
dotnet-ef migrations add Volunteer_Init -c VolunteerWriteDbContext -p .\src\Volunteers\PetFamily.Volunteers.Infrastructure -s .\src\PetFamily.Web

dotnet-ef database update -c AccountsDbContext -p .\src\Accounts\PetFamily.Accounts.Infrastructure -s .\src\PetFamily.Web
dotnet-ef database update -c SpeciesWriteDbContext -p .\src\Specieses\PetFamily.Specieses.Infrastructure -s .\src\PetFamily.Web
dotnet-ef database update -c VolunteerWriteDbContext -p .\src\Volunteers\PetFamily.Volunteers.Infrastructure -s .\src\PetFamily.Web

pause