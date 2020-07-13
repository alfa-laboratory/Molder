#!/usr/bin/env bash
set -euo pipefail

SCRIPT_ROOT="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"

NETSTANDARD_SLN="${SCRIPT_ROOT}/../EvidentInstruction.sln"

command -v dotnet >/dev/null 2>&1 || {
    echo >&2 "This script requires the dotnet core sdk tooling to be installed"
    exit 1
}

echo "!!WARNING!! This script only builds netstandard and netcoreapp targets"
echo "!!WARNING!! Do not publish nupkgs generated from this script"

dotnet restore -v m "${NETSTANDARD_SLN}"

dotnet build -c Release "${SCRIPT_ROOT}/../src/EvidentInstruction/"
dotnet build -c Release "${SCRIPT_ROOT}/../src/EvidentInstruction.Generator/"
dotnet build -c Release "${SCRIPT_ROOT}/../src/EvidentInstruction.Database/"
dotnet build -c Release "${SCRIPT_ROOT}/../src/EvidentInstruction.Service/"