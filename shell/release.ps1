$ExecutePath = $PWD
Set-Location $PSScriptRoot
Set-Location ..

# 系统发布名单配置定义
$config_systems = "./_configs/systems.config"
if (Test-Path $config_systems) {
    $systems = Get-Content $config_systems
} else {
    $systems="win-x64", "linux-x64"
    New-Item -ItemType File -Force -Path $config_systems
    for ($i = 0; $i -lt $systems.Count; $i++) {
        if ($systems.Count -eq 1) {
            $item = $systems;
        } else {
            $item = $systems[$i]
        }
        $item >> $config_systems
    }
}

# 发布应用程序
$config_apps = "./_configs/app.config"
if (Test-Path $config_apps) {
    $list = Get-Content $config_apps
    for ($i = 0; $i -lt $list.Count; $i++) {
        if ($list.Count -eq 1) {
            $item = $list;
        } else {
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
            } else {
                $sys = $systems[$j];
            }
            # 发布项目
            Write-Host "dotnet publish $item -o './_release/$sys/$name' --runtime '$sys'"
            dotnet publish $item -o "./_release/$sys/$name" --runtime "$sys"
        }
    }
} else {
    Write-Output "Publish APP Config File Not Existent: $config_apps"
    New-Item -ItemType File -Force -Path $config_apps
}

# 打包项目
$config_packages = "./_configs/packages.config"
$save_path_packages = "./_release/packages"
if (Test-Path $save_path_packages) {
    Remove-Item -Recurse -Force $save_path_packages
}
if (Test-Path $config_packages) {
    $list = Get-Content $config_packages
    for ($i = 0; $i -lt $list.Count; $i++) {
        if ($list.Count -eq 1) {
            $item = $list;
        } else {
            $item = $list[$i]
        }
        if (!$item) {
            continue;
        }
        # 打包项目
        Write-Output "dotnet pack $item --output '$save_path_packages'"
        dotnet pack $item --output "$save_path_packages"
    }
} else {
    Write-Output "Pack Package Config File Not Existent: $config_packages"
    New-Item -ItemType File -Force -Path $config_packages
}

Set-Location $ExecutePath
if ($PSScriptRoot -eq $ExecutePath) {
    timeout.exe /T -1
}
