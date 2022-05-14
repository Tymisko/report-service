
using System;

namespace ReportService.Models.Domains
{
    internal class Error
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }
}
