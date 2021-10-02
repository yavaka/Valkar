namespace ApplicationCore.Services.File
{
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.Services.Driver;
    using Infrastructure;
    using Infrastructure.Common.Enums;
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Threading.Tasks;
    using File = Infrastructure.Models.File;

    public class FileService : IFileService
    {
        private readonly ValkarDbContext _data;

        public FileService(ValkarDbContext data)
            => this._data = data;

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

        public IEnumerable<DownloadDocumentServiceModel> GetDocuments(string uploaderId, DocumentTypes documentType)
            => documentType switch
            {
                DocumentTypes.IdentityDocument => GetFiles(uploaderId, documentType),
                DocumentTypes.DrivingLicence => GetFiles(uploaderId, documentType),
                DocumentTypes.NationalInsuranceNumber => GetFiles(uploaderId, documentType),
                _ => null,
            };

        public async Task<DownloadDocumentServiceModel> GetZipFile(List<DownloadDocumentServiceModel> documents)
        {
            using var ms = new MemoryStream();
            {
                using var zip = new ZipArchive(ms, ZipArchiveMode.Create, true);

                foreach (var doc in documents)
                {
                    var entry = zip.CreateEntry($"{doc.Name}{doc.Extension}");

                    using var entryStream = entry.Open();

                    await entryStream.WriteAsync(doc.Data, 0, doc.Data.Length);
                }
            }
            return new DownloadDocumentServiceModel
            {
                Name = documents.First().Name.Split('_')[0],
                Data = ms.ToArray(),
                FileType = "application/zip",
                Extension = ".zip"
            };
        }

        #region Fetch Documents

        private IEnumerable<DownloadDocumentServiceModel> GetFiles(
            string uploaderId,
            DocumentTypes documentType)
        {
            var uploaderName = GetUploaderNameById(uploaderId);

            return this._data
                .Files
                .Where(f => f.UploadedById.ToString() == uploaderId &&
                            f.Name.Contains(documentType.ToString()))
                .Select(f => new DownloadDocumentServiceModel
                {
                    Name = $"{uploaderName}_{f.Name}",
                    FileType = f.FileType,
                    Data = f.Data,
                    Extension = f.Extension
                }).ToList();
        }

        #endregion

        #region Helpers
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

        private string GetUploaderNameById(string id) 
        {
            var driver = this._data.Drivers
                .Select(d => new
                {
                    Id = d.Id,
                    FirstNames = d.FirstNames,
                    LastNames = d.Surname
                }).FirstOrDefault(i => i.Id.ToString() == id);

            return $"{driver.FirstNames} {driver.LastNames}";
        }

        #endregion
    }
}

