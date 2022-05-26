$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

$nuget_source_url="https://api.nuget.org/v3/index.json"
$config_api_key = "./_configs/nuget.org-api-key.config"
$api = Get-Content $config_api_key

Set-Location "./_release/packages"
$list = Get-ChildItem -Recurse -Include *.nupkg -Name

if ($api) {
    for ($i = 0; $i -lt $list.Count; $i++) {
        if ($list.Count -eq 1) {
            $item = $list;
        } else {
            $item = $list[$i]
        }
        if (!$item) {
            continue;
        }
        # 推送打包文件
        Write-Output "dotnet nuget push $item --api-key $api --source $nuget_source_url --skip-duplicate"
        dotnet nuget push $item --api-key $api --source $nuget_source_url --skip-duplicate
    }
} else {
    Write-Output "API-KEY:$config_api_key Content is Null!!!"
}

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
