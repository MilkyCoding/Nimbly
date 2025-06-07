using DiscordRPC;
using DiscordRPC.Logging;

namespace NimblyApp.Services
{
    public class DiscordRPCService
    {
        private static DiscordRpcClient? _client;
        private const string ClientId = "1380253022029873184";
        private static Timestamps? _currentTimestamps;

        public static void Initialize()
        {
            if (_client != null) return;

            _client = new DiscordRpcClient(ClientId)
            {
                Logger = new ConsoleLogger() { Level = LogLevel.Warning }
            };

            _client.Initialize();
            _currentTimestamps = Timestamps.Now;
        }

        public static void UpdatePresence(string details, string state, string largeImageKey = "logo")
        {
            if (_client == null) return;

            if (_currentTimestamps == null)
            {
                _currentTimestamps = Timestamps.Now;
            }

            _client.SetPresence(new RichPresence()
            {
                Details = details,
                State = state,
                Assets = new Assets()
                {
                    LargeImageKey = largeImageKey,
                    LargeImageText = "Nimbly Editor"
                },
                Timestamps = _currentTimestamps
            });
        }

        public static void Shutdown()
        {
            if (_client != null)
            {
                _client.Dispose();
                _client = null;
            }
        }
    }
} 