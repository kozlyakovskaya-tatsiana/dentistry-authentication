using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Mapster
{
    public static class MapsterConfiguration
    {
        public static void RegisterMapsterConfiguration(this IServiceCollection services)
        {
            TypeAdapterConfig.GlobalSettings.Scan(typeof(MapsterConfiguration).Assembly);
        }
    }
}
