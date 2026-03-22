using AutomationQaAssignment.Helpers;
using AutomationQaAssignment.Pages;
using AutomationQaAssignment.Services;
using AventStack.ExtentReports;
using FluentAssertions;

namespace AutomationQaAssignment.Tests
{
    public class WikipediaComparisonTests : BaseTests
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
        public async Task DebuggingFeatures_Ui_And_Api_Should_Have_The_Same_Unique_Word_Count()
        {
            _test.Log(Status.Info, "Navigating to Wikipedia Playwright page");
            var wikipediaPage = new WikipediaPage(Page);
            await wikipediaPage.GoToAsync();

            _test.Log(Status.Info, "Extracting Debugging Features text from UI");
            var paragraph = await wikipediaPage.GetDebuggingFeaturesParagraphTextAsync();
            var listItems = await wikipediaPage.GetDebuggingFeaturesListItemsTextAsync();
            var uiText = $"{paragraph} {string.Join(" ", listItems)}".Trim();

            _test.Log(Status.Info, "Extracting Debugging Features text from API");
            var apiText = await _wikipediaApiService.GetDebuggingFeaturesTextAsync();

            _test.Log(Status.Info, "Counting unique words in both texts");
            var uiUniqueWordsCount = TextHelper.CountUniqWords(uiText);
            var apiUniqueWordsCount = TextHelper.CountUniqWords(apiText);
            _test.Log(Status.Info, $"UI unique words: {uiUniqueWordsCount}, API unique words: {apiUniqueWordsCount}");

            uiText.Should().NotBeNullOrWhiteSpace();
            apiText.Should().NotBeNullOrWhiteSpace();
            uiUniqueWordsCount.Should().Be(apiUniqueWordsCount);
        }
    }
}