namespace Infrastructure.Models
{
    using System;

    public class TempDocument
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string MessageToEmployee { get; set; }
        public bool IsSigned { get; set; }
        public DateTime UploadedOn { get; set; }

        public Guid SentToId { get; set; }
        public Driver SentTo { get; set; }
    }
}
