# AGENTS.md - Coding Agent Guidelines for Vulcan

This document provides essential guidelines for AI coding agents working on the Vulcan repository.

## Build & Test Commands

### Build
```bash
# Build the main library (Release configuration)
dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release

# Build the entire solution
dotnet build ./Vulcan.sln -c Release

# Build with MySQL debug symbols
dotnet build ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c MySQLDebug
```

### Test
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

### Package
```bash
# Create NuGet package
dotnet pack ./src/Vulcan.DapperExtensions/Vulcan.DapperExtensions.csproj -c Release -o ./artifacts

# Or use the provided script
./scripts/build_package.sh
```

## Code Style

### Formatting
- **Indentation**: 4 spaces (no tabs)
- **Line endings**: LF (Unix-style)
- **Encoding**: UTF-8 with BOM
- **Final newline**: Yes
- **Trailing whitespace**: Remove

### C# Style Rules
- Use explicit object creation: `new ClassName()` instead of `new()` when type is not apparent
- Follow standard .NET naming conventions

### Naming Conventions
- **Classes/Structs**: PascalCase (e.g., `ConnectionManager`, `BaseEntity`)
- **Interfaces**: PascalCase with 'I' prefix (e.g., `IConnectionFactory`, `IRepository`)
- **Methods**: PascalCase (e.g., `CreateDbConnection`, `QueryUserList`)
- **Properties**: PascalCase (e.g., `ConnectionString`, `UserName`)
- **Fields**: camelCase with underscore prefix for private fields (e.g., `_connectionString`, `_logger`)
- **Parameters**: camelCase (e.g., `connectionString`, `userId`)
- **Local variables**: camelCase (e.g., `userList`, `sqlQuery`)
- **Constants**: PascalCase (e.g., `MaxRetryCount`, `DefaultTimeout`)

### Import Organization
```csharp
// 1. System namespaces
using System;
using System.Collections.Generic;
using System.Data;

// 2. Third-party libraries
using Dapper;
using Microsoft.Extensions.Logging;

// 3. Project namespaces
using Vulcan.DapperExtensions.Contract;
using Vulcan.DapperExtensions.ORMapping;
```

## Project Structure

### Main Projects
- `src/Vulcan.DapperExtensions/` - Core library (netstandard2.0)
- `src/Vulcan.DapperExtensions.Npgsql/` - PostgreSQL support (if exists)

### Test Projects
- `tests/Vulcan.DapperExtensionsUnitTests/` - Unit tests (net8.0, xUnit)

### Configuration Files
- `.editorconfig` - Code style settings
- `build/version.props` - Version information
- `build/releasenotes.props` - Release notes
- `scripts/` - Build and publish scripts

## Testing Guidelines

### Framework
- **Testing framework**: xUnit 2.4.1
- **Mocking**: Moq 4.16.1 with Moq.AutoMock 3.0.0
- **Test data**: AutoFixture 4.17.0
- **Coverage**: coverlet.collector 3.1.0

### Test Naming
```csharp
[Fact]
public void MethodName_Scenario_ExpectedResult()
{
    // Arrange
    // Act  
    // Assert
}

[Theory]
[InlineData(...)]
public void MethodName_WithVariousInputs_WorksCorrectly(...)
{
    // Test implementation
}
```

### Test Organization
- Place test classes in same namespace as class under test with `.Tests` suffix
- Use test fixtures for shared setup
- Keep tests independent and isolated

## Error Handling

### Exceptions
- Use standard .NET exception types when possible
- Create custom exceptions only for domain-specific errors
- Always include meaningful error messages
- Use exception filters when appropriate

### Logging
- Use `Microsoft.Extensions.Logging.ILogger<T>`
- Log at appropriate levels (Debug, Information, Warning, Error)
- Include relevant context in log messages

## Database Conventions

### Connection Management
- Always use `IConnectionManagerFactory` to create connections
- Connections are automatically managed and disposed
- Use transactions via `BeginTransScope()` for atomic operations

### SQL Practices
- Use parameterized queries to prevent SQL injection
- Keep SQL in string constants for complex queries
- Use ORMapping attributes for entity configuration

## Dependencies

### Core Libraries
- **Dapper** 1.50.5 - SQL mapping
- **Microsoft.Extensions.Logging** 2.0.0 - Logging abstraction
- **Microsoft.CSharp** 4.5.0 - Dynamic support

### Database Providers
- **MySql.Data** 8.0.26 - MySQL support
- (Npgsql for PostgreSQL if used)

## Configuration

### Build Configurations
- **Debug**: Development with full symbols
- **Release**: Optimized for production
- **MySQLDebug**: Debug with MySQL-specific defines

### Target Frameworks
- Library: netstandard2.0
- Tests: net8.0

## Important Notes

1. **Do not commit** sensitive data (connection strings, passwords, API keys)
2. **Always run tests** before submitting changes
3. **Follow existing patterns** in the codebase
4. **Keep changes focused** - one logical change per commit
5. **Update documentation** if adding new features

## Quick Reference

```bash
# Full workflow for changes:
dotnet restore ./Vulcan.sln
dotnet build ./Vulcan.sln -c Release
dotnet test ./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj

# Check for style issues (if using dotnet format):
dotnet format --verify-no-changes --verbosity diagnostic
```

This document should be updated when adding new projects, changing build processes, or adopting new coding standards.