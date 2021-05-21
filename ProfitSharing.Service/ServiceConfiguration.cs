﻿using Microsoft.Extensions.DependencyInjection;
using ProfitSharing.Infrastructure.Integrations.Clients;
using Microsoft.Extensions.Configuration;
using ProfitSharing.Domain.Interfaces;
using Polly;
using System;
using Microsoft.Extensions.Options;

namespace ProfitSharing.Service
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<EmployeeManagementClientSettings>(
            configuration.GetSection(nameof(EmployeeManagementClientSettings)));
            services.AddSingleton<IEmployeeManagementClientSettings>(sp => sp.GetRequiredService<IOptions<EmployeeManagementClientSettings>>().Value);
            services.AddHttpClient<IEmployeeManagementClient, EmployeeManagementClient>().AddTransientHttpErrorPolicy(
            p => p.WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(20),
                TimeSpan.FromSeconds(30)
            })); ;
          
            return services;         
        }
    }
}
