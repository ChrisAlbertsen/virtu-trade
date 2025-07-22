using Integration.Tests.TestData.Factories;

namespace Integration.Tests.TestData.Collections;

[CollectionDefinition("IntegrationTest")]
public class AuthenticatedIntegrationTestCollection : ICollectionFixture<IntegrationTestSessionFactory>
{
}