using System;
using Microsoft.Extensions.DependencyInjection;
using SaltEdgeNetCore.Client;

namespace SaltEdgeNetCore
{
    public static class SaltEdgeClientExtension
    {
        public static IServiceCollection AddSaltEdge(this IServiceCollection services,
            Action<SaltEdgeOptions> configure = null)
        {
            var options = new SaltEdgeOptions();
            configure?.Invoke(options);
            services.AddTransient<ISaltEdgeClientV5, SaltEdgeClientV5>(saltEdgeV5 => new SaltEdgeClientV5(options));
            return services;
        }
    }
}