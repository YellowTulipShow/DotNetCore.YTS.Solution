#!/bin/bash
echo "正在发布项目中，请勿关闭本窗口."

# program_list[1]="Test.ConsoleProgram"
# program_list[2]="YTS.AdminWeb"
# program_list[3]="YTS.AdminWebApi"
# program_list[4]="YTS.IOFile.API"
program_list[1]="YTS.IOFile.API"

os_list[1]="win-x64"
# os_list[2]="linux-x64"

for program in ${program_list[*]}
do
	for os in ${os_list[*]}
	do
		dotnet publish ./$program/$program.csproj -o "./_release_$os/$program" --runtime "$os"
	done
done
