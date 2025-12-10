# Docker Setup for HHR CPA Website

## Overview
This setup allows you to run the ASP.NET MVC application in a Docker container while connecting to your local SQL Server.

## Prerequisites

1. **Docker Desktop for Windows** - [Download here](https://www.docker.com/products/docker-desktop)
2. **SQL Server** running locally on your machine
3. **Visual Studio 2019+** for building the application

## Architecture

```
┌─────────────────────┐
│  Docker Container   │
│  ┌───────────────┐  │
│  │  ASP.NET MVC  │  │
│  │  Website      │  │
│  │  Port: 80     │  │
│  └───────┬───────┘  │
└──────────┼──────────┘
           │
           │ host.docker.internal
           │
           ▼
┌─────────────────────┐
│  Windows Host       │
│  ┌───────────────┐  │
│  │  SQL Server   │  │
│  │  Port: 1433   │  │
│  └───────────────┘  │
└─────────────────────┘
```

## Setup Steps

### 1. Prepare SQL Server for Docker Access

SQL Server needs to accept TCP/IP connections:

1. Open **SQL Server Configuration Manager**
2. Navigate to: `SQL Server Network Configuration` → `Protocols for [Your Instance]`
3. Enable **TCP/IP**
4. Right-click TCP/IP → Properties → IP Addresses tab
5. Scroll to **IPALL** section
6. Set **TCP Port** to `1433`
7. Restart SQL Server service

### 2. Configure SQL Server Authentication

You can use either Windows Authentication or SQL Server Authentication:

#### Option A: SQL Server Authentication (Recommended for Docker)

```sql
-- Run in SQL Server Management Studio (SSMS)
-- Create a login for the application
CREATE LOGIN qwe_user WITH PASSWORD = 'YourStrongPassword123!';

-- Create database
CREATE DATABASE HHRCPA;
GO

-- Grant permissions
USE HHRCPA;
CREATE USER qwe_user FOR LOGIN qwe_user;
ALTER ROLE db_owner ADD MEMBER qwe_user;
GO
```

#### Option B: Windows Authentication

Use your Windows account credentials in the connection string.

### 3. Update Web.config with Connection String

Edit `qwe/Web.config`:

```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=host.docker.internal;Database=HHRCPA;User Id=qwe_user;Password=YourStrongPassword123!;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**Important:** When running in Docker, use `host.docker.internal` instead of `localhost` or `127.0.0.1`

### 4. Publish the Application

In Visual Studio:

1. Right-click the `qwe` project
2. Select **Publish**
3. Choose **Folder** as target
4. Set location to: `E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe\publish`
5. Click **Publish**

Or use command line:

```powershell
cd E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe
msbuild qwe\qwe.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile
```

### 5. Build Docker Image

```powershell
# Navigate to project root
cd E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe

# Build the Docker image
docker build -t hhrcpa-web:latest .
```

### 6. Run with Docker Compose

```powershell
# Start the application
docker-compose up -d

# View logs
docker-compose logs -f web

# Stop the application
docker-compose down
```

### 7. Access the Application

Open your browser and navigate to:
- **Local:** http://localhost:8080
- **Network:** http://YOUR_MACHINE_IP:8080

## Configuration Options

### Environment Variables

Edit `docker-compose.yml` to customize settings:

```yaml
environment:
  # Database connection
  - ConnectionStrings__DefaultConnection=Server=host.docker.internal;Database=HHRCPA;...
  
  # Application settings
  - ASPNETCORE_ENVIRONMENT=Production
  - TZ=America/New_York
```

### Volumes

Mount local directories to persist data:

```yaml
volumes:
  # Document uploads
  - ./qwe/App_Data:/inetpub/wwwroot/App_Data
  
  # Logs
  - ./logs:/inetpub/wwwroot/logs
```

### Ports

Change the exposed port:

```yaml
ports:
  - "8080:80"  # Change 8080 to your preferred port
```

## Alternative: Run SQL Server in Docker

If you want to run SQL Server in Docker instead of using your local instance:

1. Uncomment the SQL Server section in `docker-compose.yml`
2. Update the connection string to use `sqlserver` as the server name:
   ```
   Server=sqlserver;Database=HHRCPA;...
   ```

3. Start both services:
   ```powershell
   docker-compose up -d
   ```

## Testing Local SQL Server Connection

Test connectivity from Docker to your local SQL Server:

```powershell
# Run a test container
docker run --rm -it mcr.microsoft.com/mssql-tools /bin/bash

# Inside the container, test connection
sqlcmd -S host.docker.internal -U qwe_user -P YourStrongPassword123! -Q "SELECT @@VERSION"
```

## Troubleshooting

### Cannot Connect to SQL Server

1. **Check SQL Server is running:**
   ```powershell
   Get-Service -Name MSSQL*
   ```

2. **Verify TCP/IP is enabled:**
   - SQL Server Configuration Manager → Protocols → TCP/IP = Enabled

3. **Check Windows Firewall:**
   ```powershell
   # Add firewall rule for SQL Server
   New-NetFirewallRule -DisplayName "SQL Server" -Direction Inbound -Protocol TCP -LocalPort 1433 -Action Allow
   ```

4. **Test connection from host:**
   ```powershell
   Test-NetConnection -ComputerName localhost -Port 1433
   ```

### Docker Container Won't Start

```powershell
# Check Docker logs
docker logs qwe-web-1

# Inspect container
docker inspect qwe-web-1

# Check if port is already in use
netstat -ano | findstr :8080
```

### File Upload Permissions

If uploads fail, ensure the container has write permissions:

```powershell
# Set permissions on App_Data folder
icacls "E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe\qwe\App_Data" /grant Everyone:(OI)(CI)F
```

## Development Workflow

### Hot Reload Development

For development with auto-reload:

1. Use Visual Studio's built-in IIS Express
2. Run SQL Server locally or in Docker
3. Use Docker only for testing production builds

### CI/CD Pipeline

Integrate with GitHub Actions:

```yaml
# .github/workflows/docker-build.yml
name: Build and Push Docker Image

on:
  push:
    branches: [main]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v2
      - name: Build Docker image
        run: docker build -t hhrcpa-web:${{ github.sha }} .
```

## Production Deployment

### Option 1: Azure Container Instances

```powershell
# Push to Azure Container Registry
docker tag hhrcpa-web:latest yourregistry.azurecr.io/hhrcpa-web:latest
docker push yourregistry.azurecr.io/hhrcpa-web:latest

# Deploy to ACI
az container create --name hhrcpa-web \
  --resource-group your-rg \
  --image yourregistry.azurecr.io/hhrcpa-web:latest \
  --cpu 2 --memory 4
```

### Option 2: Azure App Service (Recommended)

- Use Azure App Service for Windows
- Connect to Azure SQL Database
- Enable Application Insights for monitoring

### Option 3: On-Premises Docker Host

- Deploy to Windows Server with Docker
- Use reverse proxy (IIS, nginx) for SSL termination
- Connect to local SQL Server cluster

## Security Best Practices

1. **Never commit passwords** - Use environment variables or Azure Key Vault
2. **Enable HTTPS** - Use reverse proxy with SSL certificate
3. **Limit SQL Server access** - Create specific user with minimal permissions
4. **Regular updates** - Keep base Docker images updated
5. **Scan for vulnerabilities** - Use Docker security scanning tools

## Monitoring

### Docker Stats

```powershell
# Monitor resource usage
docker stats qwe-web-1

# View container health
docker inspect --format='{{json .State.Health}}' qwe-web-1
```

### Application Logs

```powershell
# Real-time logs
docker-compose logs -f web

# Last 100 lines
docker-compose logs --tail=100 web
```

## Backup and Restore

### Database Backup

```sql
-- Backup database
BACKUP DATABASE HHRCPA 
TO DISK = 'C:\Backups\HHRCPA.bak'
WITH FORMAT, INIT, COMPRESSION;
```

### Volume Backup

```powershell
# Backup uploaded files
docker run --rm -v qwe_uploads:/source -v C:\Backups:/backup alpine tar czf /backup/uploads-backup.tar.gz -C /source .
```

## Resources

- [Docker Desktop Documentation](https://docs.docker.com/desktop/windows/)
- [ASP.NET Docker Images](https://hub.docker.com/_/microsoft-dotnet-framework-aspnet)
- [SQL Server Docker Images](https://hub.docker.com/_/microsoft-mssql-server)
- [Docker Compose Reference](https://docs.docker.com/compose/compose-file/)

## Support

For issues or questions:
- Check logs: `docker-compose logs web`
- GitHub Issues: https://github.com/Heyson315/qwe/issues
- Docker Community: https://forums.docker.com/
