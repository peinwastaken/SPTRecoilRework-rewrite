@echo off
setlocal

set "rootDir=%~dp0"
set "releaseDir=%rootDir%Release"
set "outFile=%rootDir%RecoilRework.7z"
set "sevenZip=%ProgramFiles%\7-Zip\7z.exe"

if not exist "%sevenZip%" set "sevenZip=%ProgramFiles(x86)%\7-Zip\7z.exe"

if not exist "%sevenZip%" (
    echo 7-Zip was not found. Install it or update the sevenZip path in this script.
    exit /b 1
)

if exist "%releaseDir%" rmdir /s /q "%releaseDir%"
if exist "%outFile%" del /f /q "%outFile%"

dotnet build "%rootDir%RecoilReworkClient\RecoilReworkClient.csproj" --configuration Release --no-restore
if errorlevel 1 goto :failure

dotnet build "%rootDir%RecoilReworkServer\RecoilReworkServer.csproj" --configuration Release --no-restore
if errorlevel 1 goto :failure

"%sevenZip%" a -t7z "%outFile%" "%releaseDir%\*"
if errorlevel 1 goto :failure

echo Release bundled: "%outFile%"
echo Release folder: "%releaseDir%"
exit /b 0

:failure
echo Release build failed.
exit /b 1
