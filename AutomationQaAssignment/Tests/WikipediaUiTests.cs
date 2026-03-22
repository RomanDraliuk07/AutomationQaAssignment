using AutomationQaAssignment.Pages;
using AventStack.ExtentReports;
using FluentAssertions;
using Microsoft.Playwright;

namespace AutomationQaAssignment.Tests
{
    public class WikipediaUiTests : BaseTests
    {
        [Test]
        public async Task Should_Open_Playwright_Wikipedia_Page()
        {
            _test.Log(Status.Info, "Navigating to Wikipedia Playwright page");
            var wikipediaPage = new WikipediaPage(Page);
            await wikipediaPage.GoToAsync();

            _test.Log(Status.Info, "Getting page title and URL");
            var pageTitle = await Page.TitleAsync();
            var currentUrl = Page.Url;
            _test.Log(Status.Info, $"Page title: {pageTitle}, URL: {currentUrl}");

            currentUrl.Should().MatchRegex("Playwright_\\(software\\)");
            pageTitle.Should().Contain("Playwright");
        }

        [Test]
        public async Task Should_Extract_Debugging_Features_Text_From_UI()
        {
            _test.Log(Status.Info, "Navigating to Wikipedia Playwright page");
            var wikipediaPage = new WikipediaPage(Page);
            await wikipediaPage.GoToAsync();

            _test.Log(Status.Info, "Extracting Debugging Features paragraph text");
            var paragraph = await wikipediaPage.GetDebuggingFeaturesParagraphTextAsync();

            _test.Log(Status.Info, "Extracting Debugging Features list items");
            var listItems = await wikipediaPage.GetDebuggingFeaturesListItemsTextAsync();

            _test.Log(Status.Info, "Validating extracted text");
            paragraph.Should().NotBeNullOrWhiteSpace();
            paragraph.Should().Contain("Playwright includes built-in debugging capabilities");
            listItems.Should().Contain(item => item.Contains("Screenshots"));
        }
        [Test]
        public async Task All_Testing_And_Debugging_Technology_Names_Should_Be_Text_Links()
        {
            _test.Log(Status.Info, "Navigating to Wikipedia Playwright page");
            var wikipediaPage = new WikipediaPage(Page);
            await wikipediaPage.GoToAsync();

            _test.Log(Status.Info, "Expanding Microsoft development tools section");
            var items = await wikipediaPage.GetTestingAndDebuggingItemsAsync();
            var count = await items.CountAsync();
            _test.Log(Status.Info, $"Found {count} technology items");

            count.Should().BeGreaterThan(0, "Testing and debugging section should contain technology items");

            for (int i = 0; i < count; i++)
            {
                var item = items.Nth(i);
                var itemText = (await item.TextContentAsync())?.Trim();
                _test.Log(Status.Info, $"Checking item: {itemText}");

                var link = item.GetByRole(AriaRole.Link);
                var isLink = await link.CountAsync() > 0;
                isLink.Should().BeTrue($"Technology '{itemText}' is not a text link");

                if (isLink)
                {
                    var href = await link.GetAttributeAsync("href");
                    href.Should().NotBeNullOrWhiteSpace($"Technology '{itemText}' should have a valid href");
                    _test.Log(Status.Info, $"'{itemText}' is a valid text link: {href}");
                }
            }
        }

        [Test]
        public async Task Selecting_Dark_Color_Should_Change_Page_To_Dark_Mode()
        {
            _test.Log(Status.Info, "Navigating to Wikipedia Playwright page");
            var wikipediaPage = new WikipediaPage(Page);
            await wikipediaPage.GoToAsync();

            _test.Log(Status.Info, "Selecting Dark color option");
            await wikipediaPage.SelectDarkColorAsync();

            _test.Log(Status.Info, "Validating dark mode is active");
            var htmlClass = await wikipediaPage.GetHtmlClassAsync();
            htmlClass.Should().Contain("skin-theme-clientpref-night");
        }
    }
}