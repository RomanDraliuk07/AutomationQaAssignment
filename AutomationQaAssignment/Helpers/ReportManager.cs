using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace AutomationQaAssignment.Helpers
{
    public static class ReportManager
    {
        private static ExtentReports? _extent;
        private static readonly object _lock = new();

        public static ExtentReports Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_extent == null)
                    {
                        var outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestResults");
                        Directory.CreateDirectory(outputDir);
                        var reporter = new ExtentSparkReporter(Path.Combine(outputDir, "report.html"));
                        _extent = new ExtentReports();
                        _extent.AttachReporter(reporter);
                    }
                    return _extent;
                }
            }
        }

        public static void Flush()
        {
            _extent?.Flush();
        }
    }
}
