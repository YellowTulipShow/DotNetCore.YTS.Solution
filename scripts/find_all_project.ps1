$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

# 查找所有项目名单
$config_projects = "./_configs/projects.config"
if (!(Test-Path $config_projects)) {
    Write-Output "config file:${config_projects} not existence, find all *.csproj files"
    New-Item -ItemType File -Force -Path $config_projects
}
$list = Get-ChildItem -Recurse -Include *.csproj -Name
$list > $config_projects
Write-Output "Get all project file:"
Write-Output $list

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
