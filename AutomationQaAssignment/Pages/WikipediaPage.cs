using Microsoft.Playwright;

namespace AutomationQaAssignment.Pages
{
    public class WikipediaPage
    {
        private readonly IPage _page;

        public WikipediaPage(IPage page)
        {
            _page = page;
        }

        public async Task GoToAsync()
        {
            await _page.GotoAsync("https://en.wikipedia.org/wiki/Playwright_(software)");
        }

        public ILocator DebuggingFeaturesContainer =>
            _page.Locator("div.mw-heading.mw-heading3").Filter(new() { Has = _page.Locator("#Debugging_features") });

        public ILocator DebuggingFeaturesParagraph =>
            DebuggingFeaturesContainer.Locator("xpath=following-sibling::p[1]");

        public ILocator DebuggingFeaturesListItems =>
            DebuggingFeaturesContainer.Locator("xpath=following-sibling::ul[1]/li");

        public async Task<string> GetDebuggingFeaturesParagraphTextAsync()
        {
           return await DebuggingFeaturesParagraph.InnerTextAsync();
        }

        public async Task<IReadOnlyList<string>> GetDebuggingFeaturesListItemsTextAsync()
        {
            return await DebuggingFeaturesListItems.AllInnerTextsAsync();
        }

        public async Task<ILocator> GetTestingAndDebuggingItemsAsync()
        {
            await _page.GetByLabel("Microsoft development tools")
                       .GetByRole(AriaRole.Button, new() { Name = "[show]" })
                       .ClickAsync();

            return _page.GetByRole(AriaRole.Row)
                        .Filter(new()
                        {
                            Has = _page.GetByRole(AriaRole.Rowheader,
                            new() { Name = "Testing and debugging" })
                        })
                        .GetByRole(AriaRole.Listitem);
        }

        public async Task SelectDarkColorAsync() => 
            await _page.GetByRole(AriaRole.Radio, new() { Name = "Dark"}).ClickAsync();

        public async Task<string> GetHtmlClassAsync()
        {
            return await _page.Locator("html").GetAttributeAsync("class") ?? string.Empty;
        }
    }
}
