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
                            <td align=center bgcolor=lightgrey>Wiadomosc</td>
                            <td align=center bgcolor=lightgrey>Data</td>
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

            html += @"</table><br /><br /><i>Automatyczna wiadomość wysłana z aplikacji ReportService</i>";

            return html;
        }
    }
}
