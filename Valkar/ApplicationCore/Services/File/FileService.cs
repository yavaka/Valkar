namespace ApplicationCore.Services.File
{
    using ApplicationCore.ServiceModels.Document;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using File = Infrastructure.Models.File;

    public class FileService : IFileService
    {
        public async Task<ICollection<File>> ProcessUploadedDocuments(DocumentsServiceModel documents)
            => new List<File>()
            {
                await ProcessFile(
                    documents.DrivingLicenceFront,
                    nameof(DocumentsServiceModel.DrivingLicenceFront)),
                await ProcessFile(
                    documents.DrivingLicenceBack,
                    nameof(DocumentsServiceModel.DrivingLicenceBack)),
                await ProcessFile(
                    documents.IdentityDocumentFront,
                    nameof(DocumentsServiceModel.IdentityDocumentFront)),
                await ProcessFile(
                    documents.IdentityDocumentBack,
                    nameof(DocumentsServiceModel.IdentityDocumentBack)),
                await ProcessFile(
                    documents.NationalInsuranceNumber,
                    nameof(DocumentsServiceModel.NationalInsuranceNumber))
            }.Where(i => i is not null)
            .ToList();


        private async Task<File> ProcessFile(IFormFile file, string fileName)
        {
            if (file is null)
            {
                return null;
            }
            return new File
            {
                Name = fileName,
                Extension = Path.GetExtension(file.FileName),
                FileType = file.ContentType,
                UploadedOn = DateTime.Now,
                Data = await GetFileData(file)
            };
        }

        private async Task<byte[]> GetFileData(IFormFile file)
        {
            using var dataStream = new MemoryStream();

            await file.CopyToAsync(dataStream);

            return dataStream.ToArray();
        }
    }
}
