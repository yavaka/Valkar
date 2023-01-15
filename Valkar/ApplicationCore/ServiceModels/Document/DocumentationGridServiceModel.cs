namespace ApplicationCore.ServiceModels.Document
{
    using System.Collections.Generic;

    public class DocumentationGridServiceModel
    {
        public DocumentationGridServiceModel()
        {
            this.Documents = new List<TempDocumentServiceModel>();
        }

        public ICollection<TempDocumentServiceModel> Documents { get; set; }
        /// <summary>
        /// Default is true
        /// </summary>
        public bool IsAdminPanel { get; set; } = true;
    }
}
