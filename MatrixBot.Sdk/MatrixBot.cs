using System.Text.Json;

namespace MatrixBot.Sdk;

public interface ILogger
{
    public void Info(string message);
    public void Error(string message);
}

class SimpleConsoleLogger : ILogger
{
    void ILogger.Error(string message)
    {
        Console.WriteLine($"ERROR {message}");
    }

    void ILogger.Info(string message)
    {
        Console.WriteLine($"INFO {message}");
    }
}

public class MatrixBot
{
    public event EventHandler<MatrixBotEventArgs>? OnEvent;
    private event EventHandler? OnSync;
    public int SyncTimeout { get; set; } = 10000;

    private readonly IMatrixBotStore _storage;
    private readonly HttpClient HttpClient;

    private Uri? _serverUri;
    private ILogger _logger;
    private MatrixBotConfig? _config;

    private long _requestId = 0L;
    private bool _isSyncing = false;
    public bool IsSyncing
    {
        get
        {
            return _isSyncing;
        }
    }

    public MatrixBot(ILogger? logger = null, IMatrixBotStore? storage = null)
    {

        if (logger == null)
        {
            _logger = new SimpleConsoleLogger();
        }
        else { _logger = logger; }


        if (storage == null)
        {
            _storage = new MatrixBotSimpleFileStorage();
        }
        else { _storage = storage; }


        HttpClient = new HttpClient();
    }

    public async void Start()
    {

        _config = _storage.Read();

        if (_config is null)
        {
            _config = new MatrixBotConfig();
            _logger.Info("configuration was empty - created default storage");
            _storage.Write(_config);
        }

        _serverUri = new Uri(_config.ServerUri);

        if (string.IsNullOrEmpty(_config.AccessToken))
        {
            try
            {
                var login = await DoRequest<MatrixBotJsonLogin>("login", HttpMethod.Post, new { type = "m.login.password", user = _config.Username, password = _config.Password });

                _config.AccessToken = login?.AccessToken;
                _config.UserId = login?.UserId;

                _storage.Write(_config);
            }
            catch (HttpRequestException)
            {
                _logger.Error("no access token available and credentials invalid.");
                return;
            }
        }

        OnSync += OnSyncEvent;
        _isSyncing = true;
        var handler = OnSync;
        handler(this, new EventArgs());
    }

    public void Stop()
    {
        OnSync -= OnSyncEvent;
        _isSyncing = false;
    }

    private async Task Sync(string? since)
    {
        var sync = await DoRequest<MatrixBotJsonSync>($"sync", HttpMethod.Get, null, new string[] { "full_state=false", $"timeout={SyncTimeout}", since == null ? "_=0" : $"since={since}" });

        if (sync is null)
        {
            _logger.Error("sync returned null ?");
            return;
        }

        if (_config is null)
        {
            _logger.Error("_config is null ?");
            return;
        }

        _config.Since = sync.NextBatch;

        _storage.Write(_config);

        if (sync.Rooms is not null && sync.Rooms.Join is not null)
        {
            foreach (var room in sync.Rooms.Join)
            {
                var roomId = room.Key;
                if (room.Value.TimeLine is not null && room.Value.TimeLine.Events is not null)
                {
                    foreach (var ev in room.Value.TimeLine.Events)
                    {
                        var args = new MatrixBotEventArgs(roomId, ev);
                        var handler = OnEvent;
                        if (handler is not null)
                        {
                            handler(this, args);
                        }
                    }
                }
            }
        }
    }

    private async void OnSyncEvent(object? sender, EventArgs e)
    {
        await Sync(_config?.Since);

        _logger.Info($"synced since {_config?.Since}");

        if (OnSync is not null)
        {
            var handler = OnSync;
            handler(this, new EventArgs());
        }
    }

    public async Task<MatrixBotJsonSend?> PostRoomMessage(string roomId, string plain, string formatted)
    {
        var content = new
        {
            msgtype = "m.text",
            format = "org.matrix.custom.html",
            body = plain,
            formatted_body = formatted
        };
        return await DoRequest<MatrixBotJsonSend>($"rooms/{Uri.EscapeDataString(roomId)}/send/m.room.message", HttpMethod.Post, content);
    }

    public async Task<MatrixBotJsonRooms?> GetJoinedRooms()
    {
        return await DoRequest<MatrixBotJsonRooms>($"joined_rooms", HttpMethod.Get);
    }

    public async Task<MatrixBotJsonProfile?> GetProfile()
    {
        return await DoRequest<MatrixBotJsonProfile>($"profile/{_config?.UserId}", HttpMethod.Get);
    }

    private async Task<T?> DoRequest<T>(string path, HttpMethod method, object? body = null, string[]? query = null)
    {
        var q = $"?access_token={_config?.AccessToken}";
        if (query != null)
        {
            q = $"{q}&{ string.Join("&", query)}";
        }
        using var request = new HttpRequestMessage(method, $"{_serverUri}_matrix/client/r0/{path}{q}");
        if (body != null)
        {
            request.Content = new StringContent(JsonSerializer.Serialize(body));
        }
        using var response = await HttpClient.SendAsync(request);
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException)
        {
            throw;
        }
        var jsonString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(jsonString);
    }
}
