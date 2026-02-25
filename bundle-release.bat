@echo off
color b

set outFile=%1
set releaseDir=%2

"%ProgramFiles%\7-Zip\7z.exe" a -t7z "%outFile%" "%releaseDir%\*"

echo Release bundled!