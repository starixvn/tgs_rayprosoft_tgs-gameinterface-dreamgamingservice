using System.Reflection;
using Tgs.GameInterface.DreamGamingService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDreamGamingServiceCore(
        this IServiceCollection services)
        => services                        
            .AddHttpClient<IGameApiClient, GameApiClient>()
            .Services            
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient);
}
