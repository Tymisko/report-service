using System;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using Cipher;
using EmailSender;
using ReportService.Repositories;
using ReportService.Core;

namespace ReportService
{
    public partial class ReportService : ServiceBase
    {
        private static int _sendingReportHour;
        private static int _errorReportSendingInterval;
        private readonly Timer _timer;

        private readonly ErrorRepository _errorRepository = new ErrorRepository();
        private readonly ReportRepository _reportRepository = new ReportRepository();

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        private readonly Email _email;
        private readonly GenerateHtmlEmail _htmlEmail = new GenerateHtmlEmail();
        private readonly string _emailReceiver;

        private readonly StringCipher _stringCipher = new StringCipher("175219F3-E67D-49D3-80D4-208E983BB6ED");
        private const string NotEncryptedPasswordPrefix = "encrypt:";


        public ReportService()
        {
            InitializeComponent();

            try
            {
                _emailReceiver = ConfigurationManager.AppSettings["ReceiverEmail"];
                _sendingReportHour = Convert.ToInt32(ConfigurationManager.AppSettings["SendingReportHour"]);
                _errorReportSendingInterval = Convert.ToInt32(ConfigurationManager.AppSettings["ErrorReportSendingInterval"]);
                _timer = new Timer(_errorReportSendingInterval * 60000);

                _email = new Email(new EmailParams
                {
                    HostSmtp = ConfigurationManager.AppSettings["HostSmtp"],
                    Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]),
                    EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]),
                    SenderName = ConfigurationManager.AppSettings["SenderName"],
                    SenderEmail = ConfigurationManager.AppSettings["SenderEmail"],
                    SenderEmailPassword = DecryptSenderEmailPassword()
                });
            }
            catch (Exception e)
            {
                Logger.Error(e, e.Message);
                throw;
            }
        }

        private string DecryptSenderEmailPassword()
        {
            var encryptedPassword = ConfigurationManager.AppSettings["SenderEmailPassword"];

            if (encryptedPassword.StartsWith("encrypt:"))
            {
                encryptedPassword = _stringCipher.Encrypt(encryptedPassword.Replace(NotEncryptedPasswordPrefix, ""));

                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configFile.AppSettings.Settings["SenderEmailPassword"].Value = encryptedPassword;
                configFile.Save();
            }

            return _stringCipher.Decrypt(encryptedPassword);
        }


        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWorkAsync;
            _timer.Start();

            Logger.Info("Service started.");
        }

        private async void DoWorkAsync(object sender, ElapsedEventArgs e)
        {
            try
            {
                await SendErrorAsync();
                await SendReportAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw;
            }
        }

        private async Task SendErrorAsync()
        {
            var errors = _errorRepository.GetLastErrors(_errorReportSendingInterval);

            if (errors == null || errors.Any() == false) return;

            await _email.SendAsync("Errors in application", _htmlEmail.GenerateErrors(errors, _errorReportSendingInterval),
                _emailReceiver);

            Logger.Info("Error sent.");
        }

        private async Task SendReportAsync()
        {
            var actualHour = DateTime.Now.Hour;

            if (actualHour < _sendingReportHour) return;

            var report = _reportRepository.GetLastNotSentReport();

            if (report == null) return;

            await _email.SendAsync("Daily report", _htmlEmail.GenerateReport(report), _emailReceiver);

            _reportRepository.ReportSent(report);

            Logger.Info("Report sent.");
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped.");
        }
    }
}
