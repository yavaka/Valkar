namespace Web.Controllers
{
    using ApplicationCore.Services.PDFGenerator;
    using Microsoft.AspNetCore.Mvc;
    using System.Text;

    public class PDFGeneratorController : Controller
    {
        private readonly IPDFGeneratorService _pdfGeneratorService;

        public PDFGeneratorController(IPDFGeneratorService pdfGeneratorService)
            => this._pdfGeneratorService = pdfGeneratorService;

        public IActionResult Generate(int id)
        {
            // Create PDFTemplateModel
            var pdfDoc = this._pdfGeneratorService.Create(GetHTMLString());
            return File(pdfDoc, "application/pdf");
        }

        public static string GetHTMLString()
        {
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                        <head>
                        </head>
                        <div class='container'>
                            <div>
                                <h1>Valkar Logo</h1>
                            </div>
                            <div class='doc-title'>
                                <h1>Time Sheet</h1>
                            </div>
                            <br>
                            <div class='header-info'>
                                <div class='employee'>
                                    <label for='EmployeeName'>Company:</label>
                                    <strong>Company Name</strong>
                                </div>
                                <div class='week-start-date'>
                                    <label for='WeekStart'>Week Start:&nbsp;</label>
                                    <strong>25/09/21</strong>
                                </div>
                            </div>
                            <br>
                            <div>
                                <table class='tg'>
                                    <colgroup>
                                        <col style='width: 132px'>
                                        <col style='width: 139px'>
                                        <col style='width: 129px'>
                                        <col style='width: 128px'>
                                        <col style='width: 130px'>
                                        <col style='width: 130px'>
                                    </colgroup>
                                    <thead>
                                        <tr>
                                            <th class='tg-a52w'>Day</th>
                                            <th class='tg-a52w'>Date</th>
                                            <th class='tg-a52w'>Time In</th>
                                            <th class='tg-a52w'>Break</th>
                                            <th class='tg-a52w'>Time Out</th>
                                            <th class='tg-a52w'>Total Hours</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class='tg-is12'>Sunday</td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                        <tr>
                                            <td class='tg-is12'>Monday</td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                        <tr>
                                            <td class='tg-is12'>Tuesday</td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                        <tr>
                                            <td class='tg-is12'>Wednesday</td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                        <tr>
                                            <td class='tg-is12'>Thursday</td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                        <tr>
                                            <td class='tg-is12'>Friday</td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                        <tr>
                                            <td class='tg-is12'>Saturday</td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                        <tr>
                                            <td style='border-left-style: none; border-right-style: none;border-bottom-style: none;'>
                                            </td>
                                            <td style='border-left-style: none; border-right-style: none;border-bottom-style: none;'>
                                            </td>
                                            <td style='border-left-style: none; border-right-style: none;border-bottom-style: none;'>
                                            </td>
                                            <td style='border-left-style: none; border-right-style: none;border-bottom-style: none;'>
                                            </td>
                                            <td class='tg-0pky' style='text-align: center;'><strong>Total Hours</strong></td>
                                            <td class='tg-0pky'></td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <br>
                            <table class='employee-details'>
                                <tr>
                                    <td class='t-section'><label for='EmployeeName'>Employee:</label></td>
                                    <td class='t-section'><strong>Employee name goes here</strong></td>
                                </tr>
                                <tr>
                                    <td class='t-section'><label for='Signature'>Signature:</label></td>
                                    <td class='t-section'><strong>Signiture goes here</strong></td>
                                </tr>
                                <tr>
                                    <td class='t-section'><label for='Date'>Date:</label></td>
                                    <td class='t-section'><strong>25/09/21</strong></td>
                                </tr>
                            </table>
                            <table class='manager-details'>
                                <tr>
                                    <td class='t-section'><label for='EmployeeName'>Manager:</label></td>
                                    <td class='t-section'><strong>Manager name goes here</strong></td>
                                </tr>
                                <tr>
                                    <td class='t-section'><label for='Signature'>Signature:</label></td>
                                    <td class='t-section'><strong>Signiture goes here</strong></td>
                                </tr>
                                <tr>
                                    <td class='t-section'><label for='Date'>Date:</label></td>
                                    <td class='t-section'><strong>25/09/21</strong></td>
                                </tr>
                            </table>
                        </div>
                        </html>");
            return sb.ToString();
        }
    }
}
