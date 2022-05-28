$ExecutePath = $PWD
Set-Location $PSScriptRoot

Write-Output "dotnet build ./YTS.IOFile.API.csproj"
dotnet build ./YTS.IOFile.API.csproj

Write-Output "dotnet run ./YTS.IOFile.API.csproj --urls http://127.0.0.1:16129"
dotnet run ./YTS.IOFile.API.csproj --urls http://127.0.0.1:16129

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}


# dotnet run .\YTS.IOFile.API.csproj --urls http://localhost:16129

# http://localhost/index.html
# http://localhost/ioapi/index.html
# http://localhost:16129/index.html
# http://localhost:16129/ioapi/index.html
# http://localhost/ioapi/swagger/v1/swagger.json
# http://127.0.0.1:16129/api/KeyValuePairsDb/GetOperableStores