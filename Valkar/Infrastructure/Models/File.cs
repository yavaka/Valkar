namespace Infrastructure.Models
{
    using System;

    public class File
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public DateTime UploadedOn { get; set; }

        public Guid UploadedById { get; set; }
        public Driver UploadedBy { get; set; }
    }
}
