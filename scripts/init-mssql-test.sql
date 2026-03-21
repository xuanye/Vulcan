-- SQL Server Test Database Initialization Script
-- Run this after starting the MSSQL container

-- Create test database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'TestDb')
BEGIN
    CREATE DATABASE TestDb;
END
GO

USE TestDb;
GO

-- The test tables will be created automatically by the test framework
-- But you can verify the database exists with:
-- SELECT name FROM sys.databases WHERE name = 'TestDb';
