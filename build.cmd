@echo off
cd %~dp0

SETLOCAL ENABLEEXTENSIONS
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
IF EXIST packages\KoreBuild goto run
.nuget\NuGet.exe install KoreBuild -ExcludeVersion -o packages -nocache -pre
.nuget\NuGet.exe install Sake -version 0.2 -o packages -ExcludeVersion 

@powershell -NoProfile -ExecutionPolicy unrestricted -Command "iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/release/kvminstall.ps1'))"
CALL %USERPROFILE%\.k\bin\kvm install latest -runtime CLR -x86 -alias default || set errorlevel=1
CALL %USERPROFILE%\.k\bin\kvm install latest -runtime CoreCLR -x86 || set errorlevel=1

:run
CALL %USERPROFILE%\.k\bin\kvm use default -runtime CLR -x86 || set errorlevel=1

ECHO "Start actual builds"

CALL kpm restore || set errorlevel=1
CALL kpm build src\SmartDev.ConfigurationMapper || set errorlevel=1
CALL kpm build tests\ConfigurationMapper.UnitTests || set errorlevel=1
CALL kpm build tests\ConfigurationMapper.IntegrationTests || set errorlevel=1

ECHO "Start tests"
IF NOT EXIST test-results MD test-results

cd tests\ConfigurationMapper.UnitTests
CALL k test -xml ..\..\test-results\ConfigurationMapper.UnitTests.xml || set errorlevel=1
cd ..\..

cd tests\ConfigurationMapper.IntegrationTests
CALL k test -xml ..\..\test-results\ConfigurationMapper.IntegrationTests.xml || set errorlevel=1
cd ..\..

ECHO "Start packaging"

IF NOT EXIST package-results MD package-results
CALL kpm pack src\SmartDev.ConfigurationMapper --out package-results --configuration release || set errorlevel=1

exit /b %errorlevel%