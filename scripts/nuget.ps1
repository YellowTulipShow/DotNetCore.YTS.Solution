$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

$nuget_source_url="https://api.nuget.org/v3/index.json"
$config_api_key = "./_release_nuget_packages/nuget.org-api-key.txt"
if (Test-Path $config_api_key) {
    $api = Get-Content $config_api_key
    Write-Output "dotnet nuget push <fileName> --api-key $api --source $nuget_source_url --skip-duplicate"
}

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
