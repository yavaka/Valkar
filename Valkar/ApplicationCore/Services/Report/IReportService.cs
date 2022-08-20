using ApplicationCore.ServiceModels.Report;
using System.Threading.Tasks;

namespace ApplicationCore.Services.Report
{
    public interface IReportService
    {
        Task<byte[]> GenerateTimeSheetReport(TimeSheetReportServiceModel model);
    }
}
