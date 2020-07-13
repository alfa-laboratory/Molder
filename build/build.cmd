@call dotnet restore -v m ..\

@if ERRORLEVEL 1 (
echo Error! Restoring dependencies failed.
exit /b 1
) else (
echo Restoring dependencies was successful.
)

@set project=..\src\EvidentInstruction\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build EvidentInstruction failed.
exit /b 1
)

@set project=..\src\EvidentInstruction.Generator\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build EvidentInstruction.Generator failed.
exit /b 1
)

@set project=..\src\EvidentInstruction.Database\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build EvidentInstruction.Database failed.
exit /b 1
)

@set project=..\src\EvidentInstruction.Service\

@call dotnet build -c Release %project%

@if ERRORLEVEL 1 (
echo Error! Build EvidentInstruction.Service failed.
exit /b 1
)