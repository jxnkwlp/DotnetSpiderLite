rem clear old nuget packages 
cd /d %~dp0 
del /q/a/f/s *.nupkg

rem build
dotnet build DotnetSpiderLite.sln -c Release
 
