using System.Text.Json;
using HtmlAgilityPack;

namespace AutomationQaAssignment.Services;

public class WikipediaApiService
{
    private readonly HttpClient _httpClient;

    public WikipediaApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        if (!_httpClient.DefaultRequestHeaders.UserAgent.Any())
        {
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("AutomationQaAssignment/1.0");
        }
    }

    public async Task<string> GetDebuggingFeaturesSectionIndexAsync()
    {
        var url =
            "https://en.wikipedia.org/w/api.php" +
            "?action=parse" +
            "&page=Playwright_(software)" +
            "&prop=sections" +
            "&format=json";

        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        var sections = document.RootElement
            .GetProperty("parse")
            .GetProperty("sections");

        foreach (var section in sections.EnumerateArray())
        {
            var anchor = section.GetProperty("anchor").GetString();

            if (anchor == "Debugging_features")
            {
                return section.GetProperty("index").GetString()!;
            }
        }

        throw new Exception("Section not found");
    }

    public async Task<string> GetDebuggingFeaturesSectionHtmlAsync()
    {
        var sectionIndex = await GetDebuggingFeaturesSectionIndexAsync();

        var url =
            "https://en.wikipedia.org/w/api.php" +
            "?action=parse" +
            "&page=Playwright_(software)" +
            $"&section={sectionIndex}" +
            "&prop=text" +
            "&format=json";

        using var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        return document.RootElement
            .GetProperty("parse")
            .GetProperty("text")
            .GetProperty("*")
            .GetString() ?? string.Empty;
    }

    public async Task<string> GetDebuggingFeaturesTextAsync()
    {
        var html = await GetDebuggingFeaturesSectionHtmlAsync();

        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(html);

        var supNodes = htmlDocument.DocumentNode.SelectNodes("//sup");
        if (supNodes != null)
        {
            foreach (var supNode in supNodes)
            {
                supNode.Remove();
            }
        }

        var paragraphNode = htmlDocument.DocumentNode.SelectSingleNode("//p");
        var listItemNodes = htmlDocument.DocumentNode.SelectNodes("//ul/li");

        var paragraphText = paragraphNode == null
            ? string.Empty
            : HtmlEntity.DeEntitize(paragraphNode.InnerText).Trim();

        var listItemsText = listItemNodes == null
            ? string.Empty
            : string.Join(" ", listItemNodes.Select(x => HtmlEntity.DeEntitize(x.InnerText).Trim()));

        return $"{paragraphText} {listItemsText}".Trim();
    }
}