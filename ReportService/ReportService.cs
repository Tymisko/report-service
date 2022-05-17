using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceProcess;
using System.Timers;
using ReportService.Models.Domains;
using ReportService.Repositories;

namespace ReportService
{
    public partial class ReportService : ServiceBase
    {
        private const int SendHour = 8;
        private const int IntervalInMinutes = 60;
        private readonly Timer _timer = new Timer(IntervalInMinutes * 60000);
        private readonly ErrorRepository _errorRepository = new ErrorRepository();
        private readonly ReportRepository _reportRepository = new ReportRepository();
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        public ReportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();

            Logger.Info("Service started.");
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            try
            {
                SendError();
                SendReport();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
                throw;
            }
        }

        private void SendError()
        {
            var errors = _errorRepository.GetLastErrors(IntervalInMinutes);

            if (errors == null || errors.Any() == false) return;

            // TODO: implement send mail

            Logger.Info("Error sent.");
        }

        private void SendReport()
        {
            var actualHour = DateTime.Now.Hour;

            if (actualHour < SendHour) return;

            var report = _reportRepository.GetLastNotSentReport();

            if (report == null) return;

            // TODO: send email

            _reportRepository.ReportSent(report);

            Logger.Info("Report sent.");
        }

        protected override void OnStop()
        {
            Logger.Info("Service stopped.");
        }
    }
}
