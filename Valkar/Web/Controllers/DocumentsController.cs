namespace Web.Controllers
{
    using ApplicationCore.Helpers;
    using ApplicationCore.ServiceModels.Document;
    using ApplicationCore.Services.Admin;
    using ApplicationCore.Services.File;
    using ApplicationCore.Services.Identity;
    using ApplicationCore.Services.PDFDocument;
    using Infrastructure.Common.Enums;
    using Infrastructure.Common.Global;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Web.ViewModels.Documents;
    using static Infrastructure.Common.ModelConstants;

    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IAdminService _adminService;
        private readonly IIdentityService _identityService;
        private readonly IDocumentService _documentService;

        public DocumentsController(IFileService fileService,
            IAdminService adminService,
            IIdentityService identityService,
            IDocumentService documentService)
        {
            this._fileService = fileService;
            this._adminService = adminService;
            this._identityService = identityService;
            this._documentService = documentService;
        }

        public async Task<FileResult> DownloadAsync(string uploaderId, EmployeeDocumentTypes documentType)
        {
            // Get the documents
            var documents = this._fileService
                .GetEmployeeDocuments(uploaderId, documentType);

            // Download one document
            if (documents.ToList().Count == 1)
            {
                var doc = documents.First();
                return File(doc.Data, doc.FileType, $"{doc.Name}{doc.Extension}");
            }

            // Download multiple documents in a zip file
            var zipFile = await this._fileService.GetZipFile(documents.ToList());
            return File(zipFile.Data, zipFile.FileType, $"{zipFile.Name}{zipFile.Extension}");
        }

        public async Task<IActionResult> PreviewDocument(string docId)
        {
            var doc = await this._documentService.GetDocumentByIdAsync(docId);

            return doc.FileType is "application/pdf"
                ? File(doc.Data, doc.FileType)
                : File(doc.Data, doc.FileType, doc.Name);
        }

        /// <summary>
        /// Admin or Employee documentation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Documentation()
        {
            var documents = User.IsInRole(Role.Admin) 
                ? await this._documentService.FetchAllDocuments() 
                : await this._documentService
                    .FetchAllDocumentsByEmployeeId(
                        this._identityService.GetUserById(
                            this._identityService.GetUserId(User)).Driver.Id.ToString());

            return View(new DocumentationGridServiceModel
            {
                Documents = documents
            });
        }

        #region Employee Documentation

        [HttpGet]
        public async Task<IActionResult> UploadSignedDocument(string docId)
        {
            return View(new UploadSignedDocumentServiceModel 
            {
                Document = await this._documentService.GetDocumentByIdAsync(docId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> UploadSignedDocument(UploadSignedDocumentServiceModel model)
        {
            ValidateFile(model.File);
            if (ModelState.IsValid)
            {
                // get file data
                var fileData = await this._fileService.ProcessDocument(model.File);
                model.Document.Data = fileData.Data;
                model.Document.Extension = fileData.Extension;
                model.Document.FileType = fileData.FileType;
                model.Document.Name = fileData.Name;
                model.Document.IsSigned = true;

                // upload signed document
                await this._documentService.UploadSignedDocument(model.Document);

                return RedirectToAction(nameof(DocumentsController.Documentation));
            }
            return View(model);
        }

        #endregion

        #region Admin Documentation

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public IActionResult SendDocument()
        {
            return View(new SendDocumentViewModel
            {
                EmployeeNameSelectList = GetEmployeeNameSelectList()
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> SendDocumentAsync(SendDocumentViewModel model)
        {
            ValidateSendDocument(model);
            if (ModelState.IsValid)
            {
                // get the pdf file data
                var serviceModel = await this._fileService.ProcessDocument(model.File);
                serviceModel.SentToId = Guid.Parse(model.EmployeeId);
                serviceModel.MessageToEmployee = model.MessageToEmployee;

                // send the pdf to employee
                await this._documentService.SendDocumentToEmployee(serviceModel);

                return RedirectToAction(nameof(DocumentsController.Documentation));
            }
            model.EmployeeNameSelectList = GetEmployeeNameSelectList();

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ConfirmDeletion(string id)
        {
            return PartialView("DocumentPartials/_DeleteDocumentConfirmation", await this._documentService.GetDocumentByIdAsync(id));
        }

        [HttpGet]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            await this._documentService.DeleteAsync(id);
            return RedirectToAction(nameof(Documentation));
        }

        #endregion

        #region Validations

        private void ValidateSendDocument(SendDocumentViewModel model)
        {
            ValidateFile(model.File);
            if (model.EmployeeId is null)
            {
                ModelState.AddModelError("EmployeeNameSelectList", $"Employee not selected");
            }
        }

        private void ValidateFile(IFormFile file)
        {
            if (file is null)
            {
                ModelState.AddModelError("File", $"File required");
            }
            else
            {
                if (file.Length > MAX_FILE_SIZE)
                {
                    ModelState.AddModelError("File", $"File cannot be more than 20MB.");
                }
                if (file.Length == 0)
                {
                    ModelState.AddModelError("File", $"File is empty. Try again with another file.");
                }
                if (ValidationHelper.RegexValidation(file.FileName.ToLower(), FILE_EXTENSIONS_REGEX) is false)
                {
                    ModelState.AddModelError("File", $"Only pdf allowed");
                }
            }
        }

        #endregion

        #region Helpers

        private SelectList GetEmployeeNameSelectList()
            => new(this._adminService.GetAllDrivers()
                .Select(d => new
                {
                    Id = d.DriverId,
                    FullName = d.FullName
                }).ToList(),
                "Id", "FullName");


        #endregion
    }
}
