@echo off
cd %~dp0

ECHO.
ECHO Preparing environment...

SETLOCAL ENABLEEXTENSIONS
SET CACHED_NUGET=%LocalAppData%\NuGet\NuGet.exe

IF "%APPVEYOR_REPO_BRANCH%" == "release" (
	SET K_BUILD_VERSION=%APPVEYOR_BUILD_NUMBER%
) ELSE (
	SET K_BUILD_VERSION=%APPVEYOR_REPO_BRANCH%-%APPVEYOR_BUILD_NUMBER%
)

IF EXIST %CACHED_NUGET% goto copynuget
ECHO.
ECHO Preparing NuGet...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://www.nuget.org/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "iex ((new-object net.webclient).DownloadString('https://raw.githubusercontent.com/aspnet/Home/release/kvminstall.ps1'))"
CALL %USERPROFILE%\.k\bin\kvm install latest -runtime CLR -x86 -alias default || set errorlevel=1
CALL %USERPROFILE%\.k\bin\kvm install latest -runtime CoreCLR -x86 || set errorlevel=1

:run
CALL %USERPROFILE%\.k\bin\kvm use default -runtime CLR -x86 || set errorlevel=1

ECHO.
ECHO Start actual builds...

CALL kpm restore || set errorlevel=1
CALL kpm build src\SmartDev.ConfigurationMapper || set errorlevel=1
CALL kpm build tests\ConfigurationMapper.UnitTests || set errorlevel=1
CALL kpm build tests\ConfigurationMapper.IntegrationTests || set errorlevel=1

ECHO.
ECHO Start tests...
IF NOT EXIST test-results MD test-results

cd tests\ConfigurationMapper.UnitTests
CALL k test -xml ..\..\test-results\ConfigurationMapper.UnitTests.xml || set errorlevel=1
cd ..\..

cd tests\ConfigurationMapper.IntegrationTests
CALL k test -xml ..\..\test-results\ConfigurationMapper.IntegrationTests.xml || set errorlevel=1
cd ..\..

ECHO.
ECHO Start packaging...

IF NOT EXIST package-results MD package-results
CALL kpm pack src\SmartDev.ConfigurationMapper --out package-results --configuration release || set errorlevel=1

exit /b %errorlevel%