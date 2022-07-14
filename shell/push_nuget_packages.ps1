param([string]$runMethod)
if ($runMethod -ne "toolScript") {
    $ExecutePath = $PWD
    Set-Location $PSScriptRoot
    Set-Location ..

    . ./shell/global_tools.ps1
}

# 查找所有项目名单
PrintLineSplit
Write-Host "[Execute] Push Nuget.org Packages:"

PrintLineSplit
$nuget_source_url = "https://api.nuget.org/v3/index.json"
$nugetApiKeyPath = "./_configs/nuget.org-api-key.config"
if (!(Test-Path $nugetApiKeyPath)) {
    Write-Host "[Config File] Not Existent: $nugetApiKeyPath !!!";
    PrintLineSplit
    return
}
$api = Get-Content $nugetApiKeyPath
if (!$api) {
    Write-Output "[API-KEY]: $nugetApiKeyPath Content is Null!!!"
    PrintLineSplit
    return
}
Write-Host "[Get nuget.org api-key]: $api"

PrintLineSplit
Write-Host "PWD: $PWD !!!";
$cpath = "./_release/packages"
if (!(Test-Path $cpath)) {
    Write-Host "[Path Null] Not Existent: $cpath !!!";
    PrintLineSplit
    return
}
Set-Location $cpath
$list = Get-ChildItem -Recurse -Include *.nupkg -Name
for ($i = 0; $i -lt $list.Count; $i++) {
    if ($list.Count -eq 1) {
        $item = $list;
    }
    else {
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

PrintLineSplit

if ($runMethod -ne "toolScript") {
    Set-Location $ExecutePath
    if ($PSScriptRoot -eq $ExecutePath) {
        timeout.exe /T -1
    }
}
