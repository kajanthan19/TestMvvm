using TestMvvm.Core;
using TestMvvm.Infrastructure;

namespace TestMvvm.Web.Extentions
{
    public static class CustomDependencyInjection
    {
        public static IServiceCollection AddCustomExtentionServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddInfrastructureExtentionServices(configuration);
            services.AddCoreExtentionServices(configuration);

            return services;
        }

    }
}
