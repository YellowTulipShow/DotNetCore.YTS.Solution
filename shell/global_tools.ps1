param([string]$runMethod)
if ($runMethod -ne "toolScript") {
    $ExecutePath = $PWD
    Set-Location $PSScriptRoot
    Set-Location ..
}

function PrintLineSplit([string]$path) {
    Write-Host ""
    Write-Host "======================================================================================="
    Write-Host ""
}

function ConfigFileHandle([string]$path) {
    $path = $path -replace "\\", "/"
    $fixed_path = $path -replace "_configs/", "_configs/fixed/"
    if (!(Test-Path $fixed_path)) {
        Write-Host "[Fixed Config File] Not Existent: $fixed_path";
        return ""
    }
    elseif (!(Test-Path $path)) {
        Write-Host "[Custom Config File] Not Existent: $fixed_path, Create ......";
        New-Item -ItemType File -Force -Path $path
        $content = Get-Content $fixed_path
        $content > $path
    }
    else {
        return $path;
    }
}

if ($runMethod -ne "toolScript") {
    Set-Location $ExecutePath
    if ($PSScriptRoot -eq $ExecutePath) {
        timeout.exe /T -1
    }
}
