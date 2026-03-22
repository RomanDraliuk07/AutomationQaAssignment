using AutomationQaAssignment.Helpers;
using AventStack.ExtentReports;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;

namespace AutomationQaAssignment.Tests
{
    public class BaseTests : PageTest
    {
        protected ExtentTest _test;

        [SetUp]
        public void StartTest()
        {
            _test = ReportManager.Instance.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            if (status == TestStatus.Failed)
                _test.Fail(TestContext.CurrentContext.Result.Message);
            else
                _test.Pass("Passed");
        }

        [OneTimeTearDown]
        public static void FlushReport()
        {
            ReportManager.Flush();
        }
    }
}
 