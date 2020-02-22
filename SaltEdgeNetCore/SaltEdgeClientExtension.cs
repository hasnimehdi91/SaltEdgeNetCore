using Microsoft.Extensions.DependencyInjection;
using SaltEdgeNetCore.Client;

namespace SaltEdgeNetCore
{
    public static class SaltEdgeClientExtension
    {
        private static IServiceCollection AddSaltEdge(this IServiceCollection services)
        {
            services.AddTransient<ISaltEdgeClient, SaltEdgeClient>();
            return services;
        }
    }
}