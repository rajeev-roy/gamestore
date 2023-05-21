# Game store API

## Starting Sql Server
```powershell
$sa_password = "[SA PASSWORD HERE]"
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=$sa_password" -p 1433:1433 -v sqlvolume:/var/opt/mssql -d --rm --name mssql mcr.microsoft.com/mssql/server:2022-latest
```
## Setting the connection string to secret manager
```powershell
$sa_password = "[SA PASSWORD HERE]"
dotnet user-secrets set "ConnectionStrings:GameStoreContext" "Server=localhost;Database=GameStore;User Id=waveuser;Password=$sa_password;TrustServerCertificate=True"
```
## Setting the Azure Storage connection string to secret manager
```powershell
$storage_connstring = ""
dotnet user-secrets set "ConnectionStrings:AzureStorage" $storage_connstring

```#   g a m e s t o r e  
 