# Quick Start: Docker + Local SQL Server

## Prerequisites Checklist
- [ ] Docker Desktop installed and running
- [ ] SQL Server running locally
- [ ] Visual Studio 2019+

## 5-Minute Setup

### Step 1: Configure SQL Server (2 minutes)

Run the automated setup script (as Administrator):

```powershell
cd E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe
.\setup-sql-for-docker.ps1
```

This script will:
- ✓ Create HHRCPA database
- ✓ Create qwe_user login
- ✓ Set up tables
- ✓ Configure firewall

### Step 2: Enable TCP/IP (1 minute)

1. Open **SQL Server Configuration Manager**
2. Go to: `SQL Server Network Configuration` → `Protocols for MSSQLSERVER`
3. Right-click **TCP/IP** → **Enable**
4. Right-click **TCP/IP** → **Properties** → **IP Addresses** tab
5. Scroll to **IPALL**, set **TCP Port** = `1433`
6. Restart SQL Server:
   ```powershell
   Restart-Service -Name MSSQLSERVER
   ```

### Step 3: Build Docker Image (1 minute)

```powershell
# Publish the application
cd E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe
dotnet publish qwe\qwe.csproj -c Release -o .\publish

# Build Docker image
docker build -t hhrcpa-web:latest .
```

### Step 4: Run the Application (1 minute)

```powershell
# Start the container
docker-compose up -d

# Check logs
docker-compose logs -f web

# Open in browser
start http://localhost:8080
```

## Verify Everything Works

### Test SQL Connection from Docker

```powershell
docker exec -it qwe-web-1 powershell
# Inside container, test connection (if sqlcmd is available)
```

### Test the Website

1. Navigate to http://localhost:8080
2. Try uploading a document
3. Test the chat widget
4. Check services page

### View Database Data

```sql
-- In SQL Server Management Studio
USE HHRCPA;

-- Check documents
SELECT * FROM Documents;

-- Check chat messages
SELECT * FROM ChatMessages;

-- Check services
SELECT * FROM Services;
```

## Troubleshooting

### Issue: Cannot connect to SQL Server

**Solution:**
```powershell
# Verify SQL Server is running
Get-Service MSSQLSERVER

# Test connection
Test-NetConnection -ComputerName localhost -Port 1433

# Check firewall
Get-NetFirewallRule -DisplayName "*SQL*"
```

### Issue: Docker container exits immediately

**Solution:**
```powershell
# Check logs for errors
docker logs qwe-web-1

# Verify published files exist
dir .\publish
```

### Issue: Port 8080 already in use

**Solution:**
```powershell
# Find process using port 8080
netstat -ano | findstr :8080

# Change port in docker-compose.yml to 8081 or other available port
```

## Common Commands

```powershell
# Stop container
docker-compose down

# Rebuild and restart
docker-compose up -d --build

# View logs
docker-compose logs -f web

# Access container shell
docker exec -it qwe-web-1 powershell

# Clean up everything
docker-compose down -v
docker system prune -a
```

## Connection String Reference

**For Docker containers:**
```
Server=host.docker.internal;Database=HHRCPA;User Id=qwe_user;Password=QweUser123!;TrustServerCertificate=True;
```

**For local development (Visual Studio):**
```
Server=(local);Database=HHRCPA;User Id=qwe_user;Password=QweUser123!;TrustServerCertificate=True;
```

## Next Steps

Once running successfully:
- [ ] Set up GitHub Pages for marketing site
- [ ] Configure SharePoint integration
- [ ] Set up QuickBooks API connection
- [ ] Deploy to Azure Container Instances
- [ ] Implement CI/CD pipeline

## Support

Need help? Check:
- `DOCKER_SETUP.md` - Detailed documentation
- `README.md` - Application features
- Docker logs: `docker-compose logs web`
