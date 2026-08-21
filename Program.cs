using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:3001");

var mcpServerEndpoint =
    builder.Configuration["McpServer:Url"] ??
    "http://localhost:3001/mcp";

builder.Services.AddSingleton<McpClientService>(serviceProvider =>
    new McpClientService(
        mcpServerEndpoint,
        serviceProvider.GetRequiredService<ILoggerFactory>()));

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();

var app = builder.Build();

// MCP endpoint - map to /mcp path
app.MapMcp("/mcp");

app.MapGet("/api/mcp/tools", async (
    McpClientService client,
    CancellationToken cancellationToken) =>
{
    var tools = await client.ListToolsAsync(cancellationToken);

    return Results.Ok(tools.Select(tool => new
    {
        tool.Name,
        tool.Title,
        tool.Description,
        tool.JsonSchema
    }));
});

app.MapPost("/api/mcp/tools/{toolName}", async (
    string toolName,
    Dictionary<string, object?> arguments,
    McpClientService client,
    CancellationToken cancellationToken) =>
{
    var result = await client.CallToolAsync(
        toolName,
        arguments,
        cancellationToken);

    return Results.Ok(result);
});

Console.WriteLine();
Console.WriteLine("🎨 Color Picker MCP App (C#)");
Console.WriteLine("============================");
Console.WriteLine("MCP server listening on http://localhost:3001/mcp");
Console.WriteLine();
Console.WriteLine("Add to your VS Code MCP config:");
Console.WriteLine("  \"url\": \"http://localhost:3001/mcp\"");
Console.WriteLine("  \"type\": \"http\"");
Console.WriteLine();
Console.WriteLine("Press Ctrl+C to stop the server");
Console.WriteLine();

await app.RunAsync();
