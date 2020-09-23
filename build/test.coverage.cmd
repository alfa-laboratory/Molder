@call dotnet test -c Release /p:CollectCoverage=true /p:Exclude="[xunit.*]*" /p:ExcludeByAttribute="ExcludeFromCodeCoverage" /p:CoverletOutputFormat=OpenCover /p:CoverletOutput=./Coverage.xml ..\
@if ERRORLEVEL 1 (
echo Error! Tests for Server failed.
exit /b 1
)