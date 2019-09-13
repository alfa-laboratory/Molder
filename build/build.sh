#!/usr/bin/env bash
set -euo pipefail

SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

NETSTANDARD_SLN="${SCRIPT_ROOT}/../AlfaBank.AFT.Core.Library.sln"

command -v dotnet >/dev/null 2>&1 || {
    echo >&2 "This script requires the dotnet core sdk tooling to be installed"
    exit 1
}

echo "!!WARNING!! This script only builds netstandard and netcoreapp targets"
echo "!!WARNING!! Do not publish nupkgs generated from this script"

dotnet restore -v m "${NETSTANDARD_SLN}"

dotnet build -c Release "${SCRIPT_ROOT}/../src/AlfaBank.AFT.Core/"
dotnet build -c Release "${SCRIPT_ROOT}/../src/AlfaBank.AFT.Core.Library.Common/"
dotnet build -c Release "${SCRIPT_ROOT}/../src/AlfaBank.AFT.Core.Library.Database/"
dotnet build -c Release "${SCRIPT_ROOT}/../src/AlfaBank.AFT.Core.Library.Service/"