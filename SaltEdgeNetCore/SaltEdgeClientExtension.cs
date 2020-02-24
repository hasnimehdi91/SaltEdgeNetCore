using Microsoft.Extensions.DependencyInjection;
using SaltEdgeNetCore.Client;

namespace SaltEdgeNetCore
{
    public static class SaltEdgeClientExtension
    {
        private static IServiceCollection AddSaltEdge(this IServiceCollection services)
        {
            services.AddTransient<ISaltEdgeClientV5, SaltEdgeClientV5>();
            return services;
        }
    }
}