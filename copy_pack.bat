REM copy  /q/a/f/s *.nupkg nupkg

cd /d %~dp0 

for /f %%i in ('dir /b /l /s "*.nupkg" ') do copy "%%i" "./nupkg/"