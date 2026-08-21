using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:3001");

builder.Services.AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();

var app = builder.Build();

// MCP endpoint - map to /mcp path
app.MapMcp("/mcp");

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
