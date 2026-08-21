using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerToolType]
public static class FileSearchTools
{
    [McpServerTool]
    [Description("Searches the uploaded text file and returns lines containing the search text.")]
    [McpMeta("ui", JsonValue = """{"resourceUri":"ui://file-search/app.html"}""")]
    public static string SearchFile(
        string? fileName = null,
        string? fileContent = null,
        string? searchText = null)
    {
        if (string.IsNullOrWhiteSpace(fileContent))
            return "Select a text file to begin searching.";

        if (string.IsNullOrWhiteSpace(searchText))
            return "Please enter something to search for.";

        var lines = fileContent.Split(
            new[] { "\r\n", "\n" },
            StringSplitOptions.None);

        var matches = lines
            .Where(line =>
                line.Contains(
                    searchText,
                    StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count == 0)
        {
            return $"No information found for '{searchText}'.";
        }

        return string.Join(
            Environment.NewLine,
            matches);
    }
}