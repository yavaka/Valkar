namespace ApplicationCore.ServiceModels.Document
{
    using ApplicationCore.ServiceModels.Driver;
    using System;

    public class TempDocumentServiceModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MessageToEmployee { get; set; }
        public byte[] Data { get; set; }
        public string Extension { get; set; }
        public string FileType { get; set; }
        public bool IsSigned { get; set; }
        public DateTime UploadedOn { get; set; }
        public Guid SentToId { get; set; }
        public DriverDetailsServiceModel SentTo { get; set; }
    }
}
