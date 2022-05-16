using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportService.Models.Domains;

namespace ReportService.Models
{
    internal class GenerateHtmlEmail
    {
        public string GenerateErrors(List<Error> errors, int interval)
        {
            if (errors == null)
            {
                throw new ArgumentNullException();
            }

            if (errors.Any() == false)
            {
                return string.Empty;
            }

            var html = $"Errors from last {interval} minutes. <br />  <br />";

            html +=
                @"
                    <table border=1 cellpadding=5 cellspacing=1>
                        <tr>
                            <td align=center bgcolor=lightgrey>Message</td>
                            <td align=center bgcolor=lightgrey>Date</td>
                        </tr>
                ";

            foreach (var error in errors)
            {
                html +=
                    $@"
                    <tr>
                        <td align=center>{error.Message}</td>      
                        <td align=center>{error.Date:dd-MM-yyyy HH:mm}</td>      
                    </tr>
                    ";
            }

            html += @"</table><br /><br /><i>The message has been sent automatically from the ReportService application.</i>";

            return html;
        }

        public string GenerateReport(Report report)
        {
            if (report == null) throw new ArgumentNullException();

            var html = $"Report {report.Title} from {report.Date:dd-MM-yyyy}. <br />  <br />";


            if (report.Positions != null && report.Positions.Any())
            {
                html +=
                    @"
                    <table border=1 cellpadding=5 cellspacing=1>
                        <tr>
                            <td align=center bgcolor=lightgrey>Title</td>
                            <td align=center bgcolor=lightgrey>Description</td>
                            <td align=center bgcolor=lightgrey>Value</td>
                        </tr>
                ";

                foreach (var reportPosition in report.Positions)
                {
                    html +=
                        $@"
                    <tr>
                        <td align=center>{reportPosition.Title}</td>      
                        <td align=center>{reportPosition.Description}</td>      
                        <td align=center>{reportPosition.Value:0.00} zł</td>      
                    </tr>
                    ";
                }

                html += "</table>";
            }
            else
            {
                html += "-- No data to display -- ";
            }

            html += @"</<br /><br /><i>The message has been sent automatically from the ReportService application.</i>";

            return html;
        }
    }
}
