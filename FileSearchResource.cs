using ModelContextProtocol.Server;
using System.ComponentModel;

[McpServerResourceType]
public static class FileSearchResource
{
    [McpServerResource(
    UriTemplate = "ui://file-search/app.html",
    MimeType = "text/html",
    Title = "File Search")]
    [Description("Interactive file search UI")]
    public static string GetFileSearchUI()
    {

        var path = Path.Combine(
       AppContext.BaseDirectory,
       "wwwroot",
       "file-search.html"
   );

        return File.ReadAllText(path);
    }
}