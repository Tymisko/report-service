using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cipher;
using EmailSender;
using ReportService.Core;
using ReportService.Core.Domains;

namespace ReportService.ConsoleApp
{
    internal class Program
    {
        private static GenerateHtmlEmail _htmlEmail = new GenerateHtmlEmail();
        private static string _emailReceiver = "";

        private static string DecryptSenderEmailPassword()
        {
            var _stringCipher = new StringCipher("");
            var encryptedPassword = "";

            if (encryptedPassword.StartsWith("encrypt:"))
            {
                encryptedPassword = _stringCipher.Encrypt(encryptedPassword.Replace("encrypt:", ""));
            }

            return _stringCipher.Decrypt(encryptedPassword);
        }

        static void Main(string[] args)
        {
            return;

            var _email = new Email(new EmailParams
            {
                HostSmtp = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                SenderName = "Report Serivce",
                SenderEmail = "urbasjanek@gmail.com",
                SenderEmailPassword = DecryptSenderEmailPassword()
            });

            var report = new Report
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

            var errors = new List<Error>
            {
                new Error {Message = "Test error 1", Date = DateTime.Now},
                new Error {Message = "Test error 2", Date = DateTime.Now}
            };

            Console.WriteLine("Sending Email - Daily Report");
            _email.SendAsync("Daily report", _htmlEmail.GenerateReport(report),
                _emailReceiver).Wait();
            Console.WriteLine("Email sent.");

            Console.WriteLine("Sending Email - Errors in Application");
            _email.SendAsync("Errors in application", _htmlEmail.GenerateErrors(errors, 10),
                _emailReceiver).Wait();
            Console.WriteLine("Email sent.");
        }
    }
}

