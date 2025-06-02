using CashFlow.Domain.Repositories.User;
using Moq;

namespace CommonTestUtilities.Repositories;

// Builder para criar mocks de IUserWriteOnlyRepository
public class UserWriteOnlyRepositoryBuilder
{
    // Cria um mock básico do repositório
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}