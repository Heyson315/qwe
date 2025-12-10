# PowerShell script to set up SQL Server for Docker connectivity

Write-Host "=== SQL Server Docker Setup Script ===" -ForegroundColor Cyan

# Check if SQL Server is running
$sqlService = Get-Service -Name "MSSQL*" -ErrorAction SilentlyContinue
if ($sqlService.Status -ne "Running") {
    Write-Host "ERROR: SQL Server is not running!" -ForegroundColor Red
    Write-Host "Please start SQL Server and run this script again." -ForegroundColor Yellow
    exit 1
}

Write-Host "✓ SQL Server is running" -ForegroundColor Green

# Test SQL Server connectivity
$serverInstance = "(local)"
Write-Host "`nTesting SQL Server connectivity..." -ForegroundColor Cyan

try {
    $connection = New-Object System.Data.SqlClient.SqlConnection
    $connection.ConnectionString = "Server=$serverInstance;Integrated Security=True;TrustServerCertificate=True;"
    $connection.Open()
    Write-Host "✓ Successfully connected to SQL Server" -ForegroundColor Green
    $connection.Close()
} catch {
    Write-Host "ERROR: Cannot connect to SQL Server" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Yellow
    exit 1
}

# Create database and user
Write-Host "`nCreating database and user..." -ForegroundColor Cyan

$sqlScript = @"
-- Create database if not exists
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'HHRCPA')
BEGIN
    CREATE DATABASE HHRCPA;
    PRINT 'Database HHRCPA created';
END
ELSE
BEGIN
    PRINT 'Database HHRCPA already exists';
END
GO

USE HHRCPA;
GO

-- Create login if not exists
IF NOT EXISTS (SELECT name FROM sys.server_principals WHERE name = 'qwe_user')
BEGIN
    CREATE LOGIN qwe_user WITH PASSWORD = 'QweUser123!', CHECK_POLICY = OFF;
    PRINT 'Login qwe_user created';
END
ELSE
BEGIN
    PRINT 'Login qwe_user already exists';
END
GO

-- Create user in database
IF NOT EXISTS (SELECT name FROM sys.database_principals WHERE name = 'qwe_user')
BEGIN
    CREATE USER qwe_user FOR LOGIN qwe_user;
    PRINT 'User qwe_user created in database';
END
ELSE
BEGIN
    PRINT 'User qwe_user already exists in database';
END
GO

-- Grant permissions
ALTER ROLE db_owner ADD MEMBER qwe_user;
PRINT 'Permissions granted to qwe_user';
GO

-- Create sample tables
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Documents')
BEGIN
    CREATE TABLE Documents (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        FileName NVARCHAR(255) NOT NULL,
        FilePath NVARCHAR(500) NOT NULL,
        ContentType NVARCHAR(100),
        FileSize BIGINT,
        UploadDate DATETIME DEFAULT GETDATE()
    );
    PRINT 'Documents table created';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ChatMessages')
BEGIN
    CREATE TABLE ChatMessages (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Message NVARCHAR(MAX) NOT NULL,
        Response NVARCHAR(MAX),
        Timestamp DATETIME DEFAULT GETDATE()
    );
    PRINT 'ChatMessages table created';
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Services')
BEGIN
    CREATE TABLE Services (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Name NVARCHAR(200) NOT NULL,
        Description NVARCHAR(MAX),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE()
    );
    
    -- Insert sample services
    INSERT INTO Services (Name, Description) VALUES
    ('Tax Preparation', 'Comprehensive tax preparation services for individuals and businesses'),
    ('Bookkeeping', 'Professional bookkeeping and accounting services'),
    ('Payroll Processing', 'Complete payroll management and processing'),
    ('Financial Consulting', 'Expert financial advice and business consulting');
    
    PRINT 'Services table created and populated';
END
GO
"@

try {
    Invoke-Sqlcmd -ServerInstance $serverInstance -Query $sqlScript -TrustServerCertificate
    Write-Host "✓ Database setup completed successfully" -ForegroundColor Green
} catch {
    Write-Host "ERROR: Failed to create database/user" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Yellow
    exit 1
}

# Enable TCP/IP (requires admin rights)
Write-Host "`nChecking TCP/IP configuration..." -ForegroundColor Cyan
Write-Host "NOTE: You need to manually enable TCP/IP in SQL Server Configuration Manager:" -ForegroundColor Yellow
Write-Host "1. Open SQL Server Configuration Manager" -ForegroundColor White
Write-Host "2. Navigate to SQL Server Network Configuration > Protocols" -ForegroundColor White
Write-Host "3. Enable TCP/IP" -ForegroundColor White
Write-Host "4. Set TCP Port to 1433 in TCP/IP Properties > IP Addresses > IPALL" -ForegroundColor White
Write-Host "5. Restart SQL Server service" -ForegroundColor White

# Check if port 1433 is listening
$portCheck = Test-NetConnection -ComputerName localhost -Port 1433 -WarningAction SilentlyContinue
if ($portCheck.TcpTestSucceeded) {
    Write-Host "✓ Port 1433 is open and listening" -ForegroundColor Green
} else {
    Write-Host "⚠ Port 1433 is not accessible" -ForegroundColor Yellow
    Write-Host "  This is normal if TCP/IP is not enabled yet" -ForegroundColor Gray
}

# Add firewall rule
Write-Host "`nConfiguring Windows Firewall..." -ForegroundColor Cyan
try {
    $existingRule = Get-NetFirewallRule -DisplayName "SQL Server (Docker)" -ErrorAction SilentlyContinue
    if ($existingRule) {
        Write-Host "✓ Firewall rule already exists" -ForegroundColor Green
    } else {
        New-NetFirewallRule -DisplayName "SQL Server (Docker)" `
            -Direction Inbound `
            -Protocol TCP `
            -LocalPort 1433 `
            -Action Allow `
            -Profile Any | Out-Null
        Write-Host "✓ Firewall rule created" -ForegroundColor Green
    }
} catch {
    Write-Host "⚠ Could not create firewall rule (may require admin rights)" -ForegroundColor Yellow
    Write-Host "  Run this command as Administrator: New-NetFirewallRule -DisplayName 'SQL Server' -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow" -ForegroundColor Gray
}

# Display connection string
Write-Host "`n=== Connection String ===" -ForegroundColor Cyan
Write-Host "For Docker containers, use this connection string:" -ForegroundColor White
Write-Host "Server=host.docker.internal;Database=HHRCPA;User Id=qwe_user;Password=QweUser123!;TrustServerCertificate=True;" -ForegroundColor Yellow

Write-Host "`n=== Next Steps ===" -ForegroundColor Cyan
Write-Host "1. Enable TCP/IP in SQL Server Configuration Manager (see above)" -ForegroundColor White
Write-Host "2. Update Web.config with the connection string" -ForegroundColor White
Write-Host "3. Build your application: docker build -t hhrcpa-web ." -ForegroundColor White
Write-Host "4. Run with Docker Compose: docker-compose up -d" -ForegroundColor White

Write-Host "`n✓ Setup complete!" -ForegroundColor Green
