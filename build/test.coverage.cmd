@call dotnet test -c Release /p:CollectCoverage=true /p:Exclude="[xunit.*]*" /p:ExcludeByAttribute="ExcludeFromCodeCoverage" /p:CoverletOutputFormat=lcov /p:CoverletOutput=../../lcov ..\
@if ERRORLEVEL 1 (
echo Error! Tests for Server failed.
exit /b 1
)