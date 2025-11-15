# HHR CPA Website

A modern, feature-rich website for HHR CPA (Certified Public Accountant) services.

## Features

### 1. **Multi-Page Website**
- **Home Page** - Welcome page with company overview
- **About Page** - Information about the company
- **Services Page** - Dynamic list of services loaded via API
- **Documents Page** - Document management system
- **Contact Page** - Contact information

### 2. **Services API**
- RESTful API endpoint: `GET /api/services`
- Returns list of services with descriptions
- Services page dynamically loads from API

### 3. **Document Management System**
- **Upload Documents** - Support for PDF, Word, Excel, and images
- **View Documents** - List all uploaded documents with metadata
- **Download Documents** - Secure document download
- **Delete Documents** - Remove uploaded documents
- Files stored in `App_Data/Uploads` directory

**API Endpoints:**
- `GET /api/documents` - List all documents
- `POST /api/documents/upload` - Upload files
- `GET /api/documents/download/{id}` - Download file
- `DELETE /api/documents/{id}` - Delete file

### 4. **AI Chatbot Assistant**
- Floating chat widget on all pages
- Intelligent responses about services
- Context-aware conversations
- Topics covered:
  - Tax preparation
  - Bookkeeping services
  - Payroll processing
  - Financial consulting
  - Document uploads
  - Scheduling appointments

**API Endpoints:**
- `POST /api/chat` - Send chat message
- `GET /api/chat/history` - Get chat history
- `DELETE /api/chat/history` - Clear chat history

## Technologies Used

- **Backend:** ASP.NET MVC 5 (.NET Framework 4.7.2)
- **Frontend:** HTML5, CSS3, JavaScript (Vanilla)
- **API:** ASP.NET Web API 2
- **Data Format:** JSON

## Project Structure

```
qwe/
├── Controllers/
│   ├── HomeController.cs          # Main page controller
│   ├── ServicesController.cs      # Services API
│   ├── DocumentsController.cs     # Document management API
│   └── ChatController.cs          # Chatbot API
├── Models/
│   ├── Service.cs                 # Service model
│   ├── Document.cs                # Document model
│   └── ChatMessage.cs             # Chat message models
├── Views/
│   ├── Home/
│   │   ├── Index.cshtml           # Home page
│   │   ├── About.cshtml           # About page
│   │   ├── Services.cshtml        # Services page
│   │   ├── Documents.cshtml       # Documents page
│   │   └── Contact.cshtml         # Contact page
│   └── Shared/
│       └── _Layout.cshtml         # Shared layout
├── Content/
│   └── chat-widget.css            # Chat widget styles
├── Scripts/
│   └── chat-widget.js             # Chat widget functionality
└── App_Data/
    └── Uploads/                   # Uploaded documents storage
```

## Setup Instructions

1. **Clone the repository**
   ```
   git clone https://github.com/Heyson315/qwe
   ```

2. **Open in Visual Studio**
   - Open `qwe.sln` in Visual Studio 2019 or later

3. **Add New Files to Project**
   - In Solution Explorer, click "Show All Files"
   - Right-click on the following files/folders and select "Include In Project":
     - `Controllers/ChatController.cs`
     - `Models/Service.cs`
     - `Models/Document.cs`
     - `Models/ChatMessage.cs`
     - `Views/Home/Documents.cshtml`
     - `Content/chat-widget.css`
     - `Scripts/chat-widget.js`

4. **Install NuGet Packages** (if needed)
   ```
   Install-Package Newtonsoft.Json
   Install-Package Microsoft.AspNet.WebApi
   ```

5. **Build and Run**
   - Press F5 to build and run the application
   - The website will open in your default browser

## Usage

### Uploading Documents
1. Navigate to the "Documents" page
2. Click "Choose Files" and select your documents
3. Click "Upload"
4. Your documents will appear in the list below

### Using the Chatbot
1. Click the green chat button (💬) in the bottom-right corner
2. Type your question and press Enter or click the send button
3. The AI assistant will respond with helpful information

### Managing Services
- Services are currently hardcoded in `ServicesController.cs`
- To add/edit services, modify the `Get()` method in that file

## Customization

### Adding New Services
Edit `Controllers/ServicesController.cs`:
```csharp
new Service { 
    Name = "Your Service Name", 
    Description = "Service description" 
}
```

### Customizing Chat Responses
Edit `Controllers/ChatController.cs` in the `GetSmartResponse()` method to add new keywords and responses.

### Styling
- Global styles are in each `.cshtml` file (inline)
- Chat widget styles are in `Content/chat-widget.css`

## Security Recommendations

⚠️ **Before deploying to production:**

1. **Add Authentication**
   - Implement user login system
   - Use ASP.NET Identity or Azure AD

2. **Secure File Uploads**
   - Add file type validation
   - Implement virus scanning
   - Add file size limits
   - Store files outside web root

3. **Add Authorization**
   - Role-based access control
   - Restrict API endpoints
   - Implement rate limiting

4. **Database Integration**
   - Replace in-memory storage with SQL Server
   - Use Entity Framework
   - Implement proper data persistence

5. **HTTPS**
   - Enable SSL/TLS
   - Use HTTPS only
   - Secure cookies

6. **Input Validation**
   - Validate all user inputs
   - Sanitize file names
   - Prevent SQL injection

## Future Enhancements

- [ ] Integration with Azure OpenAI for smarter chat responses
- [ ] User authentication and login system
- [ ] Database integration (SQL Server)
- [ ] Email notifications
- [ ] Appointment scheduling system
- [ ] Client portal with secure document sharing
- [ ] Payment processing integration
- [ ] Multi-language support
- [ ] Mobile app

## Contact

For questions or support:
- **Email:** contact@hhrcpa.us
- **Phone:** 123-456-7890
- **Website:** https://hhr-cpa.us

## License

© 2025 HHR CPA. All rights reserved.
