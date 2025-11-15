using qwe.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace qwe.Controllers
{
    public class DocumentsController : ApiController
    {
        private static readonly string UploadFolder = HttpContext.Current.Server.MapPath("~/App_Data/Uploads");
        private static List<Document> documents = new List<Document>();
        private static int nextId = 1;

        // GET: api/Documents
        public IEnumerable<Document> Get()
        {
            return documents;
        }

        // GET: api/Documents/5
        public IHttpActionResult Get(int id)
        {
            var document = documents.FirstOrDefault(d => d.Id == id);
            if (document == null)
            {
                return NotFound();
            }
            return Ok(document);
        }

        // POST: api/Documents/Upload
        [HttpPost]
        [Route("api/Documents/Upload")]
        public async Task<IHttpActionResult> Upload()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return StatusCode(HttpStatusCode.UnsupportedMediaType);
            }

            // Ensure upload directory exists
            if (!Directory.Exists(UploadFolder))
            {
                Directory.CreateDirectory(UploadFolder);
            }

            var provider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync(provider);

            var uploadedDocuments = new List<Document>();

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
                var contentType = file.Headers.ContentType.MediaType;
                var buffer = await file.ReadAsByteArrayAsync();

                // Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{filename}";
                var filePath = Path.Combine(UploadFolder, uniqueFileName);

                // Save file
                File.WriteAllBytes(filePath, buffer);

                // Create document record
                var document = new Document
                {
                    Id = nextId++,
                    FileName = filename,
                    FilePath = filePath,
                    UploadedDate = DateTime.Now,
                    FileSize = buffer.Length,
                    ContentType = contentType
                };

                documents.Add(document);
                uploadedDocuments.Add(document);
            }

            return Ok(uploadedDocuments);
        }

        // GET: api/Documents/Download/5
        [HttpGet]
        [Route("api/Documents/Download/{id}")]
        public IHttpActionResult Download(int id)
        {
            var document = documents.FirstOrDefault(d => d.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            if (!File.Exists(document.FilePath))
            {
                return NotFound();
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(File.ReadAllBytes(document.FilePath))
            };

            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = document.FileName
            };
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(document.ContentType);

            return ResponseMessage(response);
        }

        // DELETE: api/Documents/5
        public IHttpActionResult Delete(int id)
        {
            var document = documents.FirstOrDefault(d => d.Id == id);
            if (document == null)
            {
                return NotFound();
            }

            // Delete physical file
            if (File.Exists(document.FilePath))
            {
                File.Delete(document.FilePath);
            }

            documents.Remove(document);
            return Ok();
        }
    }
}
