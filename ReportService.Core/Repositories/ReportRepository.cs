using System;
using System.Collections.Generic;
using ReportService.Core.Domains;

namespace ReportService.Core.Repositories
{
    internal class ReportRepository
    {
        public Report GetLastNotSentReport()
        {
            // TODO: downloading from database

            return new Report
            {
                Id = 1,
                Title = "R/1/2022",
                Date = new DateTime(2022, 1, 1, 12, 0, 0),
                Positions = new List<ReportPosition>
                {
                    new ReportPosition()
                    {
                        Id = 1,
                        ReportId = 1,
                        Title = "Position 1",
                        Description = "Description 1",
                        Value = 43.01m,
                    },

                    new ReportPosition()
                    {
                        Id = 2,
                        ReportId = 2,
                        Title = "Position 2",
                        Description = "Description 2",
                        Value = 4311m,
                    },

                    new ReportPosition()
                    {
                        Id = 3,
                        ReportId = 3,
                        Title = "Position 3",
                        Description = "Description 3",
                        Value = 1.99m
                    }
                }
            };
        }

        public void ReportSent(Report report)
        {
            report.IsSend = true;

            // TODO: save in database
        }
    }
}
