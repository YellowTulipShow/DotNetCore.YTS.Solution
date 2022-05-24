program="YTS.IOFile.API"
os="win-x64"
dotnet publish ./$program/$program.csproj -o "./_release_$os/$program" --runtime "$os"
dotnet ./_release_$os/$program/$program.exe
