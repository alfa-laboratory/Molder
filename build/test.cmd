CHCP 65001
CLS
@call dotnet test -c Release /p:CollectCoverage=true /p:Exclude="[xunit.*]*" /p:Threshold=50 ..\
@if ERRORLEVEL 1 (
echo Error! Tests for Server failed.
exit /b 1
)