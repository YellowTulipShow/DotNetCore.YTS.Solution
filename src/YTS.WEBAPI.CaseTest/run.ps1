$ExecutePath = $PWD
Set-Location $PSScriptRoot

Write-Output "dotnet build ./YTS.WEBAPI.CaseTest.csproj"
dotnet build ./YTS.WEBAPI.CaseTest.csproj

Write-Output "dotnet run ./YTS.WEBAPI.CaseTest.csproj --urls http://127.0.0.1:16130"
dotnet run ./YTS.WEBAPI.CaseTest.csproj --urls http://127.0.0.1:16130

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}


# dotnet run .\YTS.WEBAPI.CaseTest.csproj --urls http://localhost:16130

# http://localhost/index.html
# http://localhost/ioapi/index.html
# http://localhost:16130/index.html
# http://localhost:16130/ioapi/index.html
# http://localhost/ioapi/swagger/v1/swagger.json
# http://127.0.0.1:16130/api/KeyValuePairsDb/GetOperableStores
# http://127.0.0.1/ioapi/api/KeyValuePairsDb/GetOperableStores
