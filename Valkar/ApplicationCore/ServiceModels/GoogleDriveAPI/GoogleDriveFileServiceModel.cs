namespace ApplicationCore.ServiceModels.GoogleDriveAPI
{
    public class GoogleDriveFileServiceModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MimeType { get; set; } = string.Empty;
        public byte[] Data { get; set; } = default!;
    }
}
