﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentPOS.Shared.Core.Extensions;

namespace FluentPOS.Modules.People.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPeopleCore(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddExtendedAttributeHandlersFromAssembly(Assembly.GetExecutingAssembly());
            services.AddExtendedAttributeCommandValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}