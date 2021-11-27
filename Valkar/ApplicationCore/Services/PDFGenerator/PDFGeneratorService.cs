namespace ApplicationCore.Services.PDFGenerator
{
    using DinkToPdf;
    using DinkToPdf.Contracts;
    using System.IO;

    public class PDFGeneratorService : IPDFGeneratorService
    {
        private readonly IConverter _converter;

        public PDFGeneratorService(IConverter converter)
            => this._converter = converter;

        public byte[] Create(string htmlTemplate)
            => this._converter.Convert(new HtmlToPdfDocument()
            {
                GlobalSettings = GetGlobalSettings(),
                Objects = { GetObjectSettings(htmlTemplate) }
            });

        private GlobalSettings GetGlobalSettings()
            => new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Landscape,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Valkar Employee Weekly Time Table"
            };

        private ObjectSettings GetObjectSettings(string htmlTemplate)
            => new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = htmlTemplate,
                WebSettings =
                {
                    DefaultEncoding = "utf-8",
                    UserStyleSheet = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot/css",
                        "export-to-pdf-styles.css")
                },
                FooterSettings =
                {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = true,
                    Center = "Valkar ©"
                }
            };
    }
}
