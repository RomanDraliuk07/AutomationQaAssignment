using AutomationQaAssignment.Services;
using AventStack.ExtentReports;
using FluentAssertions;

namespace AutomationQaAssignment.Tests;

public class WikipediaApiTests : BaseApiTests
{
    private HttpClient _httpClient;
    private WikipediaApiService _wikipediaApiService;

    [SetUp]
    public void SetUp()
    {
        _httpClient = new HttpClient();
        _wikipediaApiService = new WikipediaApiService(_httpClient);
    }

    [TearDown]
    public void DisposeClient()
    {
        _httpClient.Dispose();
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