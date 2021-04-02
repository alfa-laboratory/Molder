@call dotnet restore -v m ..\

@if ERRORLEVEL 1 (
echo Error! Restoring dependencies failed.
exit /b 1
) else (
echo Restoring dependencies was successful.
)

@set project=..\src\Molder\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Molder failed.
exit /b 1
)

@set project=..\src\Molder.Generator\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Molder.Generator failed.
exit /b 1
)

@set project=..\src\Molder.Database\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Molder.Database failed.
exit /b 1
)

@set project=..\src\Molder.Service\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Molder.Service failed.
exit /b 1
)

@set project=..\src\Molder.Web\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Molder.Web failed.
exit /b 1
)

@set project=..\src\Molder.Configuration\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Molder.Configuration failed.
exit /b 1
)

@set project=..\src\Molder.ReportPortal\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build Molder.ReportPortal failed.
exit /b 1
)

