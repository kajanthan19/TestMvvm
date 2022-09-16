using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TestMvvm.Core.Mapper;

namespace TestMvvm.Core
{
    public static class CoreDependencyInjection
    {
        public static IServiceCollection AddCoreExtentionServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetEntryAssembly());

            services.AddSingleton(provider => new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfile()); }).CreateMapper());

            return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()).AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
