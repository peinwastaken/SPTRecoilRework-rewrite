@echo off
color b

set /p outFile=Enter output archive name (ModRelease.zip):
set releaseDir=.\Release\

"%ProgramFiles%\7-Zip\7z.exe" a -t7z "%outFile%" ".\Release\*"

if exist %releaseDir% (
    rmdir /s /q %releaseDir%
)

echo Release bundled!
timeout /t 2 > nul