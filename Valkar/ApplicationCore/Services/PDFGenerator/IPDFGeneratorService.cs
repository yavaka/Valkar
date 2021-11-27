namespace ApplicationCore.Services.PDFGenerator
{
    public interface IPDFGeneratorService
    {
        byte[] Create(string htmlTemplate);
    }
}
