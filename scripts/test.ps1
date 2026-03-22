param(
    [string]$filter = "",
    [string]$verbosity = "normal"
)

$ErrorActionPreference = "Stop"

Write-Host "Running tests..." -ForegroundColor Cyan

$testArgs = @(
    "test",
    "./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj",
    "--verbosity", $verbosity
)

if ($filter) {
    $testArgs += "--filter"
    $testArgs += $filter
}

& dotnet @testArgs
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "All tests passed." -ForegroundColor Green
