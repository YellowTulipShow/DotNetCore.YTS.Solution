@echo off

chcp 65001

set RunPath=%cd%

echo 正在发布项目中，请勿关闭本窗口.

dotnet publish ./Test.ConsoleProgram/Test.ConsoleProgram.csproj -o "./_release/Test.ConsoleProgram/"
dotnet publish ./YTS.AdminWeb/YTS.AdminWeb.csproj -o "./_release/YTS.AdminWeb/"
dotnet publish ./YTS.AdminWebApi/YTS.AdminWebApi.csproj -o "./_release/YTS.AdminWebApi/"

TIMEOUT /T -1
