rem build
dotnet build DotnetSpiderLite.sln -c Release

rem clear old nuget packages
for %%i in (*.nupkg) do del /q/a/f/s %%i

