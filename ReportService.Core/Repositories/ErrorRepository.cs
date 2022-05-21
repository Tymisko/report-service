using System;
using System.Collections.Generic;
using ReportService.Core.Domains;

namespace ReportService.Core.Repositories
{
    internal class ErrorRepository
    {
        public List<Error> GetLastErrors(int intervalInMinutes)
        {

            // TODO: get from database

            return new List<Error>()
            {
                new Error {Message = "Test error 1", Date = DateTime.Now},
                new Error {Message = "Test error 2", Date = DateTime.Now}
            };
        }
    }
}
