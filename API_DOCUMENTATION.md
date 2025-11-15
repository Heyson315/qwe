# API Documentation

## Overview
The HHR CPA website provides several RESTful API endpoints for managing services, documents, and chatbot interactions.

## Base URL
```
http://localhost:[port]/api
```

---

## Services API

### Get All Services
Retrieves a list of all available CPA services.

**Endpoint:** `GET /api/services`

**Response:**
```json
[
  {
    "Name": "Tax Preparation",
    "Description": "Comprehensive tax preparation services for individuals and businesses."
  },
  {
    "Name": "Bookkeeping",
    "Description": "Professional bookkeeping services to keep your financial records organized."
  }
]
```

**Status Codes:**
- `200 OK` - Success
- `500 Internal Server Error` - Server error

---

## Documents API

### List All Documents
Retrieves all uploaded documents.

**Endpoint:** `GET /api/documents`

**Response:**
```json
[
  {
    "Id": 1,
    "FileName": "tax-form-2024.pdf",
    "UniqueFileName": "abc123.pdf",
    "FileSize": 1048576,
    "FileType": "application/pdf",
    "UploadDate": "2024-01-15T10:30:00"
  }
]
```

### Upload Document
Uploads one or more documents.

**Endpoint:** `POST /api/documents/upload`

**Content-Type:** `multipart/form-data`

**Request:**
- Form field: `files` (one or more files)

**Allowed File Types:**
- PDF (`.pdf`)
- Word (`.doc`, `.docx`)
- Excel (`.xls`, `.xlsx`)
- Images (`.jpg`, `.jpeg`, `.png`)

**Max File Size:** 10 MB per file

**Response:**
```json
{
  "success": true,
  "uploadedFiles": [
    {
      "Id": 2,
      "FileName": "receipt.jpg",
      "FileSize": 524288,
      "UploadDate": "2024-01-15T11:00:00"
    }
  ]
}
```

**Status Codes:**
- `200 OK` - Upload successful
- `400 Bad Request` - Invalid file type or size
- `500 Internal Server Error` - Server error

### Download Document
Downloads a specific document by ID.

**Endpoint:** `GET /api/documents/download/{id}`

**Parameters:**
- `id` (int) - Document ID

**Response:** File download

**Status Codes:**
- `200 OK` - File download
- `404 Not Found` - Document not found

### Delete Document
Deletes a specific document.

**Endpoint:** `DELETE /api/documents/{id}`

**Parameters:**
- `id` (int) - Document ID

**Response:**
```json
{
  "success": true,
  "message": "Document deleted successfully"
}
```

**Status Codes:**
- `200 OK` - Deletion successful
- `404 Not Found` - Document not found

---

## Chat API

### Send Chat Message
Sends a message to the chatbot and receives a response.

**Endpoint:** `POST /api/chat`

**Request:**
```json
{
  "Message": "What services do you offer?"
}
```

**Response:**
```json
{
  "UserMessage": "What services do you offer?",
  "BotResponse": "We offer Tax Preparation, Bookkeeping, Payroll Processing, and Financial Consulting...",
  "Timestamp": "2024-01-15T12:00:00"
}
```

**Status Codes:**
- `200 OK` - Success
- `400 Bad Request` - Invalid message

### Get Chat History
Retrieves the chat conversation history.

**Endpoint:** `GET /api/chat/history`

**Response:**
```json
[
  {
    "UserMessage": "What services do you offer?",
    "BotResponse": "We offer Tax Preparation, Bookkeeping...",
    "Timestamp": "2024-01-15T12:00:00"
  }
]
```

### Clear Chat History
Clears all chat history.

**Endpoint:** `DELETE /api/chat/history`

**Response:**
```json
{
  "success": true,
  "message": "Chat history cleared"
}
```

---

## Error Responses

All API endpoints may return error responses in the following format:

```json
{
  "error": true,
  "message": "Error description",
  "details": "Additional error details (only in development mode)"
}
```

**Common Status Codes:**
- `200 OK` - Request successful
- `400 Bad Request` - Invalid request parameters
- `404 Not Found` - Resource not found
- `500 Internal Server Error` - Server error

---

## Rate Limiting
Currently not implemented. Recommended for production:
- 100 requests per minute per IP
- 1000 requests per hour per IP

---

## Authentication
Current version: **No authentication required** (Development only)

⚠️ **Production Requirements:**
- Implement OAuth 2.0 or JWT authentication
- Require API keys for all endpoints
- Role-based access control (Admin, User)

---

## Examples

### JavaScript (Fetch API)

```javascript
// Get all services
fetch('/api/services')
  .then(response => response.json())
  .then(data => console.log(data));

// Upload document
const formData = new FormData();
formData.append('files', fileInput.files[0]);

fetch('/api/documents/upload', {
  method: 'POST',
  body: formData
})
  .then(response => response.json())
  .then(data => console.log(data));

// Send chat message
fetch('/api/chat', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ Message: 'Hello' })
})
  .then(response => response.json())
  .then(data => console.log(data));
```

### C# (HttpClient)

```csharp
// Get all services
var client = new HttpClient();
var response = await client.GetAsync("http://localhost:port/api/services");
var services = await response.Content.ReadAsAsync<List<Service>>();

// Upload document
var content = new MultipartFormDataContent();
content.Add(new StreamContent(fileStream), "files", fileName);
var uploadResponse = await client.PostAsync("/api/documents/upload", content);
```

### cURL

```bash
# Get services
curl http://localhost:port/api/services

# Upload document
curl -X POST http://localhost:port/api/documents/upload \
  -F "files=@document.pdf"

# Send chat message
curl -X POST http://localhost:port/api/chat \
  -H "Content-Type: application/json" \
  -d '{"Message":"What services do you offer?"}'
```

---

## Testing the API

### Using Postman
1. Import the provided Postman collection (if available)
2. Set the base URL to your local or deployed instance
3. Test each endpoint individually

### Using Browser Developer Tools
1. Open the website in your browser
2. Open Developer Tools (F12)
3. Go to Network tab
4. Interact with the website to see API calls

---

## Future Enhancements
- [ ] Swagger/OpenAPI specification
- [ ] API versioning (v1, v2)
- [ ] Pagination for large datasets
- [ ] Filtering and sorting options
- [ ] Webhook support
- [ ] GraphQL alternative
