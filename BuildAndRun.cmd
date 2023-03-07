echo off

IF EXIST ".\bin\" rd .\bin\  /S /Q
md .\bin
IF EXIST ".\src\bin\" rd .\src\bin\  /S /Q

:run
  IF EXIST "%WINDIR%\Microsoft.NET\Framework\v4.0.30319\" GOTO buildForV4
  IF EXIST "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\" GOTO buildMSBuild
  GOTO fails

:fails
  echo "Failed to find msbuild.exe"
  echo "Please install Visual Studio 2019 Build Tools or Net Framework 4.0.30319"
  GOTO exit

:buildForV4
  set msBuildDir=%WINDIR%\Microsoft.NET\Framework\v4.0.30319
  GOTO build

:buildMSBuild
  set msBuildDir=C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin
  GOTO build

:build
    call "%msBuildDir%\msbuild.exe"  Tubes2_DoraTheExplorer.sln /p:Configuration=Release /l:FileLogger,Microsoft.Build.Engine;logfile=Manual_MSBuild_ReleaseVersion_LOG.log
    XCOPY .\src\bin\Release\*.* .\bin\
    start .\bin\Tubes2_DoraTheExplorer.exe
    pause
    GOTO exit
  
:exit
    set msBuildDir=
    exit /b 0
  
:deleteBinInSrc
    rd .\src\bin\  /S /Q
    GOTO run
  
:deleteBinInRoot
    rd .\bin\  /S /Q