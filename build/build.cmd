@call dotnet restore -v m ..\

@if ERRORLEVEL 1 (
echo Error! Restoring dependencies failed.
exit /b 1
) else (
echo Restoring dependencies was successful.
)

@set project=..\src\AlfaBank.AFT.Core\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build AlfaBank.AFT.Core failed.
exit /b 1
)

@set project=..\src\AlfaBank.AFT.Core.Library.Common\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build AlfaBank.AFT.Core.Library.Common failed.
exit /b 1
)

@set project=..\src\AlfaBank.AFT.Core.Library.Database\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build AlfaBank.AFT.Core.Library.Database failed.
exit /b 1
)

@set project=..\src\AlfaBank.AFT.Core.Library.Service\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build AlfaBank.AFT.Core.Library.Service failed.
exit /b 1
)

@set project=..\src\AlfaBank.AFT.Core.Library.Web\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build AlfaBank.AFT.Core.Library.Web failed.
exit /b 1
)