# Development Guidelines - HHR CPA Website

## Table of Contents
1. [Getting Started](#getting-started)
2. [Code Standards](#code-standards)
3. [Git Workflow](#git-workflow)
4. [Project Structure](#project-structure)
5. [Best Practices](#best-practices)
6. [Testing](#testing)
7. [Deployment](#deployment)
8. [Troubleshooting](#troubleshooting)

---

## Getting Started

### Prerequisites
- Visual Studio 2019 or later
- .NET Framework 4.7.2
- Git for version control
- IIS Express (included with Visual Studio)

### Initial Setup
```bash
# Clone the repository
git clone https://github.com/Heyson315/qwe
cd qwe

# Open in Visual Studio
start qwe.sln

# Build the solution
# Press Ctrl+Shift+B or Build > Build Solution

# Run the application
# Press F5 or Debug > Start Debugging
```

---

## Code Standards

### C# Coding Conventions

#### Naming Conventions
```csharp
// Classes, Methods, Properties - PascalCase
public class DocumentService { }
public void UploadDocument() { }
public string FileName { get; set; }

// Private fields - _camelCase with underscore prefix
private List<Document> _documents;
private readonly string _uploadPath;

// Parameters, local variables - camelCase
public void ProcessFile(string fileName, int fileSize) { }

// Constants - UPPER_CASE
private const int MAX_FILE_SIZE = 10485760;
```

#### Code Organization
```csharp
// Order of members in a class:
// 1. Constants
// 2. Fields
// 3. Constructors
// 4. Properties
// 5. Methods (public first, then private)

public class ExampleService
{
    // Constants
    private const int MAX_RETRIES = 3;
    
    // Fields
    private readonly IRepository _repository;
    private List<Item> _cache;
    
    // Constructor
    public ExampleService(IRepository repository)
    {
        _repository = repository;
        _cache = new List<Item>();
    }
    
    // Properties
    public bool IsEnabled { get; set; }
    
    // Public methods
    public void DoSomething() { }
    
    // Private methods
    private void Helper() { }
}
```

#### Comments and Documentation
```csharp
/// <summary>
/// Uploads a document to the server
/// </summary>
/// <param name="file">The file to upload</param>
/// <returns>The uploaded document information</returns>
/// <exception cref="ArgumentException">Thrown when file is invalid</exception>
public Document UploadDocument(HttpPostedFileBase file)
{
    // Validate input
    if (file == null || file.ContentLength == 0)
        throw new ArgumentException("File cannot be empty");
    
    // TODO: Add virus scanning
    
    // Process file...
}
```

### JavaScript Conventions

```javascript
// Use const for variables that won't be reassigned
const API_URL = '/api/documents';

// Use let for variables that will change
let uploadProgress = 0;

// Function naming - camelCase
function uploadDocument(file) {
    // Function body
}

// Use async/await for asynchronous operations
async function fetchDocuments() {
    try {
        const response = await fetch(API_URL);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error('Error fetching documents:', error);
    }
}
```

---

## Git Workflow

### Branch Strategy

```
master (production)
  └─ develop (integration)
      ├─ feature/add-authentication
      ├─ feature/improve-chatbot
      ├─ bugfix/file-upload-error
      └─ hotfix/security-patch
```

### Branch Naming
- `feature/` - New features (e.g., `feature/user-authentication`)
- `bugfix/` - Bug fixes (e.g., `bugfix/upload-validation`)
- `hotfix/` - Urgent production fixes (e.g., `hotfix/security-vulnerability`)
- `refactor/` - Code refactoring (e.g., `refactor/service-layer`)

### Commit Messages
Follow the conventional commits format:

```bash
# Format: <type>: <description>

# Types:
feat: Add new document filtering feature
fix: Fix file upload validation bug
docs: Update API documentation
style: Format code according to standards
refactor: Refactor DocumentService for better testability
test: Add unit tests for ServicesService
chore: Update NuGet packages

# Examples:
git commit -m "feat: Add user authentication system"
git commit -m "fix: Resolve file upload size validation issue"
git commit -m "docs: Add API endpoint documentation"
```

### Workflow Steps

```bash
# 1. Start new feature
git checkout develop
git pull origin develop
git checkout -b feature/my-new-feature

# 2. Make changes and commit
git add .
git commit -m "feat: Add new feature"

# 3. Push to remote
git push origin feature/my-new-feature

# 4. Create Pull Request on GitHub
# - Go to GitHub repository
# - Click "Pull Requests"
# - Click "New Pull Request"
# - Select your branch
# - Fill in PR template
# - Request review

# 5. After approval, merge to develop
# 6. Delete feature branch
git branch -d feature/my-new-feature
```

---

## Project Structure

```
qwe/
├── Controllers/          # MVC and API controllers
│   ├── HomeController.cs
│   ├── ServicesController.cs
│   ├── DocumentsController.cs
│   └── ChatController.cs
│
├── Models/              # Data models
│   ├── Service.cs
│   ├── Document.cs
│   └── ChatMessage.cs
│
├── Services/            # Business logic layer
│   ├── ServicesService.cs
│   └── DocumentService.cs
│
├── Repositories/        # Data access layer (future)
│   └── (to be implemented)
│
├── Utilities/           # Helper classes
│   ├── Logger.cs
│   └── ApiExceptionFilter.cs
│
├── Configuration/       # App configuration
│   └── AppSettings.cs
│
├── Views/              # Razor views
│   ├── Home/
│   └── Shared/
│
├── Content/            # CSS and static content
│   └── chat-widget.css
│
├── Scripts/            # JavaScript files
│   └── chat-widget.js
│
├── App_Data/           # Application data
│   ├── Uploads/        # Uploaded documents
│   └── Logs/           # Application logs
│
└── App_Start/          # Application startup config
    ├── RouteConfig.cs
    └── WebApiConfig.cs
```

---

## Best Practices

### Error Handling

```csharp
// Use try-catch for expected exceptions
public IHttpActionResult UploadDocument()
{
    try
    {
        // Process upload
        return Ok(result);
    }
    catch (ArgumentException ex)
    {
        Logger.Warning(ex.Message, "DocumentsController");
        return BadRequest(ex.Message);
    }
    catch (Exception ex)
    {
        Logger.Error("Upload failed", ex, "DocumentsController");
        return InternalServerError(ex);
    }
}
```

### Logging

```csharp
// Use Logger utility for all logging
Logger.Info("User uploaded document", "DocumentService");
Logger.Warning("Large file uploaded", "DocumentService");
Logger.Error("Upload failed", exception, "DocumentService");
```

### Security

```csharp
// Always validate input
public void ProcessInput(string userInput)
{
    if (string.IsNullOrWhiteSpace(userInput))
        throw new ArgumentException("Input cannot be empty");
    
    // Sanitize input
    var sanitized = Regex.Replace(userInput, @"[^\w\s]", "");
    
    // Process...
}

// Use parameterized queries (when database is added)
// BAD:
var query = $"SELECT * FROM Users WHERE Id = {userId}"; // SQL Injection!

// GOOD:
var query = "SELECT * FROM Users WHERE Id = @userId";
cmd.Parameters.AddWithValue("@userId", userId);
```

### Performance

```csharp
// Use async/await for I/O operations
public async Task<Document> UploadDocumentAsync(HttpPostedFileBase file)
{
    await file.SaveAsAsync(filePath);
    return document;
}

// Cache frequently accessed data
private static List<Service> _servicesCache;
private static DateTime _cacheExpiry;

public List<Service> GetServices()
{
    if (_servicesCache == null || DateTime.Now > _cacheExpiry)
    {
        _servicesCache = LoadServicesFromDatabase();
        _cacheExpiry = DateTime.Now.AddMinutes(5);
    }
    return _servicesCache;
}
```

---

## Testing

### Unit Testing

```csharp
[TestClass]
public class DocumentServiceTests
{
    private DocumentService _service;
    
    [TestInitialize]
    public void Setup()
    {
        _service = new DocumentService();
    }
    
    [TestMethod]
    public void UploadDocument_ValidFile_ReturnsDocument()
    {
        // Arrange
        var file = CreateMockFile("test.pdf", 1024);
        
        // Act
        var result = _service.UploadDocument(file);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("test.pdf", result.FileName);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UploadDocument_NullFile_ThrowsException()
    {
        // Act
        _service.UploadDocument(null);
    }
}
```

### Running Tests

```bash
# Visual Studio
# Test > Run All Tests (Ctrl+R, A)

# Command line
dotnet test

# With coverage
dotnet test /p:CollectCoverage=true
```

---

## Deployment

### Pre-Deployment Checklist

- [ ] All tests passing
- [ ] Code reviewed and approved
- [ ] No TODO or FIXME comments in production code
- [ ] Sensitive data removed (no hardcoded passwords)
- [ ] Web.config transforms configured
- [ ] Database migrations ready (if applicable)
- [ ] Error logging configured
- [ ] Performance tested
- [ ] Security scan completed

### Deployment to IIS

```bash
# 1. Publish from Visual Studio
# - Right-click project > Publish
# - Choose IIS, FTP, etc.
# - Configure profile
# - Click Publish

# 2. Configure IIS
# - Create new Application Pool (.NET Framework v4.x)
# - Create new Site
# - Point to published folder
# - Set appropriate permissions
```

### Environment Configuration

```xml
<!-- Development -->
<add key="Environment" value="Development" />
<add key="ShowDetailedErrors" value="true" />

<!-- Staging -->
<add key="Environment" value="Staging" />
<add key="ShowDetailedErrors" value="true" />

<!-- Production -->
<add key="Environment" value="Production" />
<add key="ShowDetailedErrors" value="false" />
```

---

## Troubleshooting

### Common Issues

#### Build Errors

```
Error: Assembly not found
Solution: Restore NuGet packages (Right-click solution > Restore NuGet Packages)

Error: Compilation failed
Solution: Clean solution (Build > Clean Solution), then rebuild
```

#### Runtime Errors

```
Error: File upload fails
Check: App_Data/Uploads directory exists and has write permissions

Error: API returns 404
Check: WebApiConfig.cs is registered in Global.asax

Error: Logs not being created
Check: App_Data/Logs directory exists and has write permissions
```

### Debugging Tips

```csharp
// Use breakpoints (F9)
public void SomeMethod()
{
    var result = ProcessData(); // Set breakpoint here
    return result;
}

// Use Debug.WriteLine for quick debugging
Debug.WriteLine($"File size: {file.ContentLength}");

// Check Application Event Log
Logger.Error("Detailed error message", exception, "Component");
```

---

## Code Review Checklist

### Before Submitting PR

- [ ] Code builds without errors or warnings
- [ ] All tests pass
- [ ] New code has unit tests
- [ ] Code follows naming conventions
- [ ] No commented-out code
- [ ] No hardcoded values (use configuration)
- [ ] Error handling implemented
- [ ] Logging added for important operations
- [ ] Documentation updated (if needed)
- [ ] No security vulnerabilities

### Reviewer Guidelines

- Check for code quality and maintainability
- Verify proper error handling
- Ensure security best practices followed
- Confirm adequate test coverage
- Validate performance considerations
- Review for potential bugs

---

## Resources

### Documentation
- [ASP.NET MVC 5 Documentation](https://docs.microsoft.com/aspnet/mvc/)
- [Web API 2 Documentation](https://docs.microsoft.com/aspnet/web-api/)
- [C# Coding Conventions](https://docs.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions)

### Tools
- [Visual Studio Extensions](https://marketplace.visualstudio.com/)
- [ReSharper](https://www.jetbrains.com/resharper/) - Code quality tool
- [Postman](https://www.postman.com/) - API testing
- [Git Extensions](https://gitextensions.github.io/) - Git GUI

### Learning
- [ASP.NET Tutorials](https://www.asp.net/learn)
- [Pluralsight ASP.NET Courses](https://www.pluralsight.com/)
- [Microsoft Learn](https://learn.microsoft.com/)

---

## Contact

For questions or support:
- **Project Lead**: [Your Name]
- **Team Chat**: [Slack/Teams Channel]
- **Issues**: [GitHub Issues](https://github.com/Heyson315/qwe/issues)

## Version History

- v1.0.0 (2025-01-15) - Initial development guidelines
