using AutomationQaAssignment.Helpers;
using AventStack.ExtentReports;
using NUnit.Framework.Interfaces;

namespace AutomationQaAssignment.Tests
{
    public class BaseApiTests
    {
        protected ExtentTest _test;

        [SetUp]
        public void StartTest()
        {
            _test = ReportManager.Instance.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void EndTest()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            if (status == TestStatus.Failed)
                _test.Fail(TestContext.CurrentContext.Result.Message);
            else
                _test.Pass("Passed");
        }

        [OneTimeTearDown]
        public void FlushReport()
        {
            ReportManager.Flush();
        }
    }
}