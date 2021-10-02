namespace ApplicationCore.ServiceModels.Document
{
    public class DownloadDocumentServiceModel
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
    }
}
