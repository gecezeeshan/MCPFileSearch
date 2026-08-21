using ModelContextProtocol.Server;
using System.ComponentModel;

#region colorTool
/// <summary>
/// HTML content provider for the color picker UI
/// </summary>
public static class ColorPickerHtmlProvider
{
  public static async Task<string> GetHtml()
  {
    var path = Path.Combine(AppContext.BaseDirectory, "wwwroot", "file-search.html");
    return await File.ReadAllTextAsync(path);

  }
}

/// <summary>
/// Color Picker MCP Tools
/// </summary>
[McpServerToolType]
public static class ColorPickerTools
{
  /// <summary>
  /// Opens an interactive color picker UI to visually select a color.
  /// </summary>
  [McpServerTool]
  [Description("Open an interactive color picker to select a color visually. Returns an HTML UI that allows the user to pick colors interactively.")]
  [McpMeta("ui", JsonValue = """{ "resourceUri": "ui://color-picker/app.html" }""")]
  public static ColorPickerResult ColorPicker(
      [Description("Initial color to display (hex format like #FF5733). Default: #3498DB")]
        string? initialColor = "#3498DB")
  {
    return new ColorPickerResult
    {
      InitialColor = initialColor ?? "#3498DB",
      Message = "Opening color picker UI..."
    };
  }
}

/// <summary>
/// Color Picker MCP Resources
/// </summary>
[McpServerResourceType]
public static class ColorPickerResources
{
  /// <summary>
  /// Provides the HTML UI for the color picker app
  /// </summary>
  [McpServerResource(
      UriTemplate = "ui://color-picker/app.html",
      MimeType = "text/html",
      Title = "Color Picker UI")]
  [Description("Interactive color picker UI")]
  public static async Task<string> GetColorPickerUI()
  {
    return await ColorPickerHtmlProvider.GetHtml();
  }
}

public class ColorPickerResult
{
  public string InitialColor { get; set; } = "#3498DB";
  public string Message { get; set; } = "";

  public override string ToString() =>
      $"Color Picker ready. Initial color: {InitialColor}";
}
#endregion
