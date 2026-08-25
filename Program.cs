using ModelContextProtocol.Server;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:3001");

// ============================================================
// MCP
// ============================================================

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly()
    .WithResourcesFromAssembly();

var app = builder.Build();

// ============================================================
// SERVE wwwroot
// ============================================================

app.UseDefaultFiles();
app.UseStaticFiles();

// ============================================================
// MCP ENDPOINT
// ============================================================

app.MapMcp("/mcp");

// ============================================================
// START
// ============================================================

Console.WriteLine();
Console.WriteLine("======================================");
Console.WriteLine("     File Search MCP Server");
Console.WriteLine("======================================");
Console.WriteLine();

Console.WriteLine("Web UI:");
Console.WriteLine("http://localhost:3001/file-search.html");

Console.WriteLine();

Console.WriteLine("MCP endpoint:");
Console.WriteLine("http://localhost:3001/mcp");

Console.WriteLine();

Console.WriteLine("Press Ctrl+C to stop the server");

await app.RunAsync();