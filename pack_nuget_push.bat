@echo off

chcp 65001

echo 打包项目目标:

dotnet build

set nuget_source_url=https://api.nuget.org/v3/index.json

set api_key_file_path=./packages/nuget.org-api-key.txt
set /p api_key=<%api_key_file_path%
echo 获取API-Key: %api_key%

for /R ./packages %%i in (*.nupkg) do (
    echo %%i
    dotnet nuget push %%i --api-key %api_key% --source %nuget_source_url% --skip-duplicate
    del /S %%i
    )

TIMEOUT /T -1
