namespace ApplicationCore.Services.File
{
    using ApplicationCore.ServiceModels.Document;
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

        #region Employee documents

        public async Task<ICollection<File>> ProcessEmployeeUploadedDocuments(EmployeeDocumentsServiceModel documents)
            => new List<File>()
            {
                await ProcessFile(
                    documents.DrivingLicenceFront,
                    nameof(EmployeeDocumentsServiceModel.DrivingLicenceFront)),
                await ProcessFile(
                    documents.DrivingLicenceBack,
                    nameof(EmployeeDocumentsServiceModel.DrivingLicenceBack)),
                await ProcessFile(
                    documents.IdentityDocumentFront,
                    nameof(EmployeeDocumentsServiceModel.IdentityDocumentFront)),
                await ProcessFile(
                    documents.IdentityDocumentBack,
                    nameof(EmployeeDocumentsServiceModel.IdentityDocumentBack)),
                await ProcessFile(
                    documents.NationalInsuranceNumber,
                    nameof(EmployeeDocumentsServiceModel.NationalInsuranceNumber))
            }.Where(i => i is not null)
            .ToList();

        public IEnumerable<DownloadDocumentServiceModel> GetEmployeeDocuments(string uploaderId, EmployeeDocumentTypes documentType)
            => documentType switch
            {
                EmployeeDocumentTypes.IdentityDocument => GetEmployeeFiles(uploaderId, documentType),
                EmployeeDocumentTypes.DrivingLicence => GetEmployeeFiles(uploaderId, documentType),
                EmployeeDocumentTypes.NationalInsuranceNumber => GetEmployeeFiles(uploaderId, documentType),
                _ => null,
            };

        #endregion

        #region Temp Documents

        public async Task<TempDocumentServiceModel> ProcessDocument(IFormFile formFile)
        {
            var file = await ProcessFile(formFile);
            return new TempDocumentServiceModel
            {
                Name = file.Name,
                Extension = file.Extension,
                FileType = file.FileType,
                Data = file.Data,
                UploadedOn = file.UploadedOn,
            };
        }

        #endregion

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

        private IEnumerable<DownloadDocumentServiceModel> GetEmployeeFiles(
            string uploaderId,
            EmployeeDocumentTypes documentType)
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

        private async Task<File> ProcessFile(IFormFile file, string fileName = default)
        {
            if (file is null)
                return null;

            return new File
            {
                Name = fileName == default
                    ? file.FileName
                    : fileName,
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

