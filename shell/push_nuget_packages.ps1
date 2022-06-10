$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

function PrintLineSplit([string]$path)
{
    Write-Host ""
    Write-Host "======================================================================================="
    Write-Host ""
}

# 查找所有项目名单
PrintLineSplit
Write-Host "[Execute] Push Nuget.org Packages:"

$nuget_source_url="https://api.nuget.org/v3/index.json"
$cpath = "./_configs/nuget.org-api-key.config"
if (!(Test-Path $cpath)) {
    Write-Host "[Config File] Not Existent: $cpath !!!";
    PrintLineSplit
    return
}
$api = Get-Content $cpath
PrintLineSplit
Write-Host "[Get nuget.org api-key]: $api"

PrintLineSplit
$cpath = "./_release/packages"
if (!(Test-Path $cpath)) {
    Write-Host "[Path Null] Not Existent: $cpath !!!";
    PrintLineSplit
    return
}
Set-Location $cpath
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

        PrintLineSplit

        # 推送打包文件
        Write-Output "dotnet nuget push $item --api-key $api --source $nuget_source_url --skip-duplicate"
        dotnet nuget push $item --api-key $api --source $nuget_source_url --skip-duplicate
    }
} else {
    Write-Output "API-KEY:$cpath Content is Null!!!"
}

PrintLineSplit

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
