using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestMvvm.Domain.IRepository;
using TestMvvm.Domain.IUnitOfWork;
using TestMvvm.Domain.Services;
using TestMvvm.Infrastructure.BaseRepository;
using TestMvvm.Infrastructure.Service;
using TestMvvm.Migrations;

namespace TestMvvm.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructureExtentionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TestMvvmContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IFileStorageService, LocalFileStorageService>();

            services.AddScoped<IAircraftRepository, AircraftRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

            return services;
        }
    }
}
