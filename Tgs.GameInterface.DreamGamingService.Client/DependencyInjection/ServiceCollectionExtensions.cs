using Microsoft.Extensions.Options;
using Tgs.GameInterface.DreamGamingService.Client;
using Tgs.GameInterface.DreamGamingService.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceDreamGamingServiceClient(
        this IServiceCollection services,
        Action<GameInterfaceDreamGamingClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceDreamGamingClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceDreamGamingService.GameInterfaceDreamGamingServiceClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceDreamGamingClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceDreamGamingClient, GameInterfaceDreamGamingClient>();
}
