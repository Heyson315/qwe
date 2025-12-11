# Complete Setup Guide: HHR CPA Platform

## 🎯 Project Overview

This guide walks you through setting up a complete web presence for your HHR CPA application with:

1. **Docker containerization** connecting to local SQL Server
2. **GitHub Pages** for public marketing website
3. **SharePoint integration** for team collaboration and project management
4. **QuickBooks API** ready integration (future enhancement)

## 📋 Architecture Diagram

```
┌─────────────────────────────────────────────────────────┐
│                    PUBLIC LAYER                          │
│  ┌────────────────────────────────────────────────┐     │
│  │  GitHub Pages (Marketing Site)                  │     │
│  │  URL: heyson315.github.io/qwe                  │     │
│  │  - Features showcase                            │     │
│  │  - Service information                          │     │
│  │  - Contact forms                                │     │
│  └──────────────────┬─────────────────────────────┘     │
└────────────────────┼──────────────────────────────────┘
                     │
                     │ (Call-to-Action)
                     ▼
┌─────────────────────────────────────────────────────────┐
│                  APPLICATION LAYER                       │
│  ┌────────────────────────────────────────────────┐     │
│  │  Docker Container (ASP.NET MVC)                 │     │
│  │  Port: 8080                                     │     │
│  │  - Document management                          │     │
│  │  - AI Chatbot                                   │     │
│  │  - Services API                                 │     │
│  └──────────────────┬─────────────────────────────┘     │
└────────────────────┼──────────────────────────────────┘
                     │
                     │ (host.docker.internal)
                     ▼
┌─────────────────────────────────────────────────────────┐
│                    DATA LAYER                            │
│  ┌────────────────────────────────────────────────┐     │
│  │  Local SQL Server                               │     │
│  │  Port: 1433                                     │     │
│  │  Database: HHRCPA                               │     │
│  │  - Documents table                              │     │
│  │  - ChatMessages table                           │     │
│  │  - Services table                               │     │
│  └─────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│              COLLABORATION LAYER                         │
│  ┌────────────────────────────────────────────────┐     │
│  │  SharePoint (Project Management)                │     │
│  │  - Documentation                                │     │
│  │  - Task tracking                                │     │
│  │  - Team collaboration                           │     │
│  └─────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│             INTEGRATION LAYER (Future)                   │
│  ┌────────────────────────────────────────────────┐     │
│  │  QuickBooks API                                 │     │
│  │  - Financial data sync                          │     │
│  │  - Invoice management                           │     │
│  │  - Payment processing                           │     │
│  └─────────────────────────────────────────────────┘     │
└─────────────────────────────────────────────────────────┘
```

## 🚀 Quick Start (30 Minutes Total)

### Phase 1: SQL Server Setup (10 minutes)

```powershell
# Navigate to project directory
cd E:\source\Heyson315\DjangoWebProject1\Heyson315\qwe

# Run automated SQL setup script (as Administrator)
.\setup-sql-for-docker.ps1

# Enable TCP/IP manually (required):
# 1. Open SQL Server Configuration Manager
# 2. SQL Server Network Configuration → Protocols → Enable TCP/IP
# 3. TCP/IP Properties → IP Addresses → IPALL → TCP Port = 1433
# 4. Restart SQL Server:
Restart-Service MSSQLSERVER
```

### Phase 2: Docker Container (10 minutes)

```powershell
# Build the application
dotnet publish qwe\qwe.csproj -c Release -o .\publish

# Build Docker image
docker build -t hhrcpa-web:latest .

# Start with Docker Compose
docker-compose up -d

# Verify it's running
docker ps
docker-compose logs -f web

# Test the application
start http://localhost:8080
```

### Phase 3: GitHub Pages (10 minutes)

```powershell
# Commit the new docs folder
git add docs/
git add Dockerfile docker-compose.yml DOCKER_SETUP.md DOCKER_QUICKSTART.md
git commit -m "Add Docker support and GitHub Pages marketing site"
git push origin copilot/add-feature-to-vigilant-octo-engine
```

**Enable GitHub Pages:**
1. Go to: https://github.com/Heyson315/qwe/settings/pages
2. Source: Select your branch
3. Folder: `/docs`
4. Click Save
5. Visit: https://heyson315.github.io/qwe/

## 📂 Project Structure

```
qwe/
├── docs/                          # GitHub Pages website
│   ├── index.html                 # Marketing homepage
│   ├── styles.css                 # Styling
│   ├── script.js                  # JavaScript
│   └── README.md                  # Documentation
│
├── qwe/                           # ASP.NET MVC Application
│   ├── Controllers/               # API & Page controllers
│   ├── Models/                    # Data models
│   ├── Views/                     # Razor views
│   ├── App_Data/                  # File uploads
│   └── Web.config                 # Configuration
│
├── Dockerfile                     # Container definition
├── docker-compose.yml             # Multi-service orchestration
├── .dockerignore                  # Docker build exclusions
│
├── setup-sql-for-docker.ps1       # Automated SQL setup
├── DOCKER_SETUP.md                # Detailed Docker guide
├── DOCKER_QUICKSTART.md           # Quick start guide
├── COMPLETE_SETUP.md              # This file
│
└── README.md                      # Project documentation
```

## 🔧 Configuration

### Connection Strings

**For Docker Container** (`Web.config`):
```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=host.docker.internal;Database=HHRCPA;User Id=qwe_user;Password=QweUser123!;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

**For Local Development** (Visual Studio):
```xml
<connectionStrings>
  <add name="DefaultConnection" 
       connectionString="Server=(local);Database=HHRCPA;Integrated Security=True;TrustServerCertificate=True;" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

### Environment Variables

Edit `docker-compose.yml`:
```yaml
environment:
  - ConnectionStrings__DefaultConnection=Server=host.docker.internal;...
  - ASPNETCORE_ENVIRONMENT=Production
  - TZ=America/New_York
```

## 🌐 SharePoint Integration

### Setup Project Management

1. **Access your SharePoint site:**
   https://rahmanfinanceandaccounting.sharepoint.com/sites/m365appbuilder-infogrid-8856/

2. **Create folder structure:**
```
Shared Documents/
├── 01_Planning/
│   ├── Requirements.docx
│   ├── User Stories.xlsx
│   └── Timeline.mpp
├── 02_Development/
│   ├── Technical Specs/
│   ├── API Docs/
│   └── Testing/
├── 03_Marketing/
│   ├── Website Content/
│   ├── Graphics/
│   └── SEO Strategy/
└── 04_Operations/
    ├── Deployment Guides/
    └── User Manuals/
```

3. **Link from website:**
   - Navigation link in GitHub Pages site
   - Footer resources section
   - Team portal button

### Power Automate Workflows (Optional)

Create automated workflows:

**GitHub to SharePoint Sync:**
```
Trigger: New GitHub issue
Action: Create SharePoint task
Fields: Title, Description, Assignee
```

**Document Approval:**
```
Trigger: File uploaded to SharePoint
Action: Send approval request
Notify: Team via email/Teams
```

## 💼 QuickBooks Integration (Future)

### Preparation Steps

1. **Sign up for QuickBooks Developer Account:**
   - https://developer.intuit.com/

2. **Create an app:**
   - Get Client ID and Client Secret
   - Set redirect URI: `http://localhost:8080/quickbooks/callback`

3. **Install NuGet package:**
```powershell
Install-Package IppDotNetSdkForQuickBooksApiV3
```

4. **Add to Web.config:**
```xml
<appSettings>
  <add key="QuickBooks:ClientId" value="YOUR_CLIENT_ID" />
  <add key="QuickBooks:ClientSecret" value="YOUR_CLIENT_SECRET" />
  <add key="QuickBooks:RedirectUri" value="http://localhost:8080/quickbooks/callback" />
</appSettings>
```

5. **Create controller** (placeholder):
```csharp
public class QuickBooksController : Controller
{
    public ActionResult Connect()
    {
        // OAuth flow to connect to QuickBooks
        // Store access token securely
        return View();
    }
    
    public ActionResult SyncInvoices()
    {
        // Sync invoices from QuickBooks to local DB
        return Json(new { success = true });
    }
}
```

## 🧪 Testing Strategy

### 1. Local Development Testing

```powershell
# Run in Visual Studio (F5)
# Test with local SQL Server
# Use Integrated Security
```

### 2. Docker Testing

```powershell
# Build and run container
docker-compose up -d

# Test application
start http://localhost:8080

# Test database connectivity
# Upload a document → Check Documents table
# Send chat message → Check ChatMessages table
```

### 3. Production Testing Checklist

- [ ] All pages load correctly
- [ ] Document upload works
- [ ] Chatbot responds
- [ ] Services page displays data
- [ ] Contact form submits
- [ ] SQL connection is secure
- [ ] File permissions correct
- [ ] HTTPS enabled
- [ ] Error handling works
- [ ] Logs are captured

## 📊 Monitoring & Logs

### Docker Logs

```powershell
# Real-time logs
docker-compose logs -f web

# Last 100 lines
docker-compose logs --tail=100 web

# Specific time range
docker-compose logs --since 1h web
```

### SQL Server Logs

```sql
-- Check recent errors
EXEC xp_readerrorlog 0, 1

-- Check login attempts
SELECT * FROM sys.dm_exec_sessions
WHERE login_name = 'qwe_user';

-- Monitor active connections
SELECT 
    DB_NAME(database_id) as DatabaseName,
    COUNT(*) as ConnectionCount
FROM sys.dm_exec_sessions
WHERE database_id > 0
GROUP BY database_id;
```

### Application Insights (Azure - Optional)

Add to Web.config:
```xml
<system.diagnostics>
  <trace autoflush="true">
    <listeners>
      <add name="ApplicationInsights" 
           type="Microsoft.ApplicationInsights.TraceListener.ApplicationInsightsTraceListener" />
    </listeners>
  </trace>
</system.diagnostics>
```

## 🔐 Security Best Practices

### 1. SQL Server Security

```sql
-- Use strong password
ALTER LOGIN qwe_user WITH PASSWORD = 'ComplexP@ssw0rd!123';

-- Limit permissions (remove db_owner in production)
REVOKE db_owner FROM qwe_user;
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::dbo TO qwe_user;

-- Enable auditing
CREATE SERVER AUDIT HHRCPA_Audit
TO FILE (FILEPATH = 'C:\SQLAudit\');
```

### 2. Application Security

- [ ] Use HTTPS in production
- [ ] Implement authentication (ASP.NET Identity)
- [ ] Add CSRF protection
- [ ] Validate file uploads (type, size, content)
- [ ] Sanitize user inputs
- [ ] Use parameterized queries
- [ ] Store secrets in Azure Key Vault
- [ ] Enable request rate limiting

### 3. Docker Security

```dockerfile
# Use specific versions, not 'latest'
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.7.2-windowsservercore-ltsc2019

# Run as non-root user (where possible)
# Scan for vulnerabilities
docker scan hhrcpa-web:latest
```

## 🚢 Deployment Options

### Option 1: Azure Container Instances

```powershell
# Login to Azure
az login

# Create resource group
az group create --name hhrcpa-rg --location eastus

# Create container instance
az container create \
  --resource-group hhrcpa-rg \
  --name hhrcpa-web \
  --image hhrcpa-web:latest \
  --dns-name-label hhrcpa-demo \
  --ports 80
```

### Option 2: Azure App Service

```powershell
# Create App Service Plan
az appservice plan create \
  --name hhrcpa-plan \
  --resource-group hhrcpa-rg \
  --sku B1 \
  --is-linux

# Create Web App
az webapp create \
  --resource-group hhrcpa-rg \
  --plan hhrcpa-plan \
  --name hhrcpa-web \
  --runtime "DOTNET|4.7"
```

### Option 3: On-Premises Windows Server

1. Install Docker Desktop for Windows Server
2. Copy docker-compose.yml to server
3. Configure firewall rules
4. Set up reverse proxy (IIS/nginx)
5. Configure SSL certificate
6. Run: `docker-compose up -d`

## 📈 Next Steps & Roadmap

### Immediate (Week 1-2)
- [x] Set up Docker containerization
- [x] Create GitHub Pages marketing site
- [x] Configure SQL Server
- [ ] Test all features locally
- [ ] Deploy to staging environment

### Short-term (Month 1)
- [ ] Implement user authentication
- [ ] Add QuickBooks integration
- [ ] Set up CI/CD pipeline
- [ ] Configure custom domain
- [ ] Add analytics tracking

### Mid-term (Month 2-3)
- [ ] Migrate to Azure SQL Database
- [ ] Implement Azure AD authentication
- [ ] Add payment processing
- [ ] Create mobile app
- [ ] Set up monitoring/alerts

### Long-term (Month 4+)
- [ ] Multi-tenant support
- [ ] Advanced reporting
- [ ] AI-powered insights
- [ ] Mobile app release
- [ ] API for third-party integrations

## 🆘 Troubleshooting

### Common Issues

**Issue: Docker container won't start**
```powershell
# Check logs
docker logs qwe-web-1

# Verify published files
dir publish

# Rebuild
docker-compose down
docker-compose up -d --build
```

**Issue: Cannot connect to SQL Server**
```powershell
# Verify SQL Server is running
Get-Service MSSQLSERVER

# Test port
Test-NetConnection -ComputerName localhost -Port 1433

# Check firewall
Get-NetFirewallRule -DisplayName "*SQL*"
```

**Issue: GitHub Pages not updating**
```powershell
# Check build status in GitHub Actions
# Clear browser cache (Ctrl+Shift+R)
# Wait 5-10 minutes for deployment
```

## 📚 Documentation

- **Technical Docs**: `DOCKER_SETUP.md`
- **Quick Start**: `DOCKER_QUICKSTART.md`
- **API Docs**: `API_DOCUMENTATION.md`
- **Marketing Site**: `docs/README.md`
- **Main README**: `README.md`

## 🤝 Support & Resources

### Internal Resources
- SharePoint: https://rahmanfinanceandaccounting.sharepoint.com
- GitHub: https://github.com/Heyson315/qwe

### External Resources
- Docker Docs: https://docs.docker.com
- ASP.NET MVC: https://docs.microsoft.com/aspnet/mvc
- GitHub Pages: https://pages.github.com
- QuickBooks API: https://developer.intuit.com

### Get Help
- Create GitHub Issue
- Check documentation
- Review logs: `docker-compose logs web`
- Contact: contact@hhrcpa.us

## ✅ Checklist

Use this to track your progress:

**Initial Setup:**
- [ ] SQL Server installed and running
- [ ] Docker Desktop installed
- [ ] Visual Studio with project loaded
- [ ] Git repository cloned

**SQL Configuration:**
- [ ] Ran setup-sql-for-docker.ps1
- [ ] TCP/IP enabled
- [ ] Port 1433 configured
- [ ] Firewall rule added
- [ ] Database and user created

**Docker Setup:**
- [ ] Application published
- [ ] Docker image built
- [ ] Container running
- [ ] Can access http://localhost:8080
- [ ] SQL connection works

**GitHub Pages:**
- [ ] Docs folder committed
- [ ] Pushed to GitHub
- [ ] GitHub Pages enabled
- [ ] Site accessible online
- [ ] Links working

**SharePoint:**
- [ ] Folder structure created
- [ ] Team members added
- [ ] Documentation uploaded
- [ ] Links from website added

**Testing:**
- [ ] Document upload works
- [ ] Chatbot responds
- [ ] Services display
- [ ] Database queries work
- [ ] All pages load

**Production Ready:**
- [ ] HTTPS enabled
- [ ] Authentication added
- [ ] Backups configured
- [ ] Monitoring set up
- [ ] Documentation complete

---

**🎉 Congratulations!** You now have a complete, modern web platform with Docker, GitHub Pages, and SharePoint integration!
