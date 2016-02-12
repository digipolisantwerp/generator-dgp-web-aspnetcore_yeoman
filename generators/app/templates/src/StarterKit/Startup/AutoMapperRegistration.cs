using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace StarterKit
{
    public static class AutoMapperRegistration
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            // add your automapper registrations here            
            return services;
        }
    }
}