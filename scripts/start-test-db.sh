#!/bin/bash
# Quick start script for Vulcan database tests

set -e

echo "🚀 Starting Vulcan Database Test Environment..."

# Check if Docker is running
if ! docker info > /dev/null 2>&1; then
    echo "❌ Docker is not running. Please start Docker first."
    exit 1
fi

# Start database containers
echo "📦 Starting database containers..."
docker-compose -f docker-compose.test.yml up -d

# Wait for databases to be ready
echo "⏳ Waiting for databases to be ready..."
sleep 10

# Check MSSQL health
echo "🔍 Checking MSSQL health..."
until docker exec vulcan-mssql-test /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -Q "SELECT 1" > /dev/null 2>&1; do
    echo "   Waiting for MSSQL..."
    sleep 5
done
echo "✅ MSSQL is ready!"

# Check MySQL health
echo "🔍 Checking MySQL health..."
until docker exec vulcan-mysql-test mysqladmin ping -h localhost -u root -prootpassword > /dev/null 2>&1; do
    echo "   Waiting for MySQL..."
    sleep 5
done
echo "✅ MySQL is ready!"

# Create test database for MSSQL
echo "🗄️ Creating test database..."
docker exec vulcan-mssql-test /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "YourStrong!Passw0rd" -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TestDb') CREATE DATABASE TestDb;"

echo ""
echo "🎉 Database test environment is ready!"
echo ""
echo "📋 Connection Information:"
echo "   MSSQL: Server=localhost,1433;Database=TestDb;User Id=sa;Password=YourStrong!Passw0rd;"
echo "   MySQL: Server=localhost,3306;Database=testdb;User Id=develop;Password=M3YvLTd8iUni;"
echo ""
echo "🔧 To run tests:"
echo "   # For MSSQL (default):"
echo "   dotnet test ./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj"
echo ""
echo "   # For MySQL:"
echo "   dotnet test ./tests/Vulcan.DapperExtensionsUnitTests/Vulcan.DapperExtensionsUnitTests.csproj -p:DefineConstants=MySQLDebug"
echo ""
echo "🛑 To stop:"
echo "   docker-compose -f docker-compose.test.yml down"
