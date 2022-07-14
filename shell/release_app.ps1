param([string]$runMethod)
if ($runMethod -ne "toolScript") {
    $ExecutePath = $PWD
    Set-Location $PSScriptRoot
    Set-Location ..

    . ./shell/global_tools.ps1
}

# 系统发布名单配置定义
PrintLineSplit
$cpath = ConfigFileHandle("./_configs/systems.config")
if ($cpath -and (Test-Path $cpath)) {
    $systems = Get-Content $cpath
    Write-Host "[Get Release Signs]: $systems";
}
else {
    Write-Host "[Systems Sign Config File] Not Existent: $cpath";
    PrintLineSplit
    return;
}

# 发布应用程序
PrintLineSplit
Write-Host "[Release Apps]:";
$cpath = ConfigFileHandle("./_configs/app.config")
if ($cpath -and (Test-Path $cpath)) {
    $list = Get-Content $cpath
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
        $name = $item -replace '^.+[/\\]'
        $name = $name -replace '\.csproj$'
        for ($j = 0; $j -lt $systems.Count; $j++) {
            if ($systems.Count -eq 1) {
                $sys = $systems;
            }
            else {
                $sys = $systems[$j];
            }

            PrintLineSplit

            # 发布项目
            Write-Host "dotnet publish $item -o './_release/$sys/$name' --runtime '$sys'"
            dotnet publish $item -o "./_release/$sys/$name" --runtime "$sys"
        }
    }
}

if ($runMethod -ne "toolScript") {
    Set-Location $ExecutePath
    if ($PSScriptRoot -eq $ExecutePath) {
        timeout.exe /T -1
    }
}
