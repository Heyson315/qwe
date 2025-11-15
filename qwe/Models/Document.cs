using System;

namespace qwe.Models
{
    public class Document
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadedDate { get; set; }
        public long FileSize { get; set; }
        public string ContentType { get; set; }
    }
}
