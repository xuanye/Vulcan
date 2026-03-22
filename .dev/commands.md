# Standard Commands

> Always use standard CLI commands. Do not use aliases.

## Restore
```bash
dotnet restore ./Vulcan.sln
```

## Build
```bash
# Build main library (Release)
dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release

# Build entire solution
dotnet build ./Vulcan.sln -c Release

# Build with MySQL debug symbols
dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c MySQLDebug
```

## Test
```bash
# Run all unit tests
dotnet test ./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj

# Run tests with detailed output
dotnet test ./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj --verbosity normal

# Run a single test by fully qualified name
dotnet test ./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj --filter "FullyQualifiedName~Namespace.ClassName.MethodName"

# Run tests matching a pattern
dotnet test ./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj --filter "ClassName~TestClass"
```

## Pack (NuGet)
```bash
dotnet pack ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release -o ./artifacts
```

## Clean
```bash
dotnet clean ./Vulcan.sln
```

## Format
```bash
dotnet format ./Vulcan.sln
```
