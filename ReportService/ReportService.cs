using System;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using ReportService.Repositories;

namespace ReportService
{
    public partial class ReportService : ServiceBase
    {
        private const int IntervalInMinutes = 60;
        private Timer _timer = new Timer(IntervalInMinutes * 60000);
        private ErrorRepository _errorRepository = new ErrorRepository();
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

        }

        protected override void OnStop()
        {
        }
    }
}
