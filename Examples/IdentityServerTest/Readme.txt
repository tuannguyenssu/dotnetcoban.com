Trong trường hợp dùng dotnet CLI
dotnet tool install --global dotnet-ef
dotnet add package Microsoft.EntityFrameworkCore.Design


dotnet ef migrations add InitialPersistedGrantDbMigration -c PersistedGrantDbContext -o Migrations/PersistedGrantDb
dotnet ef migrations add InitialConfigurationDbMigration -c ConfigurationDbContext -o Migrations/ConfigurationDb
dotnet ef migrations add InitialApplicationDbMigration -c ApplicationDbContext -o Migrations/ApplicationDb

dotnet ef migrations remove -c ApplicationDbContext
dotnet ef migrations remove -c ConfigurationDbContext
dotnet ef migrations remove -c PersistedGrantDbContext

Trong trường hợp dùng Package Manager Console trong Visual Studio
dotnet add package Microsoft.EntityFrameworkCore.Tools

Xóa toàn bộ database cũ
DROP DATABASE IdentityServerDb

Remove-Migration -Context ApplicationDbContext
Remove-Migration -Context ConfigurationDbContext
Remove-Migration -Context PersistedGrantDbContext

Add-Migration InitialPersistedGrantDbMigration -Context PersistedGrantDbContext -o Migrations/PersistedGrantDb
Add-Migration InitialConfigurationDbMigration -Context ConfigurationDbContext -o Migrations/ConfigurationDb
Add-Migration InitialApplicationDbMigration -Context ApplicationDbContext -o Migrations/ApplicationDb

Update-database -Context PersistedGrantDbContext
Update-database -Context ConfigurationDbContext
Update-database -Context ApplicationDbContext


http://docs.identityserver.io/en/latest/index.html
https://blog.georgekosmidis.net/2019/02/08/identityserver4-asp-dotnet-core-api-and-a-client-with-username-password/
https://chsakell.com/2019/03/11/asp-net-core-identity-series-oauth-2-0-openid-connect-identityserver/