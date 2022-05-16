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

        public ReportService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            _timer.Elapsed += DoWork;
            _timer.Start();
        }

        private void DoWork(object sender, ElapsedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void SendError()
        {
            var errors = _errorRepository.GetLastErrors(IntervalInMinutes);

            if (errors == null || errors.Any() == false) return;

            // TODO: implement send mail
            throw new NotImplementedException();
        }

        private void SendReport()
        {
            var actualHour = DateTime.Now.Hour;

            if (actualHour < SendHour) return;

            var report = _reportRepository.GetLastNotSentReport();

            if (report == null) return;

            // TODO: send email

            _reportRepository.ReportSent(report);
        }

        protected override void OnStop()
        {
        }
    }
}
