param(
    [string]$configuration = "Release",
    [switch]$pack
)

$ErrorActionPreference = "Stop"

Write-Host "Building Vulcan.DapperExtensions ($configuration)..." -ForegroundColor Cyan

dotnet restore ./Vulcan.sln
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c $configuration
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

if ($pack) {
    Write-Host "Creating NuGet package..." -ForegroundColor Cyan
    dotnet pack ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c $configuration -o ./artifacts
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    Write-Host "Package created in ./artifacts" -ForegroundColor Green
}

Write-Host "Build completed successfully." -ForegroundColor Green
