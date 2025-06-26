using Integration.Tests.TestData.Factories;

namespace Integration.Tests.TestData.Collections;

[CollectionDefinition("UnauthenticatedIntegrationTest")]
public class UnauthenticatedIntegrationTestCollection : ICollectionFixture<UnauthenticatedIntegrationTestSessionFactory>
{
}