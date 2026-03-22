using AutomationQaAssignment.Helpers;
using AutomationQaAssignment.Services;
using AventStack.ExtentReports;
using FluentAssertions;
using NUnit.Framework.Interfaces;

namespace AutomationQaAssignment.Tests;

public class WikipediaApiTests
{
    private HttpClient _httpClient;
    private WikipediaApiService _wikipediaApiService;
    private ExtentTest _test;

    [SetUp]
    public void SetUp()
    {
        _httpClient = new HttpClient();
        _wikipediaApiService = new WikipediaApiService(_httpClient);
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
        _httpClient.Dispose();
    }

    [OneTimeTearDown]
    public void FlushReport()
    {
        ReportManager.Flush();
    }

    [Test]
    public async Task Should_Get_Debugging_Features_Section_Index()
    {
        _test.Log(Status.Info, "Calling MediaWiki API to get sections list");
        var sectionIndex = await _wikipediaApiService.GetDebuggingFeaturesSectionIndexAsync();

        _test.Log(Status.Info, $"Section index returned: {sectionIndex}");
        sectionIndex.Should().NotBeNullOrWhiteSpace();
    }

    [Test]
    public async Task Should_Get_Debugging_Features_Section_Html()
    {
        _test.Log(Status.Info, "Calling MediaWiki API to get Debugging Features section HTML");
        var html = await _wikipediaApiService.GetDebuggingFeaturesSectionHtmlAsync();

        _test.Log(Status.Info, "Validating HTML is not empty and contains expected content");
        html.Should().NotBeNullOrWhiteSpace();
        html.Should().Contain("Screenshots");
    }

    [Test]
    public async Task Should_Extract_Debugging_Features_Text_From_Api()
    {
        _test.Log(Status.Info, "Calling MediaWiki API to extract Debugging Features text");
        var text = await _wikipediaApiService.GetDebuggingFeaturesTextAsync();

        _test.Log(Status.Info, "Validating extracted text contains expected content");
        text.Should().NotBeNullOrWhiteSpace();
        text.Should().Contain("Playwright includes built-in debugging capabilities");
        text.Should().Contain("Screenshots");
    }
}