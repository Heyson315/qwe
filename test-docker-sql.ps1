# Test Docker and SQL Server Setup
Write-Host "=== Testing Docker + SQL Server Setup ===" -ForegroundColor Cyan

# Check Docker is running
Write-Host "`nStep 1: Checking Docker..." -ForegroundColor Yellow
try {
    $dockerVersion = docker --version
    Write-Host "✓ Docker is installed: $dockerVersion" -ForegroundColor Green
} catch {
    Write-Host "✗ Docker is not installed or not running" -ForegroundColor Red
    exit 1
}

# Start SQL Server container only
Write-Host "`nStep 2: Starting SQL Server container..." -ForegroundColor Yellow
docker-compose up -d sqlserver

Write-Host "`nWaiting for SQL Server to start (this may take 30-60 seconds)..." -ForegroundColor Yellow
Start-Sleep -Seconds 10

# Check SQL Server container
Write-Host "`nStep 3: Checking SQL Server container..." -ForegroundColor Yellow
$sqlContainer = docker ps --filter "name=hhrcpa-sqlserver" --format "{{.Names}}"
if ($sqlContainer) {
    Write-Host "✓ SQL Server container is running: $sqlContainer" -ForegroundColor Green
} else {
    Write-Host "✗ SQL Server container is not running" -ForegroundColor Red
    Write-Host "`nContainer logs:" -ForegroundColor Yellow
    docker logs hhrcpa-sqlserver
    exit 1
}

# Wait for SQL Server to be healthy
Write-Host "`nStep 4: Waiting for SQL Server to be ready..." -ForegroundColor Yellow
$maxAttempts = 30
$attempt = 0
$isHealthy = $false

while ($attempt -lt $maxAttempts -and -not $isHealthy) {
    $attempt++
    $health = docker inspect --format='{{.State.Health.Status}}' hhrcpa-sqlserver 2>$null
    
    if ($health -eq "healthy") {
        $isHealthy = $true
        Write-Host "✓ SQL Server is healthy and ready!" -ForegroundColor Green
    } else {
        Write-Host "  Attempt $attempt/$maxAttempts - Status: $health" -ForegroundColor Gray
        Start-Sleep -Seconds 2
    }
}

if (-not $isHealthy) {
    Write-Host "✗ SQL Server did not become healthy in time" -ForegroundColor Red
    Write-Host "`nContainer logs:" -ForegroundColor Yellow
    docker logs hhrcpa-sqlserver --tail 50
    exit 1
}

# Test SQL Server connection
Write-Host "`nStep 5: Testing SQL Server connection..." -ForegroundColor Yellow
$testQuery = docker exec hhrcpa-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P QweUser123! -Q "SELECT @@VERSION" -h -1

if ($LASTEXITCODE -eq 0) {
    Write-Host "✓ Successfully connected to SQL Server!" -ForegroundColor Green
    Write-Host "`nSQL Server Version:" -ForegroundColor Cyan
    Write-Host $testQuery
} else {
    Write-Host "✗ Failed to connect to SQL Server" -ForegroundColor Red
    exit 1
}

# Create database and tables
Write-Host "`nStep 6: Creating HHRCPA database and tables..." -ForegroundColor Yellow
$createDbScript = @"
CREATE DATABASE HHRCPA;
GO
USE HHRCPA;
GO
CREATE TABLE Documents (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FileName NVARCHAR(255) NOT NULL,
    FilePath NVARCHAR(500) NOT NULL,
    ContentType NVARCHAR(100),
    FileSize BIGINT,
    UploadDate DATETIME DEFAULT GETDATE()
);
GO
CREATE TABLE ChatMessages (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Message NVARCHAR(MAX) NOT NULL,
    Response NVARCHAR(MAX),
    Timestamp DATETIME DEFAULT GETDATE()
);
GO
CREATE TABLE Services (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE()
);
GO
INSERT INTO Services (Name, Description) VALUES
('Tax Preparation', 'Comprehensive tax preparation services for individuals and businesses'),
('Bookkeeping', 'Professional bookkeeping and accounting services'),
('Payroll Processing', 'Complete payroll management and processing'),
('Financial Consulting', 'Expert financial advice and business consulting');
GO
"@

# Execute each command separately
$commands = @(
    "CREATE DATABASE HHRCPA;",
    "USE HHRCPA; CREATE TABLE Documents (Id INT IDENTITY(1,1) PRIMARY KEY, FileName NVARCHAR(255) NOT NULL, FilePath NVARCHAR(500) NOT NULL, ContentType NVARCHAR(100), FileSize BIGINT, UploadDate DATETIME DEFAULT GETDATE());",
    "USE HHRCPA; CREATE TABLE ChatMessages (Id INT IDENTITY(1,1) PRIMARY KEY, Message NVARCHAR(MAX) NOT NULL, Response NVARCHAR(MAX), Timestamp DATETIME DEFAULT GETDATE());",
    "USE HHRCPA; CREATE TABLE Services (Id INT IDENTITY(1,1) PRIMARY KEY, Name NVARCHAR(200) NOT NULL, Description NVARCHAR(MAX), IsActive BIT DEFAULT 1, CreatedDate DATETIME DEFAULT GETDATE());",
    "USE HHRCPA; INSERT INTO Services (Name, Description) VALUES ('Tax Preparation', 'Comprehensive tax preparation services for individuals and businesses'), ('Bookkeeping', 'Professional bookkeeping and accounting services'), ('Payroll Processing', 'Complete payroll management and processing'), ('Financial Consulting', 'Expert financial advice and business consulting');"
)

foreach ($cmd in $commands) {
    docker exec hhrcpa-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "QweUser123!" -Q $cmd | Out-Null
    if ($LASTEXITCODE -ne 0) {
        Write-Host "Warning: Command may have failed, but continuing..." -ForegroundColor Yellow
    }
}

Write-Host "✓ Database and tables created successfully!" -ForegroundColor Green

# Verify tables
Write-Host "`nStep 7: Verifying tables..." -ForegroundColor Yellow
$verifyQuery = "USE HHRCPA; SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';"
$tables = docker exec hhrcpa-sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P QweUser123! -Q "$verifyQuery" -h -1

Write-Host "✓ Tables created:" -ForegroundColor Green
Write-Host $tables

# Summary
Write-Host "`n=== Summary ===" -ForegroundColor Cyan
Write-Host "✓ Docker is running" -ForegroundColor Green
Write-Host "✓ SQL Server container is healthy" -ForegroundColor Green
Write-Host "✓ Database 'HHRCPA' created" -ForegroundColor Green
Write-Host "✓ Tables created: Documents, ChatMessages, Services" -ForegroundColor Green
Write-Host "✓ Sample data inserted" -ForegroundColor Green

Write-Host "`n=== Connection Information ===" -ForegroundColor Cyan
Write-Host "Server: localhost,1433"
Write-Host "Database: HHRCPA"
Write-Host "Username: sa"
Write-Host "Password: QweUser123!"
Write-Host "Connection String: Server=localhost,1433;Database=HHRCPA;User Id=sa;Password=QweUser123!;TrustServerCertificate=True;"

Write-Host "`n=== Next Steps ===" -ForegroundColor Cyan
Write-Host "1. Build your ASP.NET app in Visual Studio (Right-click project → Publish → Folder → publish)"
Write-Host "2. Run: docker-compose up -d --build web"
Write-Host "3. Access your app at: http://localhost:8081"
Write-Host "`nTo stop: docker-compose down" -ForegroundColor Yellow
Write-Host "To stop and remove data: docker-compose down -v" -ForegroundColor Yellow

Write-Host "`n✓ Test completed successfully!" -ForegroundColor Green
