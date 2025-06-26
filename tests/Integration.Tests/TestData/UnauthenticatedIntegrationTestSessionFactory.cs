using System;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Tests.TestData;

public class UnauthenticatedIntegrationTestSessionFactory : IntegrationTestSessionFactory
{
    protected override void ConfigureAuth(IServiceCollection services)
    {
        // Do nothing to remove authentication
    }
}