param(
    [string]$project = "tests/Vulcan.DapperExtensionsUnitTests"
)

$ErrorActionPreference = "Stop"

Write-Host "Running project: $project" -ForegroundColor Cyan

dotnet run --project $project
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
