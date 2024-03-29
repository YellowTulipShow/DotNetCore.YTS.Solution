﻿$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

. ./shell/global_tools.ps1

. ./shell/release_app.ps1 "toolScript"

. ./shell/release_packages.ps1 "toolScript"

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
