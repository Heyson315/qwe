using qwe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace qwe.Services
{
    /// <summary>
    /// Service layer for document management
    /// Handles business logic for file operations
    /// </summary>
    public class DocumentService
    {
        private static List<Document> _documents = new List<Document>();
        private readonly string _uploadPath;

        public DocumentService()
        {
            _uploadPath = HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
            EnsureUploadDirectoryExists();
        }

        private void EnsureUploadDirectoryExists()
        {
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        /// <summary>
        /// Get all documents
        /// </summary>
        public List<Document> GetAllDocuments()
        {
            return _documents.OrderByDescending(d => d.UploadDate).ToList();
        }

        /// <summary>
        /// Get document by ID
        /// </summary>
        public Document GetDocumentById(int id)
        {
            return _documents.FirstOrDefault(d => d.Id == id);
        }

        /// <summary>
        /// Upload a new document
        /// </summary>
        public Document UploadDocument(HttpPostedFileBase file)
        {
            if (file == null || file.ContentLength == 0)
                throw new ArgumentException("No file provided");

            // Validate file extension
            var extension = Path.GetExtension(file.FileName).ToLower();
            var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".jpg", ".jpeg", ".png" };
            
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException($"File type {extension} not allowed");

            // Validate file size (10MB max)
            if (file.ContentLength > 10485760)
                throw new ArgumentException("File size exceeds 10MB limit");

            // Generate unique filename
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_uploadPath, uniqueFileName);

            // Save file
            file.SaveAs(filePath);

            // Create document record
            var document = new Document
            {
                Id = _documents.Count > 0 ? _documents.Max(d => d.Id) + 1 : 1,
                FileName = file.FileName,
                UniqueFileName = uniqueFileName,
                FileSize = file.ContentLength,
                FileType = file.ContentType,
                UploadDate = DateTime.Now
            };

            _documents.Add(document);
            return document;
        }

        /// <summary>
        /// Delete a document
        /// </summary>
        public bool DeleteDocument(int id)
        {
            var document = GetDocumentById(id);
            if (document == null)
                return false;

            // Delete physical file
            var filePath = Path.Combine(_uploadPath, document.UniqueFileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Remove from list
            _documents.Remove(document);
            return true;
        }

        /// <summary>
        /// Get document file path
        /// </summary>
        public string GetDocumentPath(int id)
        {
            var document = GetDocumentById(id);
            if (document == null)
                return null;

            return Path.Combine(_uploadPath, document.UniqueFileName);
        }

        /// <summary>
        /// Get document statistics
        /// </summary>
        public object GetDocumentStats()
        {
            return new
            {
                TotalDocuments = _documents.Count,
                TotalSize = _documents.Sum(d => d.FileSize),
                DocumentsByType = _documents.GroupBy(d => Path.GetExtension(d.FileName))
                    .Select(g => new { Type = g.Key, Count = g.Count() })
            };
        }
    }
}
