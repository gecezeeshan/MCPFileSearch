using ModelContextProtocol.Client;
using ModelContextProtocol.Protocol;

public sealed class McpClientService : IAsyncDisposable
{
    private readonly Uri _serverEndpoint;
    private readonly ILoggerFactory _loggerFactory;
    private readonly Lazy<Task<McpClient>> _clientTask;

    public McpClientService(
        string serverEndpoint,
        ILoggerFactory loggerFactory)
    {
        if (!Uri.TryCreate(serverEndpoint, UriKind.Absolute, out var endpoint) ||
            (endpoint.Scheme != Uri.UriSchemeHttp && endpoint.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException(
                "The MCP server endpoint must be an absolute HTTP or HTTPS URL.",
                nameof(serverEndpoint));
        }

        _serverEndpoint = endpoint;
        _loggerFactory = loggerFactory;
        _clientTask = new Lazy<Task<McpClient>>(
            () => CreateClientAsync());
    }

    public async Task<IList<McpClientTool>> ListToolsAsync(
        CancellationToken cancellationToken = default)
    {
        var client = await _clientTask.Value;
        return await client.ListToolsAsync(cancellationToken: cancellationToken);
    }

    public async Task<CallToolResult> CallToolAsync(
        string toolName,
        IReadOnlyDictionary<string, object?> arguments,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(toolName);
        ArgumentNullException.ThrowIfNull(arguments);

        var tools = await ListToolsAsync(cancellationToken);
        var tool = tools.FirstOrDefault(
            candidate => string.Equals(candidate.Name, toolName, StringComparison.Ordinal));

        if (tool is null)
        {
            throw new InvalidOperationException(
                $"The MCP server does not expose a tool named '{toolName}'.");
        }

        return await tool.CallAsync(
            arguments,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        if (_clientTask.IsValueCreated)
        {
            var client = await _clientTask.Value;
            await client.DisposeAsync();
        }
    }

    private async Task<McpClient> CreateClientAsync()
    {
        var transport = new HttpClientTransport(
            new HttpClientTransportOptions
            {
                Endpoint = _serverEndpoint
            },
            _loggerFactory);

        return await McpClient.CreateAsync(
            transport,
            new McpClientOptions(),
            _loggerFactory);
    }
}