@echo off

chcp 65001

:: 读取包源 API-KEY 内容
set api_key_file_path=./packages/nuget.org-api-key.txt
set /p api_key=<%api_key_file_path%
if "%api_key%" == "" (
    echo API-Key获取为空, 中断执行!
	echo 请检查 API-KEY 定义存放地址内容: %api_key_file_path%
	echo 本地地址: %cd%
	TIMEOUT /T -1
	exit
)
echo 获取API-Key: %api_key%

echo 打包项目目标:

dotnet build

:: 定义包源地址
set nuget_source_url=https://api.nuget.org/v3/index.json
echo 定义包源地址: %nuget_source_url%

echo 循环查找 *.nupkg 结尾文件进行发布:
for /R ./packages %%i in (*.nupkg) do (
    echo %%i
    dotnet nuget push %%i --api-key %api_key% --source %nuget_source_url% --skip-duplicate
    del /S %%i
    )

TIMEOUT /T -1
